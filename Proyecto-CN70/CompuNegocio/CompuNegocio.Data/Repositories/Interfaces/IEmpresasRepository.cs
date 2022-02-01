using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;


namespace Aprovi.Data.Repositories
{
    public interface IEmpresasRepository : IBaseRepository<Empresa>
    {
        bool CanDelete(int idBusiness);

        Empresa Find(string description);

        List<Empresa> WithDescriptionLike(string description);
    }
}
