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
    public class SalesPerPeriodReportPresenter
    {
        private ISalesPerPeriodReportView _view;
        private IRemisionService _billsOfSale;
        private IFacturaService _invoices;

        public SalesPerPeriodReportPresenter(ISalesPerPeriodReportView view, IRemisionService billsOfSale, IFacturaService invoices)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _invoices = invoices;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            
            DateTime todayDate = DateTime.Today;
            DateTime currentMonth = new DateTime(todayDate.Year,todayDate.Month,1);
            _view.SetDates(currentMonth, todayDate);
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                List<VMRDetalleVentaPorPeriodo> detallesRemisiones = new List<VMRDetalleVentaPorPeriodo>();

                if (_view.IncludeBillsOfSale)
                {
                    detallesRemisiones = _billsOfSale.GetSalesDetailsForPeriod(_view.StartDate, _view.EndDate);
                }

                List<VMRDetalleVentaPorPeriodo> detallesFacturas = _invoices.GetSalesDetailsForPeriod(_view.StartDate, _view.EndDate);

                //Se juntan los detalles de facturas y remisiones
                List<VMRDetalleVentaPorPeriodo> detalles = new List<VMRDetalleVentaPorPeriodo>();
                detalles.AddRange(detallesRemisiones);
                detalles.AddRange(detallesFacturas);

                view = new ReportViewerView(Reports.FillReport(new VMRVentasPorPeriodo(detalles, _view.StartDate, _view.EndDate)));
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

                List<VMRDetalleVentaPorPeriodo> detallesRemisiones = new List<VMRDetalleVentaPorPeriodo>();

                if (_view.IncludeBillsOfSale)
                {
                    detallesRemisiones = _billsOfSale.GetSalesDetailsForPeriod(_view.StartDate, _view.EndDate);
                }

                List<VMRDetalleVentaPorPeriodo> detallesFacturas = _invoices.GetSalesDetailsForPeriod(_view.StartDate, _view.EndDate);

                //Se juntan los detalles de facturas y remisiones
                List<VMRDetalleVentaPorPeriodo> detalles = new List<VMRDetalleVentaPorPeriodo>();
                detalles.AddRange(detallesRemisiones);
                detalles.AddRange(detallesFacturas);

                view = new ReportViewerView(Reports.FillReport(new VMRVentasPorPeriodo(detalles, _view.StartDate, _view.EndDate)));
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
