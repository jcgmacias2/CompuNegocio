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
    public class PropertyAccountPresenter
    {
        private IPropertyAccountView _view;
        private ICuentaPredialService _account;

        public PropertyAccountPresenter(IPropertyAccountView view, ICuentaPredialService account)
        {
            _view = view;
            _account = account;

            _view.New += New;
            _view.Quit += Quit;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Delete += Delete;
            _view.Save += Save;
        }

        private void Save()
        {
            try
            {
                if (!_view.Account.cuenta.isValid())
                    throw new Exception("No existe ninguna cuenta para registrar");

                if (_view.Account.idCuentaPredial.isValid())
                    throw new Exception("Esta cuenta ya esta registrada");

                _account.Add(_view.Account);

                _view.ShowMessage("Cuenta agregada exitosamente");
                _view.Clear();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Delete()
        {
            try
            {
                if (!_view.Account.idCuentaPredial.isValid())
                    throw new Exception("No existe ninguna cuenta seleccionada");

                _account.Remove(_view.Account);

                _view.ShowMessage("Cuenta eliminada exitosamente");
                _view.Clear();
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
                IPropertyAccountsListView view;
                PropertyAccountsListPresenter presenter;

                view = new PropertyAccountsListView();
                presenter = new PropertyAccountsListPresenter(view, _account);

                view.ShowWindow();

                if (view.Account.isValid())
                    _view.Show(view.Account);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Account.cuenta.isValid())
                return;

            try
            {
                var account = _account.Find(_view.Account.cuenta);
                if (account.isValid())
                    _view.Show(account);
                else
                    _view.Show(new CuentasPrediale() { cuenta = _view.Account.cuenta });
                    
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
    }
}
