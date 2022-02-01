using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewCodigosDeArticuloPorProveedorRepository : BaseRepository<VwCodigosDeArticuloPorProveedor>, IViewCodigosDeArticuloPorProveedorRepository
    {
        public ViewCodigosDeArticuloPorProveedorRepository(CNEntities context) : base(context) { }

        public List<VwCodigosDeArticuloPorProveedor> List(int idArticulo)
        {
            return _dbSet.Where(a => a.idArticulo.Equals(idArticulo)).ToList();
        }
    }
}
