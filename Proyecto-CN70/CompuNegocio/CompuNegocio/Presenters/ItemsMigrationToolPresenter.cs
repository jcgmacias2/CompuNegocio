using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Linq;
using System.Collections.Generic;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class ItemsMigrationToolPresenter
    {
        private IItemsMigrationToolView _view;
        private IClasificacionService _classifications;
        private IUnidadDeMedidaService _unitsOfMeasure;
        private IArticuloService _items;
        private IImpuestoService _taxes;
        private IMigrationDataService _vfpData;
        private IAjusteService _adjustments;

        public ItemsMigrationToolPresenter(IItemsMigrationToolView view, IClasificacionService classifications, IUnidadDeMedidaService unitsOfMeasure, IArticuloService items, IImpuestoService taxes, IMigrationDataService vfpData, IAjusteService adjustments)
        {
            _view = view;
            _classifications = classifications;
            _unitsOfMeasure = unitsOfMeasure;
            _items = items;
            _taxes = taxes;
            _vfpData = vfpData;
            _adjustments = adjustments;

            _view.Quit += Quit;
            _view.Migrate += Migrate;

            _view.Fill(_vfpData.GetUnidadesDeMedida(_view.dbcPath), _unitsOfMeasure.List(), _vfpData.GetClasificaciones(_view.dbcPath), _classifications.List(), _taxes.List(false));
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Migrate()
        {
            try
            {
                if (!_view.VAT.isValid() || !_view.VAT.idImpuesto.isValid())
                    throw new Exception("Debe especificar el impuesto a utilizar como IVA para los artículos");

                var unidades = _view.Units;
                if (unidades.Any(u => !u.IdUnidadDeMedida.isValid()))
                    throw new Exception("Debe especificar la unidad de medida equivalente para cada una de las medidas del sistema anterior");

                //En stock almaceno la existencia registrada en el sistema anterior
                var stock = new Dictionary<string, decimal>();
                var items = _vfpData.GetArticulos(_view.dbcPath, out stock);

                //Realizo la migración de artículos y me regresa solamente los que SI fueron migrados
                var migrated = _items.Import(items, unidades, _view.Classifications, _view.VAT);

                _view.ShowMessage("Los artículos fueron integrados exitosamente, ahora se integrará el inventario");

                //Reviso cuales artículos migrados son inventariados
                var migratedWithStock = migrated.Where(i => i.inventariado).ToList();
                if(migratedWithStock.Count.Equals(0))
                {
                    _view.ShowError("No existen artículos migrados con inventario previo");
                    return;
                }

                //Voy a relacionar los artículos migrados con la existencia que traen del sistema anterior para con ellos crear un ajuste inicial de entrada
                var migratedStock = new List<VMArticulo>();
                migratedWithStock.ForEach(s => migratedStock.Add(new VMArticulo(s, stock.FirstOrDefault(i => i.Key.Equals(s.codigo)).Value)));

                //Genero el ajuste basado en los artículos migrados, que serán inventariados y traen existencia
                var initialStockAdjustment = _adjustments.MigrateStock(migratedStock.Where(i => i.Existencia > 0.0m).ToList());

                _view.ShowMessage("Ajuste {0} registrado exitosamente", initialStockAdjustment.folio);

                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
