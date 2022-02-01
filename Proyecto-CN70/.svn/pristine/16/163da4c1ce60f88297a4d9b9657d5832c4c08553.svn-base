using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewEntradasPorFacturasCanceladasRepository : BaseRepository<VwEntradasPorFacturasCancelada>, IViewEntradasPorFacturasCanceladasRepository
    {
        public ViewEntradasPorFacturasCanceladasRepository(CNEntities context) : base(context) { }

        public List<VwEntradasPorFacturasCancelada> List(int idItem, DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                //Incluye las canceladas y anuladas
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

                //Incluye canceladas y anuladas
                var canceladas = _dbSet.Where(e => e.idArticulo.Equals(idItem) && e.fechaCancelacion.HasValue && e.fechaCancelacion.Value >= start && e.fechaCancelacion.Value <= end).ToList();

                return canceladas.Sum(e => e.Unidades);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
