using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMFacturaConSaldo
    {
        public VMFacturaConSaldo() { }

        public VMFacturaConSaldo(VwResumenPorFactura invoice)
        {
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            IdFactura = invoice.idFactura;
            FechaHora = invoice.fechaHora;
            Cliente = invoice.razonSocial;
            Total = invoice.subtotal.Value + invoice.impuestos.Value;
            Saldo = Total - invoice.abonado.Value;
            IdMoneda = invoice.idMoneda;
            Moneda = invoice.moneda;
            TipoDeCambio = invoice.tipoDeCambio;
            Abono = null;
        }

        public VMFacturaConSaldo(VwResumenPorFactura invoice, AbonosDeFactura payment)
        {
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            IdFactura = invoice.idFactura;
            FechaHora = invoice.fechaHora;
            Cliente = invoice.razonSocial;
            Total = invoice.subtotal.Value + invoice.impuestos.Value;
            Saldo = Total - invoice.abonado.Value;
            IdMoneda = invoice.idMoneda;
            Moneda = invoice.moneda;
            TipoDeCambio = invoice.tipoDeCambio;
            Abono = payment;
        }

        public string Folio { get; set; }
        public int IdFactura { get; set; }
        public DateTime FechaHora { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal Saldo { get; set; }
        public int IdMoneda { get; set; }
        public string Moneda { get; set; }
        public AbonosDeFactura Abono { get; set; }
        public decimal TipoDeCambio { get; set; }

    }
}
