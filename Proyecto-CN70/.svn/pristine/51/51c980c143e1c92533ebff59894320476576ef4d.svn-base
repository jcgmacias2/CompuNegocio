using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class CFDIUsesPresenter
    {
        private ICFDIUsesView _view;
        private IUsosCFDIService _uses;

        public CFDIUsesPresenter(ICFDIUsesView view, IUsosCFDIService uses)
        {
            _view = view;
            _uses = uses;

            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.New += New;
            _view.Deactivate += Deactivate;
            _view.Update += Update;
        }

        private void Update()
        {
            try
            {
                if (!_view.Use.idUsoCFDI.isValid())
                    throw new Exception("Debe seleccionar un uso antes de intentar usar esta opción");

                _uses.Reactivate(_view.Use);

                _view.ShowMessage("Uso de CFDI actualizado exitosamente");
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Deactivate()
        {
            try
            {
                if (!_view.Use.idUsoCFDI.isValid())
                    throw new Exception("Debe seleccionar un uso antes de intentar usar esta opción");

                _uses.Deactivate(_view.Use);

                _view.ShowMessage("Uso de CFDI inactivado exitosamente");
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
                ICFDIUsesListView view;
                CFDIUsesListPresenter presenter;

                view = new CFDIUsesListView();
                presenter = new CFDIUsesListPresenter(view, _uses, false);

                view.ShowWindow();

                if (view.Use.isValid() && view.Use.idUsoCFDI.isValid())
                    _view.Show(view.Use);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            try
            {
                if (!_view.Use.codigo.isValid())
                    return;

                var local = _uses.Find(_view.Use.codigo);

                if (!local.isValid() || !local.idUsoCFDI.isValid())
                    throw new Exception("No existe ningún uso con este código");

                if (!local.activo)
                    _view.ShowMessage("Este uso de CFDI se encuentra inactivo, para reactivarlo solo de click en Guardar");

                _view.Show(local);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
