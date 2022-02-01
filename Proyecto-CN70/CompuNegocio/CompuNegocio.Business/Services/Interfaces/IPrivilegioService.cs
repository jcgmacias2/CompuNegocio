using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IPrivilegioService
    {
        /// <summary>
        /// Permite agregar un nuevo privilegio
        /// </summary>
        /// <param name="privilege">Privilegio a añadir</param>
        /// <returns>Privilegio agregado</returns>
        Privilegio Add(Privilegio privilege);

        /// <summary>
        /// Enlista los privilegios de un usuario en particular
        /// </summary>
        /// <param name="idUser">Indentificador númerico del usuario del que se quiere consultar los privilegios</param>
        /// <returns>Lista de privilegios del usuario</returns>
        List<Privilegio> List(int idUser);

        /// <summary>
        /// Busca un privilegio para el usuario en la vista especificada
        /// </summary>
        /// <param name="idUser">Identificador númerico del usuario</param>
        /// <param name="idView">Identificador númerico de la vista (Pantalla)</param>
        /// <returns>Privilegio del usuario en la vista si encuentra, si no null</returns>
        Privilegio Find(int idUser, int idView);

        /// <summary>
        /// Elimina un privilegio
        /// </summary>
        /// <param name="privilege">Privilegio a eliminar</param>
        void Delete(Privilegio privilege);
    }
}
