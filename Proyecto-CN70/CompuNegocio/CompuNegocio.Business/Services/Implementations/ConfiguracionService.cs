using Aprovi.Business.Helpers;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace Aprovi.Business.Services
{
    public abstract class ConfiguracionService : CuentaGuardianService, IConfiguracionService
    {
        private IUnitOfWork _UOW;
        private IConfiguracionesRepository _configurations;
        private IAplicacionesRepository _applications;
        private IEmpresasRepository _businesses;
        private IEstacionesRepository _stations;
        private ISeriesRepository _series;
        private IOpcionesCostosRepository _opcionesCostos;
        
        private Criptografia _crypt;

        public ConfiguracionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _UOW = unitOfWork;
            _configurations = _UOW.Configuraciones;
            _applications = _UOW.Aplicaciones;
            _businesses = _UOW.Empresas;
            _stations = _UOW.Estaciones;
            _series = _UOW.Series;
            _opcionesCostos = _UOW.OpcionesCostos;
            _crypt = new Criptografia();
        }

        public Configuracion GetDefault()
        {
            try
            {
                Configuracion config;

                //Solo llena la parte que viene directo de la base de datos
                config = new Configuracion(_configurations.GetDefault());

                //Si hay contraseña del PAC cifrada, la descifra para que este lista para usarse
                if(config.contraseñaPAC.isValid() && config.contraseñaPAC.Last().Equals('='))
                    config.contraseñaPAC = _crypt.Decipher(config.contraseñaPAC, _applications.ReadSetting("Copyright").ToString());

                //Hay que cargar la parte que se encuentra en la implementación
                var idStation = _applications.ReadSetting("Estacion").ToInt();
                //Puede no tener ninguna estacion configurada
                if (idStation.isValid())
                    config.Estacion = _stations.Find(idStation);
                else
                    config.Estacion = null;

                //Cargo la información del ambiente
                config.Mode = (Ambiente)Enum.Parse(typeof(Ambiente), _applications.ReadSetting("Environment").ToString(), true);

                if(config.Mode.Equals(Ambiente.Production))
                {
                    //Si tiene un signo '=' al final hay que desencriptarla, de lo contrario quiere decir que ya esta desenscriptada
                    if (config.contraseñaPAC.Substring(config.contraseñaPAC.Length - 1).Equals("="))
                        config.contraseñaPAC = _crypt.Decipher(config.contraseñaPAC, _applications.ReadSetting("Copyright").ToString());
                }

                //Cargo la información de la carpeta de reportes y el logo
                config.CarpetaReportes = _applications.ReadSetting("Reports").ToString();
                var logo = _applications.ReadSetting("Logo").ToString();
                if(logo.isValid())
                    config.Logo = new Uri(logo);

                //Cargar la información de las carpetas de archivos
                config.CarpetaXml = _applications.ReadSetting("Xml").ToString();
                config.CarpetaPdf = _applications.ReadSetting("Pdf").ToString();
                config.CarpetaCbb = _applications.ReadSetting("Cbb").ToString();

                //Cargo la información respecto al uso de scanner y cajon de efectivo
                config.Escaner = _applications.ReadSetting("Scanner").ToInt() > 0; //0 false, 1 true
                config.CodigoEscaner = _applications.ReadSetting("ScannerCode").ToString(); 
                config.CajonDeEfectivo = _applications.ReadSetting("Drawer").ToInt() > 0; //0 false, 1 true

                return config;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Configuracion Update(Configuracion configuration)
        {
            try
            {
                //Actualizo la información del app config
                //Estación relacionada al equipo
                if (configuration.Estacion.isValid())
                {
                    _applications.UpdateSetting("Estacion", configuration.Estacion.idEstacion.ToString());
                    //Cargo la parte de catálogos
                    configuration = UpdateSettings(configuration.Estacion.Empresa.licencia);
                }

                //Logo
                if(configuration.Logo.isValid())
                    _applications.UpdateSetting("Logo", configuration.Logo.OriginalString);

                //Reportes MT: No hay ningúna interacción con el usuario
                //_applications.UpdateSetting("Reports", configuration.CarpetaReportes);

                //Xml's
                _applications.UpdateSetting("Xml", configuration.CarpetaXml);

                //Pdf's
                _applications.UpdateSetting("Pdf", configuration.CarpetaPdf);

                //Cbb's
                _applications.UpdateSetting("Cbb", configuration.CarpetaCbb);

                //Información respecto al uso de scanner y cajon de efectivo
                _applications.UpdateSetting("Scanner", configuration.Escaner ? "1" : "0");
                _applications.UpdateSetting("ScannerCode", configuration.CodigoEscaner.isValid() ? configuration.CodigoEscaner : string.Empty);
                _applications.UpdateSetting("Drawer", configuration.CajonDeEfectivo ? "1" : "0");

                //Ahora si actualizo lo de la base de datos
                var local = _configurations.Find(configuration.idConfiguracion);
                local.razonSocial = configuration.razonSocial;
                local.rfc = configuration.rfc;
                local.tipoDeCambio = configuration.tipoDeCambio;
                local.telefono = configuration.telefono;
                local.idOpcionCostoDisminuye = configuration.idOpcionCostoDisminuye;
                local.idOpcionCostoAumenta = configuration.idOpcionCostoAumenta;
                local.Domicilio.calle = configuration.Domicilio.calle;
                local.Domicilio.numeroExterior = configuration.Domicilio.numeroExterior;
                local.Domicilio.numeroInterior = configuration.Domicilio.numeroInterior;
                local.Domicilio.colonia = configuration.Domicilio.colonia;
                local.Domicilio.ciudad = configuration.Domicilio.ciudad;
                local.Domicilio.estado = configuration.Domicilio.estado;
                local.Domicilio.codigoPostal = configuration.Domicilio.codigoPostal;                
                local.Domicilio.idPais = configuration.Domicilio.idPais;
                local.FormatosPorConfiguracions = configuration.FormatosPorConfiguracions;

                _configurations.Update(local);
                _UOW.Save();

                var newConfig = new Configuracion(local, configuration.Estacion, configuration.Escaner, configuration.CodigoEscaner, configuration.CajonDeEfectivo);

                //Le agrego los valores que se almacenan localmente
                newConfig.CarpetaCbb = configuration.CarpetaCbb;
                newConfig.CarpetaPdf = configuration.CarpetaPdf;
                newConfig.CarpetaXml = configuration.CarpetaXml;
                newConfig.Logo = configuration.Logo;

                return newConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Configuracion UpdatePAC(Configuracion configuration)
        {
            try
            {
                var local = _configurations.Find(configuration.idConfiguracion);
                local.usuarioPAC = configuration.usuarioPAC;
                //El key del crifrado es el valor de "Copyright" dentro de los app settings
                local.contraseñaPAC = _crypt.Cipher(configuration.contraseñaPAC, _applications.ReadSetting("Copyright").ToString());
                _configurations.Update(local);
                _UOW.Save();

                //La configuración que regreso debe tener la contraseña sin crifrado, lista para usarse
                local.contraseñaPAC = configuration.contraseñaPAC;
                return new Configuracion(local, configuration.Estacion, configuration.Escaner, configuration.CodigoEscaner, configuration.CajonDeEfectivo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateConnection(string server, string user, string password, string database)
        {
            try
            {
                _applications.UpdateConnectionString("CNEntities", string.Format("metadata=res://*/Models.CNModel.csdl|res://*/Models.CNModel.ssdl|res://*/Models.CNModel.msl;provider=System.Data.SqlClient;provider connection string='data source={0};initial catalog={1};persist security info=True;user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework'", server, database, user, password));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Configuracion UpdateSettings(string license)
        {
            //1.- Al iniciar sesión en el sistema y una vez que se validaron las credenciales se configurará el ambiente para el usuario el cual puede incluir módulos 
            //2.- Para obtener estos datosvalidar la licencia se deben seguir los siguientes pasos:
            //2.1 - Obtener el valor de appSettings dentro de la llave "Copyright" la cual contiene la dirección de correo electrónico que se configuro al llevar a cabo la instalación del sistema.
            //2.2 - Encriptar el valor de copyright (cuenta de correo) utilizando la fecha del dia en formato ("MM/dd/yyyy").
            //2.3 - Codificar el valor del 2.2 a hexadecimal ya que contiene caracters invalidos para un url
            //2.4 - Obtener de la base de datos la licencia registrada en la caja.
            //2.5 - Crear un objeto JSON con este valor {Code: Licencia}
            //2.6 - Serializar el objeto JSON
            //2.7 - Encriptar la cadena Serializada del punto 2.6 utilizando el correo de la llave "Copyright"
            //2.8 - Convierto a hex la cadena del punto 2.7 ya que puede contener caracteres invalidos
            //2.9 - Hacer una llamada a la Web API api.aprovi.com.mx/v2/Configuration/{mail}/{code} donde mail es sustituido por la cadena obtenida en 2.3 y code es subsituido por la cadena obtenida en 2.8
            //3.0 - La Web API responderá con una cadena json, la cual debe deserializarse y desencriptarse utilizando la fecha del día en formato ("MM/dd/yyyy").
            //3.1 - Si la validadión fue realizada con exito la respuesta será {Sistema = 'Sistema', Modulos = {[List de modulos]}, Folios = {[Lista de folios]}}
            //3.2 - Actualizar los valores de la configuración
            //3.3 - Actualizar las series de folios

            HttpWebRequest apiRequest;
            string response;
            string token;
            dynamic jsonResponse;
            Configuracion config;

            try
            {
                //Valido los datos que llegan
                if (!license.isValid())
                    throw new Exception("No existe una licencia configurada");

                //Obtengo la cuenta de usuario al que se le registro el sistema
                var copyright = _applications.ReadSetting("Copyright").ToString();

                if (!copyright.isValid())
                    throw new Exception("El sistema no se encuentra registrado");

                //Creo el objeto Json con la licencia y la  cantidad de timbres
                var info = JsonConvert.SerializeObject(new { Code = license});
                //Encripto la información utilizando el correo copyright
                info = _crypt.Cipher(info, copyright);
                //En hexadecimal por los caracteres invalidos en url
                info = _crypt.UtfToHex(info);

                //el copyright se va también cifrado, utilizando la fecha del día
                copyright = _crypt.Cipher(copyright);
                copyright = _crypt.UtfToHex(copyright);

                //Preparo la llamada
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

                apiRequest = (HttpWebRequest)WebRequest.Create(string.Format("Configuration/{0}/{1}", copyright, info).ToApiUrl());

                apiRequest.Method = "GET";
                //apiRequest.ContentType = "application/x-www-form-urlencoded";
                apiRequest.ContentType = "application/json";

                //Hago la llamada y Recibo respuesta
                response = new StreamReader(apiRequest.GetResponse().GetResponseStream()).ReadToEnd();

                //Descifro la respuesta, que debe traer como parametro Activated
                token = HttpUtility.UrlDecode(response);
                //La comunicación con la API usa como llave de cifrado la fecha
                token = _crypt.Decipher(token);
                jsonResponse = JsonConvert.DeserializeObject(token);
                //3.0 - La Web API responderá con una cadena json, la cual debe deserializarse y desencriptarse utilizando la fecha del día en formato ("MM/dd/yyyy").
                config = this.GetDefault();

                //3.1 - Si la validadión fue realizada con exito la respuesta será {Sistema = 'Sistema', Modulos {[List de modulos]}}
                //3.2 - Actualizar los valores de la configuración 
                if (jsonResponse.Sistema == null || jsonResponse.Sistema == string.Empty)
                    config.Sistema = Customization.Default;
                else
                    config.Sistema = (Customization)jsonResponse.Sistema;

                var modulos = jsonResponse.Modulos;
                for (int i = 0; i < modulos.Count; i++)
                {
                    config.Modulos.Add((Modulos)modulos[i]);
                }

                //3.3 - Actualizar las series de folios
                var folios = jsonResponse.Folios;
                for (int i = 0; i < folios.Count; i++)
                {
                    string s = folios[i];
                    var serieRemota = s.Split('|');

                    var serie = _series.Find(s[0].ToString());
                    if (serie.isValid())
                        serie.folioFinal = serieRemota[2].ToInt();
                    else
                        _series.Add(new Series() { idConfiguracion = config.idConfiguracion, identificador = serieRemota[0].ToString(), folioInicial = serieRemota[1].ToInt(), folioFinal = serieRemota[2].ToInt() });
                }

                return config;
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
    }
}
