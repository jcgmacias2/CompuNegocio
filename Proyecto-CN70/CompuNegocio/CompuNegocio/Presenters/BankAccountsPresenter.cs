using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class BankAccountsPresenter
    {
        private IBankAccountsView _view;
        private ICuentaBancariaService _bankAccounts;
        private IBancoService _banks;
        private ICatalogosEstaticosService _catalogs;

        public BankAccountsPresenter(IBankAccountsView view, ICuentaBancariaService cuentas, IBancoService bancos, ICatalogosEstaticosService catalogos)
        {
            _view = view;
            _bankAccounts = cuentas;
            _banks = bancos;
            _catalogs = catalogos;

            _view.Find += Find;
            _view.New += New;
            _view.OpenList += OpenList;
            _view.Delete += Delete;
            _view.Update += Update;
            _view.Save += Save;
            _view.Quit += Quit;

            _view.FillCombos(_banks.List(), _catalogs.ListMonedas());
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

        private void Save()
        {
            try
            {
                _bankAccounts.Add(_view.Account);

                _view.ShowMessage("Cuenta {0} registrada exitosamente", _view.Account.numeroDeCuenta);
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            try
            {
                _bankAccounts.Update(_view.Account);

                _view.ShowMessage("Cuenta {0} actualizada exitosamente", _view.Account.numeroDeCuenta);
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            try
            {
                if (!_bankAccounts.CanDelete(_view.Account))
                    throw new Exception("Esta cuenta esta relacionada, por lo que no puede ser eliminada");

                _bankAccounts.Delete(_view.Account.idCuentaBancaria);

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
                IBankAccountsListView view;
                BankAccountsListPresenter presenter;

                view = new BankAccountsListView();
                presenter = new BankAccountsListPresenter(view, _bankAccounts);

                view.ShowWindow();

                if (view.Account.isValid() && view.Account.idCuentaBancaria.isValid())
                    _view.Show(view.Account);
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
            try
            {
                if (!_view.Account.numeroDeCuenta.isValid())
                    return;

                var account = _bankAccounts.Find(_view.Account.numeroDeCuenta);

                if (account.isValid())
                    _view.Show(account);
                else
                    _view.Show(new CuentasBancaria() { numeroDeCuenta = _view.Account.numeroDeCuenta });
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
