using Aprovi.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;


namespace Aprovi.Business.ViewModels
{
    public class VMDetalle
    {
        public VMDetalle() { }
        public VMDetalle(VMDetalleDeFactura detalle)
        {
            this.ClaveProductoServicio = detalle.Articulo.ProductosServicio.codigo;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.CantidadEnLetra = detalle.cantidad.ToDecimalString();
            this.ClaveUnidad = detalle.Articulo.UnidadesDeMedida.codigo;
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.PrecioUnitario = detalle.precioUnitario;
            this.PrecioUnitarioEnLetra = detalle.precioUnitario.ToDecimalString();
            this.Importe = detalle.cantidad * detalle.precioUnitario;
            this.ImporteEnLetra = this.Importe.ToDecimalString();
            this.Impuestos = new List<VMImpuesto>();
            detalle.Impuestos.ToList().ForEach(d => this.Impuestos.Add(new VMImpuesto(d, Operations.CalculateTaxBase(detalle.precioUnitario, detalle.cantidad, d, detalle.Impuestos.ToList()))));
            this.ImpuestosEnLetra = string.Empty;
            this.Impuestos.ToList().ForEach(i => ImpuestosEnLetra += string.Format("{0}\n", i.Impresion));
            this.Comentario = detalle.ComentariosPorDetalleDeFactura.isValid() ? detalle.ComentariosPorDetalleDeFactura.comentario : null;
            this.PedimentosEnLetra = string.Join(",", detalle.PedimentoPorDetalleDeFacturas.Select(x => x.Pedimento.ToPedimentText()).ToList());

            //Codigos por cliente
            var codigoCliente = detalle.Articulo.CodigosDeArticuloPorClientes.FirstOrDefault(x => x.idCliente == detalle.Factura.Cliente.idCliente);
            this.CodigoArticuloCliente = codigoCliente.isValid() ? codigoCliente.codigo : "";
        }

        public VMDetalle(VMDetalleDeRemision detalle)
        {
            this.ClaveProductoServicio = detalle.Articulo.ProductosServicio.codigo;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.CantidadEnLetra = detalle.cantidad.ToDecimalString();
            this.ClaveUnidad = detalle.Articulo.UnidadesDeMedida.codigo;
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.PrecioUnitario = detalle.precioUnitario;
            this.PrecioUnitarioEnLetra = detalle.precioUnitario.ToDecimalString();
            this.Importe = detalle.cantidad * detalle.precioUnitario;
            this.ImporteEnLetra = this.Importe.ToDecimalString();
            this.Impuestos = new List<VMImpuesto>();
            detalle.Impuestos.ToList().ForEach(d => this.Impuestos.Add(new VMImpuesto(d, Operations.CalculateTaxBase(detalle.precioUnitario, detalle.cantidad, d, detalle.Impuestos.ToList()))));
            this.ImpuestosEnLetra = string.Empty;
            this.Impuestos.ToList().ForEach(i => ImpuestosEnLetra += string.Format("{0}\n", i.Impresion));
            this.Comentario = detalle.ComentariosPorDetalleDeRemision.isValid() ? detalle.ComentariosPorDetalleDeRemision.comentario : null;
            this.PedimentosEnLetra = string.Join(",", detalle.PedimentoPorDetalleDeRemisions.Select(x => x.Pedimento.ToPedimentText()).ToList());

            //Codigos por cliente
            var codigoCliente = detalle.Articulo.CodigosDeArticuloPorClientes.FirstOrDefault(x => x.idCliente == detalle.Remisione.Cliente.idCliente);
            this.CodigoArticuloCliente = codigoCliente.isValid() ? codigoCliente.codigo : "";
        }

        public VMDetalle(VMDetalleDeCotizacion detalle)
        {
            this.ClaveProductoServicio = detalle.Articulo.ProductosServicio.codigo;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.CantidadEnLetra = detalle.cantidad.ToDecimalString();
            this.ClaveUnidad = detalle.Articulo.UnidadesDeMedida.codigo;
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.PrecioUnitario = detalle.precioUnitario;
            this.PrecioUnitarioEnLetra = detalle.precioUnitario.ToDecimalString();
            this.Importe = detalle.cantidad * detalle.precioUnitario;
            this.ImporteEnLetra = this.Importe.ToDecimalString();
            this.Impuestos = new List<VMImpuesto>();
            detalle.Impuestos.ToList().ForEach(d => this.Impuestos.Add(new VMImpuesto(d, Operations.CalculateTaxBase(detalle.precioUnitario, detalle.cantidad, d, detalle.Impuestos.ToList()))));
            this.ImpuestosEnLetra = string.Empty;
            this.Impuestos.ToList().ForEach(i => ImpuestosEnLetra += string.Format("{0}\n", i.Impresion));
            this.Comentario = detalle.ComentariosPorDetalleDeCotizacion.isValid() ? detalle.ComentariosPorDetalleDeCotizacion.comentario : null;

            //Codigos por cliente
            var codigoCliente = detalle.Articulo.CodigosDeArticuloPorClientes.FirstOrDefault(x => x.idCliente == detalle.Cotizacione.Cliente.idCliente);
            this.CodigoArticuloCliente = codigoCliente.isValid() ? codigoCliente.codigo : "";
        }

        public VMDetalle(DetallesDePedido detalle)
        {
            this.ClaveProductoServicio = detalle.Articulo.ProductosServicio.codigo;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.CantidadEnLetra = detalle.cantidad.ToDecimalString();
            this.ClaveUnidad = detalle.Articulo.UnidadesDeMedida.codigo;
            this.DescripcionUnidad = detalle.Articulo.UnidadesDeMedida.descripcion;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.PrecioUnitario = detalle.precioUnitario;
            this.PrecioUnitarioEnLetra = detalle.precioUnitario.ToDecimalString();
            this.Importe = detalle.cantidad * detalle.precioUnitario;
            this.ImporteEnLetra = this.Importe.ToDecimalString();
            this.Impuestos = new List<VMImpuesto>();
            detalle.Impuestos.ToList().ForEach(d => this.Impuestos.Add(new VMImpuesto(d, Operations.CalculateTaxBase(detalle.precioUnitario, detalle.cantidad, d, detalle.Impuestos.ToList()))));
            this.ImpuestosEnLetra = string.Empty;
            this.Impuestos.ToList().ForEach(i => ImpuestosEnLetra += string.Format("{0}\n", i.Impresion));
            this.Comentario = detalle.ComentariosPorDetalleDePedido.isValid() ? detalle.ComentariosPorDetalleDePedido.comentario : null;
        }

        public string ClaveProductoServicio { get; set; }
        public string CodigoArticulo { get; set; }
        public string CodigoArticuloCliente { get; set; }
        public decimal Cantidad { get; set; }
        public string CantidadEnLetra { get; set; }
        public string ClaveUnidad { get; set; }
        public string DescripcionUnidad { get; set; }
        public string DescripcionArticulo { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string PrecioUnitarioEnLetra { get; set; }
        public ICollection<VMImpuesto> Impuestos { get; set; }
        public decimal Importe { get; set; }
        public string ImporteEnLetra { get; set; }
        public string ImpuestosEnLetra { get; set; }
        public string Comentario { get; set; }
        public string PedimentosEnLetra { get; set; }
    }
}
