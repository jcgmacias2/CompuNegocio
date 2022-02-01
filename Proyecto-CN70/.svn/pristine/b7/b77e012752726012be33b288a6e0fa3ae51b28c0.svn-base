using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDePedido
    {
        public decimal Surtido { get; set; }
        public decimal Pendiente { get { return Cantidad - surtidoEnOtros - Surtido; } }
        public string CodigoArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public string DescripcionUnidad { get; set; }
        public string DescripcionArticulo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public ICollection<Impuesto> Impuestos { get; set; }
        public decimal Importe { get; set; }
        public string Comentario { get; set; }
        public decimal Descuento { get; set; }
        public string ImpuestosEnLetra { get; set; }
        public decimal surtidoEnOtros { get; set; }
        public int idPedido { get; set; }
        public int idDetallePedido { get; set; }
        public int idArticulo { get; set; }

        public VMDetalleDePedido(DetallesDePedido detalle)
        {
            this.idDetallePedido = detalle.idDetalleDePedido;
            this.idPedido = detalle.idPedido;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.PrecioUnitario = detalle.precioUnitario;
            this.Importe = detalle.cantidad * detalle.precioUnitario;
            this.Impuestos = detalle.Impuestos.ToList();
            this.Comentario = detalle.ComentariosPorDetalleDePedido.isValid() ? detalle.ComentariosPorDetalleDePedido.comentario : null;
            this.Descuento = detalle.descuento;
            this.idArticulo = detalle.Articulo.idArticulo;
            Impuestos.ToList().ForEach(x =>
            {
                ImpuestosEnLetra += string.Format("{0}\n", new VMImpuesto(x, this.Importe).Impresion);
            });
            Surtido = 0m;
            surtidoEnOtros = 0m;
            //Si es parte de un pedido ya registrado, se calcula cuantos articulos se han surtido
            if (detalle.Pedido.isValid() && detalle.Pedido.idPedido.isValid())
            {
                List<DetallesDeFactura> detallePedidosFacturados = detalle.Pedido.Facturas.Where(x => x.idEstatusDeFactura != (int)StatusDeFactura.Cancelada && x.idEstatusDeFactura != (int)StatusDeFactura.Anulada).SelectMany(x => x.DetallesDeFacturas).ToList().Where(x => x.idArticulo == detalle.idArticulo && Math.Round(Operations.CalculatePriceWithDiscount(x.precioUnitario,x.descuento),2) == detalle.precioUnitario).ToList();
                List<DetallesDeRemision> detallePedidosRemisionados = detalle.Pedido.Remisiones.Where(x => x.idEstatusDeRemision != (int)StatusDeRemision.Cancelada).SelectMany(x => x.DetallesDeRemisions).ToList().Where(x => x.idArticulo == detalle.idArticulo && Math.Round(Operations.CalculatePriceWithDiscount(x.precioUnitario, x.descuento), 2) == detalle.precioUnitario).ToList();

                surtidoEnOtros += detallePedidosFacturados.Sum(x => x.cantidad);
                surtidoEnOtros += detallePedidosRemisionados.Sum(x => x.cantidad);
            }
        }

        public DetallesDePedido ToDetalleDePedido()
        {
            DetallesDePedido detalle = new DetallesDePedido()
            {
                idArticulo = this.idArticulo,
                idPedido = this.idPedido,
                ComentariosPorDetalleDePedido = this.Comentario.isValid()?new ComentariosPorDetalleDePedido() { comentario = this.Comentario, idDetalleDePedido = idDetallePedido}:null,
                idDetalleDePedido = idDetallePedido,
                cantidad = this.Cantidad,
                descuento = this.Descuento,
                Impuestos = this.Impuestos,
                precioUnitario = this.PrecioUnitario
            };

            return detalle;
        }

        public VMDetalleDeFactura ToDetalleDeFactura(IArticuloService _items)
        {
            VMDetalleDeFactura detalle = new VMDetalleDeFactura()
            {
                idArticulo = this.idArticulo,
                ComentariosPorDetalleDeFactura = this.Comentario.isValid() ? new ComentariosPorDetalleDeFactura() { comentario = this.Comentario } : null,
                cantidad = this.Cantidad,
                descuento = this.Descuento,
                Impuestos = this.Impuestos,
                precioUnitario = this.PrecioUnitario,
                Articulo = _items.Find(idArticulo)
            };

            return detalle;
        }

        public VMDetalleDeRemision ToDetalleDeRemision(IArticuloService _items)
        {
            VMDetalleDeRemision detalle = new VMDetalleDeRemision()
            {
                idArticulo = this.idArticulo,
                ComentariosPorDetalleDeRemision = this.Comentario.isValid() ? new ComentariosPorDetalleDeRemision() { comentario = this.Comentario } : null,
                cantidad = this.Cantidad,
                descuento = this.Descuento,
                Impuestos = this.Impuestos,
                precioUnitario = this.PrecioUnitario,
                Articulo = _items.Find(idArticulo)
            };

            return detalle;
        }

        public VMDetalleDePedido()
        {
            
        }
    }
}