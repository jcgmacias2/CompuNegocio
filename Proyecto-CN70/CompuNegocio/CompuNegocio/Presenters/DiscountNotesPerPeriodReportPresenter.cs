using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;

namespace Aprovi.Presenters
{
    public class DiscountNotesPerPeriodReportPresenter
    {
        private IDiscountNotesPerPeriodReportView _view;
        private INotaDeDescuentoService _discountNotes;
        private IClienteService _customers;

        public DiscountNotesPerPeriodReportPresenter(IDiscountNotesPerPeriodReportView view,INotaDeDescuentoService discountNotes, IClienteService customers)
        {
            _view = view;
            _discountNotes = discountNotes;
            _customers = customers;

            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
            _view.OpenCustomersList += OpenCustomersList;
            _view.FindCustomer += FindCustomer;
        }

        private void FindCustomer()
        {
            try
            {
                var reportVM = _view.Report;

                if (reportVM.Customer.isValid() && reportVM.Customer.codigo.isValid())
                {
                    var customer = _customers.Find(reportVM.Customer.codigo);

                    if (customer.isValid())
                    {
                        reportVM.Customer = customer;
                        _view.Show(reportVM);
                    }
                    else
                    {
                        //Se limpia la propiedad del cliente
                        reportVM.Customer = new Cliente();
                        _view.Show(reportVM);
                        _view.ShowError("No se encontró un cliente con el valor proporcionado");
                    }
                }
                else
                {
                    //Se limpia la propiedad de cliente
                    reportVM.Customer = new Cliente();
                    _view.Show(reportVM);
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
                    var vm = _view.Report;

                    vm.Customer = view.Client;

                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }


        private void Preview()
        {
            var reportVM = _view.Report;
            
            if (reportVM.EndDate < reportVM.StartDate)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                reportVM = _discountNotes.ListDisccountNotesForReport(reportVM.Customer, reportVM.StartDate, reportVM.EndDate, reportVM.IncludeOnlyPending, reportVM.IncludeOnlyApplied);
                report = Reports.FillReport(reportVM);

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Print()
        {
            var reportVM = _view.Report;

            if (reportVM.EndDate < reportVM.StartDate)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                reportVM = _discountNotes.ListDisccountNotesForReport(reportVM.Customer, reportVM.StartDate, reportVM.EndDate, reportVM.IncludeOnlyPending, reportVM.IncludeOnlyApplied);
                report = Reports.FillReport(reportVM);

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Print();
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
