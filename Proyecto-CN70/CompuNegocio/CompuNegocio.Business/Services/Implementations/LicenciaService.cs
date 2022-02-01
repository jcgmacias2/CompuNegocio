using Aprovi.Business.Helpers;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;

namespace Aprovi.Business.Services
{
    public abstract class LicenciaService : ILicenciaService
    {
        private IUnitOfWork _UOW;
        private IAplicacionesRepository _app;
        private IConfiguracionesRepository _config;
        private Criptografia _crypto;

        public LicenciaService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _app = _UOW.Aplicaciones;
            _config = _UOW.Configuraciones;
            _crypto = new Criptografia();
        }

        public string Authenticate(string license)
        {
            //5.1 Autenticación de la licencia: La autenticación es el proceso que certifica que la licencia fue generada por Aprovi para esta empresa y el proceso de autenticación es asi:
            //5.1.1 Transformar la licencia de hexadecimal a utf
            //5.1.2 Desencriptar la cadena resultante utilizando como passscode/key el RFC de la empresa en minúsculas
            //5.1.3 Al desencriptarlo obtendrá una cadena de numeros la cual deberá descomponerse:
            //los primeros dos caracteres corresponden al sistema (EnumeradorSistema)
            //los segundos dos digitos corresponden al número de licencias generadas a ese RFC, (el cual marca el límite máximo de cajas que puede haber, es decir, si esos números son un "02" quiere decir que esta es la segunda licencia generada a ese RFC y por tanto puede haber ya una caja configurada (pero no 2), si fueron "03" esta es la tercera y por tanto puede haber 2 cajas ya registradas y así respectivamente;)
            //los siguientes dos son el número de estaciones autorizadas para esa caja,
            //el resto de la cadena obtenida de la licencia representa el idUsuario al cual se le registro la licencia en www.aprovi.com.mx. y con el es que se realiza el proceso de verificación.
            int value;
            EnumSistemas sistema;
            try
            {
                //Decodifico del formato hexadecimal en que viene la licencia a utf
                var utfLicence = _crypto.HexToUtf(license);

                //Decodifico la licencia utilizando el rfc de la empresa en minúsculas
                var code = _crypto.Decipher(utfLicence, _config.GetDefault().rfc.ToLower());

                //Obtengo el sistema que es
                sistema = (EnumSistemas)Enum.Parse(typeof(EnumSistemas), code.Substring(0, 2));
                //Valido que sea para el mismo sistema que esta configurado
                var idSistema = _app.ReadSetting("System").ToInt();
                if (!idSistema.Equals((int)sistema))
                    throw new Exception("La licencia no fue creada para el sistema que esta utilizando");

                //Obtengo el número de empresa
                if (!int.TryParse(code.Substring(2, 2), out value))
                    throw new Exception("La licencia no es autentica");
                var maxRegisters = value;

                //Obtengo el número de estaciones permitidas para esta caja
                if (!int.TryParse(code.Substring(4, 2), out value))
                    throw new Exception("La licencia no es autentica");
                var stations = value;
                
                //Obtengo el id del Usuario al que se le registro la licencia
                if (!int.TryParse(code.Substring(6), out value))
                    throw new Exception("La licencia no es autentica o no fue registrada para esta empresa");
                var idUser = value;

                //Regreso el id del usuario pues es requerido para el siguiente paso
                return idUser.ToString();
            }
            catch(CryptographicException)
            {
                throw new Exception("La licencia no fue creada para este constribuyente");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Verify(string idUser)
        {
           // 5.2 Verificación de la licencia: El proceso de verificación revisa que la licencia este activa y que sea efectivamente una licencia generada para la cuenta de usuario que se configuro en el sistema, lo cual se lleva a cabo de la siguiente manera:
           // 5.2.1 Obtener el valor de appSettings dentro de la llave "Copyright" la cual contiene la dirección de correo electrónico que se configuro al llevar a cabo la instalación del sistema.
           // 5.2.2 Encriptar el valor de copyright (cuenta de correo) utilizando la fecha del dia en formato ("MM/dd/yyyy").
           // 5.2.3 Codificar el valor del 5.2.2 a hexadecimal ya que contiene caracters invalidos para un url
           // 5.2.4 Hacer una llamada a la Web API api.aprovi.com.mx/v2/Verification/{parametro} donde parametro es sustituido por la cadena obtenida en 5.2.3
           // 5.2.5 La Web API responderá con una cadena json, la cual debe deserializarse y desencriptarse utilizando la fecha del día en formato ("MM/dd/yyyy").
           // 5.2.6 El resultante de 5.2.4 debe ser una cadena con un número entero, y este número debe ser igual al obtenido en el proceso de autenticación (5.1)
            HttpWebRequest apiRequest;
            string response;
            string token;
            dynamic jsonResponse;
            string mailUserId;

            try
            {
                //Obtengo el correo que se encuentra en "Copyright" en appSettings
                var correo = _app.ReadSetting("Copyright").ToString();

                //El correo debe ir encriptado utilizando la fecha del día
                correo = _crypto.Cipher(correo);

                //Lo convierto a hex, pues algunos caracteres especiales contenidos dentro de la dirección de correo como '.' o '@' no son permitidos en url
                correo = _crypto.UtfToHex(correo);

                //Y codificado para url
                correo = HttpUtility.UrlEncode(correo);

                //Preparo la llamada
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

                apiRequest = (HttpWebRequest)WebRequest.Create(string.Format("Verification/{0}", correo).ToApiUrl());

                apiRequest.Method = "GET";
                //apiRequest.ContentType = "application/x-www-form-urlencoded";
                apiRequest.ContentType = "application/json";

                //Hago la llamada y Recibo respuesta
                response = new StreamReader(apiRequest.GetResponse().GetResponseStream()).ReadToEnd();

                //Descifro la respuesta, que debe traer como parametro IdUser
                token = HttpUtility.UrlDecode(response);
                //La comunicación con la API usa como llave de cifrado la fecha
                token = _crypto.Decipher(token);
                jsonResponse = JsonConvert.DeserializeObject(token);
                //La web api me responde con el id del usuario que corresponde al correo que le mande
                mailUserId = jsonResponse.IdUser;

                //El id que me trae la respuesta debe ser igual al id que se le paso como parametro a este método
                return mailUserId.Equals(idUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Activate(string license)
        {
            //5.3 Activación : Una vez que el proceso de verificación ha sido exitoso, entonces debe realizarse la activación de la licencia para que no pueda volver a ser utilizada, lo cual se hace de una manera muy sencilla que se detalla a continuación:
            //5.3.1 Hacer una llamada a la Web API pi.aprovi.com.mx/v2/Activation/{parametro} donde parametro es sustituido por la cifrada con la fecha del día y en hexadecimal
            //5.3.3 La Web API responderá con una cadena json, la cual debe deserializarse y desencriptarse utilizando la fecha del día en formato ("MM/dd/yyyy").
            //5.3.4 Si la activación fue realizada con exito la respuesta será Activated = true
            //5.3.5 Almacenar en la base de datos local la licencia en el registro de la caja.
            HttpWebRequest apiRequest;
            string response;
            string token;
            dynamic jsonResponse;
            bool activated;

            try
            {
                //La licencia debe ir encriptada utilizando la fecha del día
                license = _crypto.Cipher(license);

                //En hexadecimal por los caracteres invalidos en url
                license = _crypto.UtfToHex(license);

                //Y codificado para url
                license = HttpUtility.UrlEncode(license);

                //Preparo la llamada
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

                //MT: En Development puedo probar utilizando localhost
                //apiRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://localhost:55125/v2/Activation/{0}", license));

                apiRequest = (HttpWebRequest)WebRequest.Create(string.Format("Activation/{0}", license).ToApiUrl());

                apiRequest.Method = "GET";
                //apiRequest.ContentType = "application/x-www-form-urlencoded";
                apiRequest.ContentType = "application/json";

                //Hago la llamada y Recibo respuesta
                response = new StreamReader(apiRequest.GetResponse().GetResponseStream()).ReadToEnd();

                //Descifro la respuesta, que debe traer como parametro Activated
                token = HttpUtility.UrlDecode(response);
                //La comunicación con la API usa como llave de cifrado la fecha
                token = _crypto.Decipher(token);
                jsonResponse = JsonConvert.DeserializeObject(token);
                //La web api me responde con un True en Activated si pudo realizarse exitosamente
                activated = jsonResponse.Activated;

                return activated;
            }
            catch (WebException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Validate(string license, int stamps)
        {
            //0.- Al instalar el sistema solicitará un correo electrónico (debe ser el mismo al que se le registro la licencia en aprovi.com.mx)
            //0.1 - Esto se ira al Copyright key en los AppSettings
            //1.- Al iniciar sesión en el sistema si las credenciales son validas y se encuentra en modo "Production" se validará la licencia configurada
            //2.- Para validar la licencia se deben seguir los siguientes pasos:
            //2.1 - Obtener el valor de appSettings dentro de la llave "Copyright" la cual contiene la dirección de correo electrónico que se configuro al llevar a cabo la instalación del sistema.
            //2.2 - Encriptar el valor de copyright (cuenta de correo) utilizando la fecha del dia en formato ("MM/dd/yyyy").
            //2.3 - Codificar el valor del 2.2 a hexadecimal ya que contiene caracters invalidos para un url
            //2.4 - Obtener de la base de datos la licencia registrada en la caja.
            //2.5 - Obtener de la base de datos la cantidad de timbres utilizados en el sistema (VwTimbres)
            //2.6 - Crear un objeto JSON con ambos valores {Code: Licencia, Timbres: NTimbres}
            //2.7 - Serializar el objeto JSON
            //2.8 - Encriptar la cadena Serializada del punto 2.7 utilizando el correo de la llave "Copyright"
            //2.9 - Convierto a hex la cadena del punto 2.8 ya que puede contener caracteres invalidos
            //2.10 - Hacer una llamada a la Web API api.aprovi.com.mx/v1/Validation/{mail}/{code} donde mail es sustituido por la cadena obtenida en 2.3 y code es subsituido por la cadena obtenida en 2.9
            //3.0 - La Web API responderá con una cadena json, la cual debe deserializarse y desencriptarse utilizando la fecha del día en formato ("MM/dd/yyyy").
            //3.1 - Si la validadión fue realizada con exito la respuesta será Activated = True/False según sea el caso
            //3.2 - Si Activated = True permite continuar el flujo de inicio de sesión
            //3.3 - Si Activated = False envía mensaje al usuario se que la licencia esta Vencida y debe comunicarse a Aprovi

            HttpWebRequest apiRequest;
            string response;
            string token;
            dynamic jsonResponse;
            bool activated;

            try
            {
                //Valido los datos que llegan
                if (!license.isValid())
                    throw new Exception("No existe una licencia configurada");

                //Obtengo la cuenta de usuario al que se le registro el sistema
                var copyright = _app.ReadSetting("Copyright").ToString();

                if (!copyright.isValid())
                    throw new Exception("El sistema no se encuentra registrado");

                //Creo el objeto Json con la licencia y la  cantidad de timbres
                var info = JsonConvert.SerializeObject(new { Code = license, Timbres = stamps });
                //Encripto la información utilizando el correo copyright
                info = _crypto.Cipher(info, copyright);
                //En hexadecimal por los caracteres invalidos en url
                info = _crypto.UtfToHex(info);

                //el copyright se va también cifrado, utilizando la fecha del día
                copyright = _crypto.Cipher(copyright);
                copyright = _crypto.UtfToHex(copyright);

                //Preparo la llamada
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

                apiRequest = (HttpWebRequest)WebRequest.Create(string.Format("Validation/{0}/{1}", copyright, info).ToApiUrl());

                apiRequest.Method = "GET";
                //apiRequest.ContentType = "application/x-www-form-urlencoded";
                apiRequest.ContentType = "application/json";

                //Hago la llamada y Recibo respuesta
                response = new StreamReader(apiRequest.GetResponse().GetResponseStream()).ReadToEnd();

                //Descifro la respuesta, que debe traer como parametro Activated
                token = HttpUtility.UrlDecode(response);
                //La comunicación con la API usa como llave de cifrado la fecha
                token = _crypto.Decipher(token);
                jsonResponse = JsonConvert.DeserializeObject(token);
                //La web api me responde con un True en Activated si pudo realizarse exitosamente
                activated = jsonResponse.Activated;

                return activated;
            }
            catch (WebException ex)
            {
                if (ex.Message.isValid() && ex.Message.Contains("401"))
                    throw new Exception("Usuario corporativo no existe / Licencia no válida");
                else
                    throw ex;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int IncludedStations(string license)
        {
            try
            {
                int stations;

                //Decodifico del formato hexadecimal en que viene la licencia a utf
                var utfLicence = _crypto.HexToUtf(license);

                //Decodifico la licencia utilizando el rfc de la empresa en minúsculas
                var code = _crypto.Decipher(utfLicence, _config.GetDefault().rfc.ToLower());

                //Obtengo el número de estaciones permitidas para esta empresa
                if (!int.TryParse(code.Substring(4, 2), out stations))
                    throw new Exception("La licencia no es auténtica");

                return stations;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
