using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IClientesRepository : IBaseRepository<Cliente>
    {
        Cliente Find(string code);

        bool CanDelete(int idClient);

        List<Cliente> WithNameLike(string name);

        Cliente Exist(string rfc);

        List<Cliente> List(string dbcPath);

        Cliente SearchAll(string code);
    }
}
