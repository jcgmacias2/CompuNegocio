using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMAcuse
    {
        public VMAcuse(VMFactura invoice, Configuracion config)
        {
            RazonSocial = config.razonSocial;
            Rfc = config.rfc;
            RazonSocialCliente = invoice.Cliente.razonSocial;
            RfcCliente = invoice.Cliente.rfc;
            TipoDeComprobante = "Ingreso";
            FechaExpedicion = invoice.TimbresDeFactura.fechaTimbrado;
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            UUID = invoice.TimbresDeFactura.UUID;
            Total = invoice.Total;
            FechaCancelacion = invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura.fechaHora;
            Motivo = invoice.CancelacionesDeFactura.motivo;
            Firma = invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura.acuse;
        }

        public VMAcuse(VMNotaDeCredito creditNote, Configuracion config)
        {
            RazonSocial = config.razonSocial;
            Rfc = config.rfc;
            RazonSocialCliente = creditNote.Cliente.razonSocial;
            RfcCliente = creditNote.Cliente.rfc;
            TipoDeComprobante = "Egreso";
            FechaExpedicion = creditNote.TimbresDeNotasDeCredito.fechaTimbrado;
            Folio = string.Format("{0}{1}", creditNote.serie, creditNote.folio);
            UUID = creditNote.TimbresDeNotasDeCredito.UUID;
            Total = creditNote.Total;
            FechaCancelacion = creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito.fechaHora;
            Motivo = creditNote.CancelacionesDeNotaDeCredito.motivo;
            Firma = creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito.acuse;
        }

        public VMAcuse(AbonosDeFactura fiscalPayment, Configuracion config)
        {
            RazonSocial = config.razonSocial;
            Rfc = config.rfc;
            RazonSocialCliente = fiscalPayment.Factura.Cliente.razonSocial;
            RfcCliente = fiscalPayment.Factura.Cliente.rfc;
            TipoDeComprobante = "Ingreso";
            FechaExpedicion = fiscalPayment.TimbresDeAbonosDeFactura.fechaTimbrado;
            Folio = string.Format("{0}{1}", fiscalPayment.TimbresDeAbonosDeFactura.serie, fiscalPayment.TimbresDeAbonosDeFactura.folio);
            UUID = fiscalPayment.TimbresDeAbonosDeFactura.UUID;
            Total = fiscalPayment.monto;
            FechaCancelacion = fiscalPayment.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura.fechaHora;
            Motivo = string.Empty;
            Firma = fiscalPayment.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura.acuse;
        }

        public VMAcuse(Pago payment, Configuracion config)
        {
            RazonSocial = config.razonSocial;
            Rfc = config.rfc;
            RazonSocialCliente = payment.Cliente.razonSocial;
            RfcCliente = payment.Cliente.rfc;
            TipoDeComprobante = "Ingreso";
            FechaExpedicion = payment.TimbresDePago.fechaTimbrado;
            Folio = string.Format("{0}{1}", payment.serie, payment.folio);
            UUID = payment.TimbresDePago.UUID;
            Total = payment.AbonosDeFacturas.Sum(a => a.monto.ToDocumentCurrency(a.Moneda, new Moneda(){idMoneda = (int)Monedas.Pesos }, payment.tipoDeCambio));
            FechaCancelacion = payment.TimbresDePago.CancelacionesDeTimbresDePago.fechaHora;
            Motivo = payment.CancelacionesDePago.isValid() ? payment.CancelacionesDePago.motivo : string.Empty;
            Firma = payment.TimbresDePago.CancelacionesDeTimbresDePago.acuse;
        }

        public string RazonSocial { get; set; }
        public string Rfc { get; set; }
        public string RazonSocialCliente { get; set; }
        public string RfcCliente { get; set; }
        public string TipoDeComprobante { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public string Folio { get; set; }
        public string UUID { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCancelacion { get; set; }
        public string Motivo { get; set; }
        public string Firma { get; set; }
    }
}
