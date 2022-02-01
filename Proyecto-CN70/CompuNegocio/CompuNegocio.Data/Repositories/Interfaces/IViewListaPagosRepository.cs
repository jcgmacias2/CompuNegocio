using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewListaPagosRepository : IBaseRepository<VwListaPago>
    {
        List<VwListaPago> WithFolioOrClientLike(string value, int? idEstatus);
    }
}
