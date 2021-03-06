using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMRPedido : VMPedido
    {
        public VMRPedido() : base() { }

        public VMRPedido(VMPedido pedido, Configuracion emisor): base()
        {
            //Generales
            this.FolioRemision = pedido.folio.ToString();
            this.FechaRemision = pedido.fechaHora.ToUTCFormat();
            this.tipoDeCambio = pedido.tipoDeCambio;
            this.idMoneda = pedido.idMoneda;
            this.CodigoMoneda = pedido.idMoneda.isValid() ? pedido.Moneda.codigo : string.Empty;
            this.Moneda = pedido.Moneda;
            this.CondicionesDePago = pedido.Cliente.condicionDePago;
            this.RegimenFiscal = emisor.Regimen;
            this.Total = pedido.Total;
            this.Subtotal = pedido.Subtotal;
            
            //Detalles
            this.Detalles = pedido.DetallesDePedidoes.Select(x=>new VMRDetalleDePedido(x)).ToList();

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
            this.RfcReceptor = pedido.Cliente.rfc;
            this.RazonSocialReceptor = pedido.Cliente.razonSocial;
            this.CalleReceptor = pedido.Cliente.Domicilio.calle;
            this.NumeroExteriorReceptor = pedido.Cliente.Domicilio.numeroExterior;
            this.NumeroInteriorReceptor = pedido.Cliente.Domicilio.numeroInterior;
            this.ColoniaReceptor = pedido.Cliente.Domicilio.colonia;
            this.CiudadReceptor = pedido.Cliente.Domicilio.ciudad;
            this.EstadoReceptor = pedido.Cliente.Domicilio.estado;
            this.PaisReceptor = pedido.Cliente.Domicilio.Pais.descripcion;
            this.CodigoPostalReceptor = pedido.Cliente.Domicilio.codigoPostal;
            this.TelefonoReceptor = pedido.Cliente.telefono;

            //Notas
            DatosExtraPorPedido datoNota = pedido.DatosExtraPorPedidoes.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
            this.Notas = datoNota.isValid() ? datoNota.valor : "";
        }

        public List<VMRDetalleDePedido> Detalles { get; set; }

        #region Generales

        public string FolioRemision { get; set; }
        public string FechaRemision { get; set; }
        public string CondicionesDePago { get; set; }
        public string FormaDePago { get; set; }
        public string RegimenFiscal { get; set; }
        public string Notas { get; set; }
        public string CodigoMoneda { get; set; }

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
