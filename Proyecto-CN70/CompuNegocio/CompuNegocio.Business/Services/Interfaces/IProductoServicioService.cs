using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IProductoServicioService
    {
        ProductosServicio Add(ProductosServicio productoServicio);

        ProductosServicio Find(int idProductoServicio);

        ProductosServicio Find(string codigo);

        List<ProductosServicio> List();

        List<ProductosServicio> List(string value);

        void Remove(int idProductoServicio);

        ProductosServicio Update(ProductosServicio productoServicio);
    }
}
