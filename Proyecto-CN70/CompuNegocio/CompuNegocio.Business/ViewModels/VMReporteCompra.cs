using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMReporteCompra
    {
        public int IdProveedor { get; set; }
        public string CodigoProveedor { get; set; }
        public string RazonSocialProveedor { get; set; }
        public string Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string Moneda { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }


        public VMReporteCompra(VMCompra purchase)
        {
            IdProveedor = purchase.idProveedor;
            CodigoProveedor = purchase.Proveedore.codigo;
            RazonSocialProveedor = purchase.Proveedore.razonSocial;
            Folio = purchase.folio;
            Fecha = purchase.fechaHora;
            Estado = purchase.EstatusDeCompra.descripcion;
            Moneda = purchase.Moneda.descripcion;
            Subtotal = purchase.Subtotal;
            Impuestos = purchase.Total - purchase.Subtotal;
            Total = purchase.Total;
        }
    }
}
