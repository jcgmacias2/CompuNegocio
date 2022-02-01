using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewEntradasPorRemisionesCanceladasRepository : IBaseRepository<VwEntradasPorRemisionesCancelada>
    {
        List<VwEntradasPorRemisionesCancelada> List(int idItem, DateTime start, DateTime end);

        decimal GetTotal(int idItem, DateTime start, DateTime end);
    }
}
