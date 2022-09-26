using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Aprovi.Business.Services
{
    public interface IComprobantFiscaleService
    {
        /// <summary>
        /// Obtiene la cadena original en referencia de la factura que se le pasa
        /// </summary>
        /// <param name="invoice">Factura sobre la cual generar la cadena</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Cadena original del comprobante de factura</returns>
        string GetCadenaOriginal(VMFactura invoice, Configuracion config);

        /// <summary>
        /// Obtiene la cadena original en referencia al abono que se le pasa
        /// </summary>
        /// <param name="invoice">Factura a la que pertenece el abono</param>
        /// <param name="payment">Abono sobre el cual generar la cadena</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Cadena original del comprobante de abono</returns>
        string GetCadenaOriginal(VMFactura invoice, AbonosDeFactura payment, Configuracion config);

        /// <summary>
        /// Obtiene la cadena original en referencia al pago que se le pasa
        /// </summary>
        /// <param name="payment">Pago del que se obtendra la cadena original</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Cadena original del comprobante de pago</returns>
        string GetCadenaOriginal(Pago payment, Configuracion config);

        /// <summary>
        /// Obtiene la cadena original en referencia a la nota de credito que se le pasa
        /// </summary>
        /// <param name="creditNote">Nota de credito de la que se obtendr ala cadena original</param>
        /// <param name="config">Configuracion del emisor</param>
        /// <returns>Cadena original del comprobante de pago</returns>
        string GetCadenaOriginal(VMNotaDeCredito creditNote, Configuracion config);

        /// <summary>
        /// Sella la cadena original que se le pasa utilizando el certificado que existe en la configuración
        /// </summary>
        /// <param name="cadenaOriginal">Cadena original a sellar</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Sello Digital de la cadena original</returns>
        string GetSello(string cadenaOriginal, Configuracion config);

        /// <summary>
        /// Genera la instancia del documento xml que corresponde a la factura
        /// </summary>
        /// <param name="invoice">Factura sobre la que se desde crear el xml</param>
        /// <param name="config">Configuración del emisor</param>
        /// <param name="requiresAddenda">Establece si una factura debe llevar addenda</param>
        /// <returns>Comprobante xml</returns>
        XmlDocument CreateCFDI(VMFactura invoice, Configuracion config, bool requiresAddenda);

        /// <summary>
        /// Genera la instancia del documento xml que corresponde al abono
        /// </summary>
        /// <param name="invoice">Factura a la que pertenece el abono</param>
        /// <param name="payment">Abono sobre el que se desea crear el xml</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Comprobante xml</returns>
        XmlDocument CreateCFDI(VMFactura invoice, AbonosDeFactura payment, Configuracion config);

        /// <summary>
        /// Genera la instancia del documento xml que corresponde a los pagos
        /// </summary>
        /// <param name="payment">Pago sobre el que se desea crear el xml</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Comprobante xml</returns>
        XmlDocument CreateCFDI(Pago payment, Configuracion config);

        /// <summary>
        /// Genera la instancia del documento xml que corresponde a la nota de credito
        /// </summary>
        /// <param name="invoice">Nota de credito sobre la que se desde crear el xml</param>
        /// <param name="config">Configuración del emisor</param>
        /// <returns>Comprobante xml</returns>
        XmlDocument CreateCFDI(VMNotaDeCredito invoice, Configuracion config);

        /// <summary>
        /// Timbra el comprobante que se le pasa, utilizando las credenciales de la configuración
        /// </summary>
        /// <param name="xmlComprobante">Comprobante xml a timbrar</param>
        /// <param name="config">Configuración actual del emisor</param>
        /// <returns>Comprobante xml con timbre</returns>
        XmlDocument Timbrar(XmlDocument xmlComprobante, Configuracion config);

        /// <summary>
        /// Recupera un timbre existente
        /// </summary>
        /// <param name="xmlComprobante">Comprobante xml al cual se desea recuperar el timbre</param>
        /// <param name="config">Configuración actual del emisor</param>
        /// <returns>Comprobante timbrado en utf8</returns>
        string RecuperarTimbre(XmlDocument xmlComprobante, Configuracion config);

        /// <summary>
        /// Cancela un CFDI
        /// </summary>
        /// <param name="uuid">UUID del comprobante a cancelar</param>
        /// <param name="config">Configuración actual del emisor</param>
        /// <returns>Acuse de cancelación</returns>
        string Cancelar(string uuid, string nocertificado, Configuracion config);

        /// <summary>
        /// Genera el xml del acuse de cancelación
        /// </summary>
        /// <param name="fullFilePath">Nombre completo del archivo a generar</param>
        /// <param name="acuse">Acuse de cancelación en string</param>
        /// <returns>Ruta del archivo de acuse</returns>
        string CreateAcuse(string fullFilePath, string acuse);

        /// <summary>
        /// Extrae los datos del timbre de un xml dado
        /// </summary>
        /// <param name="xmlFactura">Comprobante xml timbrado de una factura</param>
        /// <returns>Timbre contenido en el comprobante</returns>
        TimbresDeFactura GetTimbreFactura(XmlDocument xmlFactura);

        /// <summary>
        /// Extrae los datos del timbre de un xml dado
        /// </summary>
        /// <param name="xmlFactura">Comprobante xml timbrado de una nota de credito</param>
        /// <returns>Timbre contenido en el comprobante</returns>
        TimbresDeNotasDeCredito GetTimbreNotaDeCredito(XmlDocument xmlFactura);

        /// <summary>
        /// Extrae los datos del timbre de un xml dado
        /// </summary>
        /// <param name="xmlPago">Comprobante xml timbrado de un abono</param>
        /// <returns>Timbre contenido en el comprobante</returns>
        TimbresDeAbonosDeFactura GetTimbreAbono(XmlDocument xmlAbono);

        /// <summary>
        /// Extrae los datos del timbre de un xml dado
        /// </summary>
        /// <param name="xmlPago">Comprobante xml timbrado de un pago</param>
        /// <returns>Timbre contenido en el comprobante</returns>
        TimbresDePago GetTimbrePago(XmlDocument xmlPago);

        /// <summary>
        /// Genera un archivo físico de código de barras bidimensional, requerido para la impresión en pdf
        /// </summary>
        /// <param name="fullFilePath">Nombre completo de generación del archivo cbb, incluyendo la extension .bmp</param>
        /// <param name="rfcEmisor">RFC del contribuyente emisor</param>
        /// <param name="rfcReceptor">RFC del contribuyente receptor</param>
        /// <param name="total">Total del documento</param>
        /// <param name="uuid">UUID del comprobante</param>
        /// <returns>Ruta física del archivo generado</returns>
        string CreateCBB(string fullFilePath, string rfcEmisor, string rfcReceptor, decimal total, string uuid);

        /// <summary>
        /// Obtiene el total de timbres utilizados en el sistema
        /// </summary>
        /// <returns>Total de timbres utilizados</returns>
        int GetTotalTimbresUtilizados();
    }
}
