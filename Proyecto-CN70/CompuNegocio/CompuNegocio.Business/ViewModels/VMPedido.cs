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
    public class VMPedido : Pedido
    {
        public VMPedido() : base() 
        {
            this.Cliente = new Cliente();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorPedidoes = new List<DatosExtraPorPedido>();
            this.Detalles = new List<VMDetalleDePedido>();
        }

        /// <summary>
        /// Construye una VMPedido a partir de un registro de Pedido existente
        /// </summary>
        /// <param name="order">Pedido existente</param>
        public VMPedido(Pedido order)
        {
            try
            {
                this.idPedido = order.idPedido;
                this.Facturas = order.Facturas;
                this.Remisiones = order.Remisiones;
                this.idCliente = order.idCliente;
                this.Cliente = order.Cliente;
                this.folio = order.folio;
                this.fechaHora = order.fechaHora;
                this.idMoneda = order.idMoneda;
                this.Moneda = order.Moneda;
                this.DetallesDePedidoes = order.DetallesDePedidoes;
                Detalles = new List<VMDetalleDePedido>();
                //Proporciona el precio de venta para el cliente
                foreach (var d in this.DetallesDePedidoes)
                {
                    VMDetalleDePedido detail = new VMDetalleDePedido(d);
                    detail.PrecioUnitario = Operations.CalculatePriceWithDiscount(detail.PrecioUnitario, detail.Descuento);
                    detail.Descuento = 0.0m; //Previene que se le recalcule descuento de nuevo
                    Detalles.Add(detail);
                }
                this.idEstatusDePedido = order.idEstatusDePedido;
                this.EstatusDePedido = order.EstatusDePedido;
                this.idUsuarioRegistro = order.idUsuarioRegistro;
                this.Usuario = order.Usuario;
                this.tipoDeCambio = order.tipoDeCambio;
                this.idEmpresa = order.idEmpresa;
                this.Empresa = order.Empresa;
                this.DatosExtraPorPedidoes = order.DatosExtraPorPedidoes;
                this.CancelacionesDePedido = order.CancelacionesDePedido;
                this.ordenDeCompra = order.ordenDeCompra;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa un pedido integrando el cliente y folio que se le pasa
        /// </summary>
        /// <param name="client">Cliente al que va a estar relacionado el pedido</param>
        /// <param name="folio">Folio que se le va a asignar al pedido</param>
        public VMPedido(Cliente client, string folio, decimal tipoDeCambio)
        {
            try
            {
                this.idCliente = client.idCliente;
                this.Cliente = client;
                this.folio = folio.ToInt();
                this.DetallesDePedidoes = new List<DetallesDePedido>();
                this.DatosExtraPorPedidoes = new List<DatosExtraPorPedido>();
                this.Detalles = new List<VMDetalleDePedido>();

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

        public Pedido ToPedido(IArticulosRepository items)
        {
            try
            {
                var order = new Pedido();
                order.idPedido = this.idPedido;
                order.idCliente = this.idCliente;
                order.Cliente = this.Cliente;
                order.folio = this.folio;
                order.fechaHora = this.fechaHora;
                order.idMoneda = this.idMoneda;
                order.Moneda = this.Moneda;
                order.tipoDeCambio = this.tipoDeCambio;
                order.DetallesDePedidoes = this.Detalles.Select(x=>x.ToDetalleDePedido()).ToList();
                //Resolves Metadata Issue
                foreach (var d in order.DetallesDePedidoes)
                {
                    d.Articulo = items.Find(d.idArticulo);
                    //Se recalcula el descuento
                    decimal disccountedPrice = Math.Round(d.precioUnitario, 2);
                    d.precioUnitario = Math.Round(Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, order.Moneda, order.tipoDeCambio), 2);
                    d.descuento = Operations.CalculateDiscount(d.precioUnitario, disccountedPrice);
                    d.Articulo = null;
                }
                order.idEstatusDePedido = this.idEstatusDePedido;
                order.EstatusDePedido = this.EstatusDePedido;
                order.idUsuarioRegistro = this.idUsuarioRegistro;
                order.Usuario = this.Usuario;
                order.idEmpresa = this.idEmpresa;
                order.Empresa = this.Empresa;
                order.DatosExtraPorPedidoes = this.DatosExtraPorPedidoes;
                order.CancelacionesDePedido = this.CancelacionesDePedido;
                order.ordenDeCompra = this.ordenDeCompra;

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMFactura ToVMFactura(IArticuloService items)
        {
            try
            {
                VMFactura invoice = new VMFactura();

                invoice.Impuestos = this.Impuestos.Select(x => new VMImpuesto(x)).ToList();
                invoice.Subtotal = this.Subtotal;
                invoice.Total = this.Total;
                invoice.Usuario = this.Usuario;
                invoice.Cliente = this.Cliente;
                invoice.DatosExtraPorFacturas = this.DatosExtraPorPedidoes.Select(x => new DatosExtraPorFactura()
                {
                    valor = x.valor,
                    dato = x.dato,
                }).ToList();
                invoice.DetalleDeFactura = this.Detalles.Select(x => x.ToDetalleDeFactura(items)).ToList();
                //Resolves Metadata Issue
                foreach (var d in invoice.DetalleDeFactura)
                {
                    d.Articulo = items.Find(d.idArticulo);
                    d.precioUnitario = Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, this.Moneda, this.tipoDeCambio);
                    d.Articulo = null;
                }
                invoice.Empresa = this.Empresa;
                invoice.idEmpresa = this.idEmpresa;
                invoice.idEstatusDeFactura = (int)StatusDeFactura.Nueva;
                invoice.Moneda = this.Moneda;
                invoice.idMoneda = this.idMoneda;
                invoice.Pedido = this;
                invoice.idPedido = this.idPedido;
                invoice.fechaHora = this.fechaHora;
                invoice.tipoDeCambio = this.tipoDeCambio;
                invoice.ordenDeCompra = this.ordenDeCompra;

                if (this.Cliente.isValid())
                {
                    invoice.UsosCFDI = this.Cliente.UsosCFDI;
                    invoice.idUsoCFDI = this.Cliente.idUsoCFDI.GetValueOrDefault(0);
                }

                if (this.Vendedor.isValid() && this.Vendedor.idUsuario.isValid())
                {
                    invoice.idVendedor = this.Vendedor.idUsuario;
                    invoice.Usuario1 = this.Vendedor;
                }

                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMRemision ToVMRemision(IArticuloService items)
        {
            try
            {
                VMRemision billOfSale = new VMRemision();

                billOfSale.Impuestos = this.Impuestos.Select(x => new VMImpuesto(x)).ToList();
                billOfSale.Subtotal = this.Subtotal;
                billOfSale.Total = this.Total;
                billOfSale.Usuario = this.Usuario;
                billOfSale.Cliente = this.Cliente;
                billOfSale.DatosExtraPorRemisions = this.DatosExtraPorPedidoes.Select(x => new DatosExtraPorRemision()
                {
                    valor = x.valor,
                    dato = x.dato,
                }).ToList();
                billOfSale.DetalleDeRemision = this.Detalles.Select(x => x.ToDetalleDeRemision(items)).ToList();
                //Resolves Metadata Issue
                foreach (var d in billOfSale.DetalleDeRemision)
                {
                    d.Articulo = items.Find(d.idArticulo);
                    d.precioUnitario = Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, this.Moneda, this.tipoDeCambio);
                    d.Articulo = null;
                }
                billOfSale.Empresa = this.Empresa;
                billOfSale.idEmpresa = this.idEmpresa;
                billOfSale.idEstatusDeRemision = (int)StatusDeRemision.Nueva;
                billOfSale.Moneda = this.Moneda;
                billOfSale.idMoneda = this.idMoneda;
                billOfSale.Pedido = this;
                billOfSale.idPedido = this.idPedido;
                billOfSale.fechaHora = this.fechaHora;
                billOfSale.tipoDeCambio = this.tipoDeCambio;
                billOfSale.ordenDeCompra = this.ordenDeCompra;

                if (this.Vendedor.isValid() && this.Vendedor.idUsuario.isValid())
                {
                    billOfSale.idVendedor = this.Vendedor.idUsuario;
                    billOfSale.Usuario1 = this.Vendedor;
                }

                return billOfSale;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado; } }

        public List<VMDetalleDePedido> Detalles { get; set; }
        public Usuario Vendedor { get; set; }
    }
}
