using Aprovi.Business.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMCotizacion : Cotizacione
    {
        public VMCotizacion() : base() 
        {
            this.Cliente = new Cliente();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorCotizacions = new List<DatosExtraPorCotizacion>();
        }

        /// <summary>
        /// Construye una VMRemision a partir de un registro de Remisión existente
        /// </summary>
        /// <param name="billOfSale">Remisión existente</param>
        public VMCotizacion(Cotizacione quote)
        {
            try
            {
                this.idCotizacion = quote.idCotizacion;
                this.idFactura = quote.idFactura;
                this.Factura = quote.Factura;
                this.idRemision = quote.idRemision;
                this.Remisione = quote.Remisione;
                this.idCliente = quote.idCliente;
                this.Cliente = quote.Cliente;
                this.folio = quote.folio;
                this.fechaHora = quote.fechaHora;
                this.idMoneda = quote.idMoneda;
                this.Moneda = quote.Moneda;
                this.DetallesDeCotizacions = quote.DetallesDeCotizacions;
                this.DetalleDeCotizacion = quote.DetallesDeCotizacions.ToViewModel();

                //Proporciona el precio de venta para el cliente
                foreach (var d in this.DetalleDeCotizacion)
                {
                    d.precioUnitario = Operations.CalculatePriceWithDiscount(d.precioUnitario, d.descuento);
                    d.descuento = 0.0m; //Previene que se le recalcule descuento de nuevo
                }

                this.idEstatusDeCotizacion = quote.idEstatusDeCotizacion;
                this.EstatusDeCotizacion = quote.EstatusDeCotizacion;
                this.idUsuarioRegistro = quote.idUsuarioRegistro;
                this.Usuario = quote.Usuario;
                this.tipoDeCambio = quote.tipoDeCambio;
                this.idEmpresa = quote.idEmpresa;
                this.Empresa = quote.Empresa;
                this.DatosExtraPorCotizacions = quote.DatosExtraPorCotizacions;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa una cotizacion integrando el cliente y la serie y folio que se le pasa
        /// </summary>
        /// <param name="client">Cliente al que va a estar relacionada la cotizacion</param>
        /// <param name="folio">Folio que se le va a asignar a la cotizacion</param>
        public VMCotizacion(Cliente client, string folio, decimal tipoDeCambio)
        {
            try
            {
                this.idCliente = client.idCliente;
                this.Cliente = client;
                this.folio = folio.ToInt();
                this.DetallesDeCotizacions = new List<DetallesDeCotizacion>();
                this.DetalleDeCotizacion = new List<VMDetalleDeCotizacion>();
                this.DatosExtraPorCotizacions = new List<DatosExtraPorCotizacion>();

                this.fechaHora = DateTime.Now;
                this.tipoDeCambio = tipoDeCambio;
                this.Subtotal = 0.0m;
                this.Impuestos = new List<VMImpuesto>();
                this.Total = 0.0m;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione ToCotizacion(IArticulosRepository items)
        {
            try
            {
                var quote = new Cotizacione();
                quote.idCotizacion = this.idCotizacion;
                quote.idFactura = this.idFactura;
                quote.Factura = this.Factura;
                quote.idCliente = this.idCliente;
                quote.Cliente = this.Cliente;
                quote.folio = this.folio;
                quote.fechaHora = this.fechaHora;
                quote.idMoneda = this.idMoneda;
                quote.Moneda = this.Moneda;
                quote.tipoDeCambio = this.tipoDeCambio;
                quote.DetallesDeCotizacions = this.DetalleDeCotizacion.ToDetallesDeCotizacion();
                //Aqui deberia ser un objeto que no es de EF, asi que no deberia dar problemas
                //Resolves Metadata Issue
                foreach (var d in quote.DetallesDeCotizacions)
                {
                    //Se recalcula el descuento
                    decimal disccountedPrice = Math.Round(d.precioUnitario, 2);
                    d.Articulo = items.Find(d.idArticulo);
                    d.precioUnitario = Math.Round(Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, quote.Moneda, quote.tipoDeCambio), 2);
                    d.descuento = Operations.CalculateDiscount(d.precioUnitario, disccountedPrice);
                    d.Articulo = null;
                }
                quote.idEstatusDeCotizacion = this.idEstatusDeCotizacion;
                quote.EstatusDeCotizacion = this.EstatusDeCotizacion;
                quote.idUsuarioRegistro = this.idUsuarioRegistro;
                quote.Usuario = this.Usuario;
                quote.idEmpresa = this.idEmpresa;
                quote.Empresa = this.Empresa;
                quote.DatosExtraPorCotizacions = this.DatosExtraPorCotizacions;

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleDeCotizacion> DetalleDeCotizacion { get; set; }
        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
    }
}
