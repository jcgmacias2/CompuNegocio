using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Aprovi.Views
{
    /// <summary>
    /// Interface padre que contiene los minimos metodos necesarios para interactuar
    /// </summary>
    public interface IBaseView
    {
        #region General Methods every View must implement

        /// <summary>
        /// Muestra la ventana en forma modal
        /// </summary>
        void ShowWindow();

        /// <summary>
        /// Muestra la ventana en forma independiente
        /// </summary>
        void ShowWindowIndependent();

        /// <summary>
        /// Cierra la ventana
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Muestra un mensaje utilizando el icono de información y el botón ok
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        void ShowMessage(string message);

        /// <summary>
        /// Muestra un mensaje formateado con indices, que utiliza el icono de información y el botón ok
        /// </summary>
        /// <param name="textFormat">Texto formateado con indices que se substituiran con los argumentos</param>
        /// <param name="arguments">Argumentos con los cuales sustituir los indices en el texto</param>
        void ShowMessage(string textFormat, params object[] arguments);

        /// <summary>
        /// Muestra un mensaje utilizando el icono de exclamación y el botón ok y además crea un archivo de log.
        /// </summary>
        /// <param name="ex">Excepción que se registró</param>
        void ShowError(Exception ex);

        /// <summary>
        /// Muestra un mensaje utilizando el icono de exclamación y el botón ok
        /// </summary>
        /// <param name="message">Mensaje de error a desplegar</param>
        void ShowError(string message);

        /// <summary>
        /// Muestra un mensaje utilizando el icono de pregunta y los botones Yes/ No / Cancel
        /// </summary>
        /// <param name="message">Mensaje interrogativo a desplegar</param>
        /// <returns></returns>
        MessageBoxResult ShowMessageWithOptions(string message);

        /// <summary>
        /// Abre un diálogo de búsqueda de archivos con el filtro especificado
        /// </summary>
        /// <param name="filter">Filtro que se aplicará a la búsqueda ej: [Llave Privada (*.key)|*.key]</param>
        /// <returns>Ruta completa del archivo seleccionado</returns>
        string OpenFileFinder(string filter);

        /// <summary>
        /// Abre un diálogo de búsqueda de folder con la descripción especificada
        /// </summary>
        /// <param name="windowCaption">Descripción al borde superior del diálogo</param>
        /// <returns>Ruta completa del folder seleccionado</returns>
        string OpenFolderFinder(string windowCaption);
        
        #endregion
    }
}
