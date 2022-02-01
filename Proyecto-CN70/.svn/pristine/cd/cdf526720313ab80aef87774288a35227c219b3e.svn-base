using Aprovi.Business.Helpers;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.ViewModels
{
    public class VMFactura : Factura
    {
        public VMFactura() : base() 
        {
            this.Cliente = new Cliente();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorFacturas = new List<DatosExtraPorFactura>();
        }

        /// <summary>
        /// Construye una VMFactura a partir de un registro de Factura existente
        /// </summary>
        /// <param name="invoice"></param>
        public VMFactura(Factura invoice)
        {
            try
            {
                this.idFactura = invoice.idFactura;
                this.idCliente = invoice.idCliente;
                this.Cliente = invoice.Cliente;
                this.serie = invoice.serie;
                this.folio = invoice.folio;
                this.fechaHora = invoice.fechaHora;
                this.idMoneda = invoice.idMoneda;
                this.Moneda = invoice.Moneda;
                this.tipoDeCambio = invoice.tipoDeCambio;
                this.DetallesDeFacturas = invoice.DetallesDeFacturas;
                this.DetalleDeFactura = invoice.DetallesDeFacturas.ToViewModelList();
                
                //Proporciona el precio de venta para el cliente
                foreach (var d in this.DetalleDeFactura)
                {
                    d.precioUnitario = Operations.CalculatePriceWithDiscount(d.precioUnitario, d.descuento);
                    d.descuento = 0.0m; //Previene que se le recalcule descuento de nuevo
                }
                this.AbonosDeFacturas = invoice.AbonosDeFacturas;
                this.idMetodoPago = invoice.idMetodoPago;
                this.MetodosPago = invoice.MetodosPago;
                this.idEstatusDeFactura = invoice.idEstatusDeFactura;
                this.EstatusDeFactura = invoice.EstatusDeFactura;
                this.idUsuarioRegistro = invoice.idUsuarioRegistro;
                this.Usuario = invoice.Usuario;
                this.TimbresDeFactura = invoice.TimbresDeFactura;
                this.cadenaOriginal = invoice.cadenaOriginal;
                this.sello = invoice.sello;
                this.noCertificado = invoice.noCertificado;
                this.idEmpresa = invoice.idEmpresa;
                this.Empresa = invoice.Empresa;
                this.idUsoCFDI = invoice.idUsoCFDI;
                this.UsosCFDI = invoice.UsosCFDI;
                this.idRegimen = invoice.idRegimen;
                this.Regimene = invoice.Regimene;
                this.Factura1 = invoice.Factura1;
                this.idComprobanteOriginal = invoice.idComprobanteOriginal;
                this.idTipoRelacion = invoice.idTipoRelacion;
                this.TiposRelacion = invoice.TiposRelacion;
                this.ordenDeCompra = invoice.ordenDeCompra;
                this.DatosExtraPorFacturas = invoice.DatosExtraPorFacturas;
                this.CancelacionesDeFactura = invoice.CancelacionesDeFactura;
                this.NotasDeCreditoes = invoice.NotasDeCreditoes;
                this.NotasDeDescuentoes = invoice.NotasDeDescuentoes;
                this.Usuario1 = invoice.Usuario1;
                this.idEstatusCrediticio = invoice.idEstatusCrediticio;
                //this.Acreditado = invoice.NotasDeCreditoes.Sum(n => n.importe.ToDocumentCurrency(n.Moneda, invoice.Moneda, invoice.tipoDeCambio));

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
        /// <param name="serie">Serie que se le va a asignar a la factura</param>
        /// <param name="folio">Folio que se le va a asignar a la factura</param>
        public VMFactura(Cliente client, string serie, string folio, decimal tipoDeCambio)
        {
            try
            {
                this.idCliente = client.idCliente;
                this.Cliente = client;
                this.serie = serie;
                this.folio = folio.ToInt();
                this.DetallesDeFacturas = new List<DetallesDeFactura>();
                this.DetalleDeFactura = new List<VMDetalleDeFactura>();
                this.AbonosDeFacturas = new List<AbonosDeFactura>();
                this.Usuario1 = client.Usuario;

                this.fechaHora = DateTime.Now;
                this.tipoDeCambio = tipoDeCambio;
                this.Subtotal = 0.0m;
                this.Impuestos = new List<VMImpuesto>();
                this.Total = 0.0m;

                if (client.isValid())
                {
                    this.idUsoCFDI = client.idUsoCFDI.GetValueOrDefault(0);
                    this.UsosCFDI = client.UsosCFDI;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa una factura con los datos de la cotizacion proporcionada
        /// </summary>
        /// <param name="quote">Cotizacion de la que se creara la factura</param>
        public VMFactura(VMCotizacion quote)
        {
            this.Cotizaciones = new List<Cotizacione>(){quote};

            this.folio = quote.folio;
            this.fechaHora = quote.fechaHora;
            this.tipoDeCambio = quote.tipoDeCambio;
            this.ordenDeCompra = quote.numeroDePedido;
            this.idMoneda = quote.idMoneda;
            this.idCliente = quote.idCliente;
            this.idUsuarioRegistro = quote.idUsuarioRegistro;
            this.idEstatusDeFactura = (int) StatusDeFactura.Nueva;
            this.idEmpresa = quote.idEmpresa;

            //this.DetallesDeFacturas = quote.DetalleDeCotizacion.ToDetallesDeFactura().ToDetalleDeFactura();
            this.DetalleDeFactura = quote.DetalleDeCotizacion.ToDetallesDeFactura();
            this.DatosExtraPorFacturas = quote.DatosExtraPorCotizacions.Select(x => new DatosExtraPorFactura(){dato = x.dato,valor = x.valor}).ToList();

            this.Cliente = quote.Cliente;
            this.Empresa = quote.Empresa;
            this.Moneda = quote.Moneda;
            this.Usuario = quote.Usuario;

            this.UpdateAccount();
        }

        public Factura ToFactura()
        {
            try
            {
                var invoice = new Factura();
                invoice.idFactura = this.idFactura;
                invoice.idCliente = this.idCliente;
                invoice.Cliente = this.Cliente;
                invoice.serie = this.serie;
                invoice.folio = this.folio;
                invoice.fechaHora = this.fechaHora;
                invoice.idMoneda = this.idMoneda;
                invoice.Moneda = this.Moneda;
                invoice.tipoDeCambio = this.tipoDeCambio;
                invoice.DetallesDeFacturas = this.DetalleDeFactura.ToDetalleDeFactura();
                invoice.idPedido = this.idPedido;
                //Resolves Metadata Issue
                foreach (var d in invoice.DetallesDeFacturas)
                {
                    //Se recalcula el descuento
                    decimal disccountedPrice = Math.Round(d.precioUnitario, 2);
                    d.precioUnitario = Math.Round(Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, invoice.Moneda, invoice.tipoDeCambio), 2);
                    d.descuento = Operations.CalculateDiscount(d.precioUnitario, disccountedPrice);
                    d.Articulo = null;
                }
                invoice.AbonosDeFacturas = this.AbonosDeFacturas;
                invoice.idMetodoPago = this.idMetodoPago;
                invoice.MetodosPago = this.MetodosPago;
                invoice.idEstatusDeFactura = this.idEstatusDeFactura;
                invoice.EstatusDeFactura = this.EstatusDeFactura;
                invoice.idUsuarioRegistro = this.idUsuarioRegistro;
                invoice.Usuario = this.Usuario;
                invoice.TimbresDeFactura = this.TimbresDeFactura;
                invoice.noCertificado = this.noCertificado;
                invoice.cadenaOriginal = this.cadenaOriginal;
                invoice.sello = this.sello;
                invoice.idEmpresa = this.idEmpresa;
                invoice.Empresa = this.Empresa;
                invoice.idUsoCFDI = this.idUsoCFDI;
                invoice.UsosCFDI = this.UsosCFDI;
                invoice.idRegimen = this.idRegimen;
                invoice.Regimene = this.Regimene;
                invoice.idComprobanteOriginal = this.idComprobanteOriginal;
                invoice.idTipoRelacion = this.idTipoRelacion;
                invoice.ordenDeCompra = this.ordenDeCompra;
                invoice.DatosExtraPorFacturas = this.DatosExtraPorFacturas;
                invoice.idVendedor = this.idVendedor;
                invoice.Usuario1 = this.Usuario1;
                invoice.idEstatusCrediticio = this.idEstatusCrediticio;

                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleDeFactura> DetalleDeFactura { get; set; }
        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado - Acreditado; } }
        public decimal Acreditado { get; set; }
    }
}
