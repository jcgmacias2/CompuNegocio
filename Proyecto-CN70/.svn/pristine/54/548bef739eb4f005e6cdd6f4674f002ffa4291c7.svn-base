using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMROrdenDeCompra : VMOrdenDeCompra
    {
        public VMROrdenDeCompra() : base() { }

        public VMROrdenDeCompra(VMOrdenDeCompra ordenDeCompra, Configuracion emisor): base()
        {
            //Generales
            this.FolioRemision = ordenDeCompra.folio.ToString();
            this.FechaRemision = ordenDeCompra.fechaHora.ToUTCFormat();
            this.tipoDeCambio = ordenDeCompra.tipoDeCambio;
            this.idMoneda = ordenDeCompra.idMoneda;
            this.CodigoMoneda = ordenDeCompra.idMoneda.isValid() ? ordenDeCompra.Moneda.codigo : string.Empty;
            this.Moneda = ordenDeCompra.Moneda;
            this.RegimenFiscal = emisor.Regimen;
            this.Total = ordenDeCompra.Total;
            this.Subtotal = ordenDeCompra.Subtotal;
            this.NombreUsuario = ordenDeCompra.Usuario.nombreCompleto;
            
            //Detalles
            this.Detalles = ordenDeCompra.Detalles;

            //Cuenta
            //Se encarga de llenar Subtotal, Impuestos, Total y Abonado
            this.UpdateAccount();

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
            this.RfcReceptor = ordenDeCompra.Proveedore.rfc;
            this.RazonSocialReceptor = ordenDeCompra.Proveedore.razonSocial;
            this.CalleReceptor = ordenDeCompra.Proveedore.Domicilio.calle;
            this.NumeroExteriorReceptor = ordenDeCompra.Proveedore.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = ordenDeCompra.Proveedore.Domicilio.numeroInterior;
            this.ColoniaReceptor = ordenDeCompra.Proveedore.Domicilio.colonia;
            this.CiudadReceptor = ordenDeCompra.Proveedore.Domicilio.ciudad;
            this.EstadoReceptor = ordenDeCompra.Proveedore.Domicilio.estado;
            this.PaisReceptor = ordenDeCompra.Proveedore.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = ordenDeCompra.Proveedore.Domicilio.codigoPostal;

            //Notas
            DatosExtraPorOrdenDeCompra datoNota = ordenDeCompra.DatosExtraPorOrdenDeCompras.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
            this.Notas = datoNota.isValid() ? datoNota.valor : "";
        }

        #region Generales

        public string FolioRemision { get; set; }
        public string FechaRemision { get; set; }
        public string CondicionesDePago { get; set; }
        public string FormaDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string Notas { get; set; }
        public string CodigoMoneda { get; set; }
        public string NombreUsuario { get; set; }

        #endregion

        #region Cuenta
        public decimal Subtotal { get; set; }
        public decimal TotalTraslados { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(m => m.Importe); } }
        public decimal TotalRetenidos { get { return this.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(m => m.Importe); } }
        public decimal Total { get; set; }

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
        public string TelefonoReceptor { get; set; }
        public string ContactoReceptor { get; set; }

        #endregion
    }
}
