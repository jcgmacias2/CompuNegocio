using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMPago
    {
        public VMPago() { }

        public VMPago(AbonosDeFactura abono, Configuracion emisor)
        {
            //Generales
            this.TipoDeComprobante = "P - Pago";
            //this.FolioPago = string.Format("{0}{1}", abono.TimbresDeAbonosDeFactura.serie, abono.TimbresDeAbonosDeFactura.folio);
            this.Serie = abono.TimbresDeAbonosDeFactura.serie;
            this.Folio = abono.TimbresDeAbonosDeFactura.folio;
            this.FechaPago = abono.fechaHora.ToUTCFormat();
            this.CodigoMoneda = abono.Moneda.codigo;
            this.TipoDeCambio = abono.tipoDeCambio;
            this.FormaDePago = abono.FormasPago.codigo;
            this.RegimenFiscal = abono.Factura.Regimene.descripcion;
            this.MonedaPago = "XXX";


            //Certificación
            this.NumeroCertificadoSAT = abono.TimbresDeAbonosDeFactura.noCertificadoSAT;
            this.NumeroCSD = abono.TimbresDeAbonosDeFactura.noCertificado;
            this.UUID = abono.TimbresDeAbonosDeFactura.UUID;
            this.FechaHoraTimbrado = abono.TimbresDeAbonosDeFactura.fechaTimbrado.ToUTCFormat();
            this.RFCProveedorCertificacion = abono.TimbresDeAbonosDeFactura.RfcProvCertif;
            this.SelloDigital = abono.TimbresDeAbonosDeFactura.selloCFD;
            this.SelloSAT = abono.TimbresDeAbonosDeFactura.selloSAT;
            this.CadenaOriginalTimbre = abono.TimbresDeAbonosDeFactura.cadenaOriginal;

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
            this.RfcReceptor = abono.Factura.Cliente.rfc;
            this.RazonSocialReceptor = abono.Factura.Cliente.razonSocial;
            this.CalleReceptor = abono.Factura.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = abono.Factura.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = abono.Factura.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = abono.Factura.Cliente.Domicilio.colonia;
            this.CiudadReceptor = abono.Factura.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = abono.Factura.Cliente.Domicilio.estado;
            this.PaisReceptor = abono.Factura.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = abono.Factura.Cliente.Domicilio.codigoPostal;
            this.UsoCFDI = abono.Factura.UsosCFDI.descripcion;

            //Pago
            this.Cantidad = 1.0m;
            this.UnidadPago = "ACT";
            this.CodigoPago = "84111506";
            this.DescripcionPago = "Pago";
            this.FormaDePago = string.Format("{0}-{1}", abono.FormasPago.codigo, abono.FormasPago.descripcion);
            this.PrecioUnitarioPago = 0.0m;
            this.ImportePago = 0.0m;

            var abonosPrevios = abono.Factura.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado) && a.idAbonoDeFactura < abono.idAbonoDeFactura).ToList();
            var totalAbonosPrevios = abonosPrevios.Sum(a => a.monto.ToDocumentCurrency(a.Moneda, a.Factura.Moneda, a.tipoDeCambio));
            var saldoAnterior = new VMFactura(abono.Factura).Total - totalAbonosPrevios;
            var pagado = abono.monto.ToDocumentCurrency(abono.Moneda, abono.Factura.Moneda, abono.tipoDeCambio);
            DocumentosRelacionados = new List<VMCFDIRelacionado>();
            DocumentosRelacionados.Add(new VMCFDIRelacionado(abono.Factura, abono.idAbonoDeFactura, saldoAnterior, pagado));

            pago_impuestos = new List<VMImpuestoPorFactura>();

            if (abono.Factura.ImpuestoPorFacturas.ToList().Count() > 0) {
                
                foreach (var imp in abono.Factura.ImpuestoPorFacturas.ToList()) {
                    var base_imp = pagado / (1 + imp.valorTasaOCuaota);
                    var tipo = imp.codigoImpuesto.Equals("002") ? "Traslado" : "Retenido";
                    pago_impuestos.Add(new VMImpuestoPorFactura(imp, abono, tipo, base_imp));
                }
            }
        }

        #region Generales
        public string Serie { get; set; }
        public int Folio { get; set; }
        public string TipoDeComprobante { get; set; }
        public string FolioPago { get { return string.Format("{0}{1}", this.Serie, this.Folio); } }
        public string FechaPago { get; set; }
        public string CodigoMoneda { get; set; }
        public decimal TipoDeCambio { get; set; }
        public string RegimenFiscal { get; set; }
        public string MonedaPago { get; set; }

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

        #endregion

        #region Pago
        public decimal Cantidad { get; set; }
        public string UnidadPago { get; set; }
        public string CodigoPago { get; set; }
        public string DescripcionPago { get; set; }
        public string FormaDePago { get; set; }
        public decimal PrecioUnitarioPago { get; set; }
        public decimal ImportePago { get; set; }

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

        public List<VMCFDIRelacionado> DocumentosRelacionados { get; set; }
        public List<VMImpuestoPorFactura> pago_impuestos { get; set; }

    }
}
