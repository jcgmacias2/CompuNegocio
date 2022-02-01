using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public interface IOrdenesDeCompraRepository : IBaseRepository<OrdenesDeCompra>
    {
        int Next();

        int Last();

        OrdenesDeCompra Find(int folio, int idProveedor);

        OrdenesDeCompra FindById(int id);

        OrdenesDeCompra FindByFolio(int folio);

        List<OrdenesDeCompra> List(int? idEstatus);

        List<OrdenesDeCompra> WithFolioOrProviderLike(string value, int? idEstatus);

        List<OrdenesDeCompra> WithStatus(StatusDeOrdenDeCompra status);

        List<OrdenesDeCompra> WithProvider(Proveedore provider);

        void DeleteDetail(DetallesDeOrdenDeCompra detail);
    }
}
