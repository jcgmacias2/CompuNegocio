using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleDeCotizacion
    {
        public int Folio { get; set; }
        public string Cliente { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public decimal Total { get; set; }
        public string Moneda { get; set; }
        public string Usuario { get; set; }

        public int idMoneda { get; set; }

        public VMRDetalleDeCotizacion(VwCotizacionesPorPeriodo cotizacion)
        {
            Folio = cotizacion.Folio;
            Cliente = cotizacion.Cliente;
            Fecha = cotizacion.Fecha;
            Hora = cotizacion.Hora;
            Total = cotizacion.Total.GetValueOrDefault(0m);
            Moneda = cotizacion.Moneda;
            Usuario = cotizacion.Usuario;

            idMoneda = cotizacion.idMoneda;
        }
    }
}