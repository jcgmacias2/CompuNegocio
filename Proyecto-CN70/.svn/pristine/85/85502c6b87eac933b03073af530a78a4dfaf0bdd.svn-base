using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteAntiguedadSaldosFacturasRepository : IBaseRepository<VwReporteAntiguedadSaldosFactura>
    {
        List<VwReporteAntiguedadSaldosFactura> List(Cliente customer, Usuario seller, bool onlyExpired, DateTime to);
    }
}
