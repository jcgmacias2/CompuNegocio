using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRRemision : VMRemision
    {
        public VMRRemision() : base() { }

        public VMRRemision(VMRemision remision, Configuracion emisor, Usuario user, string file): base()
        {
            //Generales
            this.Archivo = file;
            this.FolioRemision = remision.folio.ToString();
            this.FechaRemision = remision.fechaHora.ToUTCFormat();
            this.tipoDeCambio = remision.tipoDeCambio;
            this.idMoneda = remision.idMoneda;
            this.CodigoMoneda = remision.idMoneda.isValid() ? remision.Moneda.codigo : string.Empty;
            this.Moneda = remision.Moneda;
            this.CondicionesDePago = remision.Cliente.condicionDePago;
            this.Vendedor = user.nombreCompleto;
            var abonos = remision.AbonosDeRemisions.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));
            //Puede ser requerido considerar la conversión de moneda
            var maxAbono = abonos.Count() > 0 ? abonos.First() : null;
            this.FormaDePago = maxAbono.isValid() ? string.Format("{0} {1}", maxAbono.FormasPago.codigo, maxAbono.FormasPago.descripcion) : string.Empty;
            this.RegimenFiscal = emisor.Regimen;


            //Detalles
            this.DetalleDeRemision = remision.DetalleDeRemision;

            //Cuenta
            //Se encarga de llenar Subtotal, Impuestos, Total y Abonado
            //this.UpdateAccount(); <-- JL: Es un reporte, ya esta calculado
            this.Impuestos = remision.Impuestos;
            this.Subtotal = remision.Subtotal;
            this.Total = remision.Total;
            this.Abonado = remision.Abonado;


            //CFDI Relacionado
            this.TipoDeRelacion = remision.idFactura.isValid() && remision.Factura.TimbresDeFactura.isValid() ? remision.Factura.TimbresDeFactura.UUID : string.Empty;

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
            this.RfcReceptor = remision.Cliente.rfc;
            this.RazonSocialReceptor = remision.Cliente.razonSocial;
            this.CalleReceptor = remision.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = remision.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = remision.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = remision.Cliente.Domicilio.colonia;
            this.CiudadReceptor = remision.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = remision.Cliente.Domicilio.estado;
            this.PaisReceptor = remision.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = remision.Cliente.Domicilio.codigoPostal;

            //Notas
            DatosExtraPorRemision datoNota = remision.DatosExtraPorRemisions.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
            this.Notas = datoNota.isValid() ? datoNota.valor : "";

            //Modificaciones
            CodigoCliente = remision.Cliente.codigo;
            DiasCredito = remision.Cliente.diasCredito.GetValueOrDefault(0);
            UsuarioRegistro = remision.Usuario.nombreCompleto;
            UsuarioVendedor = remision.Usuario1.nombreCompleto;
        }

        #region Generales

        public string FolioRemision { get; set; }
        public string FechaRemision { get; set; }
        public string CondicionesDePago { get; set; }
        public string FormaDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string Notas { get; set; }
        public string CodigoMoneda { get; set; }
        public string Vendedor { get; set; }
        public string Archivo { get; set; }

        #endregion

        #region Cuenta
        public decimal TotalTraslados { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(m => m.Importe); } }
        public decimal TotalRetenidos { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(m => m.Importe); } }

        #endregion

        #region CFDI Relacionado

        public string TipoDeRelacion { get; set; }

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

        #region Modificaciones

        public string CodigoCliente { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioVendedor { get; set; }
        public int DiasCredito { get; set; }

        #endregion
    }
}
