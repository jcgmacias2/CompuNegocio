using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene el usuario que se usa para la sesión cuando la autenticación la lleva a cabo la API
        /// </summary>
        /// <returns>Usuario default</returns>
        Usuario GetApiDefault();

        /// <summary>
        /// Agrega un nuevo usuario a la lista existente
        /// </summary>
        /// <param name="user">Usuario a agregar</param>
        /// <returns>Usuario con identificador autogenerado</returns>
        Usuario Add(Usuario user);

        /// <summary>
        /// Busca un usuario que coincida con el identificador númerico especificado
        /// </summary>
        /// <param name="idUser">Identificador númerico del usuario que se busca</param>
        /// <returns>Usuario al que pertenece el identificador</returns>
        Usuario Find(int idUser);

        /// <summary>
        /// Busca un usuario que coincida con el nombre especificado
        /// </summary>
        /// <param name="name">Nombre del usuario que se busca</param>
        /// <returns>Usuario al que pertenece el nombre</returns>
        Usuario Find(string name);

        /// <summary>
        /// Enlista todos los usuarios activos del catálogo
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        List<Usuario> List();

        /// <summary>
        /// Enlista todos los usuarios cuyo nombre de usuario o nombre completo coincide con el nombre buscado
        /// </summary>
        /// <param name="name">Nombre a buscar en coincidencia</param>
        /// <returns>Lista de usuarios activos que coinciden</returns>
        List<Usuario> WithNameLike(string name);

        /// <summary>
        /// Actualiza los datos de un usuario
        /// </summary>
        /// <param name="user">Usuario con los datos modificados</param>
        /// <returns>Usuario actualizado</returns>
        Usuario Update(Usuario user);

        /// <summary>
        /// Elimina un usuario de la lista de usuarios
        /// </summary>
        /// <param name="user">Usuario a eliminar</param>
        void Delete(Usuario user);

        /// <summary>
        /// Valida si es posible eliminar un usuario, los usuarios que se pueden eliminar son aquellos que no tienen relación con operaciones
        /// </summary>
        /// <param name="user">Usuario a validar</param>
        /// <returns>True si es posible eliminarlo</returns>
        bool CanDelete(Usuario user);
    }
}
