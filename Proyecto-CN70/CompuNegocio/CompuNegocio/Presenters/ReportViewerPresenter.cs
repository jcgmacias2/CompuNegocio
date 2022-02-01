using Aprovi.Application.Helpers;
using Aprovi.Views;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ReportViewerPresenter
    {
        private readonly IReportViewerView _view;

        public ReportViewerPresenter(IReportViewerView view)
        {
            _view = view;

            _view.PrintReport += PrintReport;
            _view.PreviewReport += PreviewReport;
        }

        private void PrintReport()
        {
            AutoPrint autoPrinter;

            try
            {
                autoPrinter = new AutoPrint(_view.Viewer.LocalReport, _view.Printer);
                autoPrinter.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void PreviewReport()
        {
            try
            {
                _view.Viewer.Visible = true;
                _view.Viewer.RefreshReport();
                _view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
