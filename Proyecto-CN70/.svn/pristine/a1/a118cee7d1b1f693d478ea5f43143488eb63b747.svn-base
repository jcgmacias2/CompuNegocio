using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface ICodigosDeArticuloPorClienteRepository : IBaseRepository<CodigosDeArticuloPorCliente>
    {
        List<CodigosDeArticuloPorCliente> ListByItem(int idArticulo);

        List<CodigosDeArticuloPorCliente> ListByCustomer(int idCliente);

        List<CodigosDeArticuloPorCliente> List(string codigo);

        CodigosDeArticuloPorCliente Find(int idCodigoDeArticuloPorCliente);
    }
}
