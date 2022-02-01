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

namespace Aprovi.Presenters
{
    public class SoldItemsCostReportPresenter
    {
        private ISoldItemsCostReportView _view;
        private ICostoDeLoVendidoService _soldItemsCost;

        public SoldItemsCostReportPresenter(ISoldItemsCostReportView view, ICostoDeLoVendidoService soldItemsCost)
        {
            _view = view;
            _soldItemsCost = soldItemsCost;

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

                var cost = _soldItemsCost.List(_view.Start, _view.End, _view.IncludeBillsOfSale);

                view = new ReportViewerView(Reports.FillReport(cost, _view.Start, _view.End));
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

                var cost = _soldItemsCost.List(_view.Start, _view.End, _view.IncludeBillsOfSale);

                view = new ReportViewerView(Reports.FillReport(cost, _view.Start, _view.End));
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
