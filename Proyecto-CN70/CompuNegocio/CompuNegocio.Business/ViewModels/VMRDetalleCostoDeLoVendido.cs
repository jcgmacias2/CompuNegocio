using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleCostoDeLoVendido
    {
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string CodigoCliente { get; set; }
        public string RazonSocialCliente { get; set; }
        public decimal CostoFiscal { get; set; }
        public decimal Importe { get; set; }
        public int idMonedaArticulo { get; set; }
        public int idMonedaTransacion { get; set; }
        public string Moneda { get; set; }
        public string NombreUsuario { get; set; }
        public string FechaCancelacion { get; set; }

        public VMRDetalleCostoDeLoVendido(VwReporteCostoDeLoVendido vm)
        {
            Folio = vm.folio;
            Fecha = vm.fechaTexto;
            CodigoCliente = vm.codigoCliente;
            RazonSocialCliente = vm.razonSocialCliente;
            CostoFiscal = vm.costo.GetValueOrDefault(0m);
            Importe = vm.importe.GetValueOrDefault(0m);
            idMonedaArticulo = vm.idMonedaArticulo;
            idMonedaTransacion = vm.idMonedaTransaccion;
            Moneda = vm.descripcionMoneda;
            NombreUsuario = vm.nombreUsuario;
            FechaCancelacion = vm.fechaCancelacionTexto;
        }
    }
}