using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IRegimenService
    {
        /// <summary>
        /// Agrega un nuevo régimen a la colección de regímenes fiscales
        /// </summary>
        /// <param name="regime">Régimen a agregar</param>
        /// <returns>Régimen grabado</returns>
        Regimene Add(Regimene regime);

        /// <summary>
        /// Busca un régimen a partir de su identificador
        /// </summary>
        /// <param name="idRegime">Identificador numérico del régimen</param>
        /// <returns>Régimen al que pertenece el id</returns>
        Regimene Find(int idRegime);

        /// <summary>
        /// Elimina un régimen de la colección
        /// </summary>
        /// <param name="regime">Régimen a eliminar (con el id basta)</param>
        void Delete(Regimene regime);
    }
}
