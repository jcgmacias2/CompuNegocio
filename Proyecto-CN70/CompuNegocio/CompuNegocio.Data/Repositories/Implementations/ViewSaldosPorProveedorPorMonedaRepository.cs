using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSaldosPorProveedorPorMonedaRepository : BaseRepository<VwSaldosPorProveedorPorMoneda>, IViewSaldosPorProveedorPorMonedaRepository
    {
        public ViewSaldosPorProveedorPorMonedaRepository(CNEntities context) : base(context) { }

        public new List<VwSaldosPorProveedorPorMoneda> List()
        {
            try
            {
                return _dbSet.Where(p => p.activo && p.total > p.abonado).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
