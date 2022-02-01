using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewSeriesRepository : IBaseRepository<VwFoliosPorSerie>
    {
        VwFoliosPorSerie Find(string serie);

        int Next(string serie);
    }
}
