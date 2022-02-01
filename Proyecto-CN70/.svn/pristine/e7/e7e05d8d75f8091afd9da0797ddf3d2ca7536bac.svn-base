using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IUsosCFDIService
    {
        /// <summary>
        /// Busca un Uso por medio de su código
        /// </summary>
        /// <param name="code">Código del uso</param>
        /// <returns>Uso de CFDI</returns>
        UsosCFDI Find(string code);

        /// <summary>
        /// Busca un Uso por medio de su id
        /// </summary>
        /// <param name="id">Identificador numerico</param>
        /// <returns>Uso de CFDI</returns>
        UsosCFDI Find(int id);

        /// <summary>
        /// Reactiva un Uso de CFDI del catálogo de usos
        /// </summary>
        /// <param name="uso">Uso a reactivar</param>
        /// <returns>Uso reactivado</returns>
        UsosCFDI Reactivate(UsosCFDI uso);

        /// <summary>
        /// Desactiva un Uso de CFDI del catálogo de usos
        /// </summary>
        /// <param name="uso">Uso a desactivar</param>
        /// <returns>Uso desactivado</returns>
        UsosCFDI Deactivate(UsosCFDI uso);

        /// <summary>
        /// Lista completa de Usos del CFDI
        /// </summary>
        /// <returns>Lista de UsosCFDI</returns>
        List<UsosCFDI> List();

        /// <summary>
        /// Lista de Usos del CFDI que coincidan total o parcialmente con el valor que se le pasa
        /// </summary>
        /// <param name="value">Valor a buscar</param>
        /// <returns>Lista de coincidencias</returns>
        List<UsosCFDI> List(string value);
    }
}
