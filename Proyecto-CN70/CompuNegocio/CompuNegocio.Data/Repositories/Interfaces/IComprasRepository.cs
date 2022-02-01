using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IComprasRepository : IBaseRepository<Compra>
    {
        Compra Find(int idSupplier, string folio);

        List<Compra> WithFolioOrSupplierLike(string search);

        Compra Last(int idSupplier);

        List<Compra> List(DateTime start, DateTime end);

        List<Compra> List(DateTime start, DateTime end, Proveedore supplier);

        List<DetallesDeCompra> Find(OrdenesDeCompra order);
    }
}
