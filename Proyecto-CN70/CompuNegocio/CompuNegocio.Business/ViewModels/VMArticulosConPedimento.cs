using Aprovi.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.ViewModels
{
    public class VMArticulosConPedimento
    {
        public Articulo Articulo { get; set; }

        public decimal Vendidos { get; set; }

        public decimal Asociados { get { return this.Pedimentos.Sum(p => p.Cantidad); } }

        public List<VMPedimentoAsociado> Pedimentos { get; set; }

        //Se debe poder identificar al detalle al que aplica el pedimento
        public decimal Descuento { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
