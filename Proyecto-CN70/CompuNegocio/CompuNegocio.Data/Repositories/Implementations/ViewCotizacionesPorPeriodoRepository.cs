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
    public class ViewCotizacionesPorPeriodoRepository : BaseRepository<VwCotizacionesPorPeriodo>, IViewCotizacionesPorPeriodoRepository
    {
        public ViewCotizacionesPorPeriodoRepository(CNEntities context) : base(context) { }

        public List<VwCotizacionesPorPeriodo> ListByPeriod(DateTime from, DateTime to, bool onlySalePending)
        {
            try
            {
                var data = _dbSet.AsQueryable();

                data = data.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= from.Date && DbFunctions.TruncateTime(x.fechaHora) <= to.Date);

                if (onlySalePending)
                {
                    data = data.Where(x => x.idFactura == null && x.idRemision == null);
                }

                return data.OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwCotizacionesPorPeriodo> ListByPeriodAndCustomer(DateTime from, DateTime to, Cliente customer, bool onlySalePending)
        {
            try
            {
                var data = _dbSet.AsQueryable();

                data = data.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= from.Date && DbFunctions.TruncateTime(x.fechaHora) <= to.Date && x.idCliente == customer.idCliente);

                if (onlySalePending)
                {
                    data = data.Where(x => x.idFactura == null && x.idRemision == null);
                }

                return data.OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
