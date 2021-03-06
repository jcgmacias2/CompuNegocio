using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMVentasPorArticulo
    {
        public TiposDeReporteVentasPorArticulo ReportType { get; set; }
        public TiposDeFiltroVentasPorArticulo FilterType { get; set; }
        public bool IncludeInvoices { get; set; }
        public bool IncludeBillsOfSale { get; set; }
        public bool IncludeCancellations { get; set; }
        public bool IncludePorcentages { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Clasificacione Classification { get; set; }
        public Articulo Item { get; set; }
    }
}