using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleAbonosFacturas
    {
        public int idFactura { get; set; }
        public string UUID { get; set; }
        public string Folio { get; set; }
        public decimal TipoCambio { get; set; }
        public string MetodoPago { get; set; }
        public int NumeroParcialidad { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string FormaDePago { get; set; }
        public string CuentaBeneficio { get; set; }
        public string Moneda { get; set; }
        public int IdMoneda { get; set; }

        public VMRDetalleAbonosFacturas(AbonosDeFactura abono, int numeroPago)
        {
            idFactura = abono.idFactura;
            UUID = abono.Factura.TimbresDeFactura.isValid() ? abono.Factura.TimbresDeFactura.UUID : "";
            Folio = string.Format("{0}{1}", abono.Factura.serie, abono.Factura.folio);
            TipoCambio = abono.tipoDeCambio;
            MetodoPago = abono.Factura.MetodosPago.codigo;
            NumeroParcialidad = numeroPago;
            FormaDePago = string.Format("{0}-{1}", abono.FormasPago.codigo, abono.FormasPago.descripcion);
            CuentaBeneficio = abono.CuentasBancaria.isValid() ? abono.CuentasBancaria.numeroDeCuenta : "";
            Moneda = abono.Moneda.descripcion;
            IdMoneda = abono.idMoneda;

            var invoice = new VMFactura(abono.Factura);
            var abonoParcial = abono.monto;
            SaldoAnterior = (invoice.Total - invoice.Abonado + abonoParcial - invoice.Acreditado).ToDocumentCurrency(abono.Factura.Moneda,abono.Moneda,abono.tipoDeCambio); 
            SaldoPagado = abono.monto;
            SaldoPendiente = (invoice.Total - invoice.Abonado - invoice.Acreditado).ToDocumentCurrency(abono.Factura.Moneda,abono.Moneda, abono.tipoDeCambio);
        }
    }
}