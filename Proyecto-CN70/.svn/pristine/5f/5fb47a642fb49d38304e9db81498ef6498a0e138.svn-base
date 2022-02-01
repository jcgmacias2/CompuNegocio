using Microsoft.Reporting.WinForms;
using System;


namespace Aprovi.Views
{
    public interface IReportViewerView : IBaseView
    {
        event Action PrintReport;
        event Action PreviewReport;

        ReportViewer Viewer { get; }
        string Printer { get; }

        void Preview();
        void Print();
    }
}
