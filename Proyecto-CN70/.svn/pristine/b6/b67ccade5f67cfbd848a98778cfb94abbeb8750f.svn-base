using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Helpers
{
    public static class Logger
    {
        private static string _log;

        static Logger()
        {
            _log = string.Format("{0}\\{1}-{2}-{3}.log", ConfigurationManager.AppSettings["Reports"].ToString(), DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            //File.Create(_log);
        }

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(_log, message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Log(Exception ex)
        {
            try
            {
                File.AppendAllText(_log, ex.ToString());
                if (ex.InnerException.isValid())
                {
                    File.AppendAllText(_log, "-----Source ----");
                    File.AppendAllText(_log, ex.InnerException.Source);
                }
                File.AppendAllText(_log, "-----Message ----");
                File.AppendAllText(_log, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
