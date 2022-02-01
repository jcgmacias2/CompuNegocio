using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMReporteRemisiones
    {
        public string Folio { get; set; }
        public string Cliente { get; set; }
        public string Fecha { get; set; }
        public string Total { get; set; }
        public string Moneda { get; set; }
        public string Factura { get; set; }
        public string Usuario { get; set; }
        public string Cancelada { get; set; }

        public VMReporteRemisiones(VMRemision remision)
        {
            Folio = remision.folio.ToString();
            Cliente = remision.Cliente.razonSocial;
            Fecha = remision.fechaHora.ToString("dd/MM/yyyy");
            Moneda = remision.Moneda.descripcion;
            Factura = remision.Factura.isValid() ? string.Format("{0}{1}",remision.Factura.serie,remision.Factura.folio) : "";
            Usuario = remision.Usuario.nombreDeUsuario;
            Cancelada = remision.CancelacionesDeRemisione.isValid() ? remision.CancelacionesDeRemisione.fechaHora.ToString("dd/MM/yyyy") : "";

            remision.UpdateAccount();

            Total = remision.Total.ToDecimalString();
        }
    }
}
