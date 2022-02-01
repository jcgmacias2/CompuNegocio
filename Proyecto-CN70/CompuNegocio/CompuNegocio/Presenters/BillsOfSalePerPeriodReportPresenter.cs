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
    public class BillsOfSalePerPeriodReportPresenter
    {
        private IBillsOfSalePerPeriodReportView _view;
        private IRemisionService _billsOfSale;
        private ICatalogosEstaticosService _catalogs;

        public BillsOfSalePerPeriodReportPresenter(IBillsOfSalePerPeriodReportView view, IRemisionService billsOfSale, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _catalogs = catalogs;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            
            _view.FillCombos(_catalogs.ListTiposReporteRemision());

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

                List<Remisione> remisiones = _billsOfSale.List(_view.StartDate,_view.EndDate,_view.BillOfSaleType);

                view = new ReportViewerView(Reports.FillReport(remisiones.Select(x=>new VMRemision(x)).ToList(),_view.StartDate,_view.EndDate,_view.BillOfSaleType));
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

                List<Remisione> remisiones = _billsOfSale.List(_view.StartDate, _view.EndDate, _view.BillOfSaleType);

                view = new ReportViewerView(Reports.FillReport(remisiones.Select(x => new VMRemision(x)).ToList(), _view.StartDate, _view.EndDate,_view.BillOfSaleType));
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
