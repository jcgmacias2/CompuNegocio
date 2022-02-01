using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IProductosServiciosRepository : IBaseRepository<ProductosServicio>
    {
        ProductosServicio Find(int idProductoServicio);

        ProductosServicio Find(string codigo);

        List<ProductosServicio> List(string value);
    }
}
