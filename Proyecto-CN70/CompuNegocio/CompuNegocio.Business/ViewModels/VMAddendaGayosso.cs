using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    [XmlRoot(ElementName = "Orden")]
    public class VMAddendaGayosso
    {
        public string OrdenDeCompra { get; set; }
        public string NumeroRM { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public List<VMImpuestoGayosso> Impuestos { get; set; }

        public VMAddendaGayosso()
        {

        }

        public VMAddendaGayosso(VMFactura invoice, List<DatosExtraPorFactura> datos)
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

    [XmlRoot(ElementName = "Impuesto")]
    [XmlType("Impuesto")]
    public class VMImpuestoGayosso
    {
        public string Tipo { get; set; }
        public string Base { get; set; }
        public string Tasa { get; set; }
        public string Total { get; set; }

        public VMImpuestoGayosso()
        {

        }

        public VMImpuestoGayosso(VMImpuesto impuesto)
        {
            Tipo = impuesto.nombre;
            Base = impuesto.MontoGravable.ToDecimalString();
            Tasa = impuesto.valor.ToDecimalString();
            Total = impuesto.Importe.ToDecimalString();
        }

    }
}