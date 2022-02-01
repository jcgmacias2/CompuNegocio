using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class EquivalenciesPresenter
    {
        private IEquivalenciesView _view;
        private IEquivalenciaService _equivalencies;

        public EquivalenciesPresenter(IEquivalenciesView view, IEquivalenciaService equivalenciesService, IUnidadDeMedidaService unitsOfMeasureService)
        {
            _view = view;
            _equivalencies = equivalenciesService;

            _view.Quit += Quit;
            _view.Add += Add;
            _view.Delete += Delete;

            _view.Show(_view.Item.Equivalencias.ToList());
            _view.FillCombos(unitsOfMeasureService.List());
        }

        private void Delete()
        {
            if(!_view.CurrentEquivalency.isValid())
            {
                _view.ShowError("No hay una equivalencia seleccionada para eliminar");
                return;
            }

            try
            {
                if(_equivalencies.HasOperations(_view.CurrentEquivalency))
                {
                    _view.ShowMessage("Esta equivalencia tiene operaciones registradas y por lo tanto no es posible eliminarla");
                    return;
                }

                _equivalencies.Delete(_view.CurrentEquivalency);
                var equivalencies = _equivalencies.List(_view.Item.idArticulo);

                _view.ShowMessage("Equivalencia removida exitosamente");
                _view.Clear();
                _view.Show(equivalencies);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Add()
        {
            string error;

            if(!IsEquivalencyValid(_view.Equivalency,out error))
            {
                _view.ShowError(error);
                return;
            }

            if (_equivalencies.Find(_view.Equivalency.idArticulo, _view.Equivalency.idUnidadDeMedida) != null)
            {
                _view.ShowError("Ya existe una equivalencia para esa unidad de medida");
                return;
            }

            try
            {
                _equivalencies.Add(_view.Equivalency);
                var equivalencies = _equivalencies.List(_view.Equivalency.idArticulo);

                _view.ShowMessage("Equivalencia agregada exitosamente");
                _view.Clear();
                _view.Show(equivalencies);
            }
            catch (Exception)
            {
                throw;
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

        private bool IsEquivalencyValid(Equivalencia equivalency, out string error)
        {
            if(!equivalency.idArticulo.isValid())
            {
                error = "No tiene artículo referenciado";
                return false;
            }

            if (!equivalency.Articulo.idUnidadDeMedida.isValid())
            {
                error = "No tiene unidad de medida mínima referenciada";
                return false;
            }

            if(!equivalency.idUnidadDeMedida.isValid())
            {
                error = "No tiene unidad de medida equivalente";
                return false;
            }

            if(equivalency.Articulo.idUnidadDeMedida.Equals(equivalency.idUnidadDeMedida))
            {
                error = "No puede tener una equivalencia registrada hacia la misma unidad de medida original";
                return false;
            }

            if(!equivalency.unidades.isValid())
            {
                error = "La cantidad de unidad equivalentes no es válida";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
