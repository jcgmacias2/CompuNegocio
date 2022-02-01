using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewResumenPorRemisionRepository : IBaseRepository<VwResumenPorRemision>
    {
        List<VwResumenPorRemision> List(int idCliente, DateTime inicio, DateTime fin, bool soloConDeuda);

        List<VwResumenPorRemision> ListActive();

        List<VwResumenPorRemision> ActiveWithFolioOrClientLike(string value);
    }
}
