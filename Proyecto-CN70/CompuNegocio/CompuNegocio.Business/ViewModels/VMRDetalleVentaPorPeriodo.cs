using System;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleVentaPorPeriodo
    {
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Importe { get; set; }
        public decimal ImpuestosTrasladados { get; set; }
        public decimal ImpuestosRetenidos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonos { get; set; }
        public string Moneda { get; set; }
        public string Usuario { get; set; }
        public string FechaCancelacion { get; set; }

        public VMRDetalleVentaPorPeriodo(VMFactura factura)
        {
            try
            {
                Folio = string.Format("{0}{1}{2}", "F", factura.serie, factura.folio);
                Fecha = factura.fechaHora.ToString("dd/MM/yyyy");
                Cliente = factura.Cliente.razonSocial;
                Importe = factura.Subtotal;
                ImpuestosRetenidos = factura.Impuestos.Where(x => x.idTipoDeImpuesto == (int) TipoDeImpuesto.Retenido).Sum(x => x.Importe);
                ImpuestosTrasladados = factura.Impuestos.Where(x => x.idTipoDeImpuesto == (int) TipoDeImpuesto.Trasladado).Sum(x => x.Importe);
                Total = factura.Total;
                Abonos = factura.Abonado + factura.Acreditado;
                Moneda = string.Format("{0}", factura.Moneda.descripcion[0]);
                Usuario = factura.Usuario.nombreDeUsuario;
                FechaCancelacion = factura.CancelacionesDeFactura.isValid() ? factura.CancelacionesDeFactura.fechaHora.ToString("dd/MM/yyyy") : "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMRDetalleVentaPorPeriodo(VMRemision remision)
        {
            try
            {
                Folio = string.Format("{0}{1}", "R", remision.folio);
                Fecha = remision.fechaHora.ToString("dd/MM/yyyy");
                Cliente = remision.Cliente.razonSocial;
                Importe = remision.Subtotal;
                ImpuestosRetenidos = remision.Impuestos.Where(x => x.idTipoDeImpuesto == (int) TipoDeImpuesto.Retenido).Sum(x => x.Importe);
                ImpuestosTrasladados = remision.Impuestos.Where(x => x.idTipoDeImpuesto == (int) TipoDeImpuesto.Trasladado).Sum(x => x.Importe);
                Total = remision.Total;
                Abonos = remision.Abonado;
                Moneda = string.Format("{0}", remision.Moneda.descripcion[0]);
                Usuario = remision.Usuario.nombreDeUsuario;
                FechaCancelacion = remision.CancelacionesDeRemisione.isValid() ? remision.CancelacionesDeRemisione.fechaHora.ToString("dd/MM/yyyy") : "";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}