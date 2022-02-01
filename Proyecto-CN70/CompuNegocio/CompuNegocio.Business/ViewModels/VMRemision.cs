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
    public class VMRemision : Remisione
    {
        public VMRemision() : base() 
        {
            this.Cliente = new Cliente();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorRemisions = new List<DatosExtraPorRemision>();
        }

        /// <summary>
        /// Construye una VMRemision a partir de un registro de Remisión existente
        /// </summary>
        /// <param name="billOfSale">Remisión existente</param>
        public VMRemision(Remisione billOfSale)
        {
            try
            {
                this.idRemision = billOfSale.idRemision;
                this.idFactura = billOfSale.idFactura;
                this.Factura = billOfSale.Factura;
                this.idCliente = billOfSale.idCliente;
                this.Cliente = billOfSale.Cliente;
                this.folio = billOfSale.folio;
                this.fechaHora = billOfSale.fechaHora;
                this.idMoneda = billOfSale.idMoneda;
                this.Moneda = billOfSale.Moneda;
                this.DetallesDeRemisions = billOfSale.DetallesDeRemisions;
                this.DetalleDeRemision = billOfSale.DetallesDeRemisions.ToViewModelList();
                this.idVendedor = billOfSale.idVendedor;
                //Proporciona el precio de venta para el cliente
                foreach (var d in this.DetalleDeRemision)
                {
                    d.precioUnitario = Operations.CalculatePriceWithDiscount(d.precioUnitario, d.descuento);
                    d.descuento = 0.0m; //Previene que se le recalcule descuento de nuevo
                }
                this.AbonosDeRemisions = billOfSale.AbonosDeRemisions;
                this.idEstatusDeRemision = billOfSale.idEstatusDeRemision;
                this.EstatusDeRemision = billOfSale.EstatusDeRemision;
                this.idUsuarioRegistro = billOfSale.idUsuarioRegistro;
                this.Usuario = billOfSale.Usuario;
                this.tipoDeCambio = billOfSale.tipoDeCambio;
                this.idEmpresa = billOfSale.idEmpresa;
                this.Empresa = billOfSale.Empresa;
                this.DatosExtraPorRemisions = billOfSale.DatosExtraPorRemisions;
                this.CancelacionesDeRemisione = billOfSale.CancelacionesDeRemisione;
                this.Usuario1 = billOfSale.Usuario1;
                this.idEstatusCrediticio = billOfSale.idEstatusCrediticio;
                this.ordenDeCompra = billOfSale.ordenDeCompra;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa una factura integrando el cliente y la serie y folio que se le pasa
        /// </summary>
        /// <param name="client">Cliente al que va a estar relacionada la factura</param>
        /// <param name="folio">Folio que se le va a asignar a la factura</param>
        public VMRemision(Cliente client, string folio, decimal tipoDeCambio)
        {
            try
            {
                this.idCliente = client.idCliente;
                this.Cliente = client;
                this.folio = folio.ToInt();
                this.DetallesDeRemisions = new List<DetallesDeRemision>();
                this.DetalleDeRemision = new List<VMDetalleDeRemision>();
                this.AbonosDeRemisions = new List<AbonosDeRemision>();
                this.DatosExtraPorRemisions = new List<DatosExtraPorRemision>();
                this.Usuario1 = client.Usuario;

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

        /// <summary>
        /// Crea una remision con la cotizacion proporcionada
        /// </summary>
        /// <param name="quote">Cotizacion de la que se creara la remision</param>
        public VMRemision(VMCotizacion quote)
        {
            this.Cotizaciones = new List<Cotizacione>() { quote };

            this.folio = quote.folio;
            this.fechaHora = quote.fechaHora;
            this.tipoDeCambio = quote.tipoDeCambio;
            //this.numeroDePedido = quote.numeroDePedido;
            this.idMoneda = quote.idMoneda;
            this.idCliente = quote.idCliente;
            this.idUsuarioRegistro = quote.idUsuarioRegistro;
            this.idEstatusDeRemision = (int)StatusDeRemision.Nueva;
            this.idEmpresa = quote.idEmpresa;

            this.DetalleDeRemision = quote.DetalleDeCotizacion.ToDetallesDeRemision();
            this.DatosExtraPorRemisions = quote.DatosExtraPorCotizacions.Select(x => new DatosExtraPorRemision() { dato = x.dato, valor = x.valor }).ToList();

            this.Cliente = quote.Cliente;
            this.Empresa = quote.Empresa;
            this.Moneda = quote.Moneda;
            this.Usuario = quote.Usuario;

            this.UpdateAccount();
        }

        public VMRemision(VwResumenPorRemision billOfSaleSummary)
        {
            try
            {
                this.idRemision = billOfSaleSummary.idRemision;
                this.idCliente = billOfSaleSummary.idCliente;
                this.Cliente = new Cliente() { codigo = billOfSaleSummary.codigo, razonSocial = billOfSaleSummary.razonSocial };
                this.folio = billOfSaleSummary.folio;
                this.fechaHora = billOfSaleSummary.fechaHora;
                this.idMoneda = billOfSaleSummary.idMoneda;
                this.Moneda = new Moneda() { idMoneda = billOfSaleSummary.idMoneda, descripcion = billOfSaleSummary.moneda };
                this.idEstatusDeRemision = billOfSaleSummary.idEstatusDeRemision;
                this.tipoDeCambio = billOfSaleSummary.tipoDeCambio;
                this.Total = billOfSaleSummary.subtotal.Value + billOfSaleSummary.impuestos.Value;
                this.Abonado = billOfSaleSummary.abonado.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Remisione ToRemision(IArticulosRepository items)
        {
            try
            {
                var billOfSale = new Remisione();
                billOfSale.idRemision = this.idRemision;
                billOfSale.idFactura = this.idFactura;
                billOfSale.Factura = this.Factura;
                billOfSale.idCliente = this.idCliente;
                billOfSale.Cliente = this.Cliente;
                billOfSale.folio = this.folio;
                billOfSale.fechaHora = this.fechaHora;
                billOfSale.idMoneda = this.idMoneda;
                billOfSale.Moneda = this.Moneda;
                billOfSale.tipoDeCambio = this.tipoDeCambio;
                billOfSale.DetallesDeRemisions = this.DetalleDeRemision.ToDetalleDeRemision();
                billOfSale.idPedido = this.idPedido;
                billOfSale.idVendedor = this.idVendedor;
                //Resolves Metadata Issue
                foreach (var d in billOfSale.DetallesDeRemisions)
                {
                    //Se recalcula el descuento
                    decimal disccountedPrice = Math.Round(d.precioUnitario, 2);
                    d.Articulo = items.Find(d.idArticulo);
                    d.precioUnitario = Math.Round(Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, billOfSale.Moneda, billOfSale.tipoDeCambio), 2);
                    d.descuento = Operations.CalculateDiscount(d.precioUnitario, disccountedPrice);
                    d.Articulo = null;
                }
                billOfSale.AbonosDeRemisions = this.AbonosDeRemisions;
                billOfSale.idEstatusDeRemision = this.idEstatusDeRemision;
                billOfSale.EstatusDeRemision = this.EstatusDeRemision;
                billOfSale.idUsuarioRegistro = this.idUsuarioRegistro;
                billOfSale.Usuario = this.Usuario;
                billOfSale.idEmpresa = this.idEmpresa;
                billOfSale.Empresa = this.Empresa;
                billOfSale.DatosExtraPorRemisions = this.DatosExtraPorRemisions;
                billOfSale.CancelacionesDeRemisione = this.CancelacionesDeRemisione;
                billOfSale.idEstatusCrediticio = this.idEstatusCrediticio;
                billOfSale.ordenDeCompra = this.ordenDeCompra;

                return billOfSale;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleDeRemision> DetalleDeRemision { get; set; }
        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado; } }
        public bool Selected { get; set; }
    }
}
