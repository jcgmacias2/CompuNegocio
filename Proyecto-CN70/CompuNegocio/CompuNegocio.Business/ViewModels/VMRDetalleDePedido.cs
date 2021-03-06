using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleDePedido
    {
        public decimal Surtido { get; set; }
        public decimal Pendiente { get { return Cantidad - surtidoEnOtros - Surtido; } }
        public string CodigoArticulo { get; set; }
        public string CodigoArticuloCliente { get; set; }
        public decimal Cantidad { get; set; }
        public string DescripcionUnidad { get; set; }
        public string DescripcionArticulo { get; set; }
        public string Moneda { get; set; }
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
        public int folio { get; set; }

        public VMRDetalleDePedido(DetallesDePedido detalle)
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
            this.folio = detalle.Pedido.isValid() ? detalle.Pedido.folio : 0;
            this.Moneda = detalle.Pedido.isValid() ? detalle.Pedido.Moneda.descripcion.Substring(0, 1) : "";
            Impuestos.ToList().ForEach(x =>
            {
                ImpuestosEnLetra += string.Format("{0}\n", new VMImpuesto(x, Operations.CalculateTaxBase(detalle.precioUnitario, detalle.cantidad, x, detalle.Impuestos.ToList())).Impresion);
            });
            Surtido = 0m;
            surtidoEnOtros = 0m;
            //Si es parte de un pedido ya registrado, se calcula cuantos articulos se han surtido
            if (detalle.Pedido.isValid() && detalle.Pedido.idPedido.isValid())
            {
                List<DetallesDeFactura> detallePedidosFacturados = detalle.Pedido.Facturas.Where(x => x.idEstatusDeFactura != (int)StatusDeFactura.Cancelada && x.idEstatusDeFactura != (int)StatusDeFactura.Anulada).SelectMany(x => x.DetallesDeFacturas).Where(x => x.idArticulo == detalle.idArticulo).ToList();
                List<DetallesDeRemision> detallePedidosRemisionados = detalle.Pedido.Remisiones.Where(x => x.idEstatusDeRemision != (int)StatusDeRemision.Cancelada).SelectMany(x => x.DetallesDeRemisions).Where(x => x.idArticulo == detalle.idArticulo).ToList();

                surtidoEnOtros += detallePedidosFacturados.Sum(x => x.cantidad);
                surtidoEnOtros += detallePedidosRemisionados.Sum(x => x.cantidad);
            }

            //Codigos por cliente
            var codigoCliente = detalle.Articulo.CodigosDeArticuloPorClientes.FirstOrDefault(x => x.idCliente == detalle.Pedido.Cliente.idCliente);
            this.CodigoArticuloCliente = codigoCliente.isValid() ? codigoCliente.codigo : "";
        }

        public VMRDetalleDePedido()
        {

        }
    }
}