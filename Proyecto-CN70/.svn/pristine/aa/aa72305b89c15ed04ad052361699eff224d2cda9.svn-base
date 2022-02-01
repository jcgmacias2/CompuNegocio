using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using System.Management;

namespace Aprovi.Data.Repositories
{
    public class AplicacionesRepository : IAplicacionesRepository
    {
        public object ReadSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSetting(string key, string value)
        {
            string configFile;
            XmlTextReader reader;
            XmlDocument xmlDoc;
            XmlNode appSettings;
            XmlElement root;
            XmlNodeList settings;

            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\App.config"))
                    configFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\App.config"; //the application configuration file name
                else
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config"))
                        configFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config";
                    else
                        configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                reader = new XmlTextReader(configFile);
                xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                reader.Close();
                appSettings = null;
                root = xmlDoc.DocumentElement;
                settings = root.SelectNodes("appSettings/add");

                for (int i = 0; i < settings.Count; i++)
                {
                    appSettings = settings[i];
                    if (appSettings.Attributes["key"].Value.Equals(key))
                        break;
                    appSettings = null;
                }
                appSettings.Attributes["value"].Value = value;
                xmlDoc.Save(configFile);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                //Update also on the memory configuration
                RefreshSection(key, value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshSection(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetComputerCode()
        {
            try
            {
                string cpuCode = null;
                string volumeSerial = null;
                ManagementClass managment;
                ManagementObjectCollection objectCollection;
                ManagementObject disk;

                managment = new ManagementClass("win32_processor");
                objectCollection = managment.GetInstances();
                    
                foreach (ManagementObject mo in objectCollection)
                {
                    if (!cpuCode.isValid())
                    {
                        //Get only the first CPU's ID
                        cpuCode = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }

                disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + Directory.GetLogicalDrives().First().Substring(0, 1) + @":""");
                disk.Get();
                volumeSerial = disk["VolumeSerialNumber"].ToString();

                return cpuCode + volumeSerial;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshConnection()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateConnectionString(string name, string value)
        {
            string configFile;
            XmlTextReader reader;
            XmlDocument xmlDoc;
            XmlNode appSettings;
            XmlElement root;
            XmlNodeList settings;

            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\App.config"))
                    configFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\App.config"; //the application configuration file name
                else
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config"))
                        configFile = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\App.config";
                    else
                        configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                reader = new XmlTextReader(configFile);
                xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                reader.Close();
                appSettings = null;
                root = xmlDoc.DocumentElement;
                settings = root.SelectNodes("connectionStrings/add");

                for (int i = 0; i < settings.Count; i++)
                {
                    appSettings = settings[i];
                    if (appSettings.Attributes["name"].Value.Equals(name))
                        break;
                    appSettings = null;
                }
                appSettings.Attributes["connectionString"].Value = value;
                xmlDoc.Save(configFile);
                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                //Update also on the memory configuration
                RefreshConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
