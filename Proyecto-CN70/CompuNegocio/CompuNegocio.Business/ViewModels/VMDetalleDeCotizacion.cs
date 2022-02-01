using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeCotizacion : DetallesDeCotizacion
    {
        public VMDetalleDeCotizacion()
        {
            
        }

        public VMDetalleDeCotizacion(DetallesDeCotizacion detail)
        {
            this.idDetalleDeCotizacion = detail.idDetalleDeCotizacion;
            this.idArticulo = detail.idArticulo;
            this.cantidad = detail.cantidad;
            this.precioUnitario = detail.precioUnitario;
            this.idCotizacion = detail.idCotizacion;
            this.descuento = detail.descuento;

            this.Articulo = detail.Articulo;
            this.ComentariosPorDetalleDeCotizacion = detail.ComentariosPorDetalleDeCotizacion;
            this.Cotizacione = detail.Cotizacione;
            this.Impuestos = detail.Impuestos;
        }

        public DetallesDeCotizacion ToDetalleDeCotizacion()
        {
            DetallesDeCotizacion detail = new DetallesDeCotizacion();

            detail.cantidad = this.cantidad;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;
            detail.idCotizacion = this.idCotizacion;
            detail.precioUnitario = this.precioUnitario;
            detail.idDetalleDeCotizacion = this.idDetalleDeCotizacion;

            detail.Cotizacione = this.Cotizacione;
            detail.Impuestos = this.Impuestos;
            detail.Articulo = this.Articulo;
            detail.ComentariosPorDetalleDeCotizacion = this.ComentariosPorDetalleDeCotizacion;

            return detail;
        }

        public VMDetalleDeFactura ToDetalleDeFactura()
        {
            VMDetalleDeFactura detail = new VMDetalleDeFactura();

            detail.Articulo = this.Articulo;
            detail.ComentariosPorDetalleDeFactura = this.ComentariosPorDetalleDeCotizacion.isValid() ? new ComentariosPorDetalleDeFactura(){comentario = this.ComentariosPorDetalleDeCotizacion.comentario} : null;
            detail.Impuestos = this.Impuestos;
            detail.cantidad = this.cantidad;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;
            detail.precioUnitario = this.precioUnitario;

            return detail;
        }

        public VMDetalleDeRemision ToDetalleDeRemision()
        {
            VMDetalleDeRemision detail = new VMDetalleDeRemision();

            detail.Articulo = this.Articulo;
            detail.ComentariosPorDetalleDeRemision = this.ComentariosPorDetalleDeCotizacion.isValid() ? new ComentariosPorDetalleDeRemision(){comentario = this.ComentariosPorDetalleDeCotizacion.comentario} : null;
            detail.Impuestos = this.Impuestos;
            detail.cantidad = this.cantidad;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;
            detail.precioUnitario = this.precioUnitario;

            return detail;
        }
    }
}