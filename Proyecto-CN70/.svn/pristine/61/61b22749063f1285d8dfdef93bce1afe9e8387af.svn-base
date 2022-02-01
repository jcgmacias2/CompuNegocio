using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeFactura : DetallesDeFactura
    {
        public VMDetalleDeFactura()
        {
            
        }

        public VMDetalleDeFactura(DetallesDeFactura detail)
        {
            this.cantidad = detail.cantidad;
            this.descuento = detail.descuento;
            this.idArticulo = detail.idArticulo;
            this.idDetalleDeFactura = detail.idDetalleDeFactura;
            this.idFactura = detail.idFactura;
            this.precioUnitario = detail.precioUnitario;

            this.Articulo = detail.Articulo;
            this.ComentariosPorDetalleDeFactura = detail.ComentariosPorDetalleDeFactura;
            this.CuentaPredialPorDetalle = detail.CuentaPredialPorDetalle;
            this.Factura = detail.Factura;
            this.Impuestos = detail.Impuestos;
            this.PedimentoPorDetalleDeFacturas = detail.PedimentoPorDetalleDeFacturas;
        }

        public DetallesDeFactura ToDetalleFactura()
        {
            DetallesDeFactura detail = new DetallesDeFactura();

            detail.cantidad = this.cantidad;
            detail.descuento = this.descuento;
            detail.idArticulo = this.idArticulo;
            detail.idDetalleDeFactura = this.idDetalleDeFactura;
            detail.idFactura = this.idFactura;
            detail.precioUnitario = this.precioUnitario;

            detail.Articulo = this.Articulo;
            detail.ComentariosPorDetalleDeFactura = this.ComentariosPorDetalleDeFactura;
            detail.CuentaPredialPorDetalle = this.CuentaPredialPorDetalle;
            detail.Factura = this.Factura;
            detail.Impuestos = this.Impuestos;
            detail.PedimentoPorDetalleDeFacturas = this.PedimentoPorDetalleDeFacturas;

            return detail;
        }

        public VMDetalleDeNotaDeCredito ToDetalleDeNotaDeCredito()
        {
            VMDetalleDeNotaDeCredito detail = new VMDetalleDeNotaDeCredito();

            detail.Articulo = this.Articulo;
            detail.Impuestos = this.Impuestos;
            detail.cantidad = this.cantidad;
            detail.precioUnitario = this.precioUnitario;
            detail.idArticulo = this.idArticulo;

            return detail;
        }
    }
}