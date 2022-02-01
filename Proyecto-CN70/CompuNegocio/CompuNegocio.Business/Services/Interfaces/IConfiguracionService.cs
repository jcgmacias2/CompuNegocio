using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IConfiguracionService : ICuentaGuardianService
    {
        /// <summary>
        /// Proporciona la configuración default de la aplicación
        /// </summary>
        /// <returns>Configuración cargada con los defaults</returns>
        Configuracion GetDefault();

        /// <summary>
        /// Actualiza la información respecto a al configuración del sistema
        /// </summary>
        /// <param name="configuration">Configuración modificada</param>
        /// <returns>Configuración actualizada</returns>
        Configuracion Update(Configuracion configuration);

        /// <summary>
        /// Actualiza las credenciales del proveedor autorizado de certificación
        /// </summary>
        /// <param name="configuration">Coonfiguración modificada</param>
        /// <returns>Configuración actualizada</returns>
        Configuracion UpdatePAC(Configuracion configuration);

        /// <summary>
        /// Actualiza el connection string
        /// </summary>
        /// <param name="server">Servidor al que se desea conectar</param>
        /// <param name="user">Usuario para conectarse al servidor</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <param name="database">Base de datos a la cual se desea conectar</param>
        void UpdateConnection(string server, string user, string password, string database);

        /// <summary>
        /// Obtiene la lista de módulos asociados a la licencia
        /// </summary>
        /// <returns>Lista de módulos</returns>
        Configuracion UpdateSettings(string license);
    }
}
