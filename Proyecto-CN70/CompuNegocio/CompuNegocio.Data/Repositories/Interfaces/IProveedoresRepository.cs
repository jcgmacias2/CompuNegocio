using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;
using Aprovi.Data.Core;

namespace Aprovi.Data.Repositories
{
    public interface IProveedoresRepository : IBaseRepository<Proveedore>
    {
        Proveedore Find(string code);

        List<Proveedore> WithCodeLike(string code);

        bool CanDelete(int idSupplier);

        bool Exist(string rfc);

        List<Proveedore> List(string dbcPath);

        Proveedore SearchAll(string code);
    }
}
