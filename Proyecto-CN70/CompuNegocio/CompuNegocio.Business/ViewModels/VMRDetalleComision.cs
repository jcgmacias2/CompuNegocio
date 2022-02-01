using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleComision
    {
        public int idUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Documento { get; set; }
        public string Fecha { get; set; }
        public string Folio { get; set; }
        public string CodigoCliente { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Comision { get; set; }
        public string Moneda { get; set; }
        public string FechaCancelacion { get; set; }
        public int idMoneda { get; set; }

        public VMRDetalleComision(VMFactura factura)
        {
            idUsuario = factura.idVendedor.GetValueOrDefault(0);
            NombreUsuario = factura.Usuario1.isValid()?factura.Usuario1.nombreDeUsuario:"";
            Documento = "Factura";
            Fecha = factura.fechaHora.ToShortDateString();
            Folio = string.Format("{0}{1}", factura.serie, factura.folio);
            CodigoCliente = factura.Cliente.codigo;
            SubTotal = factura.Subtotal;
            Total = factura.Total;
            Abonado = factura.Abonado;
            idMoneda = factura.idMoneda;

            //Si aplica alguna comision
            if (!factura.Usuario1.ComisionesPorUsuarios.IsEmpty())
            {
                //Se obtienen los articulos de la factura
                List<VMDetalleDeFactura> detalle = factura.DetalleDeFactura.ToList();
                //Comisiones del vendedor
                List<ComisionesPorUsuario> comisionesVendedor = factura.Usuario1.ComisionesPorUsuarios.ToList();

                //Articulos que pagan comision al vendedor
                List<VMDetalleDeFactura> articulosComision = detalle.Where(x=>comisionesVendedor.Any(c=>c.idTipoDeComision == x.Articulo.idTipoDeComision)).ToList();

                decimal comisionesTotales = 0m;
                foreach (var articulo in articulosComision)
                {
                    VMDetalle d = new VMDetalle(articulo);
                    ComisionesPorUsuario comisionAAplicar = factura.Usuario1.ComisionesPorUsuarios.FirstOrDefault(x=>x.idTipoDeComision == articulo.Articulo.idTipoDeComision);

                    comisionesTotales += ((d.Importe/100m)*comisionAAplicar.valor);
                }

                Comision = comisionesTotales;
            }
            else
            {
                Comision = 0m;
            }

            Moneda = factura.Moneda.descripcion.Substring(0, 1);
            FechaCancelacion = factura.CancelacionesDeFactura.isValid() ? factura.CancelacionesDeFactura.fechaHora.ToShortDateString() : "";
        }

        public VMRDetalleComision(VMRemision remision)
        {
            idUsuario = remision.idVendedor.GetValueOrDefault(0);
            NombreUsuario = remision.Usuario1.isValid()?remision.Usuario1.nombreDeUsuario:"";
            Documento = "Remisión";
            Fecha = remision.fechaHora.ToShortDateString();
            Folio = remision.folio.ToString();
            CodigoCliente = remision.Cliente.codigo;
            SubTotal = remision.Subtotal;
            Total = remision.Total;
            Abonado = remision.Abonado;
            idMoneda = remision.idMoneda;

            //Si aplica alguna comision
            if (!remision.Usuario1.ComisionesPorUsuarios.IsEmpty())
            {
                //Se obtienen los articulos de la factura
                List<VMDetalleDeRemision> detalle = remision.DetalleDeRemision.ToList();
                //Comisiones del vendedor
                List<ComisionesPorUsuario> comisionesVendedor = remision.Usuario1.ComisionesPorUsuarios.ToList();

                //Articulos que pagan comision al vendedor
                List<VMDetalleDeRemision> articulosComision = detalle.Where(x => comisionesVendedor.Any(c => c.idTipoDeComision == x.Articulo.idTipoDeComision)).ToList();

                decimal comisionesTotales = 0m;
                foreach (var articulo in articulosComision)
                {
                    VMDetalle d = new VMDetalle(articulo);
                    ComisionesPorUsuario comisionAAplicar = remision.Usuario1.ComisionesPorUsuarios.FirstOrDefault(x => x.idTipoDeComision == articulo.Articulo.idTipoDeComision);

                    comisionesTotales += ((d.Importe / 100m) * comisionAAplicar.valor);
                }

                Comision = comisionesTotales;
            }
            else
            {
                Comision = 0m;
            }

            Moneda = remision.Moneda.descripcion.Substring(0, 1);
            FechaCancelacion = remision.CancelacionesDeRemisione.isValid() ? remision.CancelacionesDeRemisione.fechaHora.ToShortDateString() : "";
        }
    }
}