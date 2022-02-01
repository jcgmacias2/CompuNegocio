using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMCompra : Compra
    {
        public VMCompra() : base() { }

        public VMCompra(Compra purchase)
        {
            this.idCompra = purchase.idCompra;
            this.idProveedor = purchase.idProveedor;
            this.Proveedore = purchase.Proveedore;
            this.folio = purchase.folio;
            this.idEstatusDeCompra = purchase.idEstatusDeCompra;
            this.EstatusDeCompra = purchase.EstatusDeCompra;
            this.idOrdenDeCompra = purchase.idOrdenDeCompra;
            this.tipoDeCambio = purchase.tipoDeCambio;
            this.fechaHora = purchase.fechaHora;
            this.idMoneda = purchase.idMoneda;
            this.Moneda = purchase.Moneda;
            this.DetallesDeCompras = purchase.DetallesDeCompras;
            this.AbonosDeCompras = purchase.AbonosDeCompras;
            this.OrdenesDeCompra = purchase.OrdenesDeCompra;
            this.cargos = purchase.cargos;
            this.descuentos = purchase.descuentos;

            //Aqui debe calcular el Subtotal, los impuestos, el Total, lo abonado y el saldo
            this.UpdateAccount();
        }

        /// <summary>
        /// Inicializa una compra integrando el proveedor que se le pasa
        /// </summary>
        /// <param name="supplier">Proveedor al que va a estar relacionada la compra</param>
        public VMCompra(Proveedore supplier)
        {
            this.idCompra = -1;
            this.idProveedor = supplier.idProveedor;
            this.Proveedore = supplier;
            this.folio = string.Empty;
            this.fechaHora = DateTime.Now;
            this.idOrdenDeCompra = null;
            this.DetallesDeCompras = new List<DetallesDeCompra>();
            this.AbonosDeCompras = new List<AbonosDeCompra>();

            this.Subtotal = 0.0m;
            this.Impuestos = new List<VMImpuesto>();
            this.Total = 0.0m;
            this.Abonado = 0.0m;
        }

        /// <summary>
        /// Inicializa una compra integrando el proveedor y el folio que se le pasa
        /// </summary>
        /// <param name="supplier">Proveedor al que va a estar relacionada la compra</param>
        /// <param name="folio">Folio asignado a la compra</param>
        public VMCompra(Proveedore supplier, string folio)
        {
            this.idCompra = -1;
            this.idProveedor = supplier.idProveedor;
            this.Proveedore = supplier;
            this.folio = folio;
            this.fechaHora = DateTime.Now;
            this.idOrdenDeCompra = null;
            this.DetallesDeCompras = new List<DetallesDeCompra>();
            this.AbonosDeCompras = new List<AbonosDeCompra>();

            this.Subtotal = 0.0m;
            this.Impuestos = new List<VMImpuesto>();
            this.Total = 0.0m;
            this.Abonado = 0.0m;
        }

        public Compra ToCompra()
        {
            var purchase = new Compra();
            purchase.AbonosDeCompras = this.AbonosDeCompras;
            purchase.DetallesDeCompras = this.DetallesDeCompras;
            purchase.EstatusDeCompra = this.EstatusDeCompra;
            purchase.OrdenesDeCompra = this.OrdenesDeCompra;
            purchase.fechaHora = this.fechaHora;
            purchase.folio = this.folio;
            purchase.idCompra = this.idCompra;
            purchase.idEstatusDeCompra = this.idEstatusDeCompra;
            purchase.idMoneda = this.idMoneda;
            purchase.idProveedor = this.idProveedor;
            purchase.idUsuarioRegistro = this.idUsuarioRegistro;
            purchase.Moneda = this.Moneda;
            purchase.Proveedore = this.Proveedore;
            purchase.tipoDeCambio = this.tipoDeCambio;
            purchase.Usuario = this.Usuario;
            purchase.idOrdenDeCompra = this.idOrdenDeCompra;
            purchase.cargos = this.cargos;
            purchase.descuentos = this.descuentos;

            return purchase;
        }

        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado; } }
    }
}
