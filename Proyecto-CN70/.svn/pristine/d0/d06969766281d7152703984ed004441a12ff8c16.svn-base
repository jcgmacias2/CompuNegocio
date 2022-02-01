using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IUsuariosRepository : IBaseRepository<Usuario>
    {
        bool CanDelete(int idUser);

        Usuario Find(string username);

        List<Usuario> WithNameLike(string name);

        Usuario Authenticate(string username, string password);
    }
}
