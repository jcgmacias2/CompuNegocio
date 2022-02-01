using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IAjusteService
    {
        /// <summary>
        /// Agrega un nuevo ajuste a la colección de ajustes existente
        /// </summary>
        /// <param name="adjustment">Ajuste que se desea agregar</param>
        /// <returns>Ajuste registrado</returns>
        Ajuste Add(Ajuste adjustment);

        /// <summary>
        /// Busca un ajuste a partir de su identificador numérico
        /// </summary>
        /// <param name="idAdjustment">Identificador numérico del ajuste</param>
        /// <returns>Ajuste al que corresponde el identificador</returns>
        Ajuste Find(int idAdjustment);

        /// <summary>
        /// Busca un ajuste a partir de su folio
        /// </summary>
        /// <param name="folio">Folio del ajuste a buscar</param>
        /// <returns>Ajuste al que corresponde el folio</returns>
        Ajuste Find(string folio);

        /// <summary>
        /// Lista de todos los ajustes registrados
        /// </summary>
        /// <returns></returns>
        List<Ajuste> List();

        /// <summary>
        /// Lista de ajustes filtrados por tipo
        /// </summary>
        /// <param name="type">Tipo de ajuste que se busca</param>
        /// <returns>Lista de ajustes del tipo especificado</returns>
        List<Ajuste> List(TiposDeAjuste type);

        /// <summary>
        /// Lista todos los ajustes filtrados por tipo en un período de tiempo
        /// </summary>
        /// <param name="start">Inicio del período</param>
        /// <param name="end">Fin del período</param>
        /// <param name="type">Tipo de ajuste</param>
        /// <returns>Lista de ajustes que cumplen con el criterio</returns>
        List<Ajuste> List(DateTime start, DateTime end, TiposDeAjuste type);

        /// <summary>
        /// Lista de ajustes en que el folio tiene coincidencia con el valor que se busca
        /// </summary>
        /// <param name="value">Valor a comparar con el folio</param>
        /// <returns>Lista de ajustes con similitud en el folio</returns>
        List<Ajuste> Like(string value);

        /// <summary>
        /// Provee el siguiente folio de ajuste
        /// </summary>
        /// <returns>Siguiente folio</returns>
        string Next();

        /// <summary>
        /// Crea un ajuste inicial con valores default por cada artículo migrado
        /// </summary>
        /// <param name="items">Lista de artículos migrados</param>
        /// <returns>Ajuste registrado</returns>
        Ajuste MigrateStock(List<VMArticulo> items);

        /// <summary>
        /// Crea un Ajuste de salida para el artículo especificado por la existencia actual
        /// </summary>
        /// <param name="item">Artículo para el cual se genera el ajuste de salida</param>
        /// <returns>Ajuste de Salida</returns>
        Ajuste GenerateExit(VMArticulo item);

        /// <summary>
        /// Crea un ajuste de entrada con un pedimento para el artículo especificado por la existencia del pedimento
        /// </summary>
        /// <param name="item">Artículo para el cual se genera el ajuste de entrada</param>
        /// <param name="customApplication">Pedimento relacionado al ajuste incluyendo la cantidad</param>
        /// <returns>Ajuste de Entrada</returns>
        Ajuste GenerateEntrance(VMArticulo item, VMPedimento customApplication);

        /// <summary>
        /// Obtiene los ajustes de entrada para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListEntranceAdjustmentsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene los ajustes de salida para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListExitAdjustmentsForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
