using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class PreciosRepository : BaseRepository<Precio>, IPreciosRepository
    {
        public PreciosRepository(CNEntities context) : base(context) { }

        public Precio Find(int idListaDePrecio, int idArticulo)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.idListaDePrecio.Equals(idListaDePrecio) && p.idArticulo.Equals(idArticulo));
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Precio First(int idArticulo, int idCliente)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT P.* FROM Precios AS P JOIN ListasDePrecio AS LP ON P.idListaDePrecio = LP.idListaDePrecio JOIN Clientes AS C  ON C.idListaDePrecio = LP.idListaDePrecio WHERE C.idCliente = {0} AND P.idArticulo = {1}", idCliente, idArticulo)).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
