using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IEmpresasAsociadasRepository : IBaseRepository<EmpresasAsociada>
    {
        bool CanDelete(EmpresasAsociada empresaAsociada);

        List<EmpresasAsociada> Like(string value);

        EmpresasAsociada Find(string descripcion);

        EmpresasAsociada FindByDatabaseName(string databaseName);

        bool ExistsByDatabaseName(string databaseName);
    }
}
