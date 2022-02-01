using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using Aprovi.Application.Helpers;
using Aprovi.Views;
using Aprovi.Presenters;
using Aprovi.Views.UI;
using System.IO;

namespace Aprovi.Application.ViewModels
{
    public class VMReporte
    {
        /// <summary>
        /// Inicializa un VMReporte con lo mínimo necesario para ver un reporte
        /// </summary>
        /// <param name="archivo">Nombre del reporte a mostrar(sin ruta, ni extensión)</param>
        /// <param name="impresora">Impresora a la cual se puede mandar a imprimir</param>
        /// <param name="datos">Fuentes de datos para llenar el reporte</param>
        /// <param name="parametros">Lista de parametros a llenar en el reporte</param>
        public VMReporte(string archivo, string impresora, List<ReportDataSource> datos, List<ReportParameter> parametros)
        {
            Archivo = string.Format("{0}\\{1}.rdlc", Reports.Folder, archivo);
            Impresora = impresora;
            Datos = datos;
            Parametros = parametros;
            SubreportHandler = null;
        }

        public VMReporte(string archivo, string impresora, List<ReportDataSource> datos, List<ReportParameter> parametros, Action<object, SubreportProcessingEventArgs> subReportInit)
        {
            Archivo = string.Format("{0}\\{1}.rdlc", Reports.Folder, archivo);
            Impresora = impresora;
            Datos = datos;
            Parametros = parametros;
            SubreportHandler = subReportInit;
        }

        public string Archivo { get; set; }
        public string Impresora { get; set; }
        public List<ReportDataSource> Datos { get; set; }
        public List<ReportParameter> Parametros { get; set; }
        public Action<object, SubreportProcessingEventArgs> SubreportHandler { get; set; }

        /// <summary>
        /// Exporta a pdf el reporte en cuestión
        /// </summary>
        /// <param name="fullFilePath">Ruta física incluyendo extensión *.pdf</param>
        public void Export(string fullFilePath)
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(this);
                presenter = new ReportViewerPresenter(view);
                var report = view.Viewer.LocalReport.Render("PDF");
                File.WriteAllBytes(fullFilePath, report);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
