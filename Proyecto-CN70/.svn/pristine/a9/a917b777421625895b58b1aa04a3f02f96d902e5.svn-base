using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteNotasDeDescuentoRepository : IBaseRepository<VwReporteNotasDeDescuento>
    {
        List<VwReporteNotasDeDescuento> List(Cliente customer, DateTime startDate, DateTime endDate, bool includeOnlyPending, bool includeOnlyApplied);
    }
}
