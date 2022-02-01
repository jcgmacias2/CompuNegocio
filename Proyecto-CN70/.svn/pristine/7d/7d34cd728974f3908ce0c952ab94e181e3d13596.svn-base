using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public interface IPedidosRepository : IBaseRepository<Pedido>
    {
        int Next();

        int Last();

        Pedido Find(int folio);

        Pedido FindById(int id);

        List<Pedido> List(int? idEstatus);

        List<Pedido> WithFolioOrClientLike(string value, int? idEstatus);

        List<Pedido> WithStatus(StatusDePedido status);

        List<Pedido> WithCustomer(Cliente customer);

        void DeleteDetail(DetallesDePedido detail);
    }
}
