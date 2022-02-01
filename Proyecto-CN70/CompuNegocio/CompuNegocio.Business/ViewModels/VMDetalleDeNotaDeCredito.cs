using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeNotaDeCredito : DetallesDeNotaDeCredito
    {
        public VMDetalleDeNotaDeCredito()
        {
            
        }

        public VMDetalleDeNotaDeCredito(DetallesDeNotaDeCredito detail)
        {
            this.cantidad = detail.cantidad;
            this.idArticulo = detail.idArticulo;
            this.idDetalleDeNotaDeCredito = detail.idDetalleDeNotaDeCredito;
            this.idNotaDeCredito = detail.idNotaDeCredito;
            this.precioUnitario = detail.precioUnitario;

            this.Articulo = detail.Articulo;
            this.NotasDeCredito = detail.NotasDeCredito;
            this.Impuestos = detail.Impuestos;
            this.PedimentoPorDetalleDeNotaDeCreditoes = detail.PedimentoPorDetalleDeNotaDeCreditoes;
        }

        public DetallesDeNotaDeCredito ToDetalleNotaDeCredito()
        {
            DetallesDeNotaDeCredito detail = new DetallesDeNotaDeCredito();

            detail.cantidad = this.cantidad;
            detail.idArticulo = this.idArticulo;
            detail.idDetalleDeNotaDeCredito = this.idDetalleDeNotaDeCredito;
            detail.idNotaDeCredito = this.idNotaDeCredito;
            detail.precioUnitario = this.precioUnitario;

            detail.Articulo = this.Articulo;
            detail.NotasDeCredito = this.NotasDeCredito;
            detail.Impuestos = this.Impuestos;
            detail.PedimentoPorDetalleDeNotaDeCreditoes = this.PedimentoPorDetalleDeNotaDeCreditoes;

            return detail;
        }
    }
}