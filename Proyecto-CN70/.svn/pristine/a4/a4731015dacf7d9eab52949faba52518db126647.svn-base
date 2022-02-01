using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeOrdenDeCompra
    {
        public decimal Surtido { get; set; }
        public decimal Pendiente { get { return Cantidad - surtidoEnOtros - Surtido; } }
        public string CodigoArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public int idUnidadArticulo { get; set; }
        public UnidadesDeMedida UnidadDeMedida { get; set; }
        public string DescripcionArticulo { get; set; }
        public decimal CostoUnitario { get; set; }
        public ICollection<Impuesto> Impuestos { get; set; }
        public decimal Importe { get; set; }
        public string Comentario { get; set; }
        public string ImpuestosEnLetra { get; set; }
        public decimal surtidoEnOtros { get; set; }
        public int idOrdenDeCompra { get; set; }
        public int idDetalleOrdenDeCompra { get; set; }
        public int idArticulo { get; set; }

        public VMDetalleDeOrdenDeCompra(DetallesDeOrdenDeCompra detalle, List<DetallesDeCompra> compras)
        {
            this.idDetalleOrdenDeCompra = detalle.idDetalleDeOrdenDeCompra;
            this.idOrdenDeCompra = detalle.idOrdenDeCompra;
            this.CodigoArticulo = detalle.Articulo.codigo;
            this.Cantidad = detalle.cantidad;
            this.idUnidadArticulo = detalle.idUnidadDeMedida;
            this.UnidadDeMedida = detalle.UnidadesDeMedida.isValid() ? detalle.UnidadesDeMedida: null;
            this.DescripcionArticulo = detalle.Articulo.descripcion;
            this.CostoUnitario = detalle.costoUnitario;
            this.Importe = detalle.cantidad * detalle.costoUnitario;
            this.Impuestos = detalle.Impuestos.ToList();
            this.Comentario = detalle.ComentariosPorDetalleDeOrdenDeCompra.isValid() ? detalle.ComentariosPorDetalleDeOrdenDeCompra.comentario : null;
            this.idArticulo = detalle.Articulo.idArticulo;
            Impuestos.ToList().ForEach(x =>
            {
                ImpuestosEnLetra += string.Format("{0}\n", new VMImpuesto(x, this.Importe).Impresion);
            });
            Surtido = 0m;
            surtidoEnOtros = 0m;

            //Si es parte de una orden de compra ya registrada, se calcula cuantos articulos se han surtido
            if (compras.isValid() && !compras.IsEmpty())
            {
                surtidoEnOtros += compras.Sum(x => x.cantidad);
            }
        }

        public DetallesDeOrdenDeCompra ToDetalleDeOrdenDeCompra()
        {
            DetallesDeOrdenDeCompra detalle = new DetallesDeOrdenDeCompra()
            {
                idArticulo = this.idArticulo,
                idOrdenDeCompra = this.idOrdenDeCompra,
                ComentariosPorDetalleDeOrdenDeCompra = this.Comentario.isValid()?new ComentariosPorDetalleDeOrdenDeCompra() { comentario = this.Comentario, idDetalleDeOrdenDeCompra = idDetalleOrdenDeCompra}:null,
                idDetalleDeOrdenDeCompra = idDetalleOrdenDeCompra,
                cantidad = this.Cantidad,
                Impuestos = this.Impuestos,
                costoUnitario = this.CostoUnitario,
                idUnidadDeMedida = this.idUnidadArticulo,
                UnidadesDeMedida = this.UnidadDeMedida
            };

            return detalle;
        }

        public DetallesDeCompra ToDetalleDeCompra(IArticuloService _items)
        {
            DetallesDeCompra detalle = new DetallesDeCompra()
            {
                idArticulo = this.idArticulo,
                //ComentariosPorDetalleDeCompra = this.Comentario.isValid() ? new ComentariosPorDetalleDeOrdenDeCompra() { comentario = this.Comentario } : null,
                cantidad = this.Cantidad,
                //descuento = this.Descuento,
                Impuestos = this.Impuestos,
                costoUnitario = this.CostoUnitario,
                Articulo = _items.Find(idArticulo),
                idUnidadDeMedida = this.idUnidadArticulo,
                UnidadesDeMedida = this.UnidadDeMedida
            };

            return detalle;
        }

        public VMDetalleDeOrdenDeCompra()
        {
            
        }
    }
}