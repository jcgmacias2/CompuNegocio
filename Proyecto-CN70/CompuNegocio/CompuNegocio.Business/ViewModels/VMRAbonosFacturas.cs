using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRAbonosFacturas
    {
        public VMRAbonosFacturas() : base() { }

        public VMRAbonosFacturas(Pago payment,List<VMRDetalleAbonosFacturas> details, Configuracion emisor): base()
        {
            //Generales
            this.FolioAbono = string.Format("{0}{1}", payment.serie, payment.folio);
            this.FechaAbono = payment.fechaHora.ToUTCFormat();
            this.TipoDeComprobante = "P - Pago";
            this.CodigoMoneda = "XXX";
            this.DescripcionMoneda = "Por Definir";
            this.TipoCambio = payment.tipoDeCambio.ToDecimalString();
            this.CondicionesDePago = payment.Cliente.condicionDePago;
            this.RegimenFiscal = payment.Regimene.descripcion;

            //Certificación
            this.NumeroCertificadoSAT = payment.TimbresDePago.noCertificadoSAT;
            this.NumeroCSD = payment.TimbresDePago.noCertificado;
            this.UUID = payment.TimbresDePago.UUID;
            this.FechaHoraTimbrado = payment.TimbresDePago.fechaTimbrado.ToUTCFormat();
            this.RFCProveedorCertificacion = payment.TimbresDePago.rfcProvCertif;
            this.SelloDigital = payment.TimbresDePago.selloCFD;
            this.SelloSAT = payment.TimbresDePago.selloSAT;
            this.CadenaOriginalTimbre = payment.TimbresDePago.cadenaOriginal;

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
            this.RfcReceptor = payment.Cliente.rfc;
            this.RazonSocialReceptor = payment.Cliente.razonSocial;
            this.CalleReceptor = payment.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = payment.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = payment.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = payment.Cliente.Domicilio.colonia;
            this.CiudadReceptor = payment.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = payment.Cliente.Domicilio.estado;
            this.PaisReceptor = payment.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = payment.Cliente.Domicilio.codigoPostal;

            //Detalle
            this.Detalle = details;
        }

        #region Generales

        public string FolioAbono { get; set; }
        public string FechaAbono { get; set; }
        public string TipoDeComprobante { get; set; }
        public string CodigoMoneda { get; set; }
        public string DescripcionMoneda { get; set; }
        public string CondicionesDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string TipoCambio { get; set; }

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

        public List<VMRDetalleAbonosFacturas> Detalle { get; set; }
    }
}
