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
    public class TaxesPerPeriodReportPresenter
    {
        private ITaxesPerPeriodReportView _view;
        private IFacturaService _invoices;
        private IImpuestoService _taxes;
        private INotaDeCreditoService _creditNotes;
        private ICatalogosEstaticosService _catalogs;

        public TaxesPerPeriodReportPresenter(ITaxesPerPeriodReportView view, IFacturaService invoices, INotaDeCreditoService creditNotes, IImpuestoService taxes, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _invoices = invoices;
            _creditNotes = creditNotes;
            _taxes = taxes;
            _catalogs = catalogs;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;

            DateTime todayDate = DateTime.Today;
            DateTime currentMonth = new DateTime(todayDate.Year, todayDate.Month, 1);
            _view.SetDates(currentMonth, todayDate);
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var taxesPerPeriod = _taxes.ListTaxesPerPeriod(_view.StartDate, _view.EndDate);

                view = new ReportViewerView(Reports.FillReport(taxesPerPeriod));
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

                var taxesPerPeriod = _taxes.ListTaxesPerPeriod(_view.StartDate, _view.EndDate);

                view = new ReportViewerView(Reports.FillReport(taxesPerPeriod));
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
