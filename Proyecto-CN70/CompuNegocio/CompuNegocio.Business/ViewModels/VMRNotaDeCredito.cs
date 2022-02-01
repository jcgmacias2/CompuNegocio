using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRNotaDeCredito
    {
        public VMRNotaDeCredito() : base() { }

        public VMRNotaDeCredito(VMNotaDeCredito creditNote, Configuracion emisor) : base()
        {
            //Generales
            this.FolioNotaDeCredito = string.Format("{0}{1}", creditNote.serie, creditNote.folio);
            this.FechaNotaDeCredito = creditNote.fechaHora.ToUTCFormat();
            this.TipoDeComprobante = "E - Egreso";
            this.CodigoMoneda = creditNote.Moneda.codigo;
            this.DescripcionMoneda = creditNote.Moneda.descripcion;
            this.TipoCambio = creditNote.tipoDeCambio.ToDecimalString();
            this.CondicionesDePago = creditNote.Cliente.condicionDePago;
            this.RegimenFiscal = creditNote.Regimene.descripcion;

            //Certificación
            this.NumeroCertificadoSAT = creditNote.TimbresDeNotasDeCredito.noCertificadoSAT;
            this.NumeroCSD = creditNote.TimbresDeNotasDeCredito.noCertificado;
            this.UUID = creditNote.TimbresDeNotasDeCredito.UUID;
            this.FechaHoraTimbrado = creditNote.TimbresDeNotasDeCredito.fechaTimbrado.ToUTCFormat();
            this.RFCProveedorCertificacion = creditNote.TimbresDeNotasDeCredito.rfcProvCertif;
            this.SelloDigital = creditNote.TimbresDeNotasDeCredito.selloCFD;
            this.SelloSAT = creditNote.TimbresDeNotasDeCredito.selloSAT;
            this.CadenaOriginalTimbre = creditNote.TimbresDeNotasDeCredito.cadenaOriginal;

            //Emisor
            this.RfcEmisor = emisor.rfc;
            this.RazonSocialEmisor = emisor.razonSocial;
            this.CalleEmisor = emisor.Domicilio.calle;
            this.NumeroExteriorEmisor = emisor.Domicilio.numeroExterior;
            this.NumeroInteriorEmisor = emisor.Domicilio.numeroInterior;
            this.ColoniaEmisor = emisor.Domicilio.colonia;
            this.CiudadEmisor = emisor.Domicilio.ciudad;
            this.EstadoEmisor = emisor.Domicilio.estado;
            this.PaisEmisor = emisor.Domicilio.Pais.descripcion;
            this.CodigoPostalEmisor = emisor.Domicilio.codigoPostal;

            //Receptor
            this.RfcReceptor = creditNote.Cliente.rfc;
            this.RazonSocialReceptor = creditNote.Cliente.razonSocial;
            this.CalleReceptor = creditNote.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = creditNote.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = creditNote.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = creditNote.Cliente.Domicilio.colonia;
            this.CiudadReceptor = creditNote.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = creditNote.Cliente.Domicilio.estado;
            this.PaisReceptor = creditNote.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = creditNote.Cliente.Domicilio.codigoPostal;

            if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid())
            {
                this.CFDIRelacionado = creditNote.Factura.TimbresDeFactura.UUID;
                this.TipoRelacion = "01 - Nota de crédito de los documentos relacionados";
            }

            this.Total = creditNote.Total;
            this.Subtotal = creditNote.Subtotal;
            this.Impuestos = creditNote.PorDevolucion ? creditNote.Impuestos[0].Importe : 0.0m;
            this.Descripcion = creditNote.descripcion;
            this.UsoCFDI = "G02 - Devoluciones, descuentos o bonificaciones";
        }

        #region Generales

        public string FolioNotaDeCredito { get; set; }
        public string FechaNotaDeCredito { get; set; }
        public string TipoDeComprobante { get; set; }
        public string CodigoMoneda { get; set; }
        public string DescripcionMoneda { get; set; }
        public string CondicionesDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string TipoCambio { get; set; }
        public string CFDIRelacionado { get; set; }
        public string TipoRelacion { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public string Descripcion { get; set; }
        public string UsoCFDI { get; set; }
        #endregion

        #region Certificación
        public string NumeroCertificadoSAT { get; set; }
        public string NumeroCSD { get; set; }
        public string UUID { get; set; }
        public string FechaHoraTimbrado { get; set; }
        public string RFCProveedorCertificacion { get; set; }
        public string SelloDigital { get; set; }
        public string SelloSAT { get; set; }
        public string CadenaOriginalTimbre { get; set; }
        #endregion

        #region Emisor

        public string RfcEmisor { get; set; }
        public string RazonSocialEmisor { get; set; }
        public string CalleEmisor { get; set; }
        public string NumeroExteriorEmisor { get; set; }
        public string NumeroInteriorEmisor { get; set; }
        public string ColoniaEmisor { get; set; }
        public string CiudadEmisor { get; set; }
        public string EstadoEmisor { get; set; }
        public string PaisEmisor { get; set; }
        public string CodigoPostalEmisor { get; set; }

        #endregion

        #region Receptor

        public string RfcReceptor { get; set; }
        public string RazonSocialReceptor { get; set; }
        public string CalleReceptor { get; set; }
        public string NumeroExteriorReceptor { get; set; }
        public string NumeroInteriorReceptor { get; set; }
        public string ColoniaReceptor { get; set; }
        public string CiudadReceptor { get; set; }
        public string EstadoReceptor { get; set; }
        public string PaisReceptor { get; set; }
        public string CodigoPostalReceptor { get; set; }

        #endregion

    }
}
