using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IClasificacionService
    {
        /// <summary>
        /// Agrega una nueva clasificación al catálogo existente
        /// </summary>
        /// <param name="classification">Clasificación a agregar</param>
        /// <returns>Clasificación agregada con identificador</returns>
        Clasificacione Add(Clasificacione classification);

        /// <summary>
        /// Provee el catálogo de clasificaciones actual
        /// </summary>
        /// <returns>Lista de clasificaciones</returns>
        List<Clasificacione> List();

        /// <summary>
        /// Provee una lista de clasificaciones que contienen el código que se busca
        /// </summary>
        /// <param name="code">Código que se busca</param>
        /// <returns>Lista de clasificaciones en coincidencia</returns>
        List<Clasificacione> WithNameLike(string code);

        /// <summary>
        /// Busca una clasificación a través de su identificador
        /// </summary>
        /// <param name="idClassification">Identificador numérico de la clasificación</param>
        /// <returns>Clasificación a la que pertenece el identificador</returns>
        Clasificacione Find(int idClassification);

        /// <summary>
        /// Busca una clasificación a través de su código único
        /// </summary>
        /// <param name="code">Código de la clasificación</param>
        /// <returns>Clasificación a la que pertenece el código</returns>
        Clasificacione Find(string code);

        /// <summary>
        /// Actualiza la descripción de una clasificacion
        /// </summary>
        /// <param name="classification">Clasificación con datos modificados</param>
        /// <returns>Clasificación modificada</returns>
        Clasificacione Update(Clasificacione classification);

        /// <summary>
        /// Elimina una clasificación del catálogo actual
        /// </summary>
        /// <param name="classification">Clasificación a eliminar</param>
        void Delete(Clasificacione classification);

        /// <summary>
        /// Valida si es posible eliminar una clasificación
        /// </summary>
        /// <param name="classification">Clasificación a validar</param>
        /// <returns>True si es posible eliminarla</returns>
        bool CanDelete(Clasificacione classification);

    }
}
