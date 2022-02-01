using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class CodigosDeArticuloPorProveedorRepository : BaseRepository<CodigosDeArticuloPorProveedor>, ICodigosDeArticuloPorProveedorRepository
    {

        public CodigosDeArticuloPorProveedorRepository(CNEntities context) : base(context) { }

        public CodigosDeArticuloPorProveedor Find(int idCodigoDeArticuloPorProveedor)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.idCodigoDeArticuloPorProveedor.Equals(idCodigoDeArticuloPorProveedor));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorProveedor> List(string codigo)
        {
            try
            {
                return _dbSet.Where(a => a.codigo.Equals(codigo, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorProveedor> ListByItem(int idArticulo)
        {
            try
            {
                return _dbSet.Where(a => a.idArticulo.Equals(idArticulo)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorProveedor> ListBySupplier(int idProveedor)
        {
            try
            {
                return _dbSet.Where(a => a.idProveedor.Equals(idProveedor)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
