using Aprovi.Business.Helpers;
using Aprovi.Data.Models;
using System.Linq;

namespace Aprovi.Business.ViewModels
{
    public class VMPrecio : Precio
    {
        public VMPrecio()
            : base()
        {
        }

        public VMPrecio(Articulo item)
            :base()
        {
            this.Articulo = item;
            this.idArticulo = item.idArticulo;
            this.idListaDePrecio = 0;
            //this.utilidad = item.utilidad;
            this.Precio = Operations.CalculatePriceWithoutTaxes(this.Articulo.costoUnitario, this.utilidad);
            this.PrecioConImpuestos = this.Precio + Operations.CalculateTaxes(this.Precio, this.Articulo.Impuestos.ToList());
        }


        public VMPrecio(Precio price)
            : base()
        {
            this.Articulo = price.Articulo;
            this.idArticulo = price.idArticulo;
            this.idListaDePrecio = price.idListaDePrecio;
            this.ListasDePrecio = price.ListasDePrecio;
            this.utilidad = price.utilidad;
            this.Precio = Operations.CalculatePriceWithoutTaxes(this.Articulo.costoUnitario, this.utilidad);
            this.PrecioConImpuestos = this.Precio + Operations.CalculateTaxes(this.Precio, this.Articulo.Impuestos.ToList());
        }

        public Precio ToPrecio()
        {
            Precio local = new Precio();
            local.Articulo = this.Articulo;
            local.idArticulo = this.idArticulo;
            local.idListaDePrecio = this.idListaDePrecio;
            local.ListasDePrecio = this.ListasDePrecio;
            local.utilidad = this.utilidad;

            return local;
        }

        public decimal Precio { get; set; }
        public decimal PrecioConImpuestos { get; set; }
    }
}
