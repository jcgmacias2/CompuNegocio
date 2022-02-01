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
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;
using Microsoft.Reporting.WinForms;

namespace Aprovi.Presenters
{
    public class CreditNotesByPeriodReportPresenter
    {
        private ICreditNotesByPeriodReportView _view;
        private INotaDeCreditoService _creditNotes;
        private IClienteService _customers;

        public CreditNotesByPeriodReportPresenter(ICreditNotesByPeriodReportView view, IClienteService customers, INotaDeCreditoService creditNotes)
        {
            _view = view;
            _customers = customers;
            _creditNotes = creditNotes;

            _view.FindCustomer += FindCustomer;
            _view.OpenCustomersList += OpenCustomersList;
            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
        }

        private void Preview()
        {
            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var creditNotes = new List<VMNotaDeCredito>();

                if (_view.Customer.idCliente.isValid())
                    creditNotes = _creditNotes.ByPeriodAndCustomer(_view.Start, _view.End, _view.Customer);
                else
                    creditNotes = _creditNotes.ByPeriod(_view.Start, _view.End);

                VMReporte report;

                //Se genera el reporte
                report = Reports.FillReport(creditNotes, _view.Start, _view.End, _view.Customer);
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
            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var creditNotes = new List<VMNotaDeCredito>();

                if (_view.Customer.idCliente.isValid())
                    creditNotes = _creditNotes.ByPeriodAndCustomer(_view.Start, _view.End, _view.Customer);
                else
                    creditNotes = _creditNotes.ByPeriod(_view.Start, _view.End);

                VMReporte report;

                //Se genera el reporte
                report = Reports.FillReport(creditNotes, _view.Start, _view.End, _view.Customer);
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

        private void OpenCustomersList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                if (view.Client.idCliente.isValid())
                    _view.Show(view.Client);
                else
                    _view.Show(new Cliente());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindCustomer()
        {           
            try
            {
                var customer = new Cliente();

                //Si el código es válido intento buscarlo
                if (_view.Customer.codigo.isValid())
                    customer = _customers.Find(_view.Customer.codigo);

                if (customer.isValid())
                    _view.Show(customer);
                else
                    _view.Show(new Cliente());
                    
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
