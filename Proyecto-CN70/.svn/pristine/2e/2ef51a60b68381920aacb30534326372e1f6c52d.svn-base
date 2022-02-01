using System;
using System.Collections.Generic;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class ProductosServiciosRepository : BaseRepository<ProductosServicio>, IProductosServiciosRepository
    {
        public ProductosServiciosRepository(CNEntities context) : base(context) { }

        public ProductosServicio Find(int idProductoServicio)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.idProductoServicio.Equals(idProductoServicio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductosServicio Find(string codigo)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.codigo.Equals(codigo, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProductosServicio> List(string value)
        {
            try
            {
                return _dbSet.Where(p => p.codigo.Contains(value) || p.descripcion.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
