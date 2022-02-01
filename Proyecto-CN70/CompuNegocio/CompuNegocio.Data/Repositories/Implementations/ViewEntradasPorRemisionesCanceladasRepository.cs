using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewEntradasPorRemisionesCanceladasRepository : BaseRepository<VwEntradasPorRemisionesCancelada>, IViewEntradasPorRemisionesCanceladasRepository
    {
        public ViewEntradasPorRemisionesCanceladasRepository(CNEntities context) : base(context) { }

        public List<VwEntradasPorRemisionesCancelada> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                return _dbSet.Where(e => e.idArticulo.Equals(idItem) && e.fechaCancelacion.HasValue && e.fechaCancelacion.Value >= start && e.fechaCancelacion.Value <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal GetTotal(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                var transactions = _dbSet.Where(e => e.idArticulo.Equals(idItem) && e.fechaCancelacion.HasValue && e.fechaCancelacion.Value >= start && e.fechaCancelacion.Value <= end);

                if (transactions.Count().Equals(0))
                    return 0.0m;

                return transactions.Sum(e => e.Unidades);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
