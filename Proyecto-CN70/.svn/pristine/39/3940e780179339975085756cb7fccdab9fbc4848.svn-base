using Aprovi.Business.Services;
using Aprovi.Application.ViewModels;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class ClientSalesListPresenter : BaseListPresenter
    {
        private IClientSalesListView _view;
        private IClienteService _customers;
        private Cliente _customer;

        public ClientSalesListPresenter(IClientSalesListView view, IClienteService customersService) : base(view)
        {
            _view = view;
            _customers = customersService;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            try
            {
                var items = _customers.GetActiveTransactions(_view.Customer, _view.Parameter, _view.WithDebtOnly);

                _view.Show(items);

                if (items.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
