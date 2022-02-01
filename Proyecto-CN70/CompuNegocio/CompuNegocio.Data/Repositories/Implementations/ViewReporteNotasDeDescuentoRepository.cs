using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteNotasDeDescuentoRepository : BaseRepository<VwReporteNotasDeDescuento>, IViewReporteNotasDeDescuentoRepository
    {
        public ViewReporteNotasDeDescuentoRepository(CNEntities context) : base(context) { }


        public List<VwReporteNotasDeDescuento> List(Cliente customer, DateTime startDate, DateTime endDate, bool includeOnlyPending, bool includeOnlyApplied)
        {
            try
            {
                var data = _dbSet.AsNoTracking().AsQueryable().Where(x=>DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date && x.idEstatusDeNotaDeDescuento != (int)StatusDeNotaDeDescuento.Cancelada);

                if (customer.isValid() && customer.idCliente.isValid())
                {
                    data = data.Where(x => x.idCliente == customer.idCliente);
                }

                if (includeOnlyPending)
                {
                    data = data.Where(x => x.idFactura == null);
                }

                if (includeOnlyApplied)
                {
                    data = data.Where(x => x.idFactura != null);
                }

                return data.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
