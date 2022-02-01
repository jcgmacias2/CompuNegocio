using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using System.Net.Mail;
using System.Net;
using Aprovi.Data.Repositories;
using System.IO;

namespace Aprovi.Business.Services
{
    public class EnvioDeCorreoService : IEnvioDeCorreoService
    {
        private IUnitOfWork _UOW;
        private IComprobantesEnviadosRepository _comprobantes;
        private IConfiguracionService _configurations;
        private IFacturasRepository _invoices;
        private IAbonosDeFacturaRepository _fiscalPayments;
        private ICotizacionesRepository _quotes;

        public EnvioDeCorreoService(IUnitOfWork unitOfWork, IConfiguracionService configurations)
        {
            _UOW = unitOfWork;
            _comprobantes = _UOW.ComprobantesEnviados;
            _configurations = configurations;
            _invoices = _UOW.Facturas;
            _fiscalPayments = _UOW.AbonosDeFactura;
            _quotes = _UOW.Cotizaciones;
        }

        public void SendTestMail(CuentasGuardian account)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;
            MailAddress address;

            try
            {
                mail = new MailMessage();
                mail.Body = "Verificación de cuenta de correo";
                mail.IsBodyHtml = false;
                address = new MailAddress(account.direccion, "Aprovi");
                mail.From = address;
                mail.To.Add(address);
                mail.Subject = "Verificación Guardián Aprovi";
                mail.Priority = MailPriority.High;
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendMail(Factura invoice)
        {
            var config = _configurations.GetDefault();
            var sent = false;

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendMail(c, config, invoice, null);
                    sent = true;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            if (!sent) //Si no se pudo enviar con ninguna cuenta envío error
                throw new Exception("No fue posible enviar el correo con ninguna de las cuentas configuradas");

        }

        public void SendMail(AbonosDeFactura fiscalPayment)
        {
            var config = _configurations.GetDefault();
            var sent = false;

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendMail(c, config, fiscalPayment, null);
                    sent = true;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            if (!sent) //Si no se pudo enviar con ninguna cuenta envío error
                throw new Exception("No fue posible enviar el correo con ninguna de las cuentas configuradas");

        }

        public void SendMail(Pago payment)
        {
            var config = _configurations.GetDefault();
            var sent = false;

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendMail(c, config, payment, null);
                    sent = true;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            if (!sent) //Si no se pudo enviar con ninguna cuenta envío error
                throw new Exception("No fue posible enviar el correo con ninguna de las cuentas configuradas");

        }

        public void SendMail(List<ComprobantesEnviado> pendingReceipts)
        {
            var config = _configurations.GetDefault();

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            //Como voy a mandar en lote, lo primero que necesito saber es cual de las cuentas es la que funciona?
            CuentasGuardian account = null;

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendTestMail(c);
                    account = c;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            //Aquí ya tengo en account la cuenta que funciona
            try
            {
                //Ahora por cada registro pendiente de envío debo intentar enviar el archivo
                foreach (var p in pendingReceipts)
                {
                    //Dos if's y no if/else por si en un futuro se agregan más Tipos de comprobantes
                    if (p.idTipoDeComprobante.Equals((int)TipoDeComprobante.Factura))
                    {
                        SendMail(account, config, _invoices.Find(p.serie, p.folio), p.idComprobanteEnviado);
                    }

                    if (p.idTipoDeComprobante.Equals((int)TipoDeComprobante.Parcialidad))
                    {
                        SendMail(account, config, _fiscalPayments.FindParcialidad(p.serie, p.folio.ToInt()), p.idComprobanteEnviado);
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendMail(Cotizacione quote, Opciones_Envio_Correo option, string givenEmail)
        {
            var config = _configurations.GetDefault();
            var sent = false;

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendMail(c, config, quote, option, givenEmail);
                    sent = true;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            if (!sent) //Si no se pudo enviar con ninguna cuenta envío error
                throw new Exception("No fue posible enviar el correo con ninguna de las cuentas configuradas");

        }

        public void SendMail(NotasDeCredito creditNote)
        {
            var config = _configurations.GetDefault();
            var sent = false;

            if (config.CuentasGuardians.Count.Equals(0))
                throw new Exception("No existen cuentas de correo configuradas para envío");

            foreach (var c in config.CuentasGuardians)
            {
                try
                {
                    SendMail(c, config, creditNote, null);
                    sent = true;
                    break;
                }
                catch (Exception)
                {
                    //Si la cuenta marcó error, continuo con la siguiente cuenta
                    continue;
                }
            }

            if (!sent) //Si no se pudo enviar con ninguna cuenta envío error
                throw new Exception("No fue posible enviar el correo con ninguna de las cuentas configuradas");
        }

        #region Private

        private void SendMail(CuentasGuardian account, Configuracion config, Factura invoice, int? idComprobanteEnviado)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;

            try
            {
                if (invoice.Cliente.CuentasDeCorreos.Count.Equals(0))
                    return;

                var xml = string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, invoice.serie, invoice.folio);
                var pdf = string.Format("{0}\\{1}{2}.pdf", config.CarpetaPdf, invoice.serie, invoice.folio);

                var envio = new ComprobantesEnviado();
                //Si es un nuevo envio creo el registro
                if (!idComprobanteEnviado.HasValue)
                {
                    //Aqui hago el registro de que se esta enviando
                    envio.fechaHora = DateTime.Now;
                    envio.xml = false; //Lo marco como falso hasta haberlos enviado
                    envio.pdf = false; //Lo marco como falso hasta haberlo enviado
                    envio.idTipoDeComprobante = (int)TipoDeComprobante.Factura;
                    envio.serie = invoice.serie;
                    envio.folio = invoice.folio.ToString();
                    envio = _comprobantes.Add(envio);
                    _UOW.Save();
                }
                else //Si ya existe solo lo cargo en memoria
                {
                    envio = _comprobantes.Find(idComprobanteEnviado.Value);
                }

                //Aqui realizo el envio del correo
                mail = new MailMessage();
                mail.Body = GetTimbradoMessage(config.razonSocial, string.Format("{0}{1}", invoice.serie, invoice.folio), invoice.fechaHora.ToShortDateString(), config.telefono);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(account.direccion, config.razonSocial);
                invoice.Cliente.CuentasDeCorreos.ToList().ForEach(delegate (CuentasDeCorreo mailAdd) { mail.To.Add(new MailAddress(mailAdd.cuenta)); });
                mail.Subject = string.Format("Guardián Aprovi - CFDI {0}{1}", invoice.serie, invoice.folio);
                mail.Priority = MailPriority.High;
                //Agrego el xml
                mail.Attachments.Add(new Attachment(xml));
                //Agrego el pdf
                mail.Attachments.Add(new Attachment(pdf));
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);

                //Si llega aqui actualizo el registro del envio
                envio.xml = File.Exists(xml);
                envio.pdf = File.Exists(pdf);
                _comprobantes.Update(envio);
                _UOW.Save();
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail(CuentasGuardian account, Configuracion config, AbonosDeFactura fiscalPayment,int? idComprobanteEnviado)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;

            try
            {
                if (fiscalPayment.Factura.Cliente.CuentasDeCorreos.Count.Equals(0))
                    return;

                var xml = string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, fiscalPayment.TimbresDeAbonosDeFactura.serie, fiscalPayment.TimbresDeAbonosDeFactura.folio);
                var pdf = string.Format("{0}\\{1}{2}.pdf", config.CarpetaPdf, fiscalPayment.TimbresDeAbonosDeFactura.serie, fiscalPayment.TimbresDeAbonosDeFactura.folio);


                var envio = new ComprobantesEnviado();
                //Si es un nuevo envio creo el registro
                if (!idComprobanteEnviado.HasValue)
                {
                    //Aqui hago el registro de que se esta enviando
                    envio.fechaHora = DateTime.Now;
                    envio.xml = false; //Lo marco como falso hasta haberlos enviado
                    envio.pdf = false; //Lo marco como falso hasta haberlo enviado
                    envio.idTipoDeComprobante = (int)TipoDeComprobante.Parcialidad;
                    envio.serie = fiscalPayment.TimbresDeAbonosDeFactura.serie;
                    envio.folio = fiscalPayment.TimbresDeAbonosDeFactura.folio.ToString();
                    envio = _comprobantes.Add(envio);
                    _UOW.Save();
                }
                else //Si ya existe solo lo cargo en memoria
                {
                    envio = _comprobantes.Find(idComprobanteEnviado.Value);
                }

                //Aqui realizo el envio del correo
                mail = new MailMessage();
                mail.Body = GetTimbradoMessage(config.razonSocial, string.Format("{0}{1}", fiscalPayment.TimbresDeAbonosDeFactura.serie, fiscalPayment.TimbresDeAbonosDeFactura.folio), fiscalPayment.fechaHora.ToShortDateString(), config.telefono);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(account.direccion, config.razonSocial);
                fiscalPayment.Factura.Cliente.CuentasDeCorreos.ToList().ForEach(delegate (CuentasDeCorreo mailAdd) { mail.To.Add(new MailAddress(mailAdd.cuenta)); });
                mail.Subject = string.Format("Guardián Aprovi - CFDI {0}{1}", fiscalPayment.TimbresDeAbonosDeFactura.serie, fiscalPayment.TimbresDeAbonosDeFactura.folio);
                mail.Priority = MailPriority.High;
                //Agrego el xml
                mail.Attachments.Add(new Attachment(xml));
                //Agrego el pdf
                mail.Attachments.Add(new Attachment(pdf));
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);

                //Si llega aqui actualizo el registro del envio
                envio.xml = File.Exists(xml);
                envio.pdf = File.Exists(pdf);
                _comprobantes.Update(envio);
                _UOW.Save();
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail(CuentasGuardian account, Configuracion config, Pago fiscalPayment, int? idComprobanteEnviado)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;

            try
            {
                if (fiscalPayment.Cliente.CuentasDeCorreos.Count.Equals(0))
                    return;

                var xml = string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, fiscalPayment.serie, fiscalPayment.folio);
                var pdf = string.Format("{0}\\{1}{2}.pdf", config.CarpetaPdf, fiscalPayment.serie, fiscalPayment.folio);


                var envio = new ComprobantesEnviado();
                //Si es un nuevo envio creo el registro
                if (!idComprobanteEnviado.HasValue)
                {
                    //Aqui hago el registro de que se esta enviando
                    envio.fechaHora = DateTime.Now;
                    envio.xml = false; //Lo marco como falso hasta haberlos enviado
                    envio.pdf = false; //Lo marco como falso hasta haberlo enviado
                    envio.idTipoDeComprobante = (int)TipoDeComprobante.Parcialidad;
                    envio.serie = fiscalPayment.serie;
                    envio.folio = fiscalPayment.folio.ToString();
                    envio = _comprobantes.Add(envio);
                    _UOW.Save();
                }
                else //Si ya existe solo lo cargo en memoria
                {
                    envio = _comprobantes.Find(idComprobanteEnviado.Value);
                }

                //Aqui realizo el envio del correo
                mail = new MailMessage();
                mail.Body = GetTimbradoMessage(config.razonSocial, string.Format("{0}{1}", fiscalPayment.serie, fiscalPayment.folio), fiscalPayment.fechaHora.ToShortDateString(), config.telefono);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(account.direccion, config.razonSocial);
                fiscalPayment.Cliente.CuentasDeCorreos.ToList().ForEach(delegate (CuentasDeCorreo mailAdd) { mail.To.Add(new MailAddress(mailAdd.cuenta)); });
                mail.Subject = string.Format("Guardián Aprovi - CFDI {0}{1}", fiscalPayment.serie, fiscalPayment.folio);
                mail.Priority = MailPriority.High;
                //Agrego el xml
                mail.Attachments.Add(new Attachment(xml));
                //Agrego el pdf
                mail.Attachments.Add(new Attachment(pdf));
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);

                //Si llega aqui actualizo el registro del envio
                envio.xml = File.Exists(xml);
                envio.pdf = File.Exists(pdf);
                _comprobantes.Update(envio);
                _UOW.Save();
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail(CuentasGuardian account, Configuracion config, Cotizacione quote,Opciones_Envio_Correo option, string givenEmail)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;

            try
            {
                if ((option == Opciones_Envio_Correo.Configuradas && quote.Cliente.CuentasDeCorreos.Count.Equals(0)) || (option == Opciones_Envio_Correo.Proporcionada && !givenEmail.isValid()))
                    return;

                //Obtiene la ruta de la carpeta de cotizaciones
                string path = Path.Combine(config.CarpetaReportes, "Cotizaciones"); ;
                var pdf = string.Format("{0}\\{1}.pdf", path, quote.folio);

                //Aqui realizo el envio del correo
                mail = new MailMessage();
                mail.Body = GetCotizacionMessage(quote.Cliente.razonSocial,quote.Cliente.contacto,config.razonSocial, quote.folio.ToString(), quote.fechaHora.ToString("dd/MM/yyyy"), config.telefono,quote.Usuario.nombreCompleto);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(account.direccion, config.razonSocial);

                if (option == Opciones_Envio_Correo.Configuradas)
                {
                    quote.Cliente.CuentasDeCorreos.ToList().ForEach(delegate (CuentasDeCorreo mailAdd) { mail.To.Add(new MailAddress(mailAdd.cuenta)); });
                }

                if (option == Opciones_Envio_Correo.Proporcionada)
                {
                    mail.To.Add(new MailAddress(givenEmail));
                }

                mail.Subject = string.Format("Guardián Aprovi - Cotización {0}", quote.folio);
                mail.Priority = MailPriority.High;
                //Agrego el pdf
                mail.Attachments.Add(new Attachment(pdf));
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail(CuentasGuardian account, Configuracion config, NotasDeCredito creditNotes, int? idComprobanteEnviado)
        {
            MailMessage mail;
            SmtpClient mailClient;
            NetworkCredential credentials;

            try
            {
                if (creditNotes.Cliente.CuentasDeCorreos.Count.Equals(0))
                    return;

                var xml = string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, creditNotes.serie, creditNotes.folio);
                var pdf = string.Format("{0}\\{1}{2}.pdf", config.CarpetaPdf, creditNotes.serie, creditNotes.folio);

                var envio = new ComprobantesEnviado();
                //Si es un nuevo envio creo el registro
                if (!idComprobanteEnviado.HasValue)
                {
                    //Aqui hago el registro de que se esta enviando
                    envio.fechaHora = DateTime.Now;
                    envio.xml = false; //Lo marco como falso hasta haberlos enviado
                    envio.pdf = false; //Lo marco como falso hasta haberlo enviado
                    envio.idTipoDeComprobante = (int)TipoDeComprobante.Nota_De_Credito;
                    envio.serie = creditNotes.serie;
                    envio.folio = creditNotes.folio.ToString();
                    envio = _comprobantes.Add(envio);
                    _UOW.Save();
                }
                else //Si ya existe solo lo cargo en memoria
                {
                    envio = _comprobantes.Find(idComprobanteEnviado.Value);
                }

                //Aqui realizo el envio del correo
                mail = new MailMessage();
                mail.Body = GetTimbradoMessage(config.razonSocial, string.Format("{0}{1}", creditNotes.serie, creditNotes.folio), creditNotes.fechaHora.ToShortDateString(), config.telefono);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(account.direccion, config.razonSocial);
                creditNotes.Cliente.CuentasDeCorreos.ToList().ForEach(delegate (CuentasDeCorreo mailAdd) { mail.To.Add(new MailAddress(mailAdd.cuenta)); });
                mail.Subject = string.Format("Guardián Aprovi - CFDI {0}{1}", creditNotes.serie, creditNotes.folio);
                mail.Priority = MailPriority.High;
                //Agrego el xml
                mail.Attachments.Add(new Attachment(xml));
                //Agrego el pdf
                mail.Attachments.Add(new Attachment(pdf));
                mailClient = new SmtpClient();
                mailClient.UseDefaultCredentials = false;
                credentials = new NetworkCredential();
                credentials.UserName = account.direccion;
                credentials.Password = account.contrasena;
                mailClient.Credentials = credentials;
                mailClient.Host = account.servidor;
                mailClient.EnableSsl = account.ssl;
                mailClient.Port = account.puerto;
                mailClient.Timeout = 60000;
                mailClient.Send(mail);

                //Si llega aqui actualizo el registro del envio
                envio.xml = File.Exists(xml);
                envio.pdf = File.Exists(pdf);
                _comprobantes.Update(envio);
                _UOW.Save();
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetTimbradoMessage(string razonSocial, string folio, string fecha, string telefono)
        {
            StringBuilder msg;

            msg = new StringBuilder();

            msg.Append("<p>A quién corresponda:<p/><p><strong>");
            msg.AppendFormat("{0}</strong> a través del servicio <strong>Guardián Aprovi</strong> y en cumplimiento de la Resolución Miscelánea Fiscal vigente hace entrega del Comprobante Fiscal Digital <strong> No. {1} </strong>de la transacción efectuada el dia <strong>{2}</strong>, el cual se encuentra adjunto a este mensaje.<p/><p>", razonSocial, folio, fecha);
            msg.Append("Este correo ha sido generado de forma automática, favor de no responder a este correo porque no será leído.<p/><p>");
            msg.AppendFormat("Para contactarnos, puede comunicarse al teléfono :<strong> {0}</strong><p/><p>", telefono);
            msg.Append("Gracias por su preferencia.<p/><p>");
            msg.Append("Atentamente:<p/><p>");
            msg.AppendFormat("{0}<p/><p style='font-size:11px'>", razonSocial);
            msg.AppendFormat("Con previa autorización este correo electrónico ha sido enviado por {0} a esta cuenta de correo, la cual fue proporcionada por su organización con el fin de recibir los Comprobantes Fiscales Digitales Correspondientes, si desea cambiar la cuenta de correo, o dejar de recibirlos, favor de ponerse en contacto en los telefonos arriba mencionados.</p>", razonSocial);

            return msg.ToString();
        }

        public string GetCancelacionMessage(string razonSocial, string folio, string fecha, string telefono, string motivo)
        {
            StringBuilder msg;

            msg = new StringBuilder();

            msg.Append("<p>A quién corresponda:<p/><p><strong>");
            msg.AppendFormat("{0}</strong> a través del servicio <strong>Guardián Aprovi</strong> y en cumplimiento de la Resolución Miscelánea Fiscal vigente hace entrega del Acuse de Cancelación del Comprobante Fiscal Digital <strong> No. {1}-Acuse Cancelación </strong>de la transacción efectuada el dia <strong>{2}</strong>, el cual se encuentra adjunto a este mensaje.<p/><p>", razonSocial, folio, fecha);
            msg.AppendFormat("Motivo de cancelación <strong>{0}</strong><p/><p>", motivo);
            msg.Append("Este correo ha sido generado de forma automática, favor de no responder a este correo porque no será leído.<p/><p>");
            msg.AppendFormat("Para contactarnos, puede comunicarse al teléfono :<strong> {0}</strong><p/><p>", telefono);
            msg.Append("Gracias por su preferencia.<p/><p>");
            msg.Append("Atentamente:<p/><p>");
            msg.AppendFormat("{0}<p/><p style='font-size:11px'>", razonSocial);
            msg.AppendFormat("Con previa autorización este correo electrónico ha sido enviado por {0} a esta cuenta de correo, la cual fue proporcionada por su organización con el fin de recibir los Comprobantes Fiscales Digitales Correspondientes, si desea cambiar la cuenta de correo, o dejar de recibirlos, favor de ponerse en contacto en los telefonos arriba mencionados.</p>", razonSocial);

            return msg.ToString();
        }

        public string GetCotizacionMessage(string razonSocialCliente,string nombreContacto,string razonSocial, string folio, string fecha, string telefono, string nombreUsuario)
        {
            StringBuilder msg;

            msg = new StringBuilder();

            msg.AppendFormat("<p><strong>{0}:</strong><br/>",razonSocialCliente);

            if (nombreContacto.isValid())
            {
                msg.AppendFormat("{0}<p/>",nombreContacto);
            }
            else
            {
                msg.Append("</p>");
            }

            msg.AppendFormat("<strong>{0}</strong> a través del servicio <strong>Guardián Aprovi</strong> hace entrega de la cotización <strong>{1}</strong> solicitada el día <strong>{2}</strong>, la cual se encuentra adjunta a este mensaje.<p/><p>", razonSocial, folio, fecha);
            msg.Append("Este correo ha sido generado de forma automática, favor de no responder a este correo porque no será leído.<p/><p>");
            msg.AppendFormat("Para contactarnos, puede comunicarse al teléfono :<strong> {0}</strong><p/><p>", telefono);
            msg.Append("Gracias por su preferencia, quedamos a sus órdenes para cualquier duda al respecto.<p/><p>");
            msg.Append("Saludos cordiales.<p/><p>");
            msg.AppendFormat("{0}<p/><p style='font-size:8px'>", nombreUsuario);
            msg.AppendFormat("Con previa autorización este correo electrónico ha sido enviado por {0} a esta cuenta de correo, la cual fue proporcionada por su organización con el fin de recibir correspondencia electrónica, si desea cambiar la cuenta de correo, o dejar de recibirlos, favor de ponerse en contacto en los telefonos arriba mencionados.</p>", razonSocial);

            return msg.ToString();
        }

        #endregion
    }
}
