using System.Collections.Generic;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRListaDePrecios
    {
        public Clasificacione Clasificacion { get; set; }
        public bool SoloConInventario { get; set; }
        public bool IncluirNoInventariados { get; set; }
        public bool IncluirImpuestos { get; set; }
        public Moneda Moneda { get; set; }
        public decimal TipoDeCambio { get; set; }

        public List<VMRDetalleListaDePrecios> Detalle { get; set; }

        public ReportesListaDePrecios ReportType { get; set; }
        public TiposFiltroReporteListaDePrecios FilterType { get; set; }
    }
}