using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRDetalleVentasPorArticulo
    {
        public string CodigoArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
        public string NombreClasificacion { get; set; }
        public string Notas { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string NombreCliente { get; set; }
        public string Estatus { get; set; }
        public int IdArticulo { get; set; }
        public int IdClasificacion { get; set; }
        public decimal Costo { get; set; }
        public decimal Utilidad { get; set; }
        public int IdMoneda { get; set; }
        public string NombreMoneda { get; set; }
        public int IdMonedaArticulo { get; set; }

        public VMRDetalleVentasPorArticulo(VwReporteVentasPorArticulo item, Clasificacione classification)
        {
            this.CodigoArticulo = item.codigoArticulo;
            this.NombreArticulo = item.articulo;
            this.Cantidad = item.cantidad;
            this.Precio = item.precio.GetValueOrDefault(0m);
            this.Costo = item.cantidad * item.costoUnitario;
            this.Importe = item.importe.GetValueOrDefault(0m);
            this.NombreClasificacion = classification.isValid() ? classification.descripcion : " ";
            this.Notas = item.nota;
            this.Folio = item.folioTexto;
            this.Fecha = item.fecha.ToString("dd/MM/yyyy");
            this.NombreCliente = item.cliente;
            this.Estatus = item.Estatus;
            this.IdArticulo = item.idArticulo;
            this.IdClasificacion = classification.isValid() ? classification.idClasificacion : 0;
            this.Utilidad = this.Importe - this.Costo.ToDocumentCurrency(new Moneda(){idMoneda = item.idMonedaArticulo}, new Moneda(){idMoneda = item.idMonedaTransaccion}, item.tipoDeCambio);
            this.IdMoneda = item.idMonedaTransaccion;
            this.NombreMoneda = item.descripcionMoneda;
            this.IdMonedaArticulo = item.idMonedaArticulo;
        }
    }
}