using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IFormasPagoRepository : IBaseRepository<FormasPago>
    {
        FormasPago Find(string description);

        List<FormasPago> WithDescriptionLike(string description);
    }
}
