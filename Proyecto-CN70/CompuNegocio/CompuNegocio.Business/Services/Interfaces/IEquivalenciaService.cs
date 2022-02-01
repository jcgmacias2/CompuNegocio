using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IEquivalenciaService
    {
        /// <summary>
        /// Agrega una nueva equivalencia al catálogo existente
        /// </summary>
        /// <param name="equivalency">Equivalencia que se desea agregar</param>
        /// <returns>Equivalencia registrada</returns>
        Equivalencia Add(Equivalencia equivalency);

        /// <summary>
        /// Busca una equivalencia en el catálogo de equivalencias a través de su identificador numérico
        /// </summary>
        /// <param name="idEquivalence">Identificador numérico de la equivalencia</param>
        /// <returns>Equivalencia correspondiente al identificador</returns>
        Equivalencia Find(int idEquivalence);

        /// <summary>
        /// Busca una equivalencia en el catálogo de equivalencias a través del artículo y la unidad de medida
        /// </summary>
        /// <param name="idItem">Identificador numérico del artículo al que pertenece la equivalencia</param>
        /// <param name="idUnitOfMeasure">Identificador numérico de la unidad de medida que pertenece a la equivalencia</param>
        /// <returns>Equivalencia que corresponde a los identificadores</returns>
        Equivalencia Find(int idItem, int idUnitOfMeasure);

        /// <summary>
        /// Provee el catálogo de las equivalencias activas
        /// </summary>
        /// <returns>Catálogo de equivalencias</returns>
        List<Equivalencia> List();

        /// <summary>
        /// Provee una lista de las equivalencias de un artículo
        /// </summary>
        /// <param name="idItem">Identificador numérico del artículo</param>
        /// <returns>Equivalencias del artículo correspondiente</returns>
        List<Equivalencia> List(int idItem);

        /// <summary>
        /// Actualiza los datos modificables de una equivalencia
        /// </summary>
        /// <param name="equivalence">Equivalencia con la nueva información</param>
        /// <returns>Equivalencia registrada</returns>
        Equivalencia Update(Equivalencia equivalence);

        /// <summary>
        /// Elimina del catálogo de equivalencias un registro
        /// </summary>
        /// <param name="equivalence">Equivalencia a ser removida</param>
        void Delete(Equivalencia equivalence);

        /// <summary>
        /// Valida si la equivalencia en cuestión tiene operaciones relacionadas
        /// </summary>
        /// <param name="equivalence">Equivalencia a validar</param>
        /// <returns>False si no tiene operaciones relacionadas</returns>
        bool HasOperations(Equivalencia equivalence);
    }
}
