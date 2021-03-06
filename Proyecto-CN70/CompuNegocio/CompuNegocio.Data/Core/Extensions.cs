using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi
{
    public static partial class Extensions
    {
        /// <summary>
        /// Valida que el string no sea nulo ni este vacio
        /// </summary>
        /// <param name="str">Cadena a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this string str)
        {
            return !(str == null || str.Equals(string.Empty));
        }

        /// <summary>
        /// Valida que el número sea mayor a zero
        /// </summary>
        /// <param name="number">Número a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this int number)
        {
            return number > 0;
        }

        /// <summary>
        /// Valida que la cantidad sea mayor que zero
        /// </summary>
        /// <param name="number">Cantidad a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this double number)
        {
            return number > 0.0;
        }

        /// <summary>
        /// Valida que la cantidad sea mayor a zero
        /// </summary>
        /// <param name="number">Cantidad a validar</param>
        /// <returns>Si para la validación o no</returns>
        public static bool isValid(this decimal number)
        {
            return number > 0.0m;
        }

        /// <summary>
        /// Valida que el objeto no sea nulo
        /// </summary>
        /// <param name="obj">Objeto a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Valida que la lista no sea nula y tenga al menos 1 objeto en su colección
        /// </summary>
        /// <param name="list">Lista genérica a validar</param>
        /// <returns>Si pasa la validación o no</returns>
        public static bool isValid(this IList list)
        {
            return !(list == null || list.Count.Equals(0));
        }

        /// <summary>
        /// Valida que la colección no sea nula y tenga al menos 1 objeto en su colección
        /// </summary>
        /// <param name="collection">Collection a validar</param>
        /// <returns>Si para la validación o no</returns>
        public static bool isValid(this ICollection collection)
        {
            return !(collection == null || collection.Count.Equals(0));
        }

        /// <summary>
        /// Parses an integer comming from the database as object into a valid int value
        /// </summary>
        /// <param name="obj">integer as object</param>
        /// <returns>Valid int value</returns>
        public static int ToInt(this object obj)
        {
            return int.Parse(obj.ToString());
        }

        public static bool ToBool(this object obj)
        {
            return bool.Parse(obj.ToString());
        }

        /// <summary>
        /// Parses a date coming from the database as object into a valid Datetime value
        /// </summary>
        /// <param name="obj">datetime as object</param>
        /// <returns>Valid datetime value</returns>
        public static DateTime ToDate(this object obj)
        {
            return DateTime.Parse(obj.ToString());
        }

        /// <summary>
        /// Provee la fecha que se le pasa con el primer segundo del día en el timestamp de la hora
        /// </summary>
        /// <param name="date">Fecha a modificar</param>
        /// <returns>Misma fecha con el primer segundo del día</returns>
        public static DateTime ToLastMidnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 1);
        }

        /// <summary>
        /// Provee la fecha que se le pasa con el ultimo segundo del día en el timestamp de la hora
        /// </summary>
        /// <param name="date">Fecha a modificar</param>
        /// <returns>Misma fecha con el último segundo del día</returns>
        public static DateTime ToNextMidnight(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Parses a double comming from the database as object into a valid Double value
        /// </summary>
        /// <param name="obj">Double as object</param>
        /// <returns>Valid double value</returns>
        public static double ToDouble(this object obj)
        {
            return Double.Parse(obj.ToString());
        }

        /// <summary>
        /// Parses a decimal comming from the source as string into a valid decimal value
        /// </summary>
        /// <param name="str">Decimal as string</param>
        /// <returns>Valid decimal value</returns>
        public static decimal ToDecimal(this string str)
        {
            return decimal.Parse(str);
        }

        /// <summary>
        /// Parses a decimal comming as object into a valid decimal value
        /// </summary>
        /// <param name="str">Decimal as string</param>
        /// <returns>Valid decimal value</returns>
        public static decimal ToDecimal(this object str)
        {
            return decimal.Parse(str.ToString());
        }

        /// <summary>
        /// Provee un decimal formateado a 2 decimales como string
        /// </summary>
        /// <param name="number">Cantidad a formatear</param>
        /// <returns>String decimal formateado</returns>
        public static string ToDecimalString(this decimal number)
        {
            return string.Format("{0:0.00}", number);
        }

        /// <summary>
        /// Provee un formato de un peso decimal a string a 2 decimales, coma para miles y Kg al final
        /// </summary>
        /// <param name="weight">Peso a formatear</param>
        /// <returns>String formateado como peso</returns>
        public static string ToWeightString(this decimal weight)
        {
            return string.Format("{0:###,##0.00} Kg", weight);
        }

        /// <summary>
        /// Provee un formato de moneda decimal a string a 2 decimales, coma para miles y $ al inicio de la cadena
        /// </summary>
        /// <param name="amount">Cantidad a formatear</param>
        /// <returns>String formateado como moneda</returns>
        public static string ToCurrencyString(this decimal amount)
        {
            return string.Format("$ {0:###,##0.00}", amount);
        }

        /// <summary>
        /// Provee un string a partir de un objeto eliminando los espacios en blanco de inicio y final y que no pase la longitud especificada
        /// </summary>
        /// <param name="str">String en object a evaluar</param>
        /// <param name="maxLength">Longitud máxima</param>
        /// <returns>String resultante</returns>
        public static string ToTrimmedString(this object str, int maxLength)
        {
            var val = str.ToString().Trim();

            if (val.Length > maxLength)
                val.Substring(0, maxLength);

            return val;
        }

        /// <summary>
        /// Provee un string a eliminando los espacios en blanco de inicio y final y que no pase la longitud especificada
        /// </summary>
        /// <param name="str">String a evaluar</param>
        /// <param name="maxLenght">Longitud máxima</param>
        /// <returns>String resultante</returns>
        public static string ToTrimmedString(this string str, int maxLenght)
        {
            return str.Length > maxLenght ? str.Substring(0, maxLenght) : str;
        }

        /// <summary>
        /// Valida si una coleccion esta vacia
        /// </summary>
        /// <typeparam name="T">Tipo de la lista</typeparam>
        /// <param name="col">Lista a validar</param>
        /// <returns>true si la lista esta vacia</returns>
        public static bool IsEmpty<T>(this ICollection<T> col)
        {
            return col.Count == 0;
        }

        /// <summary>
        /// Valida si una coleccion contiene solo 1 elemento
        /// </summary>
        /// <typeparam name="T">Tipo de la lista</typeparam>
        /// <param name="col">Lista a validar</param>
        /// <returns>true si la lista contiene solo 1 elemento</returns>
        public static bool HasSingleItem<T>(this ICollection<T> col)
        {
            return col.Count == 1;
        }

        /// <summary>
        /// Busca un dato en una lista de datos extra, regresa null si el dato no existe
        /// </summary>
        /// <param name="datos">coleccion en la que se buscara</param>
        /// <param name="tipoDato">dato a buscar</param>
        /// <returns></returns>
        public static DatosExtraPorFactura FindDatoOrDefault(this List<DatosExtraPorFactura> datos, DatoExtra tipoDato)
        {
            return datos.FirstOrDefault(x => x.dato == tipoDato.ToString());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        //Obtiene la cadena de conexion para una empresa asociada
        public static string GetConnectionString(this EmpresasAsociada company)
        {
            SqlConnectionStringBuilder sqlConnectionString = new SqlConnectionStringBuilder();
            sqlConnectionString.DataSource = company.servidor;
            sqlConnectionString.InitialCatalog = company.baseDeDatos;
            sqlConnectionString.UserID = company.usuario;
            sqlConnectionString.Password = company.contrasena;

            EntityConnectionStringBuilder entityConnectionString = new EntityConnectionStringBuilder();
            entityConnectionString.Provider = "System.Data.SqlClient";
            entityConnectionString.ProviderConnectionString = sqlConnectionString.ToString();
            entityConnectionString.Metadata = @"res://*/Models.CNModel.csdl|res://*/Models.CNModel.ssdl|res://*/Models.CNModel.msl";

            return entityConnectionString.ToString();
        }
    }
}
