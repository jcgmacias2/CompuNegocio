using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleAntiguedadSaldos
    {
        public string Cliente { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Vendedor { get; set; }
        public int Plazo { get; set; }
        public string Direccion { get; set; }

        public DateTime Fecha { get; set; }
        public int Tiempo { get; set; }
        public string Documento { get; set; }
        public decimal Importe { get; set; }
        public string Moneda { get; set; }
        public decimal Saldo { get; set; }

        public VMRDetalleAntiguedadSaldos(VwReporteAntiguedadSaldosFactura detail, DateTime to)
        {
            Cliente = detail.razonSocial;
            Contacto = detail.contacto;
            Telefono = detail.telefono;
            Vendedor = detail.nombreDeUsuario;
            Plazo = detail.diasCredito.GetValueOrDefault(0);
            Direccion = string.Format("{0} {1}-{2} {3} {4}, {5}", detail.calle, detail.numeroExterior, detail.numeroInterior, detail.colonia, detail.ciudad, detail.estado);

            Fecha = detail.fechaHora;
            Tiempo = (to - detail.fechaHora).Days;
            Documento = string.Format("F-{0}{1}", detail.serie, detail.folio);
            Moneda = detail.moneda;
            //Cuando una factura ha sido sustituida el comprobante original debe "saldarse" con el importe abonado
            if (detail.FacturaSustituta.HasValue)
                Importe = detail.abonado.Value;
            else
                Importe = detail.subtotal.Value + detail.impuestos.Value;
            Saldo = Importe - detail.abonado.Value;

        }

        public VMRDetalleAntiguedadSaldos(VwReporteAntiguedadSaldosRemisione detail, DateTime to)
        {
            Cliente = detail.razonSocial;
            Contacto = detail.contacto;
            Telefono = detail.telefono;
            Vendedor = detail.nombreDeUsuario;
            Plazo = detail.diasCredito.GetValueOrDefault(0);
            Direccion = string.Format("{0} {1}-{2} {3} {4}, {5}", detail.calle, detail.numeroExterior, detail.numeroInterior, detail.colonia, detail.ciudad, detail.estado);

            Fecha = detail.fechaHora;
            Tiempo = (to - detail.fechaHora).Days;
            Documento = string.Format("R-{0}", detail.folio);
            Importe = (detail.subtotal + detail.impuestos).GetValueOrDefault(0m);
            Moneda = detail.moneda;
            Saldo = (detail.subtotal + detail.impuestos - detail.abonado).GetValueOrDefault(0m);
        }
    }
}