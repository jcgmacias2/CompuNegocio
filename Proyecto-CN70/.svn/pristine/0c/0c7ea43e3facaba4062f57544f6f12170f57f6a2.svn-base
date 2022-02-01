using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMAbonoCuentaProveedor
    {
        public VMAbonoCuentaProveedor(VwResumenPorCompra purchase, AbonosDeCompra payment)
        {
            IdCompra = purchase.idCompra;
            FolioCompra = purchase.folio;
            MonedaCompra = purchase.moneda;
            FechaCompra = purchase.fechaHora;
            EstatusCompra = purchase.estatus;
            TipoDeCambioCompra = purchase.tipoDeCambio;
            TotalCompra = purchase.subtotal.Value + purchase.impuestos.Value + purchase.cargos - purchase.descuentos;
            AbonadoCompra = purchase.abonado.Value;
            SaldoCompra = TotalCompra - AbonadoCompra;

            IdAbono = payment.idAbonoDeCompra;
            Folio = payment.folio;
            Fecha = payment.fechaHora;
            Monto = payment.monto;
            Moneda = payment.Moneda.descripcion;
            MetodoDePago = payment.FormasPago.descripcion;
            Referencia = payment.referencia;
        }

        public VMAbonoCuentaProveedor(VwResumenPorCompra purchase)
        {
            IdCompra = purchase.idCompra;
            FolioCompra = purchase.folio;
            MonedaCompra = purchase.moneda;
            FechaCompra = purchase.fechaHora;
            EstatusCompra = purchase.estatus;
            TipoDeCambioCompra = purchase.tipoDeCambio;
            TotalCompra = purchase.subtotal.Value + purchase.impuestos.Value + purchase.cargos - purchase.descuentos;
            AbonadoCompra = purchase.abonado.Value;
            SaldoCompra = TotalCompra - AbonadoCompra;

            IdAbono = -1;
            Folio = string.Empty;
            Fecha = DateTime.Now;
            Monto = 0.0m;
            Moneda = string.Empty;
            MetodoDePago = string.Empty;
            Referencia = string.Empty;
        }

        //Datos generales por los cuales se mostrara el grupo
        public int IdCompra { get; set; }
        public string FolioCompra { get; set; }
        public string MonedaCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public string EstatusCompra { get; set; }
        public decimal TipoDeCambioCompra { get; set; }
        public decimal TotalCompra { get; set; }
        public decimal AbonadoCompra { get; set; }
        public decimal SaldoCompra { get; set; }


        // Abonos que serán agrupados
        public int IdAbono { get; set; }
        public string Folio { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string MetodoDePago { get; set; }
        public string Referencia { get; set; }
      
    }
}
