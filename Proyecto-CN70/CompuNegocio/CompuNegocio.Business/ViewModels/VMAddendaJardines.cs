using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    [XmlRoot(ElementName = "Orden")]
    public class VMAddendaJardines
    {
        public string OrdenDeCompra { get; set; }
        public string NumeroRM { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public List<VMImpuestoGayosso> Impuestos { get; set; }

        public VMAddendaJardines()
        { }

        public VMAddendaJardines(VMFactura invoice, List<DatosExtraPorFactura> datos)
        {
            DatosExtraPorFactura numeroRm = datos.FindDatoOrDefault(DatoExtra.NumeroRM);
            Impuestos = new List<VMImpuestoGayosso>();
            invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).ToList().ForEach(i => Impuestos.Add(new VMImpuestoGayosso(i)));
            OrdenDeCompra = invoice.ordenDeCompra;
            NumeroRM = numeroRm.isValid() ? numeroRm.valor : "";
            SubTotal = invoice.Subtotal.ToDecimalString();
            Total = invoice.Total.ToDecimalString();
        }
    }
}