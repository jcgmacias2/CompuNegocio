using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMCFDIRelacionado
    {
        public VMCFDIRelacionado() { }

        public VMCFDIRelacionado(Factura factura, int idAbono, decimal saldoAnterior, decimal pagado)
        {
            this.UUID = factura.TimbresDeFactura.UUID;
            this.SerieFolio = string.Format("{0}{1}", factura.serie, factura.folio);
            this.TipoDeCambio = factura.tipoDeCambio;
            this.CodigoMetodoDePago = factura.MetodosPago.codigo;
            this.CodigoMoneda = factura.Moneda.codigo;
            this.NoDeParcialidad = (factura.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado) && a.idAbonoDeFactura < idAbono).Count() + 1).ToString();
            this.SaldoAnterior = saldoAnterior;
            this.Pagado = pagado;
            this.SaldoInsoluto = saldoAnterior - pagado;
        }

        public string UUID { get; set; }
        public string SerieFolio { get; set; }
        public decimal TipoDeCambio { get; set; }
        public string CodigoMetodoDePago { get; set; }
        public string CodigoMoneda { get; set; }
        public string NoDeParcialidad { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal Pagado { get; set; }
        public decimal SaldoInsoluto { get; set; }

    }
}
