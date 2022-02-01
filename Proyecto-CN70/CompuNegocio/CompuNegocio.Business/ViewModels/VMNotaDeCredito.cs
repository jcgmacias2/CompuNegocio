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
    public class VMNotaDeCredito : NotasDeCredito
    {
        public VMNotaDeCredito() : base() 
        {
            this.Cliente = new Cliente();
            this.Impuestos = new List<VMImpuesto>();
            this.DatosExtraPorNotaDeCreditoes = new List<DatosExtraPorNotaDeCredito>();
        }

        /// <summary>
        /// Construye una VMNotaDeCredito a partir de un registro de NotaDeCredito existente
        /// </summary>
        /// <param name="creditNote"></param>
        public VMNotaDeCredito(NotasDeCredito creditNote)
        {
            try
            {
                this.idNotaDeCredito = creditNote.idNotaDeCredito;
                this.idFactura = creditNote.idFactura;
                this.idCliente = creditNote.idCliente;
                this.Cliente = creditNote.Cliente;
                this.serie = creditNote.serie;
                this.folio = creditNote.folio;
                this.fechaHora = creditNote.fechaHora;
                this.idMoneda = creditNote.idMoneda;
                this.Moneda = creditNote.Moneda;
                this.tipoDeCambio = creditNote.tipoDeCambio;
                this.DetallesDeNotaDeCreditoes = creditNote.DetallesDeNotaDeCreditoes;
                this.DetalleDeNotaDeCredito = creditNote.DetallesDeNotaDeCreditoes.ToViewModelList();
                this.importe = creditNote.importe;
                this.descripcion = creditNote.descripcion;
                this.idFormaDePago = creditNote.idFormaDePago;
                this.FormasPago = creditNote.FormasPago;
                this.idEstatusDeNotaDeCredito = creditNote.idEstatusDeNotaDeCredito;
                this.EstatusDeNotaDeCredito = creditNote.EstatusDeNotaDeCredito;
                this.idUsuarioRegistro = creditNote.idUsuarioRegistro;
                this.Usuario = creditNote.Usuario;
                this.TimbresDeNotasDeCredito = creditNote.TimbresDeNotasDeCredito;
                this.cadenaOriginal = creditNote.cadenaOriginal;
                this.idEmpresa = creditNote.idEmpresa;
                this.Empresa = creditNote.Empresa;
                this.idRegimen = creditNote.idRegimen;
                this.Regimene = creditNote.Regimene;
                this.Factura = creditNote.Factura;
                this.idFactura = creditNote.idFactura;
                this.idCuentaBancaria = creditNote.idCuentaBancaria;
                this.CuentasBancaria = creditNote.CuentasBancaria;
                this.DatosExtraPorNotaDeCreditoes = creditNote.DatosExtraPorNotaDeCreditoes;
                this.CancelacionesDeNotaDeCredito = creditNote.CancelacionesDeNotaDeCredito;

                this.UpdateAccount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa una nota de credito integrando el cliente y la serie y folio que se le pasa
        /// </summary>
        /// <param name="client">Cliente al que va a estar relacionada la nota de credito</param>
        /// <param name="serie">Serie que se le va a asignar a la nota de credito</param>
        /// <param name="folio">Folio que se le va a asignar a la nota de credito</param>
        public VMNotaDeCredito(Cliente client, string serie, string folio, decimal tipoDeCambio)
        {
            try
            {
                this.idCliente = client.idCliente;
                this.Cliente = client;
                this.serie = serie;
                this.folio = folio.ToInt();
                this.DetallesDeNotaDeCreditoes = new List<DetallesDeNotaDeCredito>();
                this.DetalleDeNotaDeCredito = new List<VMDetalleDeNotaDeCredito>();

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
        /// Inicializa una nota de credito con los datos de la factura proporcionada
        /// </summary>
        /// <param name="invoice">Factura de la que se creara la nota de credito</param>
        public VMNotaDeCredito(VMFactura invoice)
        {
            this.Factura = invoice;

            this.tipoDeCambio = invoice.tipoDeCambio;
            this.idMoneda = invoice.idMoneda;
            this.idCliente = invoice.idCliente;
            this.idEstatusDeNotaDeCredito = (int) StatusDeNotaDeCredito.Nueva;
            this.idEmpresa = invoice.idEmpresa;

            this.DetalleDeNotaDeCredito = invoice.DetalleDeFactura.ToDetallesDeNotaDeCredito();
            this.DatosExtraPorNotaDeCreditoes = invoice.DatosExtraPorFacturas.Select(x => new DatosExtraPorNotaDeCredito(){dato = x.dato,valor = x.valor}).ToList();

            this.Cliente = invoice.Cliente;
            this.Empresa = invoice.Empresa;
            this.Moneda = invoice.Moneda;
            this.Usuario = invoice.Usuario;

            this.UpdateAccount();
        }

        public NotasDeCredito ToCreditNote()
        {
            try
            {
                var creditNote = new NotasDeCredito();

                creditNote.idNotaDeCredito = this.idNotaDeCredito;
                creditNote.idFactura = this.idFactura;
                creditNote.idCliente = this.idCliente;
                creditNote.Cliente = this.Cliente;
                creditNote.serie = this.serie;
                creditNote.folio = this.folio;
                creditNote.fechaHora = this.fechaHora;
                creditNote.idMoneda = this.idMoneda;
                creditNote.Moneda = this.Moneda;
                creditNote.tipoDeCambio = this.tipoDeCambio;
                creditNote.descripcion = this.descripcion;
                creditNote.importe = this.importe;
                creditNote.DetallesDeNotaDeCreditoes = this.DetalleDeNotaDeCredito.ToDetalleDeNotaDeCredito();
                creditNote.idCuentaBancaria = this.idCuentaBancaria;
                creditNote.idFormaDePago = this.idFormaDePago;
                creditNote.FormasPago = this.FormasPago;
                creditNote.idEstatusDeNotaDeCredito = this.idEstatusDeNotaDeCredito;
                creditNote.EstatusDeNotaDeCredito = this.EstatusDeNotaDeCredito;
                creditNote.idUsuarioRegistro = this.idUsuarioRegistro;
                creditNote.Usuario = this.Usuario;
                creditNote.TimbresDeNotasDeCredito = this.TimbresDeNotasDeCredito;
                creditNote.cadenaOriginal = this.cadenaOriginal;
                creditNote.idEmpresa = this.idEmpresa;
                creditNote.Empresa = this.Empresa;
                creditNote.idRegimen = this.idRegimen;
                creditNote.Regimene = this.Regimene;
                creditNote.DatosExtraPorNotaDeCreditoes = this.DatosExtraPorNotaDeCreditoes;
                creditNote.CancelacionesDeNotaDeCredito = this.CancelacionesDeNotaDeCredito;
                creditNote.Usuario = this.Usuario;
                creditNote.Factura = this.Factura;
                creditNote.importe = this.Total;

                return creditNote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleDeNotaDeCredito> DetalleDeNotaDeCredito { get; set; }
        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal Total { get; set; }
        public bool PorDevolucion { get { return DetalleDeNotaDeCredito.Count > 0; } }
    }
}
