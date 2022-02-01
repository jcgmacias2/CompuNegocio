using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class UnitsOfMeasurePresenter
    {
        private readonly IUnitsOfMeasureView _view;
        private IUnidadDeMedidaService _units;

        public UnitsOfMeasurePresenter(IUnitsOfMeasureView view, IUnidadDeMedidaService unitService)
        {
            _view = view;
            _units = unitService;

            _view.Find += Find;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
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

        private void OpenList()
        {
            try
            {
                IUnitsOfMeasureListView view;
                UnitsOfMeasureListPresenter presenter;

                view = new UnitsOfMeasureListView();
                presenter = new UnitsOfMeasureListPresenter(view, _units);

                view.ShowWindow();

                if (view.Unit.isValid() && view.Unit.idUnidadDeMedida.isValid())
                    _view.Show(view.Unit);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            if (!_view.UnitOfMeasure.codigo.isValid())
            {
                _view.ShowMessage("Debe capturar el código de la unidad de medida");
                return;
            }

            if(!_view.UnitOfMeasure.descripcion.isValid())
            {
                _view.ShowMessage("Debe capturar la descripción de la unidad de medida");
                return;
            }

            try
            {
                var unit = _view.UnitOfMeasure;
                unit.activo = true;

                _units.Update(unit);
                _view.ShowMessage(string.Format("Unidad de medida {0} actualizada exitosamente", unit.descripcion));
                _view.Clear();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            if (!_view.UnitOfMeasure.codigo.isValid())
            {
                _view.ShowMessage("Debe capturar el código de la unidad de medida");
                return;
            }

            if (!_view.UnitOfMeasure.descripcion.isValid())
            {
                _view.ShowMessage("Debe capturar la descripción de la unidad de medida");
                return;
            }

            try
            {
                var unit = _view.UnitOfMeasure;
                unit.activo = true;

                _units.Add(unit);
                _view.ShowMessage(string.Format("Unidad de medida {0} agregada exitosamente", unit.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if (!_view.IsDirty)
            {
                _view.ShowError("No existe unidad de medida seleccionada para eliminar");
                return;
            }

            try
            {
                if(_units.CanDelete(_view.UnitOfMeasure))
                {
                    _units.Delete(_view.UnitOfMeasure);
                }
                else
                {
                    var local = _view.UnitOfMeasure;
                    local.activo = false;
                    _units.Update(local);
                }
                
                _view.ShowMessage(string.Format("Unidad de medida {0} removida exitosamente", _view.UnitOfMeasure.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.UnitOfMeasure.isValid())
                return;

            if (!_view.UnitOfMeasure.codigo.isValid())
                return;

            try
            {
                var unit = _units.Find(_view.UnitOfMeasure.codigo);

                if(unit.isValid() && !unit.activo)
                {
                    _view.ShowMessage("Esta unidad de medida ya existe pero esta marcada como inactiva, para reactivarlo solo de click en Guardar");
                }

                if (unit == null)
                    unit = new UnidadesDeMedida() { codigo = _view.UnitOfMeasure.codigo };

                _view.Show(unit);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
