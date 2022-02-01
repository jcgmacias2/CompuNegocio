using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IAbonosDeCompraRepository : IBaseRepository<AbonosDeCompra>
    {
        AbonosDeCompra Find(string folio);

        List<AbonosDeCompra> List(int idPurchase);

        string NextFolio();
    }
}
