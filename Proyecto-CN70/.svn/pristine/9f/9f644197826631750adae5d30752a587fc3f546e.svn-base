using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleDeOrdenDeCompra
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
        public string ImpuestosEnLetra { get; set; }
        public decimal surtidoEnOtros { get; set; }
        public int idOrdenDeCompra { get; set; }
        public int idDetalleOrdenDeCompra { get; set; }
        public int idArticulo { get; set; }

        public VMRDetalleDeOrdenDeCompra(VMDetalleDeOrdenDeCompra detalle)
        {
            this.idDetalleOrdenDeCompra = detalle.idDetalleOrdenDeCompra;
            this.idOrdenDeCompra = detalle.idOrdenDeCompra;
            this.CodigoArticulo = detalle.CodigoArticulo;
            this.Cantidad = detalle.Cantidad;
            this.DescripcionUnidad = detalle.UnidadDeMedida.descripcion;
            this.DescripcionArticulo = detalle.DescripcionArticulo;
            this.PrecioUnitario = detalle.CostoUnitario;
            this.Importe = detalle.Cantidad * detalle.CostoUnitario;
            this.Impuestos = detalle.Impuestos.ToList();
            this.Comentario = detalle.Comentario;
            this.idArticulo = detalle.idArticulo;
            ImpuestosEnLetra = detalle.ImpuestosEnLetra;
            Surtido = detalle.Surtido;
            surtidoEnOtros = detalle.surtidoEnOtros;
        }

        public VMRDetalleDeOrdenDeCompra()
        {

        }
    }
}