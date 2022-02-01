using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ISeguridadService
    {
        /// <summary>
        /// Valida una cuenta de usuario a partir de su nombre de usuario y contraseña contra la base de datos de www.aprovi.com.mx
        /// </summary>
        /// <param name="usuario">Nombre de usuario (no correo)</param>
        /// <param name="contraseña">Contraseña de la cuenta</param>
        /// <param name="useSSL">True para establecer comunicación https</param>
        /// <returns>True si la cuenta es válida</returns>
        bool AuthenticateWithAPI(string usuario, string contraseña, bool useSSL);


        /// <summary>
        /// Autentica las credenciales de un usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Usuario al que pertenecen las credenciales</returns>
        Usuario Authenticate(string username, string password);
    }
}
