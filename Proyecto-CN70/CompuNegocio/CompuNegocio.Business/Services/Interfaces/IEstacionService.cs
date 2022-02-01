using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IEstacionService
    {
        /// <summary>
        /// Agrega una nueva estación al catálogo de estaciones existentes
        /// </summary>
        /// <param name="station">Estación que se desea agregar</param>
        /// <returns>Estacion registrada</returns>
        Estacione Add(Estacione station);
        
        /// <summary>
        /// Busca una estación en base a su descripción
        /// </summary>
        /// <param name="description">Descripción a buscar</param>
        /// <returns>Estacion que coincide con la descripción</returns>
        Estacione Find(string description);

        /// <summary>
        /// Busca una estación en base a su identificador
        /// </summary>
        /// <param name="idStation">Identificador numérico de la estación</param>
        /// <returns>Estación a la que pertenece el identificador</returns>
        Estacione Find(int idStation);

        /// <summary>
        /// Listas las estaciones de una caja especifica
        /// </summary>
        /// <param name="idRegister">Identificador numérico de la caja de la cual se desean obtener las estaciones</param>
        /// <returns>Lista de estaciones relacionadas a la caja</returns>
        List<Estacione> List(int idRegister);

        /// <summary>
        /// Provee una lista de todas las estaciones registradas
        /// </summary>
        /// <returns>Lista de estaciones</returns>
        List<Estacione> List();

        /// <summary>
        /// Lista las estaciones que coincidan total o parcialmente con la descripción proporcionada
        /// </summary>
        /// <param name="description">Descripción total o parcial a buscar</param>
        /// <returns>Lista de estaciones que cumplen con el criterio</returns>
        List<Estacione> WithDescriptionLike(string description);

        /// <summary>
        /// Actualiza la información sobre la estación
        /// </summary>
        /// <param name="station">Estación con los nuevos datos</param>
        /// <param name="canChangeBusiness">True si no hay ninguna venta pendiente de facturar</param>
        /// <returns>Estación con los cambios registrados</returns>
        Estacione Update(Estacione station, bool canChangeBusiness);


        /// <summary>
        /// Establece la estación como la relacionada al equipo
        /// </summary>
        /// <param name="station">Estación a relacionar</param>
        /// <returns>Estación con los cambios registrados</returns>
        Estacione AssociateStation(Estacione station);

        /// <summary>
        /// Remueve la relacion de una estación con el equipo
        /// </summary>
        /// <param name="station">Estación a la cual remover la asociación</param>
        /// <returns>Estación con los cambios registrados</returns>
        Estacione DissociateStation(Estacione station);

        /// <summary>
        /// Obtiene la estación asociada al equipo que la manda llamar
        /// </summary>
        /// <returns>Una estación si es que hay alguna asociada</returns>
        Estacione GetAssociatedStation();

        /// <summary>
        /// Elimina una estación del catálogo de estaciones
        /// </summary>
        /// <param name="station">Estación a eliminar</param>
        void Remove(Estacione station);
    }
}
