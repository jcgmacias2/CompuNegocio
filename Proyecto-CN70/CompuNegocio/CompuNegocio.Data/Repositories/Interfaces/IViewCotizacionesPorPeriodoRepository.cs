using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewCotizacionesPorPeriodoRepository: IBaseRepository<VwCotizacionesPorPeriodo>
    {
        List<VwCotizacionesPorPeriodo> ListByPeriod(DateTime from, DateTime to, bool onlySalePending);

        List<VwCotizacionesPorPeriodo> ListByPeriodAndCustomer(DateTime from, DateTime to, Cliente customer, bool onlySalePending);
    }
}
