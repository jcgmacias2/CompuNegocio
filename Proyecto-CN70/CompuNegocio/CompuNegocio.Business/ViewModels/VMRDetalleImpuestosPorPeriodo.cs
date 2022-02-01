using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleImpuestosPorPeriodo
    {
        public string DescripcionImpuesto { get; set; }
        public int IdImpuesto { get; set; }
        public string DescripcionTipoDeImpuesto { get; set; }
        public int IdTipoDeImpuesto { get; set; }
        public string DescripcionTipoFactor { get; set; }
        public int IdTipoFactor { get; set; }
        public decimal ValorImpuesto { get; set; }
        public decimal BaseGravable { get; set; }
        public decimal Importe { get; set; }
    }
}