using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleCompra
    {
        public int IdProveedor { get; set; }
        public string Folio { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public string DescripcionUnidadDeMedida { get; set; }
        public decimal CostoUnitario { get; set; }

        public VMRDetalleCompra(DetallesDeCompra detail)
        {
            IdProveedor = detail.Compra.idProveedor;
            Folio = detail.Compra.folio;
            CodigoArticulo = detail.Articulo.codigo;
            DescripcionArticulo = detail.Articulo.descripcion;
            Cantidad = detail.cantidad;
            DescripcionUnidadDeMedida = detail.UnidadesDeMedida.descripcion;
            CostoUnitario = detail.costoUnitario;
        }
    }
}