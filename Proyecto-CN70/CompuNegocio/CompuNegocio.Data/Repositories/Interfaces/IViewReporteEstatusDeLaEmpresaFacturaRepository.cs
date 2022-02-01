using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteEstatusDeLaEmpresaFacturasRepository : IBaseRepository<VwReporteEstatusDeLaEmpresaFactura>
    {
        List<VwReporteEstatusDeLaEmpresaFactura> List(DateTime start, DateTime end);
    }
}
