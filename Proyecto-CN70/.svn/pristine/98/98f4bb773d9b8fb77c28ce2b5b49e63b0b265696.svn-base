using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewVentasActivasPorClienteRepository : IBaseRepository<VwVentasActivasPorCliente>
    {
        List<VwVentasActivasPorCliente> List(int idCliente, bool withDebtOnly);
        List<VwVentasActivasPorCliente> Like(int idCliente, string value, bool withDebtOnly);
    }
}
