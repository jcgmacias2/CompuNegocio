using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace CompuNegocio.SetupHelper
{
    [RunInstaller(true)]
    public partial class SetupConfiguration : System.Configuration.Install.Installer
    {
        public SetupConfiguration()
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

            if (!File.Exists(appConfigPath))
            {
                WriteLog("El archivo no existe");
                return;
            }

            if (IsFileLocked(new FileInfo(appConfigPath)))
            {
                WriteLog("No hay acceso al archivo");
                return;
            }

            WriteLog("Cargando archivo de configuración {0}", appConfigPath);
            xmlDoc.Load(appConfigPath);

            //Empiezo a hacer los cambios
            var applicationFolder = new FileInfo(Context.Parameters["assemblypath"]).DirectoryName;

            //Vendrá preconfigurado con Ambiente Configuration
            xmlDoc = UpdateAppKey(xmlDoc, "Environment", "Configuration");

            //Le especifico que es CompuNegocio
            xmlDoc = UpdateAppKey(xmlDoc, "System", "1");

            //En base a lo que me haya capturado
            xmlDoc = UpdateAppKey(xmlDoc, "Copyright", Context.Parameters["mailAddress"].ToString());

            //Establecer la ruta de reportes en la implementación
            xmlDoc = UpdateAppKey(xmlDoc, "Reports", string.Format(@"{0}\Reports\", applicationFolder));

            //Establecer la ruta de xmls
            xmlDoc = UpdateAppKey(xmlDoc, "Xml", string.Format(@"{0}\", applicationFolder));

            //Establecer la ruta de Pdf
            xmlDoc = UpdateAppKey(xmlDoc, "Pdf", string.Format(@"{0}\", applicationFolder));

            //Establecer la ruta de Cbb
            xmlDoc = UpdateAppKey(xmlDoc, "Cbb", string.Format(@"{0}\", applicationFolder));

            //Establecer la ruta de Cbb
            xmlDoc = UpdateAppKey(xmlDoc, "Estacion", "0");

            //Establecer la ruta en la configuración para utilizar el pfx implementado
            xmlDoc = UpdateAppKey(xmlDoc, "CSD", string.Format(@"{0}\Files\", applicationFolder));

            //Establecer si se esta utilizando scanner
            xmlDoc = UpdateAppKey(xmlDoc, "Scanner", "0");

            //Establecer el codigo para entender scanner
            xmlDoc = UpdateAppKey(xmlDoc, "ScannerCode", "0");

            //Establecer la ruta de Cbb
            xmlDoc = UpdateAppKey(xmlDoc, "Drawer", "0");

            // Le quito los datos de conexión, al intentar entrar por primera vez le pedirá capturar los de sus servidor
            xmlDoc = UpdateConnectionString(xmlDoc, "CNEntities", string.Format("metadata=res://*/Models.CNModel.csdl|res://*/Models.CNModel.ssdl|res://*/Models.CNModel.msl;provider=System.Data.SqlClient;provider connection string='data source={0};initial catalog={1};persist security info=True;user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework'", Context.Parameters["server"].ToString(), Context.Parameters["database"].ToString(), Context.Parameters["user"].ToString(), Context.Parameters["password"].ToString()));

            //Save changes to the appSetting file
            WriteLog("Guardando cambios al archivo de configuración");
            xmlDoc.Save(appConfigPath);
        }

        #region Private Functions

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

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
