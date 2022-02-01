using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ItemsCustomsApplicationPresenter
    {
        private IItemsCustomsApplicationView _view;
        private IAjusteService _adjustments;
        private IPedimentoService _customsApplications;

        public ItemsCustomsApplicationPresenter(IItemsCustomsApplicationView view, IAjusteService adjustments, IPedimentoService customsApplications)
        {
            _view = view;
            _adjustments = adjustments;
            _customsApplications = customsApplications;

            _view.Add += Add;
            _view.Remove += Remove;
            _view.Save += Save;
        }

        private void Save()
        {
            try
            {
                var customApplications = _view.CustomsApplications;
                //Registro un ajuste por cada pedimento que se haya pasado
                foreach (var p in customApplications)
                {
                    var a = _adjustments.GenerateEntrance(_view.Item, p);
                    a.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                    _adjustments.Add(a);
                }

                _view.ShowMessage("Ajustes registrados existosamente");
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Remove()
        {
            try
            {
                var selected = _view.Selected;

                if (!selected.isValid())
                    return;

                var customsApplications = _view.CustomsApplications;
                customsApplications.Remove(selected);

                _view.Show(customsApplications);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Add()
        {
            try
            {
                if (!_view.Current.añoOperacion.isValid() || _view.Current.añoOperacion.Length != 2)
                    throw new Exception("El año de operación no es válido");

                if (!_view.Current.aduana.isValid() || _view.Current.aduana.Length < 2)
                    throw new Exception("La aduana no es válida");

                if (!_view.Current.patente.isValid() || _view.Current.patente.Length < 4)
                    throw new Exception("La patente no es válida");

                if (!_view.Current.añoEnCurso.isValid() || _view.Current.añoEnCurso.Length != 1)
                    throw new Exception("El año en curso no es válido");

                if (!_view.Current.progresivo.isValid() || _view.Current.progresivo.Length < 5)
                    throw new Exception("El número progresivo no es válido");

                if (!_view.Current.Unidades.isValid())
                    throw new Exception("La cantidad de unidades no es válida");

                var customsApplications = _view.CustomsApplications;

                customsApplications.Add(_view.Current);

                _view.Show(customsApplications);
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
