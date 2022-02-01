using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface ICodigosDeArticuloPorProveedorRepository : IBaseRepository<CodigosDeArticuloPorProveedor>
    {
        List<CodigosDeArticuloPorProveedor> ListByItem(int idArticulo);

        List<CodigosDeArticuloPorProveedor> ListBySupplier(int idProveedor);

        List<CodigosDeArticuloPorProveedor> List(string codigo);

        CodigosDeArticuloPorProveedor Find(int idCodigoPorArticulo);
    }
}
