using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IUnidadDeMedidaService
    {
        /// <summary>
        /// Agrega una nueva unidad de medida a la colección existente
        /// </summary>
        /// <param name="unitOfMeasure">Unidad de medida</param>
        /// <returns>Unidad de medida con identificador autogenerado</returns>
        UnidadesDeMedida Add(UnidadesDeMedida unitOfMeasure);

        /// <summary>
        /// Enlista todas las unidades de medida existentes en el sistema, excepto aquellas dadas de baja
        /// </summary>
        /// <returns>Lista de unidades de medida existentes</returns>
        List<UnidadesDeMedida> List();

        /// <summary>
        /// Enlista todas las unidades de medida que tengan similitud con el valor
        /// </summary>
        /// <param name="value">valor que se busca</param>
        /// <returns>Lista de unidades de medida similar</returns>
        List<UnidadesDeMedida> Like(string value);

        /// <summary>
        /// Busca las unidades de medida que puede manejar un artículo para compra, esto incluye la unidad mínima incluida en el artículo y sus equivalencias
        /// </summary>
        /// <param name="idItem">Identificador del artículo del que se desea obtener sus unidades de medida</param>
        /// <returns>Lista de unidades de medida</returns>
        List<UnidadesDeMedida> List(int idItem);

        /// <summary>
        /// Busca una unidad de medida a través de su identificador
        /// </summary>
        /// <param name="id">Identificador númerico de la unidad de medida</param>
        /// <returns>Unidad de medida si la encuentra o null</returns>
        UnidadesDeMedida Find(int id);

        /// <summary>
        /// Busca una unidad de medida a través de su nombre/descripción
        /// </summary>
        /// <param name="code">Código de la unidad de medida que se busca</param>
        /// <returns>Unidad de medida si la encuentra o null</returns>
        UnidadesDeMedida Find(string code);

        /// <summary>
        /// Valida si la unidad de medida ya existen dentro de la colección
        /// </summary>
        /// <param name="code">Unidad de medida que se busca</param>
        /// <returns>True si ya existe, False si no</returns>
        bool Exists(string code);

        /// <summary>
        /// Actualiza la descripción de la unidad de medida
        /// </summary>
        /// <param name="unitOfMeasure">Unidad de medida modificada</param>
        /// <returns>Unidad de medida con cambios permanentes</returns>
        UnidadesDeMedida Update(UnidadesDeMedida unitOfMeasure);

        /// <summary>
        /// Las unidades de medida relacionadas a un artículo y/u operación no podrán ser eliminadas.
        /// </summary>
        /// <param name="unitOfMeasure">Unidad de medida a eliminar</param>
        void Delete(UnidadesDeMedida unitOfMeasure);

        /// <summary>
        /// Valida si es posible eliminar esta unidad de medida, basandose en si tiene relacion con registros o no
        /// </summary>
        /// <param name="unitOfMeasure">Unidad de medida que se desea vaidar</param>
        /// <returns>True si es posible eliminarla</returns>
        bool CanDelete(UnidadesDeMedida unitOfMeasure);

    }
}
