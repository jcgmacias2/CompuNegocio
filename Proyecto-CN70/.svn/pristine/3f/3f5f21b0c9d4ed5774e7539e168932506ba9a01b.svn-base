using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSalidasPorAjustesCanceladosRepository : BaseRepository<VwSalidasPorAjustesCancelado>, IViewSalidasPorAjustesCanceladosRepository
    {
        public ViewSalidasPorAjustesCanceladosRepository(CNEntities context) : base(context) { }

        public List<VwSalidasPorAjustesCancelado> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                return _dbSet.Where(s => s.idArticulo == idItem && s.fechaCancelacion >= start && s.fechaCancelacion <= end).ToList();
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

                var transactions = _dbSet.Where(s => s.idArticulo == idItem && s.fechaCancelacion >= start && s.fechaCancelacion <= end);

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
