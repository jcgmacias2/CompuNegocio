using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteCostoDeLoVendidoRepository : BaseRepository<VwReporteCostoDeLoVendido>, IViewReporteCostoDeLoVendidoRepository
    {
        public ViewReporteCostoDeLoVendidoRepository(CNEntities context) : base(context) { }


        public List<VwReporteCostoDeLoVendido> List(DateTime startDate, DateTime endDate, bool includeBillsOfSale)
        {
            try
            {
                var data = _dbSet.AsQueryable().Where(x => DbFunctions.TruncateTime(x.fecha) >= startDate.Date && DbFunctions.TruncateTime(x.fecha) <= endDate.Date);

                if (!includeBillsOfSale)
                {
                    data = data.Where(x => x.idEstatusDeRemision == null);
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
