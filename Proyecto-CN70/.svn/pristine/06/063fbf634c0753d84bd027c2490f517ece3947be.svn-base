using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ILicenciaService
    {
        /// <summary>
        /// Realiza la autenticación de la licencia, verificando que fue registrada para la empresa en cuestión
        /// </summary>
        /// <param name="license">Código de la licencia, es el que se le entrega al usuario y que fue generado por aprovi.com.mx</param>
        /// <returns>Número entero en string con el que se realizará la validación de la licencia</returns>
        string Authenticate(string license);

        /// <summary>
        /// Revisa que el idUser obtenido a través de la Autenticación de la licencia corresponda al usuario configurado en el sistema como dueño;
        /// Este fue capturado al instalar el sistema y se encuentra en los AppSettings bajo el key "CopyRight"
        /// </summary>
        /// <param name="idUser">Número entero obtenido a partir de la autenticación de la licencia</param>
        /// <returns>True si tienen correspondencia</returns>
        bool Verify(string idUser);

        /// <summary>
        /// Realiza la activación de una licencia que ya paso la verificación, para activar es necesario el rfc de la empresa
        /// </summary>
        /// <param name="license">Código de la licencia a activar</param>
        /// <returns>True cuando se activa exitosamente</returns>
        bool Activate(string license);

        /// <summary>
        /// Realiza la validación de la vigencia de la licencia configurada en el sistema
        /// </summary>
        /// <param name="license">Código de licencia a validar</param>
        /// <param name="stamps">Cantidad de timbres utilizados en el sistema de esta caja</param>
        /// <returns>True si es válida</returns>
        bool Validate(string license, int stamps);

        /// <summary>
        /// Obtiene el número de estaciones para las que la licencia fue registrada
        /// </summary>
        /// <param name="license">Licencia a verificar</param>
        /// <returns>Número de estaciones incluidas</returns>
        int IncludedStations(string license);
    }
}
