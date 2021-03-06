using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMAbonoCuentaCliente
    {
        public VMAbonoCuentaCliente(VwResumenPorRemision billOfSale, AbonosDeRemision payment)
        {
            Id = billOfSale.idRemision;
            Documento = "Remisión";
            Folio = billOfSale.folio.ToString();
            Moneda = billOfSale.moneda;
            IdMonedaDocumento = billOfSale.idMoneda;
            Fecha = billOfSale.fechaHora;
            Estatus = billOfSale.estatus;
            TipoDeCambio = billOfSale.tipoDeCambio;
            Total = billOfSale.subtotal.Value + billOfSale.impuestos.Value;
            Abonado = billOfSale.abonado.Value;
            Saldo = Total - Abonado;

            IdAbono = payment.idAbonoDeRemision;
            FolioAbono = payment.folio;
            FechaAbono = payment.fechaHora;
            MontoAbono = payment.monto;
            MonedaAbono = payment.Moneda.descripcion;
            MetodoDePagoAbono = payment.FormasPago.descripcion;
            ReferenciaAbono = string.Empty;
        }

        public VMAbonoCuentaCliente(VwResumenPorRemision billOfSale)
        {
            Id = billOfSale.idRemision;
            Documento = "Remisión";
            Folio = billOfSale.folio.ToString();
            Moneda = billOfSale.moneda;
            IdMonedaDocumento = billOfSale.idMoneda;
            Fecha = billOfSale.fechaHora;
            Estatus = billOfSale.estatus;
            TipoDeCambio = billOfSale.tipoDeCambio;
            Total = billOfSale.subtotal.Value + billOfSale.impuestos.Value;
            Abonado = billOfSale.abonado.Value;
            Saldo = Total - Abonado;

            IdAbono = -1;
            FolioAbono = string.Empty;
            FechaAbono = DateTime.Now;
            MontoAbono = 0.0m;
            MonedaAbono = string.Empty;
            MetodoDePagoAbono = string.Empty;
            ReferenciaAbono = string.Empty;
        }

        public VMAbonoCuentaCliente(VwResumenPorFactura invoice, AbonosDeFactura payment)
        {
            Id = invoice.idFactura;
            Documento = "Factura";
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            Moneda = invoice.moneda;
            IdMonedaDocumento = invoice.idMoneda;
            Fecha = invoice.fechaHora;
            Estatus = invoice.estatus;
            TipoDeCambio = invoice.tipoDeCambio;

            //Cuando una factura ha sido sustituida el comprobante original debe "saldarse" con el importe abonado
            if (invoice.FacturaSustituta.HasValue)
                Total = invoice.abonado.Value;
            else
                Total = invoice.subtotal.Value + invoice.impuestos.Value;

            Abonado = invoice.abonado.Value;
            Saldo = Total - Abonado;

            IdAbono = payment.idAbonoDeFactura;
            FolioAbono = payment.folio;
            FechaAbono = payment.fechaHora;
            MontoAbono = payment.monto;
            MonedaAbono = payment.Moneda.descripcion;
            MetodoDePagoAbono = payment.FormasPago.descripcion;
            ReferenciaAbono = payment.CuentasBancaria.isValid() ? payment.CuentasBancaria.numeroDeCuenta : string.Empty;
        }

        public VMAbonoCuentaCliente(VwResumenPorFactura invoice)
        {
            Id = invoice.idFactura;
            Documento = "Factura";
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            Moneda = invoice.moneda;
            IdMonedaDocumento = invoice.idMoneda;
            Fecha = invoice.fechaHora;
            Estatus = invoice.estatus;
            TipoDeCambio = invoice.tipoDeCambio;

            //Cuando una factura ha sido sustituida el comprobante original debe "saldarse" con el importe abonado
            if (invoice.FacturaSustituta.HasValue)
                Total = invoice.abonado.Value;
            else
                Total = invoice.subtotal.Value + invoice.impuestos.Value;

            Abonado = invoice.abonado.Value;
            Saldo = Total - Abonado;

            IdAbono = -1;
            FolioAbono = string.Empty;
            FechaAbono = DateTime.Now;
            MontoAbono = 0.0m;
            MonedaAbono = string.Empty;
            MetodoDePagoAbono = string.Empty;
            ReferenciaAbono = string.Empty;
        }

        public VMAbonoCuentaCliente(VwResumenPorFactura invoice, NotasDeCredito creditNote)
        {
            Id = invoice.idFactura;
            Documento = "Factura";
            Folio = string.Format("{0}{1}", invoice.serie, invoice.folio);
            Moneda = invoice.moneda;
            IdMonedaDocumento = invoice.idMoneda;
            Fecha = invoice.fechaHora;
            Estatus = invoice.estatus;
            TipoDeCambio = invoice.tipoDeCambio;

            //Cuando una factura ha sido sustituida el comprobante original debe "saldarse" con el importe abonado
            if (invoice.FacturaSustituta.HasValue)
                Total = invoice.abonado.Value;
            else
                Total = invoice.subtotal.Value + invoice.impuestos.Value;

            Abonado = invoice.abonado.Value;
            Saldo = Total - Abonado;

            IdAbono = creditNote.idNotaDeCredito;
            FolioAbono = string.Format("{0}{1}", creditNote.serie, creditNote.folio);
            FechaAbono = creditNote.fechaHora;
            MontoAbono = creditNote.importe.ToDocumentCurrency(creditNote.Moneda, new Moneda() { idMoneda = invoice.idMoneda }, invoice.tipoDeCambio);
            MonedaAbono = "NC";
            MetodoDePagoAbono = creditNote.FormasPago.descripcion;
            ReferenciaAbono = creditNote.CuentasBancaria.isValid() ? creditNote.CuentasBancaria.numeroDeCuenta : string.Empty;
        }

        //Datos generales por los cuales se mostrara el grupo
        public int Id { get; set; }
        public string Folio { get; set; }
        public string Moneda { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; }
        public decimal TipoDeCambio { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get; set; }
        public string Documento { get; set; }

        // Abonos que serán agrupados
        public int IdAbono { get; set; }
        public string FolioAbono { get; set; }
        public DateTime FechaAbono { get; set; }
        public decimal MontoAbono { get; set; }
        public string MonedaAbono { get; set; }
        public string MetodoDePagoAbono { get; set; }
        public string ReferenciaAbono { get; set; }

        public int IdMonedaDocumento { get; set; }
    }
}
