using Aprovi.Application.Helpers;
using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi
{
    //Extiende a la clase Extensions del proyecto Data
    public static partial class Extensions
    {
        #region General

        /// <summary>
        /// Convierte a entero la cantidad expresada en texto, si no es valida regresa 0
        /// </summary>
        /// <param name="cuantity">Cantidad entera en texto</param>
        /// <returns>Equivalente entero del texto, o si no es una cantida valida 0</returns>
        public static int ToIntOrDefault(this string cuantity)
        {
            int value;

            if (int.TryParse(cuantity, out value))
                return value;

            return 0;
        }

        /// <summary>
        /// Valida que la acción no sea nula
        /// </summary>
        /// <param name="obj">Acción a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this Action obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Convierte a decimal la cantidad expresada en texto, si no es valida regresa 0.0
        /// </summary>
        /// <param name="cuantity">Cantidad decimal en texto</param>
        /// <returns>Equivalente decimal del texto, o si no es una cantidad valida 0.0</returns>
        public static decimal ToDecimalOrDefault(this string cuantity)
        {
            decimal value;

            if (decimal.TryParse(cuantity, out value))
                return value;

            return 0.0m;
        }

        /// <summary>
        /// Convierte una string a un numero entero nullable
        /// </summary>
        /// <param name="value">texto a convertir</param>
        /// <returns>Numero entero, -1 si la conversion falló o null si no se proporciono ningun numero</returns>
        public static int? ToValidatedInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            int val;

            if (int.TryParse(value, out val))
            {
                return val;
            }

            return -1;
        }

        /// <summary>
        /// Convierte una string a un numero decimal nullable
        /// </summary>
        /// <param name="value">texto a convertir</param>
        /// <returns>Numero decimal, -1.0 si la conversion falló o null si no se proporciono ningun valor</returns>
        public static decimal? ToValidatedDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            decimal val;

            if (decimal.TryParse(value, out val))
            {
                return val;
            }

            return -1m;
        }

        /// <summary>
        /// Convierte a entero la selección de un combobox que esta expresada en objeto, si no hay valor seleccionado regresa 0
        /// </summary>
        /// <param name="value">Número seleccionado en tipo objeto</param>
        /// <returns>Valor seleccionado o 0</returns>
        public static int ToIntOrDefault(this object value)
        {
            int val;

            if (value == null)
                return 0;

            if (int.TryParse(value.ToString(), out val))
                return val;

            return 0;
        }

        /// <summary>
        /// Convierte a string la selección de un combobox que esta expresada en objeto, si no hay valor seleccionado regresa vacio
        /// </summary>
        /// <param name="value">String seleccionado en tipo objeto</param>
        /// <returns>Valor seleccionado o vacio</returns>
        public static string ToStringOrDefault(this object value)
        {
            if (value == null)
                return string.Empty;

            return value.ToString();
        }

        /// <summary>
        /// Crea una fecha hora a partir de dos valores de fecha y hora
        /// </summary>
        /// <param name="date">Porción que contiene la fecha</param>
        /// <param name="timePortion">Porción que contiene la hora</param>
        /// <returns>Fecha y hora en una sola instancia</returns>
        public static DateTime ToFullDateTime(this DateTime date, DateTime timePortion)
        {
            return new DateTime(date.Year, date.Month, date.Day, timePortion.Hour, timePortion.Minute, timePortion.Second);
        }

        /// <summary>
        /// Convierte una DateTime? a una string en formato dd/MM/yyyy, si es nula regresa una string vacia
        /// </summary>
        /// <param name="date">fecha a convertir</param>
        /// <returns></returns>
        public static string ToDateStringOrDefault(this DateTime? date)
        {
            if (!date.isValid())
            {
                return string.Empty;
            }

            return date.Value.ToString("dd/MM/yyyy");
        }
        #endregion

        #region Validaciones

        /// <summary>
        /// Verifica si el módulo especificado se encuentra dentro de los módulos activos del sistema
        /// </summary>
        /// <param name="module">Módulo a verificar</param>
        /// <returns>True si esta activo</returns>
        public static bool IsActive(this Modulos module)
        {
            try
            {
                return Session.Configuration.Modulos.Exists(m => m.ToString().Equals(module.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public static bool HasPredial(this IEnumerable<Clasificacione> classifications)
        {
            try
            {
                return classifications.Any(c => c.descripcion.Equals(Modulos.Predial.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica si el sistema en sessión es igual al sistema que se le pasa
        /// </summary>
        /// <param name="system">Sistema a verificar</param>
        /// <returns>True si es el mismo</returns>
        public static bool InSession(this Customization system)
        {
            try
            {
                return Session.Configuration.Sistema.Equals(system);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
