using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMReporteNotaDeCredito
    {
        public int IdCliente { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocialCliente { get; set; }
        public string Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Moneda { get; set; }
        public decimal Total { get; set; }
        public string Factura { get; set; }
        public string UUIDFactura { get; set; }
        public string Usuario { get; set; }


        public VMReporteNotaDeCredito(VMNotaDeCredito creditNote)
        {
            IdCliente = creditNote.idCliente;
            CodigoCliente = creditNote.Cliente.codigo;
            RazonSocialCliente = creditNote.Cliente.razonSocial;
            Folio = string.Format("{0}{1}",creditNote.serie,creditNote.folio);
            Fecha = creditNote.fechaHora;
            Estado = creditNote.EstatusDeNotaDeCredito.descripcion;
            Moneda = creditNote.Moneda.descripcion;
            Total = creditNote.Total;
            Usuario = creditNote.Usuario.nombreDeUsuario;

            if (creditNote.Factura.isValid())
            {
                Factura = string.Format("{0}{1}", creditNote.Factura.serie, creditNote.Factura.folio);
                UUIDFactura = creditNote.Factura.TimbresDeFactura.isValid() ? creditNote.Factura.TimbresDeFactura.UUID : "";
            }
        }
    }
}
