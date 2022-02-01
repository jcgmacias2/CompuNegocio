using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewEntradasPorTraspasosRechazadosRepository : BaseRepository<VwEntradasPorTraspasosRechazado>, IViewEntradasPorTraspasosRechazadosRepository
    {

        public ViewEntradasPorTraspasosRechazadosRepository(CNEntities context) : base(context) { }

        public decimal GetTotal(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                var transactions = _dbSet.Where(e => e.idArticulo.Equals(idItem) && e.fechaHora >= start && e.fechaHora <= end);

                if (transactions.Count().Equals(0))
                    return 0.0m;

                return transactions.Sum(e => e.Unidades.GetValueOrDefault(0.0m));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwEntradasPorTraspasosRechazado> List(int idItem, DateTime start, DateTime end)
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
    }
}
