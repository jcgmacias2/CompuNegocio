using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewEntradasPorComprasRepository : BaseRepository<VwEntradasPorCompra>, IViewEntradasPorComprasRepository
    {
        public ViewEntradasPorComprasRepository(CNEntities context) : base(context) { }

        public List<VwEntradasPorCompra> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                return _dbSet.Where(e => e.idArticulo.Equals(idItem) && e.fechaHora >= start && e.fechaHora <= end).ToList();
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

                var transactions = _dbSet.Where(s => s.idArticulo.Equals(idItem) && s.fechaHora >= start && s.fechaHora <= end);

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
