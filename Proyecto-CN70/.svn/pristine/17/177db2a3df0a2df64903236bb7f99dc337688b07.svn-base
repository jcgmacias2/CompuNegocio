using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IBancosRepository : IBaseRepository<Banco>
    {
        Banco Find(int idBank);

        Banco Find(string name);

        List<Banco> Like(string value);

        bool CanDelete(int idBank);

    }
}
