using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ICertificadoService
    {
        /// <summary>
        /// Obtiene el certificado por defecto configurado en el sistema, en teoría solo hay 1 por configuración
        /// </summary>
        /// <returns>Certificado</returns>
        Certificado GetDefault();

        /// <summary>
        /// Realiza la configuración de un certificado de sello digital a partir de los archivos físicos (*.cer y *.key), el cual incluye la generación del pfx y su registro en el app config
        /// </summary>
        /// <param name="certificateFile">Ruta física de la ubicación del certificado (*.cer)</param>
        /// <param name="keyFile">Ruta física de la ubicación de la llave privada (*.key)</param>
        /// <param name="keyPass">Contraseña que abre la llave privada</param>
        /// <param name="pfxLocation">Directorio donde será depositado el certificado</param>
        /// <returns>Certificado configurado con contraseña de acceso autogenerada</returns>
        Certificado Configure(string certificateFile, string keyFile, string keyPass, string pfxLocation);

        /// <summary>
        /// Realiza la configuración de un certificado de sello digital a partir del pfx, intentará abrirlo con la contraseña de acceso autogenerada
        /// </summary>
        /// <param name="pfxFile">Ruta física de la ubicación del certificado (*.pfx)</param>
        /// <returns>Certificado configurado</returns>
        Certificado Configure(string pfxFile);
    }
}
