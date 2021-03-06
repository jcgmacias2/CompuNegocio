using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRCotizacion
    {

        public VMRCotizacion(VMCotizacion quote,Configuracion config, Usuario usuario)
        {
            RazonSocial = config.razonSocial;
            Direccion = string.Format("{0} {1}{2} {3}",config.Domicilio.calle, config.Domicilio.numeroExterior,config.Domicilio.numeroInterior, config.Domicilio.colonia);
            Direccion2 = string.Format("{0}, {1}, {2}, C.P. {3}", config.Domicilio.ciudad, config.Domicilio.estado, config.Domicilio.Pais.descripcion, config.Domicilio.codigoPostal);

            RazonSocialCliente = quote.Cliente.razonSocial;
            DomicilioCliente = string.Format("{0} No. {1} Col. {2} C.P. {3}, {4}, {5}", quote.Cliente.Domicilio.calle, quote.Cliente.Domicilio.numeroExterior, quote.Cliente.Domicilio.colonia, quote.Cliente.Domicilio.codigoPostal, quote.Cliente.Domicilio.ciudad, quote.Cliente.Domicilio.estado);

            Folio = quote.folio.ToString();
            Fecha = quote.fechaHora.ToString("dd/MM/yyyy");
            TipoDeCambio = quote.tipoDeCambio.ToDecimalString();

            DetalleCotizacion = quote.DetalleDeCotizacion.Select(x => new VMDetalle(x)).ToList();

            SubTotal = quote.Subtotal.ToDecimalString();
            Total = quote.Total.ToDecimalString();
            ImpuestosTrasladados = quote.Impuestos.Where(x=>x.idTipoDeImpuesto == (int)TipoDeImpuesto.Trasladado).Sum(x=>x.Importe).ToDecimalString();
            ImpuestosRetenidos = quote.Impuestos.Where(x=>x.idTipoDeImpuesto == (int)TipoDeImpuesto.Retenido).Sum(x=>x.Importe).ToDecimalString();

            MonedaEnLetra = quote.Moneda.descripcion;
            Moneda = quote.Moneda;
            idMoneda = quote.idMoneda;
            Vendedor = usuario.nombreCompleto;

            DatosExtraPorCotizacion notaCotizacion = quote.DatosExtraPorCotizacions.FirstOrDefault(x=>x.dato == DatoExtra.Nota.ToString());
            NotaCotizacion = notaCotizacion.isValid() ? notaCotizacion.valor : "";
            RfcEmisor = config.rfc;
            RfcCliente = quote.Cliente.rfc;

            //Modificaciones
            CodigoCliente = quote.Cliente.codigo;
            DiasCredito = quote.Cliente.diasCredito.GetValueOrDefault(0);
            UsuarioRegistro = quote.Usuario.nombreCompleto;
            UsuarioVendedor = quote.Cliente.idVendedor.isValid() ? quote.Cliente.Usuario.nombreCompleto : string.Empty;
        }

        public string RazonSocial { get; set; }
        public string Direccion { get; set; }

        public string RazonSocialCliente { get; set; }
        public string DomicilioCliente { get; set; }

        public string Folio { get; set; }
        public string Fecha { get; set; }
        public string TipoDeCambio { get; set; }

        public string Vendedor { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string ImpuestosTrasladados { get; set; }
        public string ImpuestosRetenidos { get; set; }

        public string MonedaEnLetra { get; set; }
        public string NotaCotizacion { get; set; }
        public string RfcEmisor { get; set; }
        public string Direccion2 { get; set; }
        public string RfcCliente { get; set; }

        public List<VMDetalle> DetalleCotizacion { get; set; }
        public Moneda Moneda { get; set; }
        public int idMoneda { get; set; }


        #region Modificaciones

        public string CodigoCliente { get; set; }
        public string UsuarioRegistro { get; set; }
        public string UsuarioVendedor { get; set; }
        public int DiasCredito { get; set; }

        #endregion
    }
}