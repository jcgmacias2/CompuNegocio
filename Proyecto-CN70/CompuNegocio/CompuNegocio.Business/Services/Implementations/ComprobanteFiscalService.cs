using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using MessageBox = System.Windows.MessageBox;

namespace Aprovi.Business.Services
{
    public abstract class ComprobanteFiscalService : IComprobantFiscaleService
    {
        private IUnitOfWork _UOW;
        private IAplicacionesRepository _app;
        private IViewSeriesRepository _folios;
        private IDirectorioRepository _directorio;
        private ISeccionesRepository _secciones;
        private IImpuestoPorFacturaRepository _impuestoPorFactura;
        private IFacturasRepository _invoices;

        public ComprobanteFiscalService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _app = _UOW.Aplicaciones;
            _folios = _UOW.Folios;
            _directorio = _UOW.Directorio;
            _secciones = _UOW.Secciones;
            _impuestoPorFactura = _UOW.ImpuestoPorFactura;
            _invoices = _UOW.Facturas;
        }

        #region Generales

        public string GetSello(string cadenaOriginal, Configuracion config)
        {
            try
            {
                //Obtengo el archivo físico del certificado
                var pfxFile = _app.ReadSetting("CSD").ToString();

                X509Certificate2 certificate = new X509Certificate2(pfxFile, string.Format("Aprovi{0}", config.rfc.ToLower()), X509KeyStorageFlags.Exportable);
                RSACryptoServiceProvider key = certificate.PrivateKey as RSACryptoServiceProvider;
                RSACryptoServiceProvider rsaProv = new RSACryptoServiceProvider();
                rsaProv.ImportParameters(key.ExportParameters(true));
                byte[] selloDigitalData = rsaProv.SignData(Encoding.UTF8.GetBytes(cadenaOriginal), "SHA256");

                //verificar el sello
                var isValid = rsaProv.VerifyData(Encoding.UTF8.GetBytes(cadenaOriginal), "SHA256", selloDigitalData);

                if (isValid)
                    return Convert.ToBase64String(selloDigitalData);
                else
                    throw new Exception("No fue posible verificar el sello");

            }
            catch (CryptographicException)
            {
                throw new Exception("La contraseña del certificado que esta intentando utilizar no coincide con la configuración");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public XmlDocument Timbrar(XmlDocument xmlComprobante, Configuracion config)
        {
            try
            {
                HttpWebRequest webRequest;
                Stream requestWriter;
                JObject timbreRequest;
                JObject timbreParams;
                JObject respuesta;
                JObject result;
                byte[] content;
                string data;

                result = null;

                timbreParams = new JObject();
                timbreParams["user"] = config.usuarioPAC;
                timbreParams["pass"] = config.contraseñaPAC;
                timbreParams["RFC"] = config.rfc;
                timbreParams["xmldata"] = xmlComprobante.OuterXml;

                timbreRequest = new JObject();
                if (config.Mode.Equals(Ambiente.Production))
                    timbreRequest["id"] = string.Format("{0}{1}{2}", config.rfc, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Serie"].Value, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Folio"].Value);
                else
                    timbreRequest["id"] = string.Format("Mario{0}{1}{2}", config.rfc, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Serie"].Value, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Folio"].Value);
                timbreRequest["method"] = "cfd2cfdi";
                timbreRequest["params"] = timbreParams;

                var jsonEncode = HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest));
                content = Encoding.UTF8.GetBytes(string.Format("q={0}", HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest))));

                //La configuración me dice el ambiente en el que estoy trabajando
                if (config.Mode.Equals(Ambiente.Production))
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbre.php");
                else //Pruebas 4.0
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbreprueba.php");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = content.Length;

                requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(content, 0, content.Length);
                requestWriter.Close();

                respuesta = JObject.Parse(new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd());
                result = (JObject)respuesta["result"];
                switch (((string)result["retcode"]))
                {
                    case "1":
                        data = (string)result["data"];
                        break;
                    case "307":
                        data = RecuperarTimbre(xmlComprobante, config); //No le digo nada al usuario, solo lo recupero
                        break;
                    default:
                        throw new Exception((string)result["error"]);
                }

                xmlComprobante.LoadXml(data);

                return xmlComprobante;
            }
            catch (Exception)
            {
                xmlComprobante.Save(string.Format("{0}\\Error{1}-{2}-{3}.xml", _app.ReadSetting("Xml").ToString(), DateTime.Now.DayOfYear, DateTime.Now.Minute, DateTime.Now.Second));
                throw;
            }
        }

        public XmlDocument Timbrar_v2(XmlDocument xmlComprobante, Configuracion config)
        {
            try
            {
                HttpWebRequest webRequest;
                Stream requestWriter;
                JObject timbreRequest;
                JObject timbreParams;
                JObject respuesta;
                JObject result;
                byte[] content;
                string data;

                result = null;

                timbreParams = new JObject();
                timbreParams["user"] = config.usuarioPAC;
                timbreParams["pass"] = config.contraseñaPAC;
                timbreParams["RFC"] = config.rfc;
                timbreParams["xmldata"] = xmlComprobante.OuterXml;

                timbreRequest = new JObject();
                if (config.Mode.Equals(Ambiente.Production))
                    timbreRequest["id"] = "101";
                else
                    timbreRequest["id"] = string.Format("Mario{0}{1}{2}", config.rfc, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Serie"].Value, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Folio"].Value);
                
                timbreRequest["method"] = "cfd2cfdi";
                timbreRequest["params"] = timbreParams;

                var jsonEncode = HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest));
                content = Encoding.UTF8.GetBytes(string.Format("q={0}", HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest))));

                //La configuración me dice el ambiente en el que estoy trabajando
                if (config.Mode.Equals(Ambiente.Production))
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbre.php");
                else //Pruebas 4.0
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbreprueba.php");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = content.Length;

                requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(content, 0, content.Length);
                requestWriter.Close();

                respuesta = JObject.Parse(new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd());
                result = (JObject)respuesta["result"];
                switch (((string)result["retcode"]))
                {
                    case "1":
                        data = (string)result["data"];
                        break;
                    case "307":
                        data = RecuperarTimbre(xmlComprobante, config); //No le digo nada al usuario, solo lo recupero
                        break;
                    default:
                        throw new Exception((string)result["error"]);
                }

                xmlComprobante.LoadXml(data);

                return xmlComprobante;
            }
            catch (Exception)
            {
                xmlComprobante.Save(string.Format("{0}\\Error{1}-{2}-{3}.xml", _app.ReadSetting("Xml").ToString(), DateTime.Now.DayOfYear, DateTime.Now.Minute, DateTime.Now.Second));
                throw;
            }
        }

        public string RecuperarTimbre(XmlDocument xmlComprobante, Configuracion config)
        {
            HttpWebRequest webRequest;
            Stream requestWriter;
            JObject timbreRequest;
            JObject timbreParams;
            JObject respuesta;
            JObject result;
            byte[] content;
            DateTime fecha;

            result = null;

            try
            {
                timbreParams = new JObject();
                timbreParams["user"] = config.usuarioPAC;
                timbreParams["pass"] = config.contraseñaPAC;
                timbreParams["RFC"] = ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).GetElementsByTagName("cfdi:Emisor")[0].Attributes["Rfc"].Value;
                fecha = DateTime.SpecifyKind(DateTime.Parse(((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Fecha"].Value), DateTimeKind.Utc);
                timbreParams["fecha_inicio"] = string.Format("{0}-{1}-{2}", fecha.Year, fecha.Month.ToString("00"), fecha.Day.ToString("00"));
                timbreParams["fecha_final"] = string.Format("{0}-{1}-{2}", fecha.Year, fecha.Month.ToString("00"), fecha.Day.ToString("00"));

                timbreRequest = new JObject();
                if (config.Mode.Equals(Ambiente.Production))
                    timbreRequest["id"] = string.Format("{0}{1}{2}", config.rfc, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Serie"].Value, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Folio"].Value);
                else
                    timbreRequest["id"] = string.Format("Mario{0}{1}{2}", config.rfc, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Serie"].Value, ((XmlElement)xmlComprobante.GetElementsByTagName("cfdi:Comprobante")[0]).Attributes["Folio"].Value);
                timbreRequest["method"] = "buscarCFDI";
                timbreRequest["params"] = timbreParams;

                content = Encoding.UTF8.GetBytes(string.Format("q={0}", HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest))));

                //La configuración me dice el ambiente en el que estoy trabajando
                if (config.Mode.Equals(Ambiente.Production))
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbre.php");
                else
                    webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbreprueba.php");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = content.Length;

                requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(content, 0, content.Length);
                requestWriter.Close();

                respuesta = JObject.Parse(new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd());
                result = (JObject)respuesta["result"];


                if (!((string)result["retcode"]).Equals("1"))
                {
                    throw new Exception((string)result["error"]);
                }

                return (string)((JArray)result["data"])[0]["xml_data"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TimbresDePago GetTimbrePago(XmlDocument xmlPago)
        {
            try
            {
                XmlNode xNode;
                XmlNamespaceManager xNms;
                TimbresDePago timbre;

                xNms = new XmlNamespaceManager(xmlPago.NameTable);
                xNms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                xNms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                xNode = xmlPago.ChildNodes[1].SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", xNms);

                timbre = new TimbresDePago();
                timbre.version = xNode.Attributes["Version"].InnerText;
                timbre.selloSAT = xNode.Attributes["SelloSAT"].InnerText;
                timbre.selloCFD = xNode.Attributes["SelloCFD"].InnerText;
                timbre.noCertificadoSAT = xNode.Attributes["NoCertificadoSAT"].InnerText;
                timbre.UUID = xNode.Attributes["UUID"].InnerText;
                timbre.fechaTimbrado = xNode.Attributes["FechaTimbrado"].InnerText.ToDateFromUTC();
                timbre.leyenda = xNode.Attributes["Leyenda"].InnerText;
                timbre.rfcProvCertif = xNode.Attributes["RfcProvCertif"].InnerText;
                timbre.cadenaOriginal = string.Format("||1.1|{0}|{1}|{2}|{3}|{4}||", timbre.UUID, timbre.fechaTimbrado.ToUTCFormat(), timbre.rfcProvCertif, timbre.leyenda, timbre.selloCFD, timbre.noCertificadoSAT);

                return timbre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateCBB(string fullFilePath, string rfcEmisor, string rfcReceptor, decimal total, string uuid)
        {
            try
            {
                string code;
                //?re=RFCEmisor&rr=RFCReceptor&tt=Total17&id=UUID
                code = string.Format("?re={0}&rr={1}&tt={2}&id={3}", rfcEmisor, rfcReceptor, total.ToFormat17(), uuid);

                QRCode.FastQRCode(code, fullFilePath);

                return fullFilePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Cancelar(string uuid, string nocertificado, Configuracion config)
        {
            //q = {"id":1001,"method":"cfd2cfdi","params":{"user":"miemail@midominio.com","pass":"cabb17fb8536180e11af6dff0da42132","RFC":"EEM010101XYZ","pfx_pass":"Clave de mi archivo PFX","pfx_pem":"Archivo PFX de mi CSD","folios":["25916C58-672A-43CD-96EE-F14E0FDD4378"]}}

            HttpWebRequest webRequest;
            JObject timbreRequest;
            JObject timbreParams;
            JArray timbreFolios;
            JObject respuesta;
            JObject result;
            //X509Certificate2 certificate;
            byte[] content;

            result = null;

            //Obtengo el archivo físico del certificado
            var pfxFile = _app.ReadSetting("CSD").ToString();
            var pass = string.Format("Aprovi{0}", config.rfc.ToLower());
            timbreFolios = new JArray();
            timbreFolios.Add(uuid);
            timbreParams = new JObject();
            timbreParams["user"] = config.usuarioPAC;
            timbreParams["pass"] = config.contraseñaPAC;
            timbreParams["RFC"] = config.rfc.ToUpper();
            timbreParams["NoCertificado"] = nocertificado;
            //timbreParams["pfx_pass"] = pass;
            //certificate = new X509Certificate2(pfxFile, pass, X509KeyStorageFlags.Exportable);
            //timbreParams["pfx_pem"] = Convert.ToBase64String(certificate.Export(X509ContentType.Pkcs12, pass));
            timbreParams["folios"] = timbreFolios;

            timbreRequest = new JObject();
            //timbreRequest["id"] = uuid;
            timbreRequest["method"] = "cancelarCFDI";
            timbreRequest["params"] = timbreParams;

            content = Encoding.UTF8.GetBytes(string.Format("q={0}", HttpUtility.UrlEncode(JsonConvert.SerializeObject(timbreRequest))));

            MessageBox.Show("Datos de cancelación que se mandan a itimbre: " + timbreRequest.ToString(), "Datos de Cancelacion", MessageBoxButton.OK, MessageBoxImage.Information);

            //La configuración me dice el ambiente en el que estoy trabajando
            if (config.Mode.Equals(Ambiente.Production))
                webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbre.php");
            else
                webRequest = (HttpWebRequest)WebRequest.Create("https://portalws.itimbre.com/itimbreprueba.php");

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = content.Length;

            try
            {
                Stream requestWriter = webRequest.GetRequestStream();
                requestWriter.Write(content, 0, content.Length);
                requestWriter.Close();

                respuesta = JObject.Parse(new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd());
                result = (JObject)respuesta["result"];
                if (!((string)result["retcode"]).Equals("1"))
                {
                    throw new Exception("Error al realizar la cancelacion en itimbre: "+(string)result["error"]);
                }

                return (string)result["acuse"];
                //return "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><CancelaCFDResponse xmlns=\"http://cancelacfd.sat.gob.mx\"><CancelaCFDResult Fecha=\"2018-12-20T12:22:28.2969987\" RfcEmisor=\"CSO1211145F0\"><Folios><UUID>A19143A8-D876-464F-8346-BA5F2DE69DAD</UUID><EstatusUUID>201</EstatusUUID></Folios><Signature Id=\"SelloSAT\" xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\"/><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#hmac-sha512\"/><Reference URI=\"\"><Transforms><Transform Algorithm=\"http://www.w3.org/TR/1999/REC-xpath-19991116\"><XPath>not(ancestor-or-self::*[local-name()='Signature'])</XPath></Transform></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha512\"/><DigestValue>fpbCj0jw4uuZnhzSmjzsKk3l5qOoHcA4otBiE7ojXAWl8o7SWMcmVvh8QUQhk0UG7cqZ37UtIl0fL45s7yYewg==</DigestValue></Reference></SignedInfo><SignatureValue>iKiN8sgKX419KQ3T3lT2FVSrfx+SBCIg2/7uy4as0DD/lZgKjTajQnOreHIBnfMXVf59nRwhlaZJ5V1nTREXSw==</SignatureValue><KeyInfo><KeyName>00001088888800000031</KeyName><KeyValue><RSAKeyValue><Modulus>ujwIJaMKWWmawqDpHx/OS10pXzEh2SQhY02y64v9Q0+I+0dGlIrjFJeGrsHqAT3JoYnh38Dxwta98t/7++dh2hOgiZEwRignWRIlOgM1MefBHEyY+hi4vHpZgPKq/hJVfHf9nOvlb5UgIHMTCEwrDp3qk9O5XtTEycnWwiqleG0c1J9sfbRxC0gYBHsNTH85OEtSXYMkiWNYNnFbIc7B0sgp2y18jUxUCNFBMMTV0tz2sxRF+V4hblaPjI75RWmvs9E4lD7MVmW3z7LIlSajuSL8eOqoerSkQhPBABIeQenEPQwRTt3ej3XpVaBsOmagIPZZI3RvOVh+5mcXDE5txQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue></KeyValue></KeyInfo></Signature></CancelaCFDResult></CancelaCFDResponse></s:Body></s:Envelope>";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string CreateAcuse(string fullFilePath, string acuse)
        {
            try
            {
                XmlDocument xmlDoc;

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(acuse);
                xmlDoc.Save(fullFilePath);

                return fullFilePath;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTotalTimbresUtilizados()
        {
            try
            {
                return _folios.List().Sum(f => f.timbresConsumidos).GetValueOrDefault(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Facturas

        public string GetCadenaOriginal(VMFactura invoice, Configuracion config)
        {
            try
            {
                StringBuilder cadena;

                //Comprobante
                cadena = new StringBuilder();
                cadena.Append("||"); // Inicio de la cadena con un doble pipe
                cadena.Append("4.0|"); // Version 
                cadena.AppendFormat("{0}|", invoice.serie.Trim()); // Serie
                cadena.AppendFormat("{0}|", invoice.folio.ToString().Trim()); // Folio
                cadena.AppendFormat("{0}|", invoice.fechaHora.AddHours(-6).ToUTCFormat()); // Fecha
                if (invoice.NotasDeCreditoes.Any(x => x.IsPreSaleCreditNote(invoice)))
                {
                    //Si hay un cfdi de egresos a venta futura asociado, debe ser la clave 23 (novacion)
                    cadena.AppendFormat("{0}|", "23"); // Novacion
                }
                else if (invoice.AbonosDeFacturas.Count > 0)
                {
                    var abono = Operations.GetFormaDePago(invoice.AbonosDeFacturas);
                    if (abono.isValid())
                        cadena.AppendFormat("{0}|", abono.FormasPago.codigo); //FormaPago
                }
                else
                    cadena.AppendFormat("{0}|", "99"); // Por definir
                cadena.AppendFormat("{0}|", invoice.noCertificado.Trim()); // NoCertificado
                if (invoice.Cliente.condicionDePago.isValid())
                    cadena.AppendFormat("{0}|", invoice.Cliente.condicionDePago.Trim()); // CondicionDePago
                cadena.AppendFormat("{0}|", invoice.Subtotal.ToDecimalString()); //Subtotal antes de impuestos y descuentos
                cadena.AppendFormat("{0}|", invoice.Moneda.codigo.Trim()); // Moneda
                if (!invoice.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                    cadena.AppendFormat("{0}|", invoice.tipoDeCambio.ToDecimalString()); // TipoDePago
                cadena.AppendFormat("{0}|", invoice.Total.ToStringRoundedCurrency(invoice.Moneda));
                cadena.Append("I|"); //tipo de comprobante
                cadena.AppendFormat("{0}|", "01"); //Exportacion JCRV
                cadena.AppendFormat("{0}|", invoice.MetodosPago.codigo); //MetodoPago
                cadena.AppendFormat("{0}|", config.Domicilio.codigoPostal); //LugarExpedición

                //CadenaOriginal CFDIRelacionados (Sustitución)
                if (invoice.idComprobanteOriginal.isValid() && invoice.idComprobanteOriginal > 0)
                {
                    cadena.AppendFormat("{0}|", invoice.TiposRelacion.codigo);
                    cadena.AppendFormat("{0}|", invoice.Factura1.TimbresDeFactura.UUID);
                }

                //CFDIRelacionados (egresos)
                if (invoice.NotasDeCreditoes.Any(x => x.IsPreSaleCreditNote(invoice)))
                {
                    cadena.AppendFormat("{0}|", "02");

                    foreach (var n in invoice.NotasDeCreditoes)
                    {
                        cadena.AppendFormat("{0}|", n.TimbresDeNotasDeCredito.UUID);
                    }
                }

                //Periodicidad
                if (invoice.Periodicidad.EsFacturaGlobal)
                {
                    cadena.AppendFormat("{0}|", invoice.Periodicidad.CodigoPeriodicidad);
                    cadena.AppendFormat("{0}|", invoice.Periodicidad.Mes);
                    cadena.AppendFormat("{0}|", invoice.Periodicidad.Year);
                }

                //Emisor
                cadena.AppendFormat("{0}|", config.rfc);
                cadena.AppendFormat("{0}|", config.razonSocial.Trim());
                cadena.AppendFormat("{0}|", config.CodigoRegimen);

                //Receptor
                cadena.AppendFormat("{0}|", invoice.Cliente.rfc.Trim());
                cadena.AppendFormat("{0}|", invoice.Cliente.razonSocial.Trim());
                if (!invoice.Cliente.Domicilio.Pais.idPais.Equals((int)Paises.México)) //Solo cuando es extranjero
                    cadena.AppendFormat("{0}|", invoice.Cliente.Domicilio.Pais.codigo);
                cadena.AppendFormat("{0}|", invoice.Cliente.Domicilio.codigoPostal); //Codigo Postal Receptor JCRV
                cadena.AppendFormat("{0}|", invoice.Cliente.Regimene.codigo); // Regimen Receptor JCRV
                cadena.AppendFormat("{0}|", invoice.UsosCFDI.codigo);

                //Conceptos
                foreach (var concepto in invoice.DetalleDeFactura)
                {
                    cadena.AppendFormat("{0}|", concepto.Articulo.ProductosServicio.codigo);
                    cadena.AppendFormat("{0}|", concepto.Articulo.codigo);
                    cadena.AppendFormat("{0}|", concepto.cantidad.ToDecimalString());
                    cadena.AppendFormat("{0}|", concepto.Articulo.UnidadesDeMedida.codigo);
                    cadena.AppendFormat("{0}|", concepto.Articulo.UnidadesDeMedida.descripcion);

                    //Tratándose de las ventas de primera mano, en este campo se debe registrar la fecha del documento aduanero, la cual se puede registrar utilizando un formato libre, ya sea antes o después de la descripción del producto.
                    if (concepto.PedimentoPorDetalleDeFacturas.Count > 0)
                    {
                        var descripcion = string.Format("{0} Fecha importación:", concepto.Articulo.descripcion);
                        concepto.PedimentoPorDetalleDeFacturas.ToList().ForEach(p => descripcion = string.Format("{0} {1}", descripcion, p.Pedimento.fecha.ToShortDateString()));
                        cadena.AppendFormat("{0}|", descripcion);
                    }
                    else
                        cadena.AppendFormat("{0}|", concepto.Articulo.descripcion);

                    cadena.AppendFormat("{0}|", concepto.precioUnitario.ToStringRoundedCurrency(invoice.Moneda));
                    cadena.AppendFormat("{0}|", (concepto.cantidad * concepto.precioUnitario).ToStringRoundedCurrency(invoice.Moneda));
                    cadena.AppendFormat("{0}|", (concepto.Impuestos.Count > 0) ? "02" : "01"); //ObjetoImp JCRV

                    if (concepto.Impuestos.Count > 0)
                    {
                        var traslados = concepto.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));
                        if (traslados.Count() > 0)
                        {
                            foreach (var impuesto in traslados)
                            {
                                var baseImpuesto = Operations.CalculateTaxBase(Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento), concepto.cantidad, impuesto, concepto.Impuestos.ToList()); // Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento) * concepto.cantidad;
                                cadena.AppendFormat("{0}|", baseImpuesto.ToStringRoundedCurrency(invoice.Moneda));
                                cadena.AppendFormat("{0}|", impuesto.codigo);
                                cadena.AppendFormat("{0}|", impuesto.TiposFactor.codigo);
                                cadena.AppendFormat("{0}|", impuesto.valor.ToPorcentageString());
                                var taxes = new List<Impuesto>();
                                taxes.Add(impuesto);
                                cadena.AppendFormat("{0}|", Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes)).ToTdCFDI_Importe());
                            }
                        }

                        var retenciones = concepto.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido));
                        if (retenciones.Count() > 0)
                        {
                            foreach (var impuesto in retenciones)
                            {
                                var baseImpuesto = Operations.CalculateTaxBase(Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento), concepto.cantidad, impuesto, concepto.Impuestos.ToList()); // Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento) * concepto.cantidad;
                                cadena.AppendFormat("{0}|", baseImpuesto.ToStringRoundedCurrency(invoice.Moneda));
                                cadena.AppendFormat("{0}|", impuesto.codigo);
                                cadena.AppendFormat("{0}|", impuesto.TiposFactor.codigo);
                                cadena.AppendFormat("{0}|", impuesto.valor.ToPorcentageString());
                                var taxes = new List<Impuesto>();
                                taxes.Add(impuesto);
                                cadena.AppendFormat("{0}|", Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes)).ToTdCFDI_Importe());
                            }
                        }
                    }

                    //Pedimentos
                    foreach (var pedimento in concepto.PedimentoPorDetalleDeFacturas)
                    {
                        //Le agrego el dato del pedimento
                        cadena.AppendFormat("{0}|", string.Format("{0}  {1}  {2}  {3}{4}", pedimento.Pedimento.añoOperacion, pedimento.Pedimento.aduana, pedimento.Pedimento.patente, pedimento.Pedimento.añoEnCurso, pedimento.Pedimento.progresivo));
                    }

                    //Cuenta Predial
                    if (concepto.CuentaPredialPorDetalle.isValid() && concepto.CuentaPredialPorDetalle.idCuentaPredialPorDetalle.isValid())
                        cadena.AppendFormat("{0}|", concepto.CuentaPredialPorDetalle.cuenta);
                }

                //Impuestos Retenidos
                var totalRetenciones = invoice.Impuestos.Where(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(i => i.Importe.ToRoundedCurrency(invoice.Moneda));
                var cantidadRetenciones = invoice.Impuestos.Count(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido));
                if (totalRetenciones >= 0.0m && cantidadRetenciones > 0)
                {
                    foreach (VMImpuesto imp in invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)))
                    {
                        cadena.AppendFormat("{0}|", imp.codigo);
                        cadena.AppendFormat("{0}|", imp.Importe.ToStringRoundedCurrency(invoice.Moneda));
                    }
                    cadena.AppendFormat("{0}|", totalRetenciones.ToStringRoundedCurrency(invoice.Moneda));
                }

                //Impuestos Trasladados
                var totalTraslados = invoice.Impuestos.Where(it => it.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(i => i.Importe.ToRoundedCurrency(invoice.Moneda));
                var cantidadTraslados = invoice.Impuestos.Count(it => it.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));
                if (totalTraslados >= 0.0m && cantidadTraslados > 0)
                {
                    foreach (VMImpuesto imp in invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)))
                    {
                        cadena.AppendFormat("{0}|", imp.MontoGravable.ToStringRoundedCurrency(invoice.Moneda)); //Base JCRV
                        cadena.AppendFormat("{0}|", imp.codigo);
                        cadena.AppendFormat("{0}|", imp.TiposFactor.codigo);
                        cadena.AppendFormat("{0}|", imp.valor.ToPorcentageString());
                        cadena.AppendFormat("{0}|", imp.Importe.ToStringRoundedCurrency(invoice.Moneda));
                    }
                    cadena.AppendFormat("{0}|", totalTraslados.ToStringRoundedCurrency(invoice.Moneda));
                }

                cadena.Append("|"); //Finaliza la cadena

                return cadena.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public XmlDocument CreateCFDI(VMFactura invoice, Configuracion config, bool requiresAddenda)
        {
            try
            {
                XmlDocument xmlDoc;

                xmlDoc = new XmlDocument();
                xmlDoc.CreateProcessingInstruction("xml", "version= \"1.0\" encoding=\"UTF-8\"");
                XmlElement nodoComprobante = Comprobante(xmlDoc, invoice, config);
                xmlDoc.AppendChild(nodoComprobante);
                nodoComprobante.Attributes.Append(Schema(xmlDoc, false));
                if (invoice.idComprobanteOriginal.isValid() || invoice.NotasDeCreditoes.Any(x => x.IsPreSaleCreditNote(invoice)))
                    CFDIRelacionados(xmlDoc, invoice).ForEach(x => nodoComprobante.AppendChild(x));

                if (invoice.Periodicidad != null)
                {
                    if (invoice.Cliente.rfc == "XAXX010101000" && invoice.Periodicidad.EsFacturaGlobal != null && invoice.Periodicidad.EsFacturaGlobal) //RVC Pendiente ver como manejar la factura global desde la pantalla de ventas/facturacion
                        nodoComprobante.AppendChild(CFDIInformacionGlobal(xmlDoc, invoice));
                }

                nodoComprobante.AppendChild(Emisor(xmlDoc, config));
                nodoComprobante.AppendChild(Receptor(xmlDoc, invoice.Cliente, invoice.UsosCFDI, invoice.Cliente.Domicilio, invoice.Cliente.Regimene));
                nodoComprobante.AppendChild(Conceptos(xmlDoc, invoice));
                if (invoice.Impuestos.Count > 0)
                    nodoComprobante.AppendChild(Impuestos(xmlDoc, invoice.Impuestos, (MetodoDePago)invoice.idMetodoPago, invoice.Moneda));

                if (requiresAddenda)
                {
                    AddendaDeCliente addenda = invoice.Cliente.AddendaDeClientes.First();
                    List<DatosExtraPorFactura> datos = invoice.DatosExtraPorFacturas.ToList();

                    XmlNode nodoImportado = nodoComprobante.OwnerDocument.ImportNode(Addenda(xmlDoc, addenda, datos, invoice), true);
                    nodoComprobante.AppendChild(nodoImportado);
                }

                return xmlDoc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TimbresDeFactura GetTimbreFactura(XmlDocument xmlFactura)
        {
            try
            {
                XmlNode xNode;
                XmlNamespaceManager xNms;
                TimbresDeFactura timbre;

                xNms = new XmlNamespaceManager(xmlFactura.NameTable);
                xNms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                xNms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                xNode = xmlFactura.ChildNodes[1].SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", xNms);

                timbre = new TimbresDeFactura();
                timbre.version = xNode.Attributes["Version"].InnerText;
                timbre.selloSAT = xNode.Attributes["SelloSAT"].InnerText;
                timbre.selloCFD = xNode.Attributes["SelloCFD"].InnerText;
                timbre.noCertificadoSAT = xNode.Attributes["NoCertificadoSAT"].InnerText;
                timbre.UUID = xNode.Attributes["UUID"].InnerText;
                timbre.fechaTimbrado = xNode.Attributes["FechaTimbrado"].InnerText.ToDateFromUTC();
                timbre.Leyenda = xNode.Attributes["Leyenda"].InnerText;
                timbre.RfcProvCertif = xNode.Attributes["RfcProvCertif"].InnerText;
                timbre.cadenaOriginal = string.Format("||1.1|{0}|{1}|{2}|{3}|{4}||", timbre.UUID, timbre.fechaTimbrado.ToUTCFormat(), timbre.RfcProvCertif, timbre.Leyenda, timbre.selloCFD, timbre.noCertificadoSAT);

                return timbre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Parcialidades

        public string GetCadenaOriginal(VMFactura invoice, AbonosDeFactura payment, Configuracion config)
        {
            try
            {
                StringBuilder cadena;

                //Comprobante
                cadena = new StringBuilder();
                cadena.Append("||"); // Inicio de la cadena con un doble pipe
                cadena.Append("4.0|"); // Version 
                cadena.AppendFormat("{0}|", payment.TimbresDeAbonosDeFactura.serie.Trim()); // Serie
                cadena.AppendFormat("{0}|", payment.TimbresDeAbonosDeFactura.folio.ToString().Trim()); // Folio
                cadena.AppendFormat("{0}|", payment.fechaHora.AddHours(-6).ToUTCFormat()); // Fecha
                cadena.AppendFormat("{0}|", payment.TimbresDeAbonosDeFactura.noCertificado.Trim()); // NoCertificado
                cadena.AppendFormat("{0}|", "0"); //Subtotal antes de impuestos y descuentos
                cadena.AppendFormat("{0}|", "XXX"); // Moneda
                cadena.AppendFormat("{0}|", "0"); //Total
                cadena.Append("P|"); //tipo de comprobante
                cadena.AppendFormat("{0}|", "01"); //Exportacion JCRV /*Preguntar que pex */
                cadena.AppendFormat("{0}|", config.Domicilio.codigoPostal); //LugarExpedición

                //Emisor
                cadena.AppendFormat("{0}|", config.rfc);
                cadena.AppendFormat("{0}|", config.razonSocial.Trim());
                //cadena.AppendFormat("{0}|", invoice.Regimene.codigo);
                cadena.AppendFormat("{0}|", config.CodigoRegimen);

                //Receptor
                cadena.AppendFormat("{0}|", invoice.Cliente.rfc.Trim());
                cadena.AppendFormat("{0}|", invoice.Cliente.razonSocial.Trim());
                if (!invoice.Cliente.Domicilio.Pais.idPais.Equals((int)Paises.México)) //Solo cuando es extranjero
                    cadena.AppendFormat("{0}|", invoice.Cliente.Domicilio.Pais.codigo);
                cadena.AppendFormat("{0}|", invoice.Cliente.Domicilio.codigoPostal); //Codigo Postal Receptor JCRV
                cadena.AppendFormat("{0}|", invoice.Cliente.Regimene.codigo); // Regimen Receptor JCRV
                cadena.AppendFormat("{0}|", "CP01"); //UsoCFDI

                //Conceptos
                //Por el momento es solo un concepto a la vez
                cadena.AppendFormat("{0}|", "84111506");
                cadena.AppendFormat("{0}|", (1.0m).ToDecimalString());
                cadena.AppendFormat("{0}|", "ACT");
                cadena.AppendFormat("{0}|", "Pago");
                cadena.AppendFormat("{0}|", "0");
                cadena.AppendFormat("{0}|", "0");
                cadena.AppendFormat("{0}|", "01"); //ObjetoImp JCRV

                //Complemento de pago
                cadena.AppendFormat("{0}|", "2.0"); //Version Complemento Pago

                var totales = payment.monto.ToRoundedCurrency(payment.Moneda); //Totales JCRV
                cadena.AppendFormat("{0}|", totales.ToDecimalString()); //MontoTotalPagos JCRV 

                cadena.AppendFormat("{0}|", payment.fechaHora.ToUTCFormat()); //FechaPago
                cadena.AppendFormat("{0}|", payment.FormasPago.codigo); //FormaPagoP
                cadena.AppendFormat("{0}|", payment.Moneda.codigo); //MonedaP
                /*
                if (!payment.idMoneda.Equals((int)Monedas.Pesos)) // tipoCambio
                    cadena.AppendFormat("{0}|", payment.tipoDeCambio.ToDecimalString()); //TipoCambioP
                else
                    cadena.AppendFormat("{0}|", "1"); //TipoCambioP
                */

                /* JCRV 07/04/23 Se solicito que se pusiera 1 cuando fuera pago en dolares */
                if (payment.idMoneda.Equals((int)Monedas.Pesos) || payment.idMoneda.Equals((int)Monedas.Dólares)) //Si no es pesos se incluye
                    cadena.AppendFormat("{0}|", "1");
                else
                    cadena.AppendFormat("{0}|", payment.tipoDeCambio.ToDecimalString());

                cadena.AppendFormat("{0}|", payment.monto.ToDecimalString()); //Monto
                cadena.AppendFormat("{0}|", invoice.TimbresDeFactura.UUID); //idDocumentoRelacionado
                cadena.AppendFormat("{0}|", invoice.serie); //SerieDocumentoRelacionado
                cadena.AppendFormat("{0}|", invoice.folio.ToString()); //FolioDocumentoRelacionado
                cadena.AppendFormat("{0}|", invoice.Moneda.codigo); //MonedaDocumentoRelacionado

                /*
                if (!invoice.idMoneda.Equals(payment.idMoneda)) //Cuando son distintas debe agregarse tipo de cambio
                {
                    if (invoice.Moneda.codigo.Equals("MXN")) //CRP220 si el valor del atributo MonedaDR es MXN y el valor MonedaP es diferente, el atributo TipoCambioDR debe tener el valor 1
                        cadena.AppendFormat("{0}|", "1"); //MonedaPago
                    else
                        cadena.AppendFormat("{0}|", invoice.tipoDeCambio.ToStringRoundedCurrency(invoice.Moneda)); //MonedaPago
                }*/

                /*JCRV Para Pago20 se cambia TipoCambioDR por EquivalenciaDR */
                cadena.AppendFormat("{0}|", (!invoice.idMoneda.Equals(payment.idMoneda) ? invoice.tipoDeCambio.ToStringRoundedCurrency(invoice.Moneda) : "1"));

                //cadena.AppendFormat("{0}|", invoice.MetodosPago.codigo); //MetodoDePagoDocumentoRelacionado
                var numParcialidad = invoice.AbonosDeFacturas.Count(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado && a.fechaHora <= payment.fechaHora.ToNextMidnight()); // Abonos anteriores 
                cadena.AppendFormat("{0}|", numParcialidad.ToString()); //NumParcialidad
                var abonoParcial = payment.monto.ToDocumentCurrency(payment.Moneda, invoice.Moneda, payment.tipoDeCambio); //cantidad abonada respecto a la moneda del documento
                cadena.AppendFormat("{0}|", (invoice.Total - invoice.Abonado + abonoParcial).ToStringRoundedCurrency(invoice.Moneda)); //ImpSaldoAnt
                cadena.AppendFormat("{0}|", abonoParcial.ToStringRoundedCurrency(invoice.Moneda)); //ImpPagadoDocumentoRelacionado
                cadena.AppendFormat("{0}|", (invoice.Total - invoice.Abonado).ToStringRoundedCurrency(invoice.Moneda)); //ImpSaldoInsoluto
                cadena.AppendFormat("{0}|", invoice.ImpuestoPorFacturas.Count() > 0 ? "02" : "01"); //ObjetoImpDR JCRV validar que valor poner*****************

                if (invoice.ImpuestoPorFacturas.Count() > 0) 
                { 
                    foreach (var impuesto in invoice.ImpuestoPorFacturas.ToList())
                    {
                        var base_imp = abonoParcial / (1 + impuesto.valorTasaOCuaota);
                        //cadena.AppendFormat("{0}|", impuesto.@base.ToStringRoundedCurrency(invoice.Moneda)); //BaseDR
                        cadena.AppendFormat("{0}|", base_imp.ToStringRoundedCurrency(invoice.Moneda)); //BaseDR
                        cadena.AppendFormat("{0}|", impuesto.codigoImpuesto); //ImpuestoDR
                        cadena.AppendFormat("{0}|", impuesto.codigoTipoFactor); //TipoFactorDR
                        cadena.AppendFormat("{0}|", Math.Abs(impuesto.valorTasaOCuaota).ToTdCFDI_Importe()); //TasaOCuotaDR JCRV
                        cadena.AppendFormat("{0}|", (base_imp * impuesto.valorTasaOCuaota).ToStringRoundedCurrency(invoice.Moneda)); //ImporteDR JCRV
                    }
                }

                cadena.Append("|"); //Finaliza la cadena

                return cadena.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public string GetCadenaOriginal(Pago payment, Configuracion config)
        {
            try
            {
                StringBuilder cadena;

                //Comprobante
                cadena = new StringBuilder();
                cadena.Append("||"); // Inicio de la cadena con un doble pipe
                cadena.Append("4.0|"); // Version 
                cadena.AppendFormat("{0}|", payment.serie.Trim()); // Serie
                cadena.AppendFormat("{0}|", payment.folio.ToString().Trim()); // Folio
                cadena.AppendFormat("{0}|", payment.fechaHora.AddHours(-6).ToUTCFormat()); // Fecha
                cadena.AppendFormat("{0}|", payment.TimbresDePago.noCertificado.Trim()); // NoCertificado
                cadena.AppendFormat("{0}|", "0"); //Subtotal antes de impuestos y descuentos
                cadena.AppendFormat("{0}|", "XXX"); // Moneda
                cadena.AppendFormat("{0}|", "0"); //Total
                cadena.Append("P|"); //tipo de comprobante
                cadena.AppendFormat("{0}|", "01"); //Exportacion JCRV /*Preguntar que pex */
                cadena.AppendFormat("{0}|", config.Domicilio.codigoPostal); //LugarExpedición

                //Emisor
                cadena.AppendFormat("{0}|", config.rfc);
                cadena.AppendFormat("{0}|", config.razonSocial.Trim());
                //cadena.AppendFormat("{0}|", payment.Regimene.codigo);
                cadena.AppendFormat("{0}|", config.CodigoRegimen);

                //Receptor
                cadena.AppendFormat("{0}|", payment.Cliente.rfc.Trim());
                cadena.AppendFormat("{0}|", payment.Cliente.razonSocial.Trim());
                if (!payment.Cliente.Domicilio.Pais.idPais.Equals((int)Paises.México)) //Solo cuando es extranjero
                    cadena.AppendFormat("{0}|", payment.Cliente.Domicilio.Pais.codigo);
                cadena.AppendFormat("{0}|", payment.Cliente.Domicilio.codigoPostal); //Codigo Postal Receptor JCRV
                cadena.AppendFormat("{0}|", payment.Cliente.Regimene.codigo); // Regimen Receptor JCRV
                cadena.AppendFormat("{0}|", "CP01"); //UsoCFDI

                //Conceptos
                //Por el momento es solo un concepto a la vez
                cadena.AppendFormat("{0}|", "84111506");
                cadena.AppendFormat("{0}|", (1.0m).ToDecimalString());
                cadena.AppendFormat("{0}|", "ACT");
                cadena.AppendFormat("{0}|", "Pago");
                cadena.AppendFormat("{0}|", "0");
                cadena.AppendFormat("{0}|", "0");
                cadena.AppendFormat("{0}|", "01"); //ObjetoImp  JCRV

                //Complemento de pago
                cadena.AppendFormat("{0}|", "2.0"); //Version Complemento Pago

                var totales = payment.AbonosDeFacturas.Sum(p => p.monto.ToRoundedCurrency(p.Moneda));//Totales JCRV
                cadena.AppendFormat("{0}|", totales.ToDecimalString()); //MontoTotalPagos JCRV 

                //Despues de aqui se debe repetir por cada pago generado(http://www.sat.gob.mx/informacion_fiscal/factura_electronica/Documents/Complementoscfdi/Pagos10.pdf)
                for (int i = 0; i < payment.AbonosDeFacturas.Count; i++)
                {
                    var p = payment.AbonosDeFacturas.ElementAt(i);
                    var invoice = new VMFactura(p.Factura);

                    cadena.AppendFormat("{0}|", p.fechaHora.ToUTCFormat()); //FechaPago
                    cadena.AppendFormat("{0}|", p.FormasPago.codigo); //FormaPago
                    cadena.AppendFormat("{0}|", p.Moneda.codigo); //MonedaPago
                    //Aqui se debe agregar el tipo de cambio si es que aplica
                    /*
                    if (p.idMoneda != (int)Monedas.Pesos)
                        cadena.AppendFormat("{0}|", p.tipoDeCambio); //TipoCambioP
                    else
                        cadena.AppendFormat("{0}|", "1"); //TipoCambioP
                    */

                    /* JCRV 07/04/23 Se solicito que se pusiera 1 cuando fuera pago en dolares */
                    if (p.idMoneda == (int)Monedas.Pesos || p.idMoneda == (int)Monedas.Dólares)
                        cadena.AppendFormat("{0}|", "1"); //TipoCambioP
                    else
                        cadena.AppendFormat("{0}|", p.tipoDeCambio); //TipoCambioP

                    cadena.AppendFormat("{0}|", p.monto.ToDecimalString()); //Monto
                    cadena.AppendFormat("{0}|", invoice.TimbresDeFactura.UUID); //idDocumentoRelacionado
                    cadena.AppendFormat("{0}|", invoice.serie); //SerieDocumentoRelacionado
                    cadena.AppendFormat("{0}|", invoice.folio.ToString()); //FolioDocumentoRelacionado
                    cadena.AppendFormat("{0}|", invoice.Moneda.codigo); //MonedaDocumentoRelacionado
                    
                    /*
                    if (!invoice.idMoneda.Equals(p.idMoneda)) //Cuando son distintas debe agregarse tipo de cambio
                    {
                        if (invoice.Moneda.codigo.Equals("MXN")) //CRP220 si el valor del atributo MonedaDR es MXN y el valor MonedaP es diferente, el atributo TipoCambioDR debe tener el valor 1
                            cadena.AppendFormat("{0}|", "1"); //MonedaPago
                        else
                            cadena.AppendFormat("{0}|", invoice.tipoDeCambio.ToStringRoundedCurrency(invoice.Moneda)); //MonedaPago
                    }
                    */

                    /*JCRV Para Pago20 se cambia TipoCambioDR por EquivalenciaDR */
                    cadena.AppendFormat("{0}|", (!invoice.idMoneda.Equals(p.idMoneda) ? invoice.tipoDeCambio.ToStringRoundedCurrency(invoice.Moneda) : "1"));

                    //cadena.AppendFormat("{0}|", invoice.MetodosPago.codigo); //MetodoDePagoDocumentoRelacionado
                    var numParcialidad = invoice.AbonosDeFacturas.Count(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado && a.fechaHora <= payment.fechaHora.ToNextMidnight()); // Abonos anteriores 
                    cadena.AppendFormat("{0}|", numParcialidad.ToString()); //NumParcialidad
                    var abonoParcial = p.monto.ToDocumentCurrency(p.Moneda, invoice.Moneda, payment.tipoDeCambio); //cantidad abonada respecto a la moneda del documento
                    cadena.AppendFormat("{0}|", (invoice.Total - invoice.Abonado + abonoParcial - invoice.Acreditado).ToStringRoundedCurrency(invoice.Moneda)); //ImpSaldoAnt
                    cadena.AppendFormat("{0}|", abonoParcial.ToStringRoundedCurrency(invoice.Moneda)); //ImpPagadoDocumentoRelacionado
                    cadena.AppendFormat("{0}|", (invoice.Total - invoice.Abonado - invoice.Acreditado).ToStringRoundedCurrency(invoice.Moneda)); //ImpSaldoInsoluto
                    cadena.AppendFormat("{0}|", invoice.ImpuestoPorFacturas.Count() > 0 ? "02" : "01"); //ObjetoImpDR JCRV

                    /** JCRV SECCION DE IMPUESTOS**/
                    if (invoice.ImpuestoPorFacturas.Count() > 0)
                    {
                        foreach(var impuesto in invoice.ImpuestoPorFacturas.ToList())
                        {
                            var base_imp = abonoParcial / (1 + impuesto.valorTasaOCuaota);
                            //cadena.AppendFormat("{0}|", impuesto.@base.ToStringRoundedCurrency(invoice.Moneda)); //BaseDR
                            cadena.AppendFormat("{0}|", base_imp.ToStringRoundedCurrency(invoice.Moneda)); //BaseDR
                            cadena.AppendFormat("{0}|", impuesto.codigoImpuesto); //ImpuestoDR
                            cadena.AppendFormat("{0}|", impuesto.codigoTipoFactor); //TipoFactorDR
                            cadena.AppendFormat("{0}|", Math.Abs(impuesto.valorTasaOCuaota).ToTdCFDI_Importe()); //TasaOCuotaDR JCRV
                            cadena.AppendFormat("{0}|", (base_imp * impuesto.valorTasaOCuaota).ToStringRoundedCurrency(invoice.Moneda)); //ImporteDR JCRV
                        }
                    }

                }

                cadena.Append("|"); //Finaliza la cadena

                return cadena.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }

        }

        public XmlDocument CreateCFDI(VMFactura invoice, AbonosDeFactura payment, Configuracion config)
        {
            try
            {
                XmlDocument xmlDoc;

                var impuestos = new List<VMImpuesto>();
                foreach (var i in invoice.Impuestos)
                {
                    impuestos.Add(new VMImpuesto(i, payment.monto)); //Este constructor me calcula el importe, con solo darle el monto gravable
                }

                xmlDoc = new XmlDocument();
                xmlDoc.CreateProcessingInstruction("xml", "version= \"1.0\" encoding=\"UTF-8\"");
                XmlElement nodoComprobante = Comprobante(xmlDoc, payment, config);
                xmlDoc.AppendChild(nodoComprobante);
                nodoComprobante.Attributes.Append(Schema(xmlDoc, true));
                nodoComprobante.AppendChild(Emisor(xmlDoc, config));
                nodoComprobante.AppendChild(Receptor(xmlDoc, invoice.Cliente, new UsosCFDI() { codigo = "CP01" }, invoice.Cliente.Domicilio, invoice.Cliente.Regimene));
                nodoComprobante.AppendChild(Conceptos(xmlDoc, payment));
                nodoComprobante.AppendChild(Pagos(xmlDoc, invoice, payment));

                return xmlDoc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public XmlDocument CreateCFDI(Pago payment, Configuracion config)
        {
            try
            {
                XmlDocument xmlDoc;

                xmlDoc = new XmlDocument();
                xmlDoc.CreateProcessingInstruction("xml", "version= \"1.0\" encoding=\"UTF-8\"");

                XmlElement nodoComprobante = Comprobante(xmlDoc, payment, config);
                xmlDoc.AppendChild(nodoComprobante);
                nodoComprobante.Attributes.Append(Schema(xmlDoc, true));
                nodoComprobante.AppendChild(Emisor(xmlDoc, config));
                nodoComprobante.AppendChild(Receptor(xmlDoc, payment.Cliente, new UsosCFDI() { codigo = "CP01" }, payment.Cliente.Domicilio, payment.Cliente.Regimene));
                nodoComprobante.AppendChild(Conceptos(xmlDoc, payment));
                nodoComprobante.AppendChild(Pagos(xmlDoc, payment));

                return xmlDoc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TimbresDeAbonosDeFactura GetTimbreAbono(XmlDocument xmlPago)
        {
            try
            {
                XmlNode xNode;
                XmlNamespaceManager xNms;
                TimbresDeAbonosDeFactura timbre;

                xNms = new XmlNamespaceManager(xmlPago.NameTable);
                xNms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                xNms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                xNode = xmlPago.ChildNodes[1].SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", xNms);

                timbre = new TimbresDeAbonosDeFactura();
                timbre.version = xNode.Attributes["Version"].InnerText;
                timbre.selloSAT = xNode.Attributes["SelloSAT"].InnerText;
                timbre.selloCFD = xNode.Attributes["SelloCFD"].InnerText;
                timbre.noCertificadoSAT = xNode.Attributes["NoCertificadoSAT"].InnerText;
                timbre.UUID = xNode.Attributes["UUID"].InnerText;
                timbre.fechaTimbrado = xNode.Attributes["FechaTimbrado"].InnerText.ToDateFromUTC();
                timbre.Leyenda = xNode.Attributes["Leyenda"].InnerText;
                timbre.RfcProvCertif = xNode.Attributes["RfcProvCertif"].InnerText;
                timbre.cadenaOriginalAbono = string.Format("||1.1|{0}|{1}|{2}|{3}|{4}||", timbre.UUID, timbre.fechaTimbrado.ToUTCFormat(), timbre.RfcProvCertif, timbre.Leyenda, timbre.selloCFD, timbre.noCertificadoSAT);

                return timbre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Notas de crédito

        public string GetCadenaOriginal(VMNotaDeCredito creditNote, Configuracion config)
        {
            try
            {
                StringBuilder cadena;

                //Comprobante
                cadena = new StringBuilder();
                cadena.Append("||"); // Inicio de la cadena con un doble pipe
                cadena.Append("4.0|"); // Version 
                cadena.AppendFormat("{0}|", creditNote.serie.Trim()); // Serie
                cadena.AppendFormat("{0}|", creditNote.folio.ToString().Trim()); // Folio
                cadena.AppendFormat("{0}|", creditNote.fechaHora.ToUTCFormat()); // Fecha
                cadena.AppendFormat("{0}|", creditNote.FormasPago.codigo);
                cadena.AppendFormat("{0}|", creditNote.TimbresDeNotasDeCredito.noCertificado.Trim()); // NoCertificado
                if (creditNote.Cliente.condicionDePago.isValid())
                    cadena.AppendFormat("{0}|", creditNote.Cliente.condicionDePago.Trim()); // CondicionDePago
                cadena.AppendFormat("{0}|", creditNote.Subtotal.ToStringRoundedCurrency(creditNote.Moneda)); //Subtotal antes de impuestos y descuentos
                cadena.AppendFormat("{0}|", creditNote.Moneda.codigo.Trim()); // Moneda
                if (!creditNote.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                    cadena.AppendFormat("{0}|", creditNote.tipoDeCambio.ToDecimalString()); // TipoDePago
                cadena.AppendFormat("{0}|", creditNote.Total.ToStringRoundedCurrency(creditNote.Moneda));
                cadena.Append("E|"); //tipo de comprobante
                cadena.Append("PUE|"); //MetodoPago
                cadena.AppendFormat("{0}|", config.Domicilio.codigoPostal); //LugarExpedición

                //CadenaOriginal CFDIRelacionados
                if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid())
                {
                    cadena.Append("01|"); //Nota de crédito de los documentos relacionados
                    cadena.AppendFormat("{0}|", creditNote.Factura.TimbresDeFactura.UUID);
                }

                //Emisor
                cadena.AppendFormat("{0}|", config.rfc);
                cadena.AppendFormat("{0}|", config.razonSocial.Trim());
                //cadena.AppendFormat("{0}|", creditNote.Regimene.codigo);
                cadena.AppendFormat("{0}|", config.CodigoRegimen);

                //Receptor
                cadena.AppendFormat("{0}|", creditNote.Cliente.rfc.Trim());
                cadena.AppendFormat("{0}|", creditNote.Cliente.razonSocial.Trim());
                if (!creditNote.Cliente.Domicilio.Pais.idPais.Equals((int)Paises.México)) //Solo cuando es extranjero
                    cadena.AppendFormat("{0}|", creditNote.Cliente.Domicilio.Pais.codigo);
                cadena.Append("G02|");//UsoCFDI G02 - Devoluciones, descuentos o bonificaciones

                //Concepto
                cadena.Append("84111506|"); //Codigo producto-servicio fijo
                //cadena.AppendFormat("{0}|", concepto.Articulo.codigo);
                cadena.Append("1|");
                cadena.Append("ACT|");
                cadena.Append("Actividad|");
                cadena.AppendFormat("{0}|", creditNote.descripcion);
                cadena.AppendFormat("{0}|", creditNote.Subtotal.ToStringRoundedCurrency(creditNote.Moneda));
                cadena.AppendFormat("{0}|", creditNote.Subtotal.ToStringRoundedCurrency(creditNote.Moneda));

                //Dictionary<int, decimal> montoBase = new Dictionary<int, decimal>();
                //Dictionary<int, decimal> total = new Dictionary<int, decimal>();

                ////Se calculan los totales para el detalle
                //foreach (var d in creditNote.DetalleDeNotaDeCredito.ToList())
                //{
                //    if (d.Impuestos.Count > 0)
                //    {
                //        var traslados = d.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));
                //        if (traslados.Count() > 0)
                //        {
                //            foreach (var impuesto in traslados)
                //            {
                //                var baseImpuesto = Operations.CalculateTaxBase(d.precioUnitario, d.cantidad, impuesto, d.Impuestos.ToList());
                //                var taxes = new List<Impuesto>();
                //                taxes.Add(impuesto);
                //                var cantidad = Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes));

                //                if (montoBase.ContainsKey(impuesto.idImpuesto))
                //                {
                //                    montoBase[impuesto.idImpuesto] += baseImpuesto;
                //                    total[impuesto.idImpuesto] += cantidad;
                //                }
                //                else
                //                {
                //                    montoBase.Add(impuesto.idImpuesto, baseImpuesto);
                //                    total.Add(impuesto.idImpuesto, cantidad);
                //                }
                //            }
                //        }

                //        var retenciones = d.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido));
                //        if (retenciones.Count() > 0)
                //        {
                //            foreach (var impuesto in retenciones)
                //            {
                //                var baseImpuesto = Operations.CalculateTaxBase(d.precioUnitario, d.cantidad, impuesto, d.Impuestos.ToList());
                //                var taxes = new List<Impuesto>();
                //                taxes.Add(impuesto);
                //                var cantidad = Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes));

                //                if (montoBase.ContainsKey(impuesto.idImpuesto))
                //                {
                //                    montoBase[impuesto.idImpuesto] += baseImpuesto;
                //                    total[impuesto.idImpuesto] += cantidad;
                //                }
                //                else
                //                {
                //                    montoBase.Add(impuesto.idImpuesto, baseImpuesto);
                //                    total.Add(impuesto.idImpuesto, cantidad);
                //                }
                //            }
                //        }
                //    }
                //}

                ////Impuestos detalle
                //El detalle de la nota de credito contiene los impuestos totalizados, que deberian ser los mismos que el global de la nota de crédito
                foreach (VMImpuesto imp in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)))
                {
                    cadena.AppendFormat("{0}|", imp.MontoGravable.ToStringRoundedCurrency(creditNote.Moneda));
                    cadena.AppendFormat("{0}|", imp.codigo);
                    cadena.AppendFormat("{0}|", imp.TiposFactor.codigo);
                    cadena.AppendFormat("{0}|", imp.valor.ToPorcentageString());
                    cadena.AppendFormat("{0}|", imp.Importe.ToTdCFDI_Importe());
                }
                foreach (VMImpuesto imp in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)))
                {
                    cadena.AppendFormat("{0}|", imp.MontoGravable.ToStringRoundedCurrency(creditNote.Moneda));
                    cadena.AppendFormat("{0}|", imp.codigo);
                    cadena.AppendFormat("{0}|", imp.TiposFactor.codigo);
                    cadena.AppendFormat("{0}|", imp.valor.ToPorcentageString());
                    cadena.AppendFormat("{0}|", imp.Importe.ToTdCFDI_Importe());
                }

                //foreach (var i in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).ToList())
                //{
                //    decimal baseImpuesto = montoBase[i.idImpuesto];
                //    decimal cantidad = total[i.idImpuesto];

                //    cadena.AppendFormat("{0}|", baseImpuesto.ToStringRoundedCurrency(creditNote.Moneda));
                //    cadena.AppendFormat("{0}|", i.codigo);
                //    cadena.AppendFormat("{0}|", i.TiposFactor.codigo);
                //    cadena.AppendFormat("{0}|", i.valor.ToPorcentageString());
                //    cadena.AppendFormat("{0}|", cantidad.ToTdCFDI_Importe());
                //}

                //foreach (var i in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).ToList())
                //{
                //    decimal baseImpuesto = montoBase[i.idImpuesto];
                //    decimal cantidad = total[i.idImpuesto];

                //    cadena.AppendFormat("{0}|", baseImpuesto.ToStringRoundedCurrency(creditNote.Moneda));
                //    cadena.AppendFormat("{0}|", i.codigo);
                //    cadena.AppendFormat("{0}|", i.TiposFactor.codigo);
                //    cadena.AppendFormat("{0}|", i.valor.ToPorcentageString());
                //    cadena.AppendFormat("{0}|", cantidad.ToTdCFDI_Importe());
                //}

                //Impuestos Retenidos
                var totalRetenciones = creditNote.Impuestos.Where(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(i => i.Importe.ToRoundedCurrency(creditNote.Moneda));
                var cantidadRetenciones = creditNote.Impuestos.Where(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Count();
                if (totalRetenciones >= 0.0m && cantidadRetenciones > 0)
                {
                    foreach (VMImpuesto imp in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)))
                    {
                        cadena.AppendFormat("{0}|", imp.codigo);
                        cadena.AppendFormat("{0}|", imp.Importe.ToTdCFDI_Importe());
                    }
                    cadena.AppendFormat("{0}|", totalRetenciones.ToStringRoundedCurrency(creditNote.Moneda));
                }

                //Impuestos Trasladados
                var totalTraslados = creditNote.Impuestos.Where(it => it.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(i => i.Importe.ToRoundedCurrency(creditNote.Moneda));
                var cantidadTraslados = creditNote.Impuestos.Where(it => it.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Count();
                if (totalTraslados >= 0.0m && cantidadTraslados > 0)
                {
                    foreach (VMImpuesto imp in creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)))
                    {
                        cadena.AppendFormat("{0}|", imp.codigo);
                        cadena.AppendFormat("{0}|", imp.TiposFactor.codigo);
                        cadena.AppendFormat("{0}|", imp.valor.ToPorcentageString());
                        cadena.AppendFormat("{0}|", imp.Importe.ToStringRoundedCurrency(creditNote.Moneda));
                    }
                    cadena.AppendFormat("{0}|", totalTraslados.ToStringRoundedCurrency(creditNote.Moneda));
                }

                cadena.Append("|"); //Finaliza la cadena

                return cadena.ToString().Replace("  ", " ");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TimbresDeNotasDeCredito GetTimbreNotaDeCredito(XmlDocument xmlNotaDeCredito)
        {
            try
            {
                XmlNode xNode;
                XmlNamespaceManager xNms;
                TimbresDeNotasDeCredito timbre;

                xNms = new XmlNamespaceManager(xmlNotaDeCredito.NameTable);
                xNms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                xNms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                xNode = xmlNotaDeCredito.ChildNodes[1].SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", xNms);

                timbre = new TimbresDeNotasDeCredito();
                timbre.version = xNode.Attributes["Version"].InnerText;
                timbre.selloSAT = xNode.Attributes["SelloSAT"].InnerText;
                timbre.selloCFD = xNode.Attributes["SelloCFD"].InnerText;
                timbre.noCertificadoSAT = xNode.Attributes["NoCertificadoSAT"].InnerText;
                timbre.UUID = xNode.Attributes["UUID"].InnerText;
                timbre.fechaTimbrado = xNode.Attributes["FechaTimbrado"].InnerText.ToDateFromUTC();
                timbre.leyenda = xNode.Attributes["Leyenda"].InnerText;
                timbre.rfcProvCertif = xNode.Attributes["RfcProvCertif"].InnerText;
                timbre.cadenaOriginal = string.Format("||1.1|{0}|{1}|{2}|{3}|{4}||", timbre.UUID, timbre.fechaTimbrado.ToUTCFormat(), timbre.rfcProvCertif, timbre.leyenda, timbre.selloCFD, timbre.noCertificadoSAT);

                return timbre;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public XmlDocument CreateCFDI(VMNotaDeCredito creditNote, Configuracion config)
        {
            try
            {
                XmlDocument xmlDoc;

                xmlDoc = new XmlDocument();
                xmlDoc.CreateProcessingInstruction("xml", "version= \"1.0\" encoding=\"UTF-8\"");
                XmlElement nodoComprobante = Comprobante(xmlDoc, creditNote, config);
                xmlDoc.AppendChild(nodoComprobante);
                nodoComprobante.Attributes.Append(Schema(xmlDoc, false));
                if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid())
                {
                    nodoComprobante.AppendChild(CFDIRelacionados(xmlDoc, creditNote.Factura));
                }
                nodoComprobante.AppendChild(Emisor(xmlDoc, config));
                nodoComprobante.AppendChild(Receptor(xmlDoc, creditNote.Cliente, new UsosCFDI() { codigo = "G02" }, creditNote.Cliente.Domicilio, creditNote.Cliente.Regimene));
                nodoComprobante.AppendChild(Conceptos(xmlDoc, creditNote));
                if (creditNote.Impuestos.Count > 0)
                    nodoComprobante.AppendChild(Impuestos(xmlDoc, creditNote.Impuestos, MetodoDePago.Pago_en_una_sola_exhibicion, creditNote.Moneda));

                return xmlDoc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Comprobante Xml Utilities

        private XmlAttribute Schema(XmlDocument xml, bool isPago)
        {
            try
            {
                XmlAttribute xSchema = xml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                if (!isPago)
                {
                    xSchema.Value = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";

                    //   xSchema.Value = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd " +
                    //"http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/cfd/TimbreFiscalDigital/TimbreFiscalDigitalv11.xsd " +
                    //"http://www.sat.gob.mx/sitio_internet/cfd/catalogos http://www.sat.gob.mx/sitio_internet/cfd/catalogos/catCFDI.xsd " +
                    //"http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI/tdCFDI.xsd";
                }
                else
                {
                    xSchema.Value = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd " +
                 "http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd";

                }

                return xSchema;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Comprobante(XmlDocument xml, VMFactura factura, Configuracion config)
        {
            try
            {
                XmlElement comprobante = xml.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/4");
                comprobante.SetAttribute("xmlns:catCFDI", "http://www.sat.gob.mx/sitio_internet/cfd/catalogos");
                comprobante.SetAttribute("xmlns:tdCFDI", "http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI");
                comprobante.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                comprobante.SetAttribute("Version", "4.0");
                comprobante.SetAttribute("Serie", factura.serie);
                comprobante.SetAttribute("Folio", factura.folio.ToString());
                comprobante.SetAttribute("Fecha", factura.fechaHora.AddHours(-6).ToUTCFormat());
                comprobante.SetAttribute("Sello", factura.sello);
                //<xs:documentation>Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante, Si no se conoce la forma de pago este atributo se debe omitir.</xs:documentation>
                if (factura.NotasDeCreditoes.Any(x => x.IsPreSaleCreditNote(factura)))
                {
                    //Los cfdis con descuento a futuro debe tener la forma de pago 23 - novacion
                    comprobante.SetAttribute("FormaPago", "23");
                }
                else if (factura.AbonosDeFacturas.Count > 0)
                {
                    var abono = Operations.GetFormaDePago(factura.AbonosDeFacturas);
                    if (abono.isValid())
                        comprobante.SetAttribute("FormaPago", abono.FormasPago.codigo);
                }
                else
                    comprobante.SetAttribute("FormaPago", "99");
                comprobante.SetAttribute("NoCertificado", factura.noCertificado);
                comprobante.SetAttribute("Certificado", "");// config.Certificados.FirstOrDefault(c => c.numero.Equals(factura.noCertificado)).certificadoBase64);
                if (factura.Cliente.condicionDePago.isValid())
                    comprobante.SetAttribute("CondicionesDePago", factura.Cliente.condicionDePago.Trim());
                comprobante.SetAttribute("SubTotal", factura.Subtotal.ToDecimalString());
                comprobante.SetAttribute("Moneda", factura.Moneda.codigo);
                if (!factura.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                    comprobante.SetAttribute("TipoCambio", factura.tipoDeCambio.ToDecimalString());
                comprobante.SetAttribute("Total", factura.Total.ToDecimalString());
                comprobante.SetAttribute("TipoDeComprobante", "I");
                comprobante.SetAttribute("MetodoPago", factura.MetodosPago.codigo);
                comprobante.SetAttribute("LugarExpedicion", config.Domicilio.codigoPostal);
                comprobante.SetAttribute("Exportacion", "01"); //JCRV

                return comprobante;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Comprobante(XmlDocument xml, VMNotaDeCredito creditNote, Configuracion config)
        {
            try
            {
                XmlElement comprobante = xml.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/4");
                comprobante.SetAttribute("xmlns:catCFDI", "http://www.sat.gob.mx/sitio_internet/cfd/catalogos");
                comprobante.SetAttribute("xmlns:tdCFDI", "http://www.sat.gob.mx/sitio_internet/cfd/tipoDatos/tdCFDI");
                comprobante.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                comprobante.SetAttribute("Version", "4.0");
                comprobante.SetAttribute("Serie", creditNote.serie);
                comprobante.SetAttribute("Folio", creditNote.folio.ToString());
                comprobante.SetAttribute("Fecha", creditNote.fechaHora.ToUTCFormat());
                comprobante.SetAttribute("Sello", creditNote.TimbresDeNotasDeCredito.sello);
                //<xs:documentation>Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios amparados por el comprobante, Si no se conoce la forma de pago este atributo se debe omitir.</xs:documentation>
                //if (!factura.idMetodoPago.Equals((int)MetodoDePago.Pago_en_parcialidades_o_diferido))
                comprobante.SetAttribute("FormaPago", creditNote.FormasPago.codigo);
                comprobante.SetAttribute("NoCertificado", creditNote.TimbresDeNotasDeCredito.noCertificado);
                comprobante.SetAttribute("Certificado", config.Certificados.FirstOrDefault(c => c.numero.Equals(creditNote.TimbresDeNotasDeCredito.noCertificado)).certificadoBase64);
                if (creditNote.Cliente.condicionDePago.isValid())
                    comprobante.SetAttribute("CondicionesDePago", creditNote.Cliente.condicionDePago.Trim());
                comprobante.SetAttribute("SubTotal", creditNote.Subtotal.ToDecimalString());
                comprobante.SetAttribute("Moneda", creditNote.Moneda.codigo);
                if (!creditNote.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                    comprobante.SetAttribute("TipoCambio", creditNote.tipoDeCambio.ToDecimalString());
                comprobante.SetAttribute("Total", creditNote.Total.ToDecimalString());
                comprobante.SetAttribute("TipoDeComprobante", "E");
                comprobante.SetAttribute("MetodoPago", "PUE");
                comprobante.SetAttribute("LugarExpedicion", config.Domicilio.codigoPostal);

                return comprobante;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Comprobante(XmlDocument xml, AbonosDeFactura abono, Configuracion config)
        {
            try
            {
                XmlElement comprobante = xml.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/4");
                comprobante.SetAttribute("xmlns:pago20", "http://www.sat.gob.mx/Pagos20");
                comprobante.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                comprobante.SetAttribute("Version", "4.0");
                comprobante.SetAttribute("Serie", abono.TimbresDeAbonosDeFactura.serie);
                comprobante.SetAttribute("Folio", abono.TimbresDeAbonosDeFactura.folio.ToString());
                comprobante.SetAttribute("Fecha", abono.fechaHora.AddHours(-6).ToUTCFormat());
                comprobante.SetAttribute("Sello", abono.TimbresDeAbonosDeFactura.sello);
                comprobante.SetAttribute("NoCertificado", abono.TimbresDeAbonosDeFactura.noCertificado);
                comprobante.SetAttribute("Certificado", config.Certificados.FirstOrDefault(c => c.numero.Equals(abono.TimbresDeAbonosDeFactura.noCertificado)).certificadoBase64);
                comprobante.SetAttribute("SubTotal", "0");
                comprobante.SetAttribute("Moneda", "XXX");
                comprobante.SetAttribute("Total", "0");
                comprobante.SetAttribute("TipoDeComprobante", "P");
                comprobante.SetAttribute("LugarExpedicion", config.Domicilio.codigoPostal);
                comprobante.SetAttribute("Exportacion", "01"); //JCRV Validar dato

                return comprobante;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Comprobante(XmlDocument xml, Pago pago, Configuracion config)
        {
            try
            {
                XmlElement comprobante = xml.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/4");
                comprobante.SetAttribute("xmlns:pago20", "http://www.sat.gob.mx/Pagos20");
                comprobante.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                comprobante.SetAttribute("Version", "4.0");
                comprobante.SetAttribute("Serie", pago.serie);
                comprobante.SetAttribute("Folio", pago.folio.ToString());
                comprobante.SetAttribute("Fecha", pago.fechaHora.AddHours(-6).ToUTCFormat());
                comprobante.SetAttribute("Sello", pago.TimbresDePago.sello);
                comprobante.SetAttribute("NoCertificado", pago.TimbresDePago.noCertificado);
                comprobante.SetAttribute("Certificado", config.Certificados.FirstOrDefault(c => c.numero.Equals(pago.TimbresDePago.noCertificado)).certificadoBase64);
                comprobante.SetAttribute("SubTotal", "0");
                comprobante.SetAttribute("Moneda", "XXX");
                comprobante.SetAttribute("Total", "0");
                comprobante.SetAttribute("TipoDeComprobante", "P");
                comprobante.SetAttribute("LugarExpedicion", config.Domicilio.codigoPostal);
                comprobante.SetAttribute("Exportacion", "01"); //JCRV Validar dato

                return comprobante;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<XmlElement> CFDIRelacionados(XmlDocument xml, VMFactura invoice)
        {
            try
            {
                List<XmlElement> cfdis = new List<XmlElement>();
                
                if (invoice.idComprobanteOriginal.isValid())
                {
                    XmlElement nodoCfdisRelacionados = xml.CreateElement("cfdi", "CfdiRelacionados", "http://www.sat.gob.mx/cfd/4");
                    nodoCfdisRelacionados.SetAttribute("TipoRelacion", invoice.TiposRelacion.codigo);
                    XmlElement nodoCfdiRelacionado = xml.CreateElement("cfdi", "CfdiRelacionado", "http://www.sat.gob.mx/cfd/4");
                    nodoCfdisRelacionados.AppendChild(nodoCfdiRelacionado);
                    nodoCfdiRelacionado.SetAttribute("UUID", invoice.Factura1.TimbresDeFactura.UUID);

                    cfdis.Add(nodoCfdisRelacionados);
                }

                if (invoice.NotasDeCreditoes.Any(x => x.IsPreSaleCreditNote(invoice)))
                {
                    XmlElement nodoCfdisRelacionados = xml.CreateElement("cfdi", "CfdiRelacionados", "http://www.sat.gob.mx/cfd/4");
                    nodoCfdisRelacionados.SetAttribute("TipoRelacion", "02");//Al crear un cfdi con descuento a futuro se debe usar el tipo de relacion 02

                    foreach (var n in invoice.NotasDeCreditoes.Where(x => x.IsPreSaleCreditNote(invoice)).ToList())
                    {
                        XmlElement nodoCfdiRelacionado = xml.CreateElement("cfdi", "CfdiRelacionado", "http://www.sat.gob.mx/cfd/4");
                        nodoCfdisRelacionados.AppendChild(nodoCfdiRelacionado);
                        nodoCfdiRelacionado.SetAttribute("UUID", n.TimbresDeNotasDeCredito.UUID);

                        cfdis.Add(nodoCfdisRelacionados);
                    }

                    cfdis.Add(nodoCfdisRelacionados);
                }

                return cfdis;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement CFDIRelacionados(XmlDocument xml, Factura invoice)
        {
            try
            {
                XmlElement nodoCfdisRelacionados = xml.CreateElement("cfdi", "CfdiRelacionados", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoCfdiRelacionado = xml.CreateElement("cfdi", "CfdiRelacionado", "http://www.sat.gob.mx/cfd/4");
                nodoCfdisRelacionados.SetAttribute("TipoRelacion", "01");
                nodoCfdisRelacionados.AppendChild(nodoCfdiRelacionado);
                nodoCfdiRelacionado.SetAttribute("UUID", invoice.TimbresDeFactura.UUID);

                return nodoCfdisRelacionados;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement CFDIInformacionGlobal(XmlDocument xml, VMFactura invoice)
        {
            try
            {
                XmlElement nodoCfdiInformacionGlobal = xml.CreateElement("cfdi", "InformacionGlobal", "http://www.sat.gob.mx/cfd/4");
                nodoCfdiInformacionGlobal.SetAttribute("Periodicidad", invoice.Periodicidad.CodigoPeriodicidad);
                nodoCfdiInformacionGlobal.SetAttribute("Meses", invoice.Periodicidad.Mes);
                nodoCfdiInformacionGlobal.SetAttribute("Año", invoice.Periodicidad.Year);

                return nodoCfdiInformacionGlobal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Emisor(XmlDocument xml, Configuracion config)
        {
            try
            {
                XmlElement emisor = xml.CreateElement("cfdi", "Emisor", "http://www.sat.gob.mx/cfd/4");
                emisor.SetAttribute("Rfc", config.rfc);
                emisor.SetAttribute("Nombre", config.razonSocial);
                //emisor.SetAttribute("RegimenFiscal", regimen.codigo);
                emisor.SetAttribute("RegimenFiscal", config.CodigoRegimen);
                return emisor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Receptor(XmlDocument xml, Cliente cliente, UsosCFDI uso, Domicilio domicilio, Regimene regimene)
        {
            try
            {
                XmlElement receptor = xml.CreateElement("cfdi", "Receptor", "http://www.sat.gob.mx/cfd/4");
                receptor.SetAttribute("Rfc", cliente.rfc.Trim());
                receptor.SetAttribute("Nombre", cliente.razonSocial);
                if (!cliente.Domicilio.Pais.idPais.Equals((int)Paises.México)) //Solo cuando es extranjero
                    receptor.SetAttribute("ResidenciaFiscal", cliente.Domicilio.Pais.codigo);
                receptor.SetAttribute("UsoCFDI", uso.codigo);
                receptor.SetAttribute("DomicilioFiscalReceptor", domicilio.codigoPostal);

                if (regimene != null && regimene.codigo != null)
                {
                    receptor.SetAttribute("RegimenFiscalReceptor", regimene.codigo);
                }
                else
                {
                    throw new Exception("El cliente no tiene configurado un regimen fiscal.");
                }
                return receptor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Conceptos(XmlDocument xml, VMFactura factura)
        {
            try
            {
                XmlElement nodoConceptos = xml.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoConcepto;
                foreach (var concepto in factura.DetalleDeFactura)
                {
                    nodoConcepto = xml.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/4");
                    nodoConceptos.AppendChild(nodoConcepto);
                    nodoConcepto.SetAttribute("ClaveProdServ", concepto.Articulo.ProductosServicio.codigo);
                    nodoConcepto.SetAttribute("NoIdentificacion", concepto.Articulo.codigo);
                    nodoConcepto.SetAttribute("Cantidad", concepto.cantidad.ToDecimalString());
                    nodoConcepto.SetAttribute("ClaveUnidad", concepto.Articulo.UnidadesDeMedida.codigo);
                    nodoConcepto.SetAttribute("Unidad", concepto.Articulo.UnidadesDeMedida.descripcion);

                    //Tratándose de las ventas de primera mano, en este campo se debe registrar la fecha del documento aduanero, la cual se puede registrar utilizando un formato libre, ya sea antes o después de la descripción del producto.
                    if (concepto.PedimentoPorDetalleDeFacturas.Count>0)
                    {
                        var descripcion = string.Format("{0} Fecha importación: ", concepto.Articulo.descripcion);
                        concepto.PedimentoPorDetalleDeFacturas.ToList().ForEach(p => descripcion = string.Format("{0} {1}", descripcion, p.Pedimento.fecha.ToShortDateString()));
                        nodoConcepto.SetAttribute("Descripcion", descripcion);
                    }
                    else
                        nodoConcepto.SetAttribute("Descripcion", concepto.Articulo.descripcion);

                    nodoConcepto.SetAttribute("ValorUnitario", concepto.precioUnitario.ToStringRoundedCurrency(factura.Moneda));
                    nodoConcepto.SetAttribute("Importe", (concepto.cantidad * concepto.precioUnitario).ToStringRoundedCurrency(factura.Moneda));
                    nodoConcepto.SetAttribute("ObjetoImp", (concepto.Impuestos.Count > 0) ? "02" : "01");
                    /* ObjetoImporte  JCRV */

                    XmlElement nodoImpuestos;
                    if (concepto.Impuestos.Count > 0)
                    {
                        //Solamente agrego el nodo si existen impuestos
                        nodoImpuestos = xml.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/4");
                        nodoConcepto.AppendChild(nodoImpuestos);

                        XmlElement nodoTrasladados;
                        var traslados = concepto.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));
                        if (traslados.Count() > 0)
                        {
                            //Solamente agrego traslados cuando existan impuestos trasladados
                            nodoTrasladados = xml.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/4");
                            nodoImpuestos.AppendChild(nodoTrasladados);
                            foreach (var impuesto in traslados)
                            {
                                XmlElement nodoImpuesto;
                                ImpuestoPorFactura imp;

                                nodoImpuesto = xml.CreateElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/4");
                                nodoTrasladados.AppendChild(nodoImpuesto);
                                var baseImpuesto = Operations.CalculateTaxBase(Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento), concepto.cantidad, impuesto, concepto.Impuestos.ToList()); // Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento) * concepto.cantidad;
                                nodoImpuesto.SetAttribute("Base", baseImpuesto.ToStringRoundedCurrency(factura.Moneda));
                                nodoImpuesto.SetAttribute("Impuesto", impuesto.codigo);
                                nodoImpuesto.SetAttribute("TipoFactor", impuesto.TiposFactor.codigo);
                                nodoImpuesto.SetAttribute("TasaOCuota", impuesto.valor.ToPorcentageString());
                                var taxes = new List<Impuesto>();
                                taxes.Add(impuesto);
                                nodoImpuesto.SetAttribute("Importe", Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes)).ToTdCFDI_Importe());

                                imp = new ImpuestoPorFactura {
                                    idFactura = factura.idFactura,
                                    @base = baseImpuesto.ToStringRoundedCurrency(factura.Moneda).ToDecimal(),
                                    codigoImpuesto = impuesto.codigo,
                                    codigoTipoFactor = impuesto.TiposFactor.codigo,
                                    valorTasaOCuaota = impuesto.valor.ToPorcentageString().ToDecimal(),
                                    importe = Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes)).ToTdCFDI_Importe().ToDecimal()
                                };

                                
                                _impuestoPorFactura.Add(imp);
                                _UOW.Save();
                            }
                        }

                        XmlElement nodoRetenidos;
                        var retenciones = concepto.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido));
                        if (retenciones.Count() > 0)
                        {
                            //Solamente agrego traslados cuando existan impuestos trasladados
                            nodoRetenidos = xml.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/4");
                            nodoImpuestos.AppendChild(nodoRetenidos);
                            foreach (var impuesto in retenciones)
                            {
                                XmlElement nodoImpuesto;
                                nodoImpuesto = xml.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/4");
                                nodoRetenidos.AppendChild(nodoImpuesto);
                                var baseImpuesto = Operations.CalculateTaxBase(Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento), concepto.cantidad, impuesto, concepto.Impuestos.ToList()); //Operations.CalculatePriceWithDiscount(concepto.precioUnitario, concepto.descuento) * concepto.cantidad;
                                nodoImpuesto.SetAttribute("Base", baseImpuesto.ToStringRoundedCurrency(factura.Moneda));
                                nodoImpuesto.SetAttribute("Impuesto", impuesto.codigo);
                                nodoImpuesto.SetAttribute("TipoFactor", impuesto.TiposFactor.codigo);
                                nodoImpuesto.SetAttribute("TasaOCuota", impuesto.valor.ToPorcentageString());
                                var taxes = new List<Impuesto>();
                                taxes.Add(impuesto);
                                nodoImpuesto.SetAttribute("Importe", Math.Abs(Operations.CalculateTaxes(baseImpuesto, taxes)).ToTdCFDI_Importe());
                            }
                        }
                    }

                    //Si lleva pedimentos
                    if (concepto.PedimentoPorDetalleDeFacturas.Count > 0)
                    {
                        foreach (var pedimento in concepto.PedimentoPorDetalleDeFacturas)
                        {
                            //Pedimentos
                            XmlElement nodoInformacionAduanera;
                            //Solamente agrego el nodo si existen pedimentos
                            nodoInformacionAduanera = xml.CreateElement("cfdi", "InformacionAduanera", "http://www.sat.gob.mx/cfd/4");
                            nodoConcepto.AppendChild(nodoInformacionAduanera);

                            //Le agrego el dato del pedimento
                            nodoInformacionAduanera.SetAttribute("NumeroPedimento", string.Format("{0}  {1}  {2}  {3}{4}", pedimento.Pedimento.añoOperacion, pedimento.Pedimento.aduana, pedimento.Pedimento.patente, pedimento.Pedimento.añoEnCurso, pedimento.Pedimento.progresivo));
                        }


                    }

                    XmlElement nodoCuentaPredial;
                    if (concepto.CuentaPredialPorDetalle.isValid() && concepto.CuentaPredialPorDetalle.idCuentaPredialPorDetalle.isValid())
                    {
                        //Solamente agrego el nodo si existe cuenta predial
                        nodoCuentaPredial = xml.CreateElement("cfdi", "CuentaPredial", "http://www.sat.gob.mx/cfd/4");
                        nodoConcepto.AppendChild(nodoCuentaPredial);

                        nodoCuentaPredial.SetAttribute("Numero", concepto.CuentaPredialPorDetalle.cuenta);
                    }
                }

                return nodoConceptos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Conceptos(XmlDocument xml, AbonosDeFactura abono)
        {
            try
            {
                XmlElement nodoConceptos = xml.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoConcepto;
                nodoConcepto = xml.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/4");
                nodoConceptos.AppendChild(nodoConcepto);
                nodoConcepto.SetAttribute("ClaveProdServ", "84111506");
                nodoConcepto.SetAttribute("Cantidad", (1.0m).ToDecimalString());
                nodoConcepto.SetAttribute("ClaveUnidad", "ACT");
                nodoConcepto.SetAttribute("Descripcion", "Pago");
                nodoConcepto.SetAttribute("ValorUnitario", "0");
                nodoConcepto.SetAttribute("Importe", "0");
                nodoConcepto.SetAttribute("ObjetoImp", "01"); //JCRV Validar dato

                return nodoConceptos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Conceptos(XmlDocument xml, Pago payment)
        {
            try
            {
                XmlElement nodoConceptos = xml.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoConcepto;
                nodoConcepto = xml.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/4");
                nodoConceptos.AppendChild(nodoConcepto);
                nodoConcepto.SetAttribute("ClaveProdServ", "84111506");
                nodoConcepto.SetAttribute("Cantidad", (1.0m).ToDecimalString());
                nodoConcepto.SetAttribute("ClaveUnidad", "ACT");
                nodoConcepto.SetAttribute("Descripcion", "Pago");
                nodoConcepto.SetAttribute("ValorUnitario", "0");
                nodoConcepto.SetAttribute("Importe", "0");
                nodoConcepto.SetAttribute("ObjetoImp", "01"); //JCRV Validar dato

                return nodoConceptos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Conceptos(XmlDocument xml, VMNotaDeCredito creditNote)
        {
            try
            {
                XmlElement nodoConceptos = xml.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoConcepto;

                nodoConcepto = xml.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/4");
                nodoConceptos.AppendChild(nodoConcepto);
                nodoConcepto.SetAttribute("ClaveProdServ", "84111506");
                nodoConcepto.SetAttribute("Cantidad", "1");
                nodoConcepto.SetAttribute("ClaveUnidad", "ACT");
                nodoConcepto.SetAttribute("Unidad", "Actividad");
                nodoConcepto.SetAttribute("Descripcion", creditNote.descripcion);
                nodoConcepto.SetAttribute("ValorUnitario", creditNote.Subtotal.ToStringRoundedCurrency(creditNote.Moneda));
                nodoConcepto.SetAttribute("Importe", creditNote.Subtotal.ToStringRoundedCurrency(creditNote.Moneda));

                var impuestosTrasladados = creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).ToList();
                var impuestosRetenidos = creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).ToList();

                if (creditNote.Impuestos.Any(x => x.idImpuesto.isValid()))
                {
                    XmlElement nodoImpuestos;
                    nodoImpuestos = xml.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/4");
                    nodoConcepto.AppendChild(nodoImpuestos);

                    if (!impuestosTrasladados.IsEmpty())
                    {
                        XmlElement nodoTrasladados;
                        nodoTrasladados = xml.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/4");
                        nodoImpuestos.AppendChild(nodoTrasladados);

                        foreach (var i in impuestosTrasladados)
                        {
                            XmlElement nodoImpuesto;
                            nodoImpuesto = xml.CreateElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/4");
                            nodoTrasladados.AppendChild(nodoImpuesto);
                            nodoImpuesto.SetAttribute("Base", i.MontoGravable.ToStringRoundedCurrency(creditNote.Moneda));
                            nodoImpuesto.SetAttribute("Impuesto", i.codigo);
                            nodoImpuesto.SetAttribute("TipoFactor", i.TiposFactor.codigo);
                            nodoImpuesto.SetAttribute("TasaOCuota", i.valor.ToPorcentageString());
                            nodoImpuesto.SetAttribute("Importe", i.Importe.ToTdCFDI_Importe());
                        }
                    }

                    if (!impuestosRetenidos.IsEmpty())
                    {
                        XmlElement nodoRetenidos;
                        nodoRetenidos = xml.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/4");
                        nodoImpuestos.AppendChild(nodoRetenidos);

                        foreach (var i in impuestosRetenidos)
                        {
                            XmlElement nodoImpuesto;
                            nodoImpuesto = xml.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/4");
                            nodoRetenidos.AppendChild(nodoImpuesto);
                            nodoImpuesto.SetAttribute("Base", i.MontoGravable.ToStringRoundedCurrency(creditNote.Moneda));
                            nodoImpuesto.SetAttribute("Impuesto", i.codigo);
                            nodoImpuesto.SetAttribute("TipoFactor", i.TiposFactor.codigo);
                            nodoImpuesto.SetAttribute("TasaOCuota", i.valor.ToPorcentageString());
                            nodoImpuesto.SetAttribute("Importe", i.Importe.ToTdCFDI_Importe());
                        }
                    }
                }

                return nodoConceptos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Pagos(XmlDocument xml, VMFactura facturaOriginal, AbonosDeFactura abono)
        {
            try
            {
                XmlElement nodoComplemento = xml.CreateElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoPagos = xml.CreateElement("pago20", "Pagos", "http://www.sat.gob.mx/Pagos20");
                XmlElement nodoPago = xml.CreateElement("pago20", "Pago", "http://www.sat.gob.mx/Pagos20");
                XmlElement nodoDoctoRelacionado = xml.CreateElement("pago20", "DoctoRelacionado", "http://www.sat.gob.mx/Pagos20");
                XmlElement nodoPagoTotales = xml.CreateElement("pago20", "Totales", "http://www.sat.gob.mx/Pagos20");
                XmlElement impuestosP = xml.CreateElement("pago20", "ImpuestosP", "http://www.sat.gob.mx/Pagos20");

                var totales = (abono.monto*abono.tipoDeCambio).ToRoundedCurrency(abono.Moneda);//JCRV Totales //26/04/2023 se multiplica el monto del pago por el tipo de cambio
                nodoPagoTotales.SetAttribute("MontoTotalPagos", totales.ToDecimalString());
                nodoPagos.AppendChild(nodoPagoTotales);

                nodoComplemento.AppendChild(nodoPagos);

                nodoPagos.SetAttribute("Version", "2.0");

                nodoPagos.AppendChild(nodoPago);

                nodoPago.SetAttribute("FechaPago", abono.fechaHora.ToUTCFormat());
                nodoPago.SetAttribute("FormaDePagoP", abono.FormasPago.codigo);
                nodoPago.SetAttribute("MonedaP", abono.Moneda.codigo);
                /*
                if (!abono.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                    nodoPago.SetAttribute("TipoCambioP", abono.tipoDeCambio.ToDecimalString());
                else
                    nodoPago.SetAttribute("TipoCambioP", "1");
                */

                /* JCRV 07/04/23 Se solicito que se pusiera 1 cuando fuera pago en dolares */
                if (abono.idMoneda.Equals((int)Monedas.Pesos) || abono.idMoneda.Equals((int)Monedas.Dólares)) //Si no es pesos se incluye
                    nodoPago.SetAttribute("TipoCambioP", "1");
                else
                    nodoPago.SetAttribute("TipoCambioP", abono.tipoDeCambio.ToDecimalString());

                nodoPago.SetAttribute("Monto", abono.monto.ToDecimalString());
                nodoPago.SetAttribute("MonedaP", abono.Moneda.codigo);
                nodoPago.AppendChild(nodoDoctoRelacionado);
                nodoDoctoRelacionado.SetAttribute("IdDocumento", facturaOriginal.TimbresDeFactura.UUID);
                nodoDoctoRelacionado.SetAttribute("Serie", facturaOriginal.serie);
                nodoDoctoRelacionado.SetAttribute("Folio", facturaOriginal.folio.ToString());

                nodoDoctoRelacionado.SetAttribute("MonedaDR", facturaOriginal.Moneda.codigo);

                /*JCRV Se Agrega EquivalenciaDR*/
                nodoDoctoRelacionado.SetAttribute("EquivalenciaDR", (facturaOriginal.Moneda.codigo != abono.Moneda.codigo ? facturaOriginal.tipoDeCambio.ToStringRoundedCurrency(facturaOriginal.Moneda) : "1"));

                /* En la documentacion se indica que se reemplaza TipoCambioDR por EquivalenciaDR
                if (!facturaOriginal.idMoneda.Equals(abono.idMoneda)) //Cuando son distintas debe agregarse tipo de cambio
                {
                    if (facturaOriginal.Moneda.codigo.Equals("MXN")) //CRP220 si el valor del atributo MonedaDR es MXN y el valor MonedaP es diferente, el atributo TipoCambioDR debe tener el valor 1
                        nodoDoctoRelacionado.SetAttribute("TipoCambioDR", "1");
                    else
                        nodoDoctoRelacionado.SetAttribute("TipoCambioDR", facturaOriginal.tipoDeCambio.ToStringRoundedCurrency(facturaOriginal.Moneda));
                }
                */

                //nodoDoctoRelacionado.SetAttribute("MetodoDePagoDR", facturaOriginal.MetodosPago.codigo);
                var numParcialidad = facturaOriginal.AbonosDeFacturas.Count(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado && a.fechaHora <= abono.fechaHora.ToNextMidnight()); // Abonos anteriores 
                nodoDoctoRelacionado.SetAttribute("NumParcialidad", numParcialidad.ToString());

                var abonoParcial = abono.monto.ToDocumentCurrency(abono.Moneda, facturaOriginal.Moneda, abono.tipoDeCambio); //cantidad abonada respecto a la moneda del documento
                nodoDoctoRelacionado.SetAttribute("ImpSaldoAnt", (facturaOriginal.Total - facturaOriginal.Abonado + abonoParcial).ToStringRoundedCurrency(facturaOriginal.Moneda)); //ImpSaldoAnt
                nodoDoctoRelacionado.SetAttribute("ImpPagado", abonoParcial.ToStringRoundedCurrency(facturaOriginal.Moneda));
                nodoDoctoRelacionado.SetAttribute("ImpSaldoInsoluto", (facturaOriginal.Total - facturaOriginal.Abonado).ToStringRoundedCurrency(facturaOriginal.Moneda));

                var impuestos = facturaOriginal.ImpuestoPorFacturas.ToList();
                nodoDoctoRelacionado.SetAttribute("ObjetoImpDR", impuestos.Count > 0 ? "02" : "01"); //ObjetoImpDR JCRV validar que valor poner*****************

                List<ImpuestosPago> impuestop = new List<ImpuestosPago>();

                if (impuestos.Count > 0)
                {
                    XmlElement nodoImpuestosDR;
                    XmlElement trasladosDR;

                    nodoImpuestosDR = xml.CreateElement("pago20", "ImpuestosDR", "http://www.sat.gob.mx/Pagos20");
                    nodoDoctoRelacionado.AppendChild(nodoImpuestosDR);

                    trasladosDR = xml.CreateElement("pago20", "TrasladosDR", "http://www.sat.gob.mx/Pagos20");
                    nodoImpuestosDR.AppendChild(trasladosDR);

                    //var traslados = concepto.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));

                    foreach (var traslado in impuestos)
                    {
                        XmlElement trasladoDR;

                        var base_imp = abonoParcial / (1 + traslado.valorTasaOCuaota);

                        trasladoDR = xml.CreateElement("pago20", "TrasladoDR", "http://www.sat.gob.mx/Pagos20");
                        trasladosDR.AppendChild(trasladoDR);

                        trasladoDR.SetAttribute("BaseDR", base_imp.ToStringRoundedCurrency(facturaOriginal.Moneda));
                        trasladoDR.SetAttribute("ImporteDR", (base_imp * traslado.valorTasaOCuaota).ToStringRoundedCurrency(facturaOriginal.Moneda));
                        trasladoDR.SetAttribute("ImpuestoDR", traslado.codigoImpuesto);
                        trasladoDR.SetAttribute("TasaOCuotaDR", Math.Abs(traslado.valorTasaOCuaota).ToTdCFDI_Importe());
                        trasladoDR.SetAttribute("TipoFactorDR", traslado.codigoTipoFactor);

                        bool existe = (from i in impuestop where i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor select i).Count() > 0;

                        if (existe)
                        {
                            impuestop.Where(i => i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor).ToList().ForEach(i => i.basep += base_imp.ToDouble());
                            impuestop.Where(i => i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor).ToList().ForEach(i => i.importep += (base_imp * traslado.valorTasaOCuaota).ToDouble());
                        }
                        else
                        {
                            impuestop.Add(new ImpuestosPago
                            {
                                basep = base_imp.ToDouble(),
                                factorp = traslado.codigoTipoFactor,
                                importep = (base_imp * traslado.valorTasaOCuaota).ToDouble(),
                                impuestop = traslado.codigoImpuesto,
                                tasaCuota = Math.Abs(traslado.valorTasaOCuaota),
                                tipo = "Traslado",
                            });
                        }
                    }

                    if (impuestop.Count > 0)
                    {
                        XmlElement trasladosP;

                        impuestosP = xml.CreateElement("pago20", "ImpuestosP", "http://www.sat.gob.mx/Pagos20");
                        trasladosP = xml.CreateElement("pago20", "TrasladosP", "http://www.sat.gob.mx/Pagos20");


                        nodoPago.AppendChild(impuestosP);
                        impuestosP.AppendChild(trasladosP);


                        foreach (var traslado in impuestop)
                        {
                            XmlElement trasladoP;
                            trasladoP = xml.CreateElement("pago20", "TrasladoP", "http://www.sat.gob.mx/Pagos20");
                            trasladosP.AppendChild(trasladoP);

                            trasladoP.SetAttribute("BaseP", traslado.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                            trasladoP.SetAttribute("ImpuestoP", traslado.impuestop);
                            trasladoP.SetAttribute("TipoFactorP", traslado.factorp);
                            trasladoP.SetAttribute("TasaOCuotaP", traslado.tasaCuota.ToDecimal().ToTdCFDI_Importe());
                            trasladoP.SetAttribute("ImporteP", traslado.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                        }

                        foreach (var tasa in impuestop)
                        {
                            if (tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.000000" && tasa.impuestop == "002")
                            {
                                nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA0", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA0", "0");
                            }
                            else if (tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.080000" && tasa.impuestop == "002")
                            {
                                nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA8", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA8", tasa.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                            }
                            else if (tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.160000" && tasa.impuestop == "002")
                            {
                                nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA16", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA16", tasa.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                            }
                            else if (tasa.factorp == "Exento")
                            {
                                nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVAExento", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                            }
                        }
                    }

                }

                return nodoComplemento;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Pagos(XmlDocument xml, Pago payment)
        {
            try
            {
                XmlElement nodoComplemento = xml.CreateElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/4");
                XmlElement nodoPagos = xml.CreateElement("pago20", "Pagos", "http://www.sat.gob.mx/Pagos20");

                //Esto se repite por cada abono
                XmlElement nodoPago;
                XmlElement nodoPagoTotales;
                XmlElement nodoDoctoRelacionado;
                XmlElement impuestosP;

                nodoPagoTotales = xml.CreateElement("pago20", "Totales", "http://www.sat.gob.mx/Pagos20");
                var totales = payment.AbonosDeFacturas.Sum(p => (p.monto*p.tipoDeCambio).ToRoundedCurrency(p.Moneda));//JCRV Totales
                nodoPagoTotales.SetAttribute("MontoTotalPagos", totales.ToDecimalString() /*ToStringRoundedCurrency(payment.AbonosDeFacturas.First().Moneda)*/);//JCRV
                nodoPagos.AppendChild(nodoPagoTotales);

                foreach (var abono in payment.AbonosDeFacturas)
                {
                    //var facturaOriginal = new VMFactura(abono.Factura);
                    var f = _invoices.getFactura(abono.idFactura);
                    var facturaOriginal = new VMFactura(f);// new VMFactura(abono.idFactura);

                    nodoPago = xml.CreateElement("pago20", "Pago", "http://www.sat.gob.mx/Pagos20");
                    nodoDoctoRelacionado = xml.CreateElement("pago20", "DoctoRelacionado", "http://www.sat.gob.mx/Pagos20");
                    nodoComplemento.AppendChild(nodoPagos);

                    nodoPagos.SetAttribute("Version", "2.0");
                    nodoPagos.AppendChild(nodoPago);

                    nodoPago.SetAttribute("FechaPago", abono.fechaHora.ToUTCFormat());
                    nodoPago.SetAttribute("FormaDePagoP", abono.FormasPago.codigo);
                    nodoPago.SetAttribute("MonedaP", abono.Moneda.codigo);
                    /*
                    if (!abono.idMoneda.Equals((int)Monedas.Pesos)) //Si no es pesos se incluye
                        nodoPago.SetAttribute("TipoCambioP", abono.tipoDeCambio.ToDecimalString());
                    else
                        nodoPago.SetAttribute("TipoCambioP", "1");
                    */

                    /* JCRV 07/04/23 Se solicito que se pusiera 1 cuando fuera pago en dolares */
                    if (abono.idMoneda.Equals((int)Monedas.Pesos) || abono.idMoneda.Equals((int)Monedas.Dólares)) //Si no es pesos se incluye
                        nodoPago.SetAttribute("TipoCambioP", "1");
                    else
                        nodoPago.SetAttribute("TipoCambioP", abono.tipoDeCambio.ToDecimalString());

                    nodoPago.SetAttribute("Monto", abono.monto.ToDecimalString());
                    nodoPago.SetAttribute("MonedaP", abono.Moneda.codigo);
                    nodoPago.AppendChild(nodoDoctoRelacionado);
                    nodoDoctoRelacionado.SetAttribute("IdDocumento", facturaOriginal.TimbresDeFactura.UUID);
                    nodoDoctoRelacionado.SetAttribute("Serie", facturaOriginal.serie);
                    nodoDoctoRelacionado.SetAttribute("Folio", facturaOriginal.folio.ToString());
                    nodoDoctoRelacionado.SetAttribute("MonedaDR", facturaOriginal.Moneda.codigo);
                    
                    /*JCRV Se Agrega EquivalenciaDR*/
                    nodoDoctoRelacionado.SetAttribute("EquivalenciaDR", (facturaOriginal.Moneda.codigo != abono.Moneda.codigo ? facturaOriginal.tipoDeCambio.ToStringRoundedCurrency(facturaOriginal.Moneda) : "1"));
                    
                    /*
                    if (!facturaOriginal.idMoneda.Equals(abono.idMoneda)) //Cuando son distintas debe agregarse tipo de cambio
                        nodoDoctoRelacionado.SetAttribute("TipoCambioDR", facturaOriginal.tipoDeCambio.ToStringRoundedCurrency(facturaOriginal.Moneda));
                    */
                    //nodoDoctoRelacionado.SetAttribute("MetodoDePagoDR", facturaOriginal.MetodosPago.codigo);
                    var numParcialidad = facturaOriginal.AbonosDeFacturas.Count(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado && a.fechaHora <= abono.fechaHora.ToNextMidnight()); // Abonos anteriores 
                    nodoDoctoRelacionado.SetAttribute("NumParcialidad", numParcialidad.ToString());

                    var abonoParcial = abono.monto.ToDocumentCurrency(abono.Moneda, facturaOriginal.Moneda, abono.tipoDeCambio); //cantidad abonada respecto a la moneda del documento
                    nodoDoctoRelacionado.SetAttribute("ImpSaldoAnt", (facturaOriginal.Total - facturaOriginal.Abonado + abonoParcial - facturaOriginal.Acreditado).ToStringRoundedCurrency(facturaOriginal.Moneda)); //ImpSaldoAnt
                    nodoDoctoRelacionado.SetAttribute("ImpPagado", abonoParcial.ToStringRoundedCurrency(facturaOriginal.Moneda));
                    nodoDoctoRelacionado.SetAttribute("ImpSaldoInsoluto", (facturaOriginal.Total - facturaOriginal.Abonado - facturaOriginal.Acreditado).ToStringRoundedCurrency(facturaOriginal.Moneda));

                    var impuestos = facturaOriginal.ImpuestoPorFacturas.ToList();
                    nodoDoctoRelacionado.SetAttribute("ObjetoImpDR", impuestos.Count > 0 ? "02" : "01"); //ObjetoImpDR JCRV*****************

                    List<ImpuestosPago> impuestop = new List<ImpuestosPago>();

                    if (impuestos.Count > 0)
                    {
                        XmlElement nodoImpuestosDR;
                        XmlElement trasladosDR;

                        nodoImpuestosDR = xml.CreateElement("pago20", "ImpuestosDR", "http://www.sat.gob.mx/Pagos20");
                        nodoDoctoRelacionado.AppendChild(nodoImpuestosDR);

                        trasladosDR = xml.CreateElement("pago20", "TrasladosDR", "http://www.sat.gob.mx/Pagos20");
                        nodoImpuestosDR.AppendChild(trasladosDR);

                        foreach (var traslado in impuestos)
                        {
                            XmlElement trasladoDR;
                            var base_imp = abonoParcial / (1 + traslado.valorTasaOCuaota);

                            trasladoDR = xml.CreateElement("pago20", "TrasladoDR", "http://www.sat.gob.mx/Pagos20");
                            trasladosDR.AppendChild(trasladoDR);

                            trasladoDR.SetAttribute("BaseDR", base_imp.ToStringRoundedCurrency(facturaOriginal.Moneda));
                            trasladoDR.SetAttribute("ImporteDR", (base_imp * traslado.valorTasaOCuaota).ToStringRoundedCurrency(facturaOriginal.Moneda));
                            trasladoDR.SetAttribute("ImpuestoDR", traslado.codigoImpuesto);
                            trasladoDR.SetAttribute("TasaOCuotaDR", Math.Abs(traslado.valorTasaOCuaota).ToTdCFDI_Importe());
                            trasladoDR.SetAttribute("TipoFactorDR", traslado.codigoTipoFactor);


                            bool existe = (from i in impuestop where i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor select i).Count() > 0;

                            if (existe)
                            {
                                impuestop.Where(i => i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor).ToList().ForEach(i => i.basep += base_imp.ToDouble());
                                impuestop.Where(i => i.impuestop == traslado.codigoImpuesto && i.factorp == traslado.codigoTipoFactor).ToList().ForEach(i => i.importep += (base_imp * traslado.valorTasaOCuaota).ToDouble());
                            }
                            else
                            {
                                impuestop.Add(new ImpuestosPago
                                {
                                    basep = base_imp.ToDouble(),
                                    factorp = traslado.codigoTipoFactor,
                                    importep = (base_imp * traslado.valorTasaOCuaota).ToDouble(),
                                    impuestop = traslado.codigoImpuesto,
                                    tasaCuota = Math.Abs(traslado.valorTasaOCuaota),
                                    tipo = "Traslado",
                                });
                            }
                        }

                        if(impuestop.Count > 0)
                        {
                            XmlElement trasladosP;

                            impuestosP = xml.CreateElement("pago20", "ImpuestosP", "http://www.sat.gob.mx/Pagos20");
                            trasladosP = xml.CreateElement("pago20", "TrasladosP", "http://www.sat.gob.mx/Pagos20");
                            

                            nodoPago.AppendChild(impuestosP);
                            impuestosP.AppendChild(trasladosP);
                            

                            foreach(var traslado in impuestop)
                            {
                                XmlElement trasladoP;
                                trasladoP = xml.CreateElement("pago20", "TrasladoP", "http://www.sat.gob.mx/Pagos20");
                                trasladosP.AppendChild(trasladoP);

                                trasladoP.SetAttribute("BaseP", traslado.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                trasladoP.SetAttribute("ImpuestoP", traslado.impuestop);
                                trasladoP.SetAttribute("TipoFactorP", traslado.factorp);
                                trasladoP.SetAttribute("TasaOCuotaP", traslado.tasaCuota.ToDecimal().ToTdCFDI_Importe());
                                trasladoP.SetAttribute("ImporteP", traslado.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                            }

                            //var x = impuestop.Where(i => i.impuestop == "002").Select(i => i.tasaCuota).ToList();
                            foreach (var tasa in impuestop)
                            {
                                if(tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.000000" && tasa.impuestop == "002")
                                {
                                    nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA0", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                    nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA0", "0");
                                }
                                else if (tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.080000" && tasa.impuestop == "002")
                                {
                                    nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA8", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                    nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA8", tasa.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                }
                                else if (tasa.tasaCuota.ToDecimal().ToTdCFDI_Importe() == "0.160000" && tasa.impuestop == "002")
                                {
                                    nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVA16", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                    nodoPagoTotales.SetAttribute("TotalTrasladosImpuestoIVA16", tasa.importep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                }
                                else if(tasa.factorp == "Exento")
                                {
                                    nodoPagoTotales.SetAttribute("TotalTrasladosBaseIVAExento", tasa.basep.ToDecimal().ToStringRoundedCurrency(facturaOriginal.Moneda));
                                }
                            }
                        }
                    }

                }

                return nodoComplemento;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Impuestos(XmlDocument xml, List<VMImpuesto> impuestos, MetodoDePago formaDePago, Moneda moneda)
        {
            try
            {
                XmlElement nodoImpuestos = xml.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/4");

                var totalRetenciones = impuestos.Where(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(i => i.Importe.ToRoundedCurrency(moneda));
                var cantidadRetenciones = impuestos.Count(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido));
                if (totalRetenciones >= 0.0m && cantidadRetenciones > 0)
                {
                    nodoImpuestos.SetAttribute("TotalImpuestosRetenidos", totalRetenciones.ToStringRoundedCurrency(moneda));
                    nodoImpuestos.AppendChild(Retenciones(xml, impuestos, formaDePago, moneda));
                }

                var totalTraslados = impuestos.Where(ir => ir.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(i => i.Importe.ToRoundedCurrency(moneda));
                var cantidadTraslados = impuestos.Count(it => it.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado));
                if (totalTraslados >= 0.0m && cantidadTraslados > 0)
                {
                    nodoImpuestos.SetAttribute("TotalImpuestosTrasladados", totalTraslados.ToStringRoundedCurrency(moneda));
                    nodoImpuestos.AppendChild(Traslados(xml, impuestos, formaDePago, moneda));
                }

                return nodoImpuestos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Domicilio(XmlDocument xml, Domicilio domicilio, string tipoDomicilio)
        {
            try
            {
                XmlElement domicilioFiscal = xml.CreateElement("cfdi", tipoDomicilio, "http://www.sat.gob.mx/cfd/4");
                domicilioFiscal.SetAttribute("calle", domicilio.calle);
                domicilioFiscal.SetAttribute("noExterior", domicilio.numeroExterior);
                domicilioFiscal.SetAttribute("noInterior", domicilio.numeroInterior);
                domicilioFiscal.SetAttribute("colonia", domicilio.colonia);
                domicilioFiscal.SetAttribute("municipio", domicilio.ciudad);
                domicilioFiscal.SetAttribute("estado", domicilio.estado);
                domicilioFiscal.SetAttribute("pais", domicilio.Pais.codigo);
                domicilioFiscal.SetAttribute("codigoPostal", domicilio.codigoPostal);
                return domicilioFiscal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Traslados(XmlDocument xml, List<VMImpuesto> impuestos, MetodoDePago formaDePago, Moneda moneda)
        {
            try
            {
                XmlElement traslados = xml.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/4");
                foreach (VMImpuesto imp in impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)))
                {
                    XmlElement traslado = xml.CreateElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/4");
                    traslados.AppendChild(traslado);
                    traslado.SetAttribute("Impuesto", imp.codigo);
                    traslado.SetAttribute("TipoFactor", imp.TiposFactor.codigo);
                    traslado.SetAttribute("TasaOCuota", imp.valor.ToPorcentageString());
                    traslado.SetAttribute("Importe", imp.Importe.ToStringRoundedCurrency(moneda));
                    traslado.SetAttribute("Base", imp.MontoGravable.ToStringRoundedCurrency(moneda)); //JCRV i.MontoGravable.ToStringRoundedCurrency(creditNote.Moneda)


                }

                return traslados;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Retenciones(XmlDocument xml, List<VMImpuesto> impuestos, MetodoDePago formaDePago, Moneda moneda)
        {
            try
            {
                XmlElement retenciones = xml.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/4");
                foreach (VMImpuesto imp in impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)))
                {
                    XmlElement retencion = xml.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/4");
                    retenciones.AppendChild(retencion);
                    retencion.SetAttribute("Impuesto", imp.codigo);
                    retencion.SetAttribute("Importe", imp.Importe.ToStringRoundedCurrency(moneda));
                }
                return retenciones;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlElement Addenda(XmlDocument xml, AddendaDeCliente addenda, List<DatosExtraPorFactura> datos, VMFactura invoice)
        {
            try
            {
                XDocument template = XDocument.Parse(addenda.xslt);
                XDocument dataSource = GetAddendaDataSource(invoice, addenda.Addenda, datos);

                XDocument result = new XDocument();

                using (XmlWriter writer = result.CreateWriter())
                {
                    using (XmlReader templateReader = template.CreateReader())
                    {
                        using (XmlReader dataSourceReader = dataSource.CreateReader())
                        {
                            XslCompiledTransform transform = new XslCompiledTransform();
                            transform.Load(templateReader);

                            transform.Transform(dataSourceReader, writer);
                        }
                    }
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(result.CreateReader());

                return doc.DocumentElement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Metodos para generacion de addendas

        private XDocument GetAddendaDataSource(VMFactura invoice, Addenda addenda, List<DatosExtraPorFactura> datos)
        {
            XDocument document = new XDocument();

            using (var writer = document.CreateWriter())
            {
                switch (addenda.idAddenda)
                {
                    case (int)Addendas.Gayosso:
                        VMAddendaGayosso viewModelGayosso = new VMAddendaGayosso(invoice, datos);
                        XmlSerializer serializerGayosso = new XmlSerializer(typeof(VMAddendaGayosso));
                        serializerGayosso.Serialize(writer, viewModelGayosso);
                        break;
                    case (int)Addendas.Jardines:
                        VMAddendaJardines viewModelJardines = new VMAddendaJardines(invoice, datos);
                        XmlSerializer serializerJardines = new XmlSerializer(typeof(VMAddendaJardines));
                        serializerJardines.Serialize(writer, viewModelJardines);
                        break;
                    case (int)Addendas.Calimax:
                        DatosExtraPorFactura datoSucursal = datos.FindDatoOrDefault(DatoExtra.Sucursal);
                        Directorio directorio = datoSucursal.isValid() && datoSucursal.valor.isValid() ? _directorio.Find(int.Parse(datoSucursal.valor)) : new Directorio();

                        VMAddendaCalimax viewModelCalimax = new VMAddendaCalimax(invoice, datos, directorio);
                        XmlSerializer serializerCalimax = new XmlSerializer(typeof(VMAddendaCalimax));
                        serializerCalimax.Serialize(writer, viewModelCalimax);
                        break;
                    case (int)Addendas.ComercialMexicana:
                        DatosExtraPorFactura sucursal = datos.FindDatoOrDefault(DatoExtra.Sucursal);
                        DatosExtraPorFactura seccion = datos.FindDatoOrDefault(DatoExtra.Seccion);
                        Directorio directorioSucursal = sucursal.isValid() && sucursal.valor.isValid() ? _directorio.Find(int.Parse(sucursal.valor)) : new Directorio();
                        Seccione seccionSucursal = seccion.isValid() && seccion.valor.isValid() ? _secciones.Find(int.Parse(seccion.valor)) : new Seccione();

                        VMAddendaComercialMexicana viewModelComercialMexicana = new VMAddendaComercialMexicana(invoice, datos, directorioSucursal, seccionSucursal);
                        XmlSerializer serializerComercialMexicana = new XmlSerializer(typeof(VMAddendaComercialMexicana));
                        serializerComercialMexicana.Serialize(writer, viewModelComercialMexicana);
                        break;
                }
            }

            return document;
        }

        #endregion
    }
}
