using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IUnidadesDeMedidaRepository : IBaseRepository<UnidadesDeMedida>
    {
        bool CanDelete(int idUnitOfMeasure);

        List<UnidadesDeMedida> Like(string value);

        UnidadesDeMedida Find(string code);

        UnidadesDeMedida SearchAll(string code);
    }
}
