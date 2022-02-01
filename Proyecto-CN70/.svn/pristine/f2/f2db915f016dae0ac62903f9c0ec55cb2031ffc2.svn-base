using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class QuotesPerPeriodReportPresenter
    {
        private IQuotesPerPeriodReportView _view;
        private ICotizacionService _quotes;
        private IClienteService _customers;

        public QuotesPerPeriodReportPresenter(IQuotesPerPeriodReportView view, ICotizacionService quotes, IClienteService customers)
        {
            _view = view;
            _quotes = quotes;
            _customers = customers;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            _view.OpenCustomersList += OpenCustomersList;
            _view.FindCustomer += FindCustomer;
            
            DateTime todayDate = DateTime.Today;
            DateTime currentMonth = new DateTime(todayDate.Year,todayDate.Month,1);
            _view.SetDates(currentMonth, todayDate);
        }

        private void FindCustomer()
        {
            try
            {
                var customer = _customers.Find(_view.Customer.codigo);

                if (customer.isValid())
                {
                    _view.Show(customer);
                }
                else
                {
                    _view.ShowMessage("No se encontró un cliente con el código proporcionado");
                    _view.Show(new Cliente());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenCustomersList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                if (view.Client.isValid() && view.Client.idCliente.isValid())
                {
                    _view.Show(view.Client);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;
                List<VMRDetalleDeCotizacion> quotes = _view.Customer.isValid() && _view.Customer.idCliente.isValid()
                    ? _quotes.GetDetailsForReport(_view.StartDate, _view.EndDate, _view.Customer, _view.OnlySalePending)
                    : _quotes.GetDetailsForReport(_view.StartDate, _view.EndDate, _view.OnlySalePending);

                view = new ReportViewerView(Reports.FillReport(quotes,_view.StartDate,_view.EndDate));
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                List<VMRDetalleDeCotizacion> quotes = _view.Customer.isValid() && _view.Customer.idCliente.isValid()
                    ? _quotes.GetDetailsForReport(_view.StartDate, _view.EndDate, _view.Customer, _view.OnlySalePending)
                    : _quotes.GetDetailsForReport(_view.StartDate, _view.EndDate, _view.OnlySalePending);

                view = new ReportViewerView(Reports.FillReport(quotes, _view.StartDate, _view.EndDate));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
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
    }
}
