using Aprovi.Business.Services;
using Aprovi.Helpers;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class MigrationToolsPresenter
    {
        private IMigrationToolsView _view;
        private IArticuloService _items;
        private IClienteService _clients;
        private IProveedorService _suppliers;
        private IClasificacionService _classifications;
        private IUnidadDeMedidaService _unitsOfMeasure;
        private IImpuestoService _taxes;
        private IMigrationDataService _vfpData;
        private IAjusteService _adjustments;

        public MigrationToolsPresenter(IMigrationToolsView view, IArticuloService items, IClienteService clients, IProveedorService suppliers, IClasificacionService classifications, IUnidadDeMedidaService unitsOfMeasure, IImpuestoService taxes, IMigrationDataService vfpData, IAjusteService  adjustments)
        {
            _view = view;
            _items = items;
            _clients = clients;
            _suppliers = suppliers;
            _classifications = classifications;
            _unitsOfMeasure = unitsOfMeasure;
            _taxes = taxes;
            _vfpData = vfpData;
            _adjustments = adjustments;

            _view.Quit += Quit;
            _view.Process += ProcessRecords;
            _view.OpenFindDbc += OpenFindDbc;
        }

        private void OpenFindDbc()
        {
            try
            {
                var dbcPath = _view.OpenFileFinder("Contenedor de tablas (*.dbc)|*.dbc");

                if (!dbcPath.isValid())
                {
                    _view.ShowMessage("Debe seleccionar un contenedor de tablas de donde obtener los registros");
                    return;
                }

                _view.Show(dbcPath);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ProcessRecords()
        {
            try
            {
                if(!_view.DbcPath.isValid())
                {
                    _view.ShowError("Debe especificar el contenedor de tablas");
                    return;
                }

                if(!_view.Items && !_view.Clients && !_view.Suppliers)
                {
                    _view.ShowError("Debe seleccionar al menos uno de los catálogos a migrar");
                    return;
                }

                if (_view.Clients)
                    _clients.Import(_view.DbcPath);

                if (_view.Suppliers)
                    _suppliers.Import(_view.DbcPath);

                if (_view.Items)
                {
                    IItemsMigrationToolView view;
                    ItemsMigrationToolPresenter presenter;

                    view = new ItemsMigrationToolView(_view.DbcPath);
                    presenter = new ItemsMigrationToolPresenter(view, _classifications, _unitsOfMeasure, _items, _taxes, _vfpData, _adjustments);

                    view.ShowWindow();
                }

                _view.ShowMessage("Importación de catálogos finalizada exitosamente");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex);
            }
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
    }
}
