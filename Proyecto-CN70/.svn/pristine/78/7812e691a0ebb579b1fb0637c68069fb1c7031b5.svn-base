using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace CompuNegocio.DemoHelper
{
    [RunInstaller(true)]
    public partial class DemoConfiguration : System.Configuration.Install.Installer
    {
        public DemoConfiguration()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            //Cargo el archivo de configuracion
            string appConfigPath = string.Format(@"{0}\CompuNegocio.exe.config", new FileInfo(Context.Parameters["assemblypath"]).DirectoryName);
            XmlDocument xmlDoc = new XmlDocument();
            WriteLog("Cargando archivo de configuración {0}", appConfigPath);
            xmlDoc.Load(appConfigPath);

            //Empiezo a hacer los cambios

            //Vendrá preconfigurado con Ambiente Demo Environment
            xmlDoc = UpdateAppKey(xmlDoc, "Environment", "Demo");
            //No solicitará credenciales en la instalación
            xmlDoc = UpdateAppKey(xmlDoc, "Copyright", "demo@aprovi.com.mx");

            //Le especifico que es CompuNegocio
            xmlDoc = UpdateAppKey(xmlDoc, "System", "1");

            var applicationFolder = new FileInfo(Context.Parameters["assemblypath"]).DirectoryName;
            //Establecer la ruta en la configuración para utilizar el pfx implementado
            xmlDoc = UpdateAppKey(xmlDoc, "CSD", string.Format(@"{0}\Files\CSDCOM1010055E0.pfx", applicationFolder));
            //Establecer la ruta de reportes en la implementación
            xmlDoc = UpdateAppKey(xmlDoc, "Reports", string.Format(@"{0}\Reports\", applicationFolder));

            //Establecer la ruta de reportes en la implementación
            xmlDoc = UpdateAppKey(xmlDoc, "XML", string.Format(@"{0}\XML\", applicationFolder));
            //Establecer la ruta de reportes en la implementación
            xmlDoc = UpdateAppKey(xmlDoc, "CBB", string.Format(@"{0}\CBB\", applicationFolder));
            //Establecer la ruta de reportes en la implementación
            xmlDoc = UpdateAppKey(xmlDoc, "PDF", string.Format(@"{0}\PDF\", applicationFolder));

            // Traerá por default la conexión a la base de datos "demo"
            //xmlDoc = UpdateConnectionString(xmlDoc, "CNEntities", "metadata=res://*/Models.CRModel.csdl|res://*/Models.CRModel.ssdl|res://*/Models.CRModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=www.desarrollo.dyndns-office.com;initial catalog=CRMDK;persist security info=True;user id=MDK;password=MDK2016.;MultipleActiveResultSets=True;App=EntityFramework&quot;");

            //Save changes to the appSetting file
            WriteLog("Guardando cambios al archivo de configuración");
            xmlDoc.Save(appConfigPath);
        }


        #region Private Functions

        public XmlDocument UpdateAppKey(XmlDocument appConfig, string strKey, string newValue)
        {
            try
            {
                XmlNode appSettingsNode = appConfig.SelectSingleNode("configuration/appSettings");

                WriteLog("Intentando actualizar valor de {0} por {1}", strKey, newValue);
                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in appSettingsNode)
                {
                    if (childNode.Attributes["key"].Value == strKey)
                        childNode.Attributes["value"].Value = newValue;
                }

                WriteLog("Cambio Realizado al archivo de configuración");

                return appConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public XmlDocument UpdateConnectionString(XmlDocument appConfig, string strConnectionName, string newValue)
        {
            try
            {
                XmlNode connectionStringsNode = appConfig.SelectSingleNode("configuration/connectionStrings");

                WriteLog("Intentando actualizar valor de {0} por {1}", strConnectionName, newValue);
                // Attempt to locate the requested setting.
                foreach (XmlNode childNode in connectionStringsNode)
                {
                    if (childNode.Attributes["name"].Value == strConnectionName)
                        childNode.Attributes["connectionString"].Value = newValue;
                }

                WriteLog("Cambio Realizado al archivo de configuración");

                return appConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Escribe el texto que se le pasa en una linea de la bitacora de instalación
        /// </summary>
        /// <param name="textFormat">Texto formateado con indices a escribir</param>
        /// <param name="obj">Lista de objetos a sustituir por los indices</param>
        private void WriteLog(string textFormat, params object[] obj)
        {
            try
            {
                //Obtiene el archivo de applicación
                var file = string.Format(@"{0}\AproviInstall.Log", new FileInfo(Context.Parameters["assemblypath"]).DirectoryName);
                File.AppendAllText(file, string.Format(textFormat, obj) + "\r\n");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
