using Aprovi.Data.Models;
using System;

namespace Aprovi.Business.ViewModels
{
    public class VMParcialidad
    {
        public VMParcialidad()
        {

        }

        public VMParcialidad(AbonosDeFactura abono)
        {
            this.IdAbono = abono.idAbonoDeFactura;
            this.Serie = abono.TimbresDeAbonosDeFactura.serie;
            this.Folio = abono.TimbresDeAbonosDeFactura.folio.ToString();
            this.SerieFactura = abono.Factura.serie;
            this.FolioFactura = abono.Factura.folio.ToString();
            this.RazonSocial = abono.Factura.Cliente.razonSocial;
            this.Importe = abono.monto;
            this.Moneda = abono.Moneda.descripcion;
            this.Fecha = abono.fechaHora;
        }

        public int IdAbono { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }
        public string SerieFactura { get; set; }
        public string FolioFactura { get; set; }
        public string RazonSocial { get; set; }
        public decimal Importe { get; set; }
        public string Moneda { get; set; }
        public DateTime Fecha { get; set; }
    }
}
