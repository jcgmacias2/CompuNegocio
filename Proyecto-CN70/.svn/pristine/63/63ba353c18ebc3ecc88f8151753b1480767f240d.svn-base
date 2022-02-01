using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ISerieService
    {
        /// <summary>
        /// Agrega una nueva serie de folios
        /// </summary>
        /// <param name="serie">Serie a agregar</param>
        /// <returns>Serie registrada</returns>
        Series Add(Series serie);

        /// <summary>
        /// Busca una serie a partir de su identificador alfabetico
        /// </summary>
        /// <param name="identifier">Identificador alfabetico</param>
        /// <returns></returns>
        Series Find(char identifier);

        /// <summary>
        /// Obtiene la serie asignada a un tipo de comprobante
        /// </summary>
        /// <param name="idTipoDeComprobante">Identificador del tipo de comprobante</param>
        /// <returns>Serie asignada</returns>
        Series Find(int idTipoDeComprobante);

        /// <summary>
        /// Enlista las series configuradas
        /// </summary>
        /// <returns>Lista de series</returns>
        List<Series> List();

        /// <summary>
        /// Actualiza los foliajes
        /// </summary>
        /// <param name="serie">Serie con el foliaje actualizado</param>
        /// <returns>Serie registrada</returns>
        Series Update(Series serie);

        /// <summary>
        /// Establece una serie para el comprobante especificado
        /// </summary>
        /// <param name="serie">Serie que se ligara al tipo de comprobante</param>
        /// <param name="comprobante">Tipo de comprobante que utilizara a la serie especificada como default</param>
        void Update(Series serie, TiposDeComprobante comprobante);

        /// <summary>
        /// Proporciona el último folio utilizado de la serie especificada para el tipo de comprobante especificado
        /// </summary>
        /// <param name="serie">Serie para la cual se desea conocer el último folio</param>
        /// <param name="tipo">Tipo de comprobante para el cual se desea conocer el último folio utilizado de la serie</param>
        /// <returns>Ultimo folio utilizado</returns>
        int Last(string serie, TipoDeComprobante tipo);

        /// <summary>
        /// Proporciona el último folio utilizado de una serie
        /// </summary>
        /// <param name="serie">Serie para la cual se desea conocer el último folio</param>
        /// <returns>Ultimo folio utilizado</returns>
        int Last(string serie);

        /// <summary>
        /// Proporciona el siguiente folio a utilizar de una serie
        /// </summary>
        /// <param name="serie">Serie para la cual se desea conocer el siguiente folio</param>
        /// <returns>Siguiente folio a utilizar</returns>
        int Next(string serie);
    }
}
