using Aprovi.Application.ViewModels;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ReportViewerView.xaml
    /// </summary>
    public partial class ReportViewerView : BaseView, IReportViewerView
    {
        public event Action PrintReport;
        public event Action PreviewReport;
        private string _printer;
        private string _filePath;

        public ReportViewerView(VMReporte report)
        {
            InitializeComponent();
            BindComponents();

            rvMainViewer.LocalReport.EnableExternalImages = true;
            rvMainViewer.LocalReport.ReportPath = report.Archivo;
            PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
            rvMainViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(permissions);
            if (report.Datos.isValid())
                report.Datos.ForEach(d => rvMainViewer.LocalReport.DataSources.Add(d));
            if(report.Parametros.isValid())
                rvMainViewer.LocalReport.SetParameters(report.Parametros);
            if (report.SubreportHandler.isValid())
                rvMainViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(report.SubreportHandler);

            _printer = report.Impresora;
            _filePath = string.Empty;
        }

        public ReportViewerView(VMReporte report, Action<object, SubreportProcessingEventArgs> subReportInit):this(report)
        {
            this.Viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(subReportInit);
        }

        private void BindComponents()
        {
            rvMainViewer.ProcessingMode = ProcessingMode.Local;
            this.SizeChanged += ReportViewerView_SizeChanged;
        }

        void ReportViewerView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            wfhContainer.Width = e.NewSize.Width;
            wfhContainer.Height = e.NewSize.Height; 
        }

        public void Preview()
        {
            if (PreviewReport.isValid())
                PreviewReport();
        }

        public void Print()
        {
            if (PrintReport.isValid())
                PrintReport();
        }

        public ReportViewer Viewer
        {
            get { return rvMainViewer; }
        }

        public string Printer
        {
            get { return _printer; }
        }

    }
}
