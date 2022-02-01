using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMReporteTraspasos
    {
        public TiposDeReporteTraspasos ReportType { get; set; }
        public EmpresasAsociada OriginCompany { get; set; }
        public EmpresasAsociada DestinationCompany { get; set; }
        public Traspaso Transfer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}