using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IEmpresaAsociadaService
    {
        EmpresasAsociada Add(EmpresasAsociada empresaAsociada);

        EmpresasAsociada Find(string nombre);

        EmpresasAsociada Find(int idEmpresaAsociada);

        EmpresasAsociada FindByDatabaseName(string databaseName);

        List<EmpresasAsociada> List();

        List<EmpresasAsociada> List(string value);

        EmpresasAsociada Update(EmpresasAsociada empresaAsociada);

        bool CanDelete(EmpresasAsociada empresaAsociada);

        bool CompanyExistsByDatabaseName(EmpresasAsociada empresaAsociada);

        void Delete(int idEmpresaAsociada);
    }
}
