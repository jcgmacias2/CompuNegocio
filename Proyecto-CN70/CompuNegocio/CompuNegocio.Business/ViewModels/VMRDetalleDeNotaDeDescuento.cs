using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleDeNotaDeDescuento
    {
        public string RazonSocialCliente { get; set; }
        public DateTime Fecha { get; set; }
        public int FolioNotaDescuento { get; set; }
        public decimal Importe { get; set; }
        public int IdMoneda { get; set; }
        public string DescripcionMoneda { get; set; }
        public string FolioFactura { get; set; }
        public DateTime? FechaFactura { get; set; }

        public VMRDetalleDeNotaDeDescuento(VwReporteNotasDeDescuento detail)
        {
            RazonSocialCliente = detail.razonSocial;
            Fecha = detail.fechaHora.GetValueOrDefault(DateTime.MinValue);
            FolioNotaDescuento = detail.folio;
            Importe = detail.monto;
            IdMoneda = detail.idMoneda;
            DescripcionMoneda = detail.descripcionMoneda;
            FolioFactura = detail.folioFactura;
            FechaFactura = detail.fechaFactura;
        }
    }
}