using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRFactura : Factura
    {
        public VMRFactura() : base() { }

        public VMRFactura(VMFactura factura, Configuracion emisor, Usuario user): base()
        {
            //Generales
            this.FolioFactura = string.Format("{0}{1}", factura.serie, factura.folio);
            this.FechaFactura = factura.fechaHora.ToUTCFormat();
            this.TipoDeComprobante = "I - Ingreso";
            this.CodigoMoneda = factura.Moneda.codigo;
            this.DescripcionMoneda = factura.Moneda.descripcion;
            this.tipoDeCambio = factura.tipoDeCambio;
            this.CondicionesDePago = factura.Cliente.condicionDePago;
            var abonos = factura.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));
            //Puede ser requerido considerar la conversión de moneda
            var maxAbono = abonos.Count() > 0 ? abonos.First() : null;
            this.FormaDePago = maxAbono.isValid() ? string.Format("{0} {1}", maxAbono.FormasPago.codigo, maxAbono.FormasPago.descripcion) : "99 - Por definir";
            this.NumeroDeCuenta = maxAbono.isValid() && maxAbono.CuentasBancaria.isValid()? maxAbono.CuentasBancaria.numeroDeCuenta : string.Empty;
            this.MetodoDePago = string.Format("{0} {1}", factura.MetodosPago.codigo, factura.MetodosPago.descripcion);
            this.RegimenFiscal = factura.Regimene.descripcion;
            this.ordenDeCompra = factura.ordenDeCompra;
            this.Vendedor = user.nombreDeUsuario;
            this.Moneda = factura.Moneda;
            
            //Detalles
            this.DetallesDeFacturas = factura.DetallesDeFacturas;
            this.DetalleDeFactura = factura.DetalleDeFactura;

            //Certificación
            this.NumeroCertificadoSAT = factura.TimbresDeFactura.noCertificadoSAT;
            this.NumeroCSD = factura.noCertificado;
            this.UUID = factura.TimbresDeFactura.UUID;
            this.FechaHoraTimbrado = factura.TimbresDeFactura.fechaTimbrado.ToUTCFormat();
            this.RFCProveedorCertificacion = factura.TimbresDeFactura.RfcProvCertif;
            this.SelloDigital = factura.TimbresDeFactura.selloCFD;
            this.SelloSAT = factura.TimbresDeFactura.selloSAT;
            this.CadenaOriginalTimbre = factura.TimbresDeFactura.cadenaOriginal;

            //Cuenta
            //Se encarga de llenar Subtotal, Impuestos, Total y Abonado
            this.UpdateAccount();

            //CFDI Relacionado
            this.TipoDeRelacion = factura.idTipoRelacion.isValid() ? factura.TiposRelacion.descripcion : string.Empty;
            this.UUIDRelacionado = factura.idComprobanteOriginal.isValid() ? factura.Factura1.TimbresDeFactura.UUID : string.Empty;

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
            this.RfcReceptor = factura.Cliente.rfc;
            this.RazonSocialReceptor = factura.Cliente.razonSocial;
            this.CalleReceptor = factura.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = factura.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = factura.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = factura.Cliente.Domicilio.colonia;
            this.CiudadReceptor = factura.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = factura.Cliente.Domicilio.estado;
            this.PaisReceptor = factura.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = factura.Cliente.Domicilio.codigoPostal;
            this.UsoCFDI = factura.UsosCFDI.descripcion;
            this.RegimenFiscalReceptor = factura.Cliente.Regimene.descripcion;

            //Notas
            DatosExtraPorFactura datoNota = factura.DatosExtraPorFacturas.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
            this.Notas = datoNota.isValid() ? datoNota.valor : "";

            //Modificaciones
            CodigoCliente = factura.Cliente.codigo;
            DiasCredito = factura.Cliente.diasCredito.GetValueOrDefault(0);
            UsuarioRegistro = factura.Usuario.nombreCompleto;
            UsuarioVendedor = factura.Usuario1.nombreCompleto;
        }

        #region Generales

        public string FolioFactura { get; set; }
        public string FechaFactura { get; set; }
        public string TipoDeComprobante { get; set; }
        public string CodigoMoneda { get; set; }
        public string DescripcionMoneda { get; set; }
        public string CondicionesDePago { get; set; }
        public string FormaDePago { get; set; }
        public string NumeroDeCuenta { get; set; }
        public string MetodoDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string Notas { get; set; }
        public string Vendedor { get; set; }

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

        #region Cuenta
        public List<VMDetalleDeFactura> DetalleDeFactura { get; set; }
        public decimal Subtotal { get; set; }
        public List<VMImpuesto> Impuestos { get; set; }
        public decimal TotalTraslados { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(m => m.Importe.ToRoundedCurrency(this.Moneda)); } }
        public decimal TotalRetenidos { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(m => m.Importe.ToRoundedCurrency(this.Moneda)); } }
        public decimal Total { get; set; }
        public decimal Abonado { get; set; }
        public decimal Saldo { get { return Total - Abonado; } }

        #endregion

        #region CFDI Relacionado

        public string TipoDeRelacion { get; set; }
        public string UUIDRelacionado { get; set; }

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
        public string UsoCFDI { get; set; }
        public string RegimenFiscalReceptor { get; set; }

        #endregion

        #region Modificaciones

        public string CodigoCliente { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioVendedor { get; set; }
        public int DiasCredito { get; set; }

        #endregion
    }
}
