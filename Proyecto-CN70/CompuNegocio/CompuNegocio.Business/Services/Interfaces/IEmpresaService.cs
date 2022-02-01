using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IEmpresaService
    {
        /// <summary>
        /// Agrega una nueva empresa a la colección de empresas configuradas
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        Empresa Add(Empresa business);

        /// <summary>
        /// Busca una empresa a partir de su identificador numérico
        /// </summary>
        /// <param name="idBusiness">Identificador numérico de la empresa</param>
        /// <returns>Empresa correspondiente al identificador</returns>
        Empresa Find(int idBusiness);

        /// <summary>
        /// Busca una empresa a partir de su descripción
        /// </summary>
        /// <param name="description">Descripción a buscar</param>
        /// <returns>Empresa correspondiente a la descripción</returns>
        Empresa Find(string description);

        /// <summary>
        /// Actualiza los datos de una empresa existente
        /// </summary>
        /// <param name="business">Caja con los datos modificados</param>
        /// <returns>Empresa modificada</returns>
        Empresa Update(Empresa business);

        /// <summary>
        /// Verifica si es posible eliminar una empresa, ya que cuando se relaciona a la empresa con operaciones no es posible eliminarla
        /// </summary>
        /// <param name="business">Empresa a validar</param>
        /// <returns>True si es posible eliminarla</returns>
        bool CanDelete(Empresa business);

        /// <summary>
        /// Elimina una empresa del catálogo de empresas existene
        /// </summary>
        /// <param name="business">Empresa a eliminar</param>
        void Delete(Empresa business);

        /// <summary>
        /// Provee una lista de las empresas configuradas que estan activas
        /// </summary>
        /// <returns>Lista de empresas activas</returns>
        List<Empresa> List();

        /// <summary>
        /// Provee una lista de las empresas que tienen similitud con la descripción proporcionada
        /// </summary>
        /// <param name="description">Descripción o parte de la descripción de la empresa</param>
        /// <returns>Lista de empresas que coinciden y estan activas</returns>
        List<Empresa> WithDescriptionLike(string description);
    }
}
