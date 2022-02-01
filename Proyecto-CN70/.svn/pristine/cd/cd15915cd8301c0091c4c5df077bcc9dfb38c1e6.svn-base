using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSalidasPorComprasCanceladasRepository : BaseRepository<VwSalidasPorComprasCancelada>, IViewSalidasPorComprasCanceladasRepository
    {
        public ViewSalidasPorComprasCanceladasRepository(CNEntities context) : base(context) { }

        public List<VwSalidasPorComprasCancelada> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                return _dbSet.Where(s => s.idArticulo.Equals(idItem) && s.fechaCancelacion.HasValue && s.fechaCancelacion.Value >= start && s.fechaCancelacion.Value <= end).ToList();
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

                var transactions = _dbSet.Where(s => s.idArticulo.Equals(idItem) && s.fechaCancelacion.HasValue && s.fechaCancelacion.Value >= start && s.fechaCancelacion.Value <= end);

                if (transactions.Count().Equals(0))
                    return 0.0m;

                return transactions.Sum(s => s.Unidades.Value);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
