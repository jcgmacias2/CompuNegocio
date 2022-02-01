using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IClasificacionesRepository : IBaseRepository<Clasificacione>
    {
        bool CanDelete(int idClasification);

        Clasificacione Find(string code);

        List<Clasificacione> WithNameLike(string name);
    }
}
