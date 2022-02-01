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
    public class VMOrdenDeCompra : OrdenesDeCompra
    {
        public VMOrdenDeCompra() : base() 
        {
            this.Proveedore = new Proveedore();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorOrdenDeCompras = new List<DatosExtraPorOrdenDeCompra>();
            this.Detalles = new List<VMDetalleDeOrdenDeCompra>();
        }

        /// <summary>
        /// Construye una VMOrdenDeCompra a partir de un registro de OrdenesDeCompra existente
        /// </summary>
        /// <param name="order">OrdenDeCompra existente</param>
        public VMOrdenDeCompra(OrdenesDeCompra order, List<DetallesDeCompra> detallesCompras)
        {
            try
            {
                this.idOrdenDeCompra = order.idOrdenDeCompra;
                this.Compras = order.Compras;
                this.idProveedor = order.idProveedor;
                this.Proveedore = order.Proveedore;
                this.folio = order.folio;
                this.fechaHora = order.fechaHora;
                this.idMoneda = order.idMoneda;
                this.Moneda = order.Moneda;
                this.DetallesDeOrdenDeCompras = order.DetallesDeOrdenDeCompras;
                this.Detalles = new List<VMDetalleDeOrdenDeCompra>();

                //Se convierten los detalles a VMDetalleDeOrdenDeCompra (calculo de articulos surtidos, pendientes y ordenados)
                foreach (var d in this.DetallesDeOrdenDeCompras)
                {
                    List<DetallesDeCompra> comprasDelArticulo = detallesCompras.Where(x=>x.idArticulo == d.idArticulo).ToList();

                    Detalles.Add(new VMDetalleDeOrdenDeCompra(d,comprasDelArticulo));
                }

                this.idEstatusDeOrdenDeCompra = order.idEstatusDeOrdenDeCompra;
                this.EstatusDeOrdenDeCompra = order.EstatusDeOrdenDeCompra;
                this.idUsuarioRegistro = order.idUsuarioRegistro;
                this.Usuario = order.Usuario;
                this.tipoDeCambio = order.tipoDeCambio;
                this.idEmpresa = order.idEmpresa;
                this.Empresa = order.Empresa;
                this.DatosExtraPorOrdenDeCompras = order.DatosExtraPorOrdenDeCompras;
                this.CancelacionesDeOrdenesDeCompra = order.CancelacionesDeOrdenesDeCompra;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Construye una VMOrdenDeCompra a partir de un registro de OrdenesDeCompra existente
        /// </summary>
        /// <param name="order">OrdenDeCompra existente</param>
        public VMOrdenDeCompra(OrdenesDeCompra order, List<VMDetalleDeOrdenDeCompra> detalles)
        {
            try
            {
                this.idOrdenDeCompra = order.idOrdenDeCompra;
                this.Compras = order.Compras;
                this.idProveedor = order.idProveedor;
                this.Proveedore = order.Proveedore;
                this.folio = order.folio;
                this.fechaHora = order.fechaHora;
                this.idMoneda = order.idMoneda;
                this.Moneda = order.Moneda;
                this.DetallesDeOrdenDeCompras = order.DetallesDeOrdenDeCompras;
                Detalles = detalles;

                this.idEstatusDeOrdenDeCompra = order.idEstatusDeOrdenDeCompra;
                this.EstatusDeOrdenDeCompra = order.EstatusDeOrdenDeCompra;
                this.idUsuarioRegistro = order.idUsuarioRegistro;
                this.Usuario = order.Usuario;
                this.tipoDeCambio = order.tipoDeCambio;
                this.idEmpresa = order.idEmpresa;
                this.Empresa = order.Empresa;
                this.DatosExtraPorOrdenDeCompras = order.DatosExtraPorOrdenDeCompras;
                this.CancelacionesDeOrdenesDeCompra = order.CancelacionesDeOrdenesDeCompra;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa una orden de compra integrando el proveedor y folio que se le pasa
        /// </summary>
        /// <param name="provider">Proveedor al que va a estar relacionada la orden de compra</param>
        /// <param name="folio">Folio que se le va a asignar la orden de compra</param>
        public VMOrdenDeCompra(Proveedore provider, string folio)
        {
            try
            {
                this.idProveedor = provider.idProveedor;
                this.Proveedore = provider;
                this.folio = folio.ToInt();
                this.DetallesDeOrdenDeCompras = new List<DetallesDeOrdenDeCompra>();
                this.DatosExtraPorOrdenDeCompras = new List<DatosExtraPorOrdenDeCompra>();
                this.Detalles = new List<VMDetalleDeOrdenDeCompra>();

                this.fechaHora = DateTime.Now;
                this.Subtotal = 0.0m;
                this.Impuestos = new List<VMImpuesto>();
                this.Total = 0.0m;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra ToOrdenDeCompra(IArticulosRepository items)
        {
            try
            {
                var order = new OrdenesDeCompra();
                order.idOrdenDeCompra = this.idOrdenDeCompra;
                order.idProveedor = this.idProveedor;
                order.Proveedore = this.Proveedore;
                order.folio = this.folio;
                order.fechaHora = this.fechaHora;
                order.idMoneda = this.idMoneda;
                order.Moneda = this.Moneda;
                order.tipoDeCambio = this.tipoDeCambio;
                order.DetallesDeOrdenDeCompras = this.Detalles.Select(x=>x.ToDetalleDeOrdenDeCompra()).ToList();
                //Resolves Metadata Issue
                //foreach (var d in order.DetallesDeOrdenDeCompras)
                //{
                //    d.Articulo = items.Find(d.idArticulo);
                //    d.costoUnitario = Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario,d.Articulo.Impuestos.ToList()).ToDocumentCurrency(d.Articulo.Moneda, order.Moneda, order.tipoDeCambio);
                //    d.Articulo = null;
                //}
                order.idEstatusDeOrdenDeCompra = this.idEstatusDeOrdenDeCompra;
                order.EstatusDeOrdenDeCompra = this.EstatusDeOrdenDeCompra;
                order.idUsuarioRegistro = this.idUsuarioRegistro;
                order.Usuario = this.Usuario;
                order.idEmpresa = this.idEmpresa;
                order.Empresa = this.Empresa;
                order.DatosExtraPorOrdenDeCompras = this.DatosExtraPorOrdenDeCompras;
                order.CancelacionesDeOrdenesDeCompra = this.CancelacionesDeOrdenesDeCompra;

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMCompra ToVMCompra(IArticuloService items)
        {
            VMCompra purchase = new VMCompra();

            purchase.Impuestos = this.Impuestos.Select(x=>new VMImpuesto(x)).ToList();
            purchase.Subtotal = this.Subtotal;
            purchase.Total = this.Total;
            purchase.Usuario = this.Usuario;
            purchase.Proveedore = this.Proveedore;
            purchase.idProveedor = this.idProveedor;
            //purchase.DatosExtraPorCompra = this.DatosExtraPorCompra.Select(x=>new DatosExtraPorCompra()
            //{
            //    valor = x.valor,
            //    dato = x.dato,
            //}).ToList();
            purchase.DetallesDeCompras = this.Detalles.Select(x => x.ToDetalleDeCompra(items)).ToList();
            //Resolves Metadata Issue
            //foreach (var d in invoice.DetallesDeFacturas)
            //{
            //    d.Articulo = items.Find(d.idArticulo);
            //    d.precioUnitario = Operations.CalculatePriceWithoutTaxes(d.Articulo.costoUnitario, d.Articulo.Precios.First(p => p.idListaDePrecio.Equals(this.Cliente.idListaDePrecio)).utilidad).ToDocumentCurrency(d.Articulo.Moneda, this.Moneda, this.tipoDeCambio);
            //    d.Articulo = null;
            //}
            //purchase.Empresa = this.Empresa;
            //purchase.idEmpresa = this.idEmpresa;
            //purchase.idEstatusDeCompra = (int) StatusDeCompra.Pagada;
            purchase.Moneda = this.Moneda;
            purchase.idMoneda = this.idMoneda;
            purchase.OrdenesDeCompra = this;
            purchase.idOrdenDeCompra = this.idOrdenDeCompra;
            purchase.fechaHora = this.fechaHora;
            purchase.tipoDeCambio = this.tipoDeCambio;

            return purchase;
        }

        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado; } }

        public List<VMDetalleDeOrdenDeCompra> Detalles { get; set; }
    }
}
