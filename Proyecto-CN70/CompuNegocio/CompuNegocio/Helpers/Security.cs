using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Aprovi.Application.Helpers
{
    public static class Security
    {
        #region Esto aplica la parte de seguridad

        //Este metodo al ser llamado obtendra el nivel de acceso requerido y lo validara contra la lista de privilegios del usuario loggeado

        /// <summary>
        /// Valida que la acción no sea nula y  que el usuario loggeado tenga el privilegio requerido sobre el presenter que lo manda llamar
        /// </summary>
        /// <param name="obj">Acción a validar</param>
        /// <param name="acceso">Privilegio mínimo requerido</param>
        /// <returns>True si no es nula y tiene el privilegio</returns>
        public static bool isValid(this Action obj, AccesoRequerido acceso)
        {
            //Verifico que la acción no sea nula
            if (obj == null)
                return false;

            //Obtengo el privilegio que tiene el usuario loggeado para el presenter que lo esta mandando llamar
            //La asignación de privilegios se hace en base a la Vista (View), por cada vista hay un Presentador (Presenter) que es el que
            //obtenemos y contra el que validamos el privilegio
            //El nombre del presenter que esta mandando a llamar a esta acción lo obtenemos asi:
            //obj.Method.DeclaringType.Name 
            var access = Session.LoggedUser.Privilegios.FirstOrDefault(p => p.Pantalla.vista.Equals(obj.Method.DeclaringType.Name, StringComparison.InvariantCultureIgnoreCase));

            //Verifico que el usuario tenga algún privilegio para esta Vista
            if (access == null)
            {
                MessageBox.Show("No tiene privilegios suficientes", "Acceso inválido", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            //Verifico que el privilegio que tiene el usuario sea suficiente para lo que desea hacer
            //Tengo 3 tipos de permiso que van de menor a mayor, 1 = Solo ver, 2 = Ver y Agrega, 3 = Total
            //Por lo que si el Id es mayor o igual al acceso requerido entonces puede continuar
            if (access.idPermiso < (int)acceso)
            {
                MessageBox.Show("No tiene privilegios suficientes", "Acceso inválido", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            
            //Si llega aqui es que si tiene el permiso y puede ejecutar la acción
            return true;
        }

        /// <summary>
        /// Valida si un usuario tiene el nivel de acceso especificado al presenter / vista especificado y si no lo tiene envia un mensaje de error
        /// </summary>
        /// <param name="user">Usuario al que se le desea validar el acceso</param>
        /// <param name="acceso">Privilegio mínimo requerido</param>
        /// <param name="presenter">Nombre del presenter al que se desea validar si se tiene el privilegio</param>
        /// <param name="showMessage">True para que muestre un mensaje de error de manera automática si no tiene el privilegio validado</param>
        /// <returns>True si el acceso es permitido</returns>
        public static bool HasAccess(this Usuario user, AccesoRequerido acceso, string presenter, bool showMessage)
        {
            //La asignación de privilegios se hace en base a la Vista (View), por cada vista hay un Presentador (Presenter) que es el que
            //obtenemos y contra el que validamos el privilegio

            //Obtengo el privilegio que tiene el usuario para el presenter especificado
            var access = user.Privilegios.FirstOrDefault(p => p.Pantalla.vista.Equals(presenter, StringComparison.InvariantCultureIgnoreCase));

            //Verifico que el usuario tenga algún privilegio para este presenter
            if (access == null)
            {
                if(showMessage)
                    MessageBox.Show("No tiene privilegios suficientes", "Acceso inválido", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            //Verifico que el privilegio que tiene el usuario sea suficiente para lo que desea hacer
            //Tengo 3 tipos de permiso que van de menor a mayor, 1 = Solo ver, 2 = Ver y Agrega, 3 = Total
            //Por lo que si el Id es mayor o igual al acceso requerido entonces puede continuar
            if (access.idPermiso < (int)acceso)
            {
                if(showMessage)
                    MessageBox.Show("No tiene privilegios suficientes", "Acceso inválido", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            //Si llega aqui es que si tiene el permiso
            return true;
        }

        /// <summary>
        /// Valida si el módulo existe dentro de la lista de módulos activos en el sistema
        /// </summary>
        /// <param name="modules">Lista de módulos activos</param>
        /// <param name="module">Módulo que se desea saber si esta activo</param>
        /// <returns>True si esta activo</returns>
        public static bool IsActive(this List<Modulos> modules, Modulos module)
        {
            try
            {
                return modules.Exists(m => ((int)m).Equals((int)module));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
