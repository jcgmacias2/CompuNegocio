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

    public class PropertyAccountsListPresenter : BaseListPresenter
    {
        private readonly IPropertyAccountsListView _view;
        private ICuentaPredialService _propertyAccounts;

        public PropertyAccountsListPresenter(IPropertyAccountsListView view, ICuentaPredialService propertyAccountsService)
            : base(view)
        {
            _view = view;
            _propertyAccounts = propertyAccountsService;

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
            List<CuentasPrediale> accounts;

            try
            {
                if (_view.Parameter.isValid())
                    accounts = _propertyAccounts.List(_view.Parameter);
                else
                    accounts = _propertyAccounts.List();

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
