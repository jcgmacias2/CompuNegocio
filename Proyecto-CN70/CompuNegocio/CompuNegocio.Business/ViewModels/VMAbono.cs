using Aprovi.Data.Models;
using System;

namespace Aprovi.Business.ViewModels
{
    public class VMAbono
    {
        public VMAbono(VMNotaDeCredito payment)
        {
            Folio = payment.folio.ToInt();
            FechaAbono = payment.fechaHora;
            CodigoCliente = payment.Cliente.codigo;
            RazonSocialCliente = payment.Cliente.razonSocial;
            FechaDocumento = payment.fechaHora;
            TipoDocumento = "N";
            FolioDocumento = string.Format("{0}{1}", payment.serie, payment.folio);
            Monto = payment.Total;
            Moneda = payment.Moneda.descripcion.Substring(0, 1);
            UsuarioRegistro = payment.Usuario.nombreDeUsuario;
            Estatus = payment.EstatusDeNotaDeCredito.descripcion;
            Cancelado = payment.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Cancelada) || payment.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Anulada);
        }

        public VMAbono(AbonosDeFactura payment)
        {
            Folio = payment.folio.ToInt();
            FechaAbono = payment.fechaHora;
            CodigoCliente = payment.Factura.Cliente.codigo;
            RazonSocialCliente = payment.Factura.Cliente.razonSocial;
            FechaDocumento = payment.Factura.fechaHora;
            TipoDocumento = "F";
            FolioDocumento = string.Format("{0}{1}", payment.Factura.serie, payment.Factura.folio);
            Monto = payment.monto;
            Moneda = payment.Moneda.descripcion.Substring(0, 1);
            UsuarioRegistro = payment.Factura.Usuario.nombreDeUsuario;
            Estatus = payment.EstatusDeAbono.descripcion;
            Cancelado = payment.idEstatusDeAbono.Equals((int)StatusDeAbono.Cancelado);
        }

        public VMAbono(AbonosDeRemision payment)
        {
            Folio = payment.folio.ToInt();
            FechaAbono = payment.fechaHora;
            CodigoCliente = payment.Remisione.Cliente.codigo;
            RazonSocialCliente = payment.Remisione.Cliente.razonSocial;
            FechaDocumento = payment.Remisione.fechaHora;
            TipoDocumento = "R";
            FolioDocumento = payment.Remisione.folio.ToString();
            Monto = payment.monto;
            Moneda = payment.Moneda.descripcion.Substring(0,1);
            UsuarioRegistro = payment.Remisione.Usuario.nombreDeUsuario;
            Estatus = payment.EstatusDeAbono.descripcion;
            Cancelado = payment.idEstatusDeAbono.Equals((int)StatusDeAbono.Cancelado);
        }

        public int Folio { get; set; }
        public DateTime FechaAbono { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocialCliente { get; set; }
        public DateTime FechaDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string FolioDocumento { get; set; }
        public decimal Monto { get; set; }
        public string Moneda { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Estatus { get; set; }
        public bool Cancelado { get; set; }
    }
}
