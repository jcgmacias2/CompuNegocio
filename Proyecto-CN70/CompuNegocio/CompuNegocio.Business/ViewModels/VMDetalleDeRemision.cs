using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeRemision : DetallesDeRemision
    {
        public VMDetalleDeRemision()
        {
            
        }

        public VMDetalleDeRemision(DetallesDeRemision detail)
        {
            this.precioUnitario = detail.precioUnitario;
            this.cantidad = detail.cantidad;
            this.descuento = detail.descuento;
            this.idArticulo = detail.idArticulo;
            this.idDetalleDeRemision = detail.idDetalleDeRemision;
            this.idRemision = detail.idRemision;

            this.Articulo = detail.Articulo;
            this.ComentariosPorDetalleDeRemision = detail.ComentariosPorDetalleDeRemision;
            this.PedimentoPorDetalleDeRemisions = detail.PedimentoPorDetalleDeRemisions;
            this.Impuestos = detail.Impuestos;
            this.Remisione = detail.Remisione;
        }

        public VMDetalleDeFactura ToDetalleDeFactura()
        {
            VMDetalleDeFactura detail = new VMDetalleDeFactura();

            detail.precioUnitario = this.precioUnitario;
            detail.cantidad = this.cantidad;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;

            detail.Articulo = this.Articulo;
            detail.ComentariosPorDetalleDeFactura = this.ComentariosPorDetalleDeRemision.isValid() ? new ComentariosPorDetalleDeFactura(){comentario = this.ComentariosPorDetalleDeRemision.comentario} : null;
            detail.PedimentoPorDetalleDeFacturas = this.PedimentoPorDetalleDeRemisions.ToPedimentosFactura();
            detail.Impuestos = this.Impuestos;

            return detail;
        }

        public DetallesDeRemision ToDetalleDeRemision()
        {
            DetallesDeRemision detail = new DetallesDeRemision();

            detail.idDetalleDeRemision = this.idDetalleDeRemision;
            detail.idRemision = this.idRemision;
            detail.precioUnitario = this.precioUnitario;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;
            detail.cantidad = this.cantidad;

            detail.Articulo = this.Articulo;
            detail.Remisione = this.Remisione;
            detail.ComentariosPorDetalleDeRemision = this.ComentariosPorDetalleDeRemision;
            detail.Impuestos = this.Impuestos;
            detail.PedimentoPorDetalleDeRemisions = this.PedimentoPorDetalleDeRemisions;

            return detail;
        }
    }
}