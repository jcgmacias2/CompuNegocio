using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IListasDePrecioRepository : IBaseRepository<ListasDePrecio>
    {
        ListasDePrecio Find(string code);

        List<ListasDePrecio> WithCodeLike(string code);

        List<ListasDePrecio> WithItemLike(string item);

        List<ListasDePrecio> WithClientLike(string client);
    }
}
