using Aprovi.Business.Helpers;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Aprovi.Business.Services
{
    public abstract class CertificadoService : ICertificadoService
    {
        private IUnitOfWork _UOW;
        private ICertificadosRepository _certificates;
        private IAplicacionesRepository _app;
        private Criptografia _crypto;
        private IConfiguracionesRepository _config;

        public CertificadoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _certificates = _UOW.Certificados;
            _app = _UOW.Aplicaciones;
            _config = _UOW.Configuraciones;
            _crypto = new Criptografia();
        }

        public Certificado GetDefault()
        {
            try
            {
                return _certificates.GetDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Certificado Configure(string certificateFile, string keyFile, string keyPass, string pfxLocation)
        {
            try
            {
                //Crear el archivo físico pfx con contraseña autogenerada
                //Contraseña = 'Aprovi' + RFC Configurado en Minusculas, Ej: Aprovicom1010055e0

                X509Certificate2 pfxCertificate;
                Certificado certificate;

                //Primero obtengo los archivos en PEM
                var pemCer = _crypto.CerToPem(certificateFile);
                var pemKey = _crypto.KeyToPem(keyFile, keyPass);

                //Ahora si obtengo el X509Certificate2 de esos PEM
                pfxCertificate = _crypto.GetCertificateFromPEM(pemCer, pemKey);

                //Obtengo la configuración actual
                var config = _config.GetDefault();

                //Ahora exporto el X509Certificate2 como pfx
                File.WriteAllBytes(string.Format(@"{0}\CSD{1}.pfx", pfxLocation, config.rfc), pfxCertificate.Export(X509ContentType.Pkcs12, string.Format("Aprovi{0}", config.rfc.ToLower())));

                //Registrar la ruta del archivo físico en AppSettings
                _app.UpdateSetting("CSD", string.Format(@"{0}\CSD{1}.pfx", pfxLocation, config.rfc));

                //Leer los datos del certificado que son expuestos al usuario y actualizar el registro con ellos
                certificate = _certificates.GetDefault();
                certificate.activo = true;
                certificate.certificadoBase64 = Convert.ToBase64String(pfxCertificate.RawData);
                certificate.expedicion = pfxCertificate.NotBefore;
                certificate.vencimiento = pfxCertificate.NotAfter;
                certificate.numero = Reverse(Encoding.ASCII.GetString(pfxCertificate.GetSerialNumber()));

                //Actualizar en la base de datos la información del nuevo certificado
                _certificates.Update(certificate);
                _UOW.Save();

                //Regresar objeto certificado
                return certificate;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Certificado Configure(string pfxFile)
        {
            try
            {
                X509Certificate2 pfxCertificate;
                Certificado certificate;
                
                //Obtengo la configuración actual
                var config = _config.GetDefault();

                //Intenta abrir el archivo pfx con la contraseña autogenerada
                //Contraseña = 'Aprovi' + RFC Configurado en Minusculas, Ej: Aprovicom1010055e0
                pfxCertificate = new X509Certificate2(pfxFile, string.Format("Aprovi{0}", config.rfc.ToLower()));

                //Registrar la ruta del archivo físico en AppSettings
                _app.UpdateSetting("CSD", pfxFile);

                //Leer los datos del certificado que son expuestos al usuario y actualizar el registro con ellos
                certificate = _certificates.GetDefault();
                certificate.activo = true;
                certificate.certificadoBase64 = Convert.ToBase64String(pfxCertificate.RawData);
                certificate.expedicion = pfxCertificate.NotBefore;
                certificate.vencimiento = pfxCertificate.NotAfter;
                //Como volteo el numero
                certificate.numero = Reverse(Encoding.ASCII.GetString(pfxCertificate.GetSerialNumber()));

                //Actualizar en la base de datos la información del nuevo certificado
                _certificates.Update(certificate);
                _UOW.Save();

                //Regresar objeto certificado
                return certificate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string Reverse(string originalString)
        {
            string result = string.Empty;

            for (int i = originalString.Length - 1; i >= 0; i--)
            {
                result += originalString[i];
            }

            return result;
        }
    }
}
