using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewEntradasPorNotasDeCreditoRepository : BaseRepository<VwEntradasPorNotasDeCredito>, IViewEntradasPorNotasDeCreditoRepository
    {

        public ViewEntradasPorNotasDeCreditoRepository(CNEntities context) : base(context) { }

        public decimal GetTotal(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();
                var transactions = _dbSet.Where(nc => nc.idArticulo.Equals(idItem) && nc.fechaHora >= start && nc.fechaHora <= end);

                if (transactions.Count().Equals(0))
                    return 0.0m;

                return transactions.Sum(s => s.Unidades);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwEntradasPorNotasDeCredito> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();
                return _dbSet.Where(nc => nc.idArticulo.Equals(idItem) && nc.fechaHora >= start && nc.fechaHora <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
