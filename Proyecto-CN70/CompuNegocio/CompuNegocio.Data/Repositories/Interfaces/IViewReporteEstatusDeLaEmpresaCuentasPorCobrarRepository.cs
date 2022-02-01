using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository : IBaseRepository<VwReporteEstatusDeLaEmpresaCuentasPorCobrar>
    {
        List<VwReporteEstatusDeLaEmpresaCuentasPorCobrar> List(DateTime start, DateTime end);
    }
}
