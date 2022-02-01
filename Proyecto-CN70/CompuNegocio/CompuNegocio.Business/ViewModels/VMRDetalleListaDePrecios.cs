using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleListaDePrecios
    {
        public int IdArticulo { get; set; }
        public decimal PrecioA { get; set; }
        public decimal PrecioB { get; set; }
        public decimal PrecioC { get; set; }
        public decimal PrecioD { get; set; }
        public int IdMoneda { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }

        public VMRDetalleListaDePrecios(VwReporteListaDePrecio vm)
        {
            IdArticulo = vm.idArticulo;
            PrecioA = vm.precioUnitarioA.GetValueOrDefault(0m);
            PrecioB = vm.precioUnitarioB.GetValueOrDefault(0m);
            PrecioC = vm.precioUnitarioC.GetValueOrDefault(0m);
            PrecioD = vm.precioUnitarioD.GetValueOrDefault(0m);
            IdMoneda = vm.idMoneda;
            CodigoArticulo = vm.codigoArticulo;
            DescripcionArticulo = vm.descripcionArticulo;
        }

        public VMRDetalleListaDePrecios(VwReporteListaDePreciosConImpuesto vm)
        {
            IdArticulo = vm.idArticulo;
            PrecioA = vm.precioUnitarioA.GetValueOrDefault(0m);
            PrecioB = vm.precioUnitarioB.GetValueOrDefault(0m);
            PrecioC = vm.precioUnitarioC.GetValueOrDefault(0m);
            PrecioD = vm.precioUnitarioD.GetValueOrDefault(0m);
            IdMoneda = vm.idMoneda;
            CodigoArticulo = vm.codigoArticulo;
            DescripcionArticulo = vm.descripcionArticulo;
        }

        public VMRDetalleListaDePrecios(VwReporteListaDePreciosPorLista vm)
        {
            IdArticulo = vm.idArticulo;
            PrecioA = vm.precioUnitario.GetValueOrDefault(0m);
            IdMoneda = vm.idMoneda;
            CodigoArticulo = vm.codigoArticulo;
            DescripcionArticulo = vm.descripcionArticulo;
        }

        public VMRDetalleListaDePrecios(VwReporteListaDePreciosPorListaConImpuesto vm)
        {
            IdArticulo = vm.idArticulo;
            PrecioA = vm.precioUnitario.GetValueOrDefault(0m);
            IdMoneda = vm.idMoneda;
            CodigoArticulo = vm.codigoArticulo;
            DescripcionArticulo = vm.descripcionArticulo;
        }
    }
}