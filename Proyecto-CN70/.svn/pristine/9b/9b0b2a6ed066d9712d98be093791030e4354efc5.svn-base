using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMDetalleDeCompra
    {
        public VMDetalleDeCompra(DetallesDeCompra detalle)
        {
            this.Codigo = detalle.Articulo.codigo;
            this.Descripcion = detalle.Articulo.descripcion;
            this.Cantidad = detalle.cantidad;
            this.Costo = detalle.costoUnitario;
            this.Importe = this.Cantidad * this.Costo;
        }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
        public decimal Importe { get; set; }
    }
}
