using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRNotaDeDescuento
    {
        public VMRNotaDeDescuento() : base() { }

        public VMRNotaDeDescuento(NotasDeDescuento discountNote, Configuracion emisor): base()
        {
            //Generales
            this.FolioNotaDeDescuento = discountNote.folio.ToString();
            this.FechaNotaDeDescuento = discountNote.fechaHora.Value.ToUTCFormat();
            this.CodigoMoneda = discountNote.Moneda.codigo;
            this.DescripcionMoneda = discountNote.Moneda.descripcion;
            this.TipoCambio = discountNote.tipoDeCambio.ToDecimalString();
            this.CondicionesDePago = discountNote.Cliente.condicionDePago;

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
            this.RfcReceptor = discountNote.Cliente.rfc;
            this.RazonSocialReceptor = discountNote.Cliente.razonSocial;
            this.CalleReceptor = discountNote.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = discountNote.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = discountNote.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = discountNote.Cliente.Domicilio.colonia;
            this.CiudadReceptor = discountNote.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = discountNote.Cliente.Domicilio.estado;
            this.PaisReceptor = discountNote.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = discountNote.Cliente.Domicilio.codigoPostal;

            this.Total = discountNote.monto;
            this.Descripcion = discountNote.descripcion;
        }

        #region Generales

        public string FolioNotaDeDescuento { get; set; }
        public string FechaNotaDeDescuento { get; set; }
        public string CodigoMoneda { get; set; }
        public string DescripcionMoneda { get; set; }
        public string CondicionesDePago { get; set; }
        public string TipoCambio { get; set; }
        public decimal Total { get; set; }
        public string Descripcion { get; set; }
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
