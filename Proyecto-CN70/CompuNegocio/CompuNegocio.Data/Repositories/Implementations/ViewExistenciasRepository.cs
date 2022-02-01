using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class ViewExistenciasRepository : BaseRepository<VwExistencia>, IViewExistenciasRepository
    {
        public ViewExistenciasRepository(CNEntities context) : base(context) { }

        public VwExistencia Find(int idItem)
        {
            try
            {
                var stock = _dbSet.FirstOrDefault(e => e.idArticulo.Equals(idItem));
                _context.Entry(stock).Reload();
                return stock;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
