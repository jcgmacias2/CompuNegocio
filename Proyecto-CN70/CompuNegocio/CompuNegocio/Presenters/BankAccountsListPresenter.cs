using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Presenters
{
    public class BankAccountsListPresenter : BaseListPresenter
    {
        private readonly IBankAccountsListView _view;
        private ICuentaBancariaService _bankAccounts;

        public BankAccountsListPresenter(IBankAccountsListView view, ICuentaBancariaService banksAccountsService)
            : base(view)
        {
            _view = view;
            _bankAccounts = banksAccountsService;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<CuentasBancaria> accounts;

            try
            {
                if (_view.Parameter.isValid())
                    accounts = _bankAccounts.List(_view.Parameter);
                else
                    accounts = _bankAccounts.List();

                _view.Show(accounts);

                if (accounts.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
