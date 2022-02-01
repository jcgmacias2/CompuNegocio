using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IVFPDataExtractorRepository
    {
        List<Articulo> GetArticulos(string dbcPath, out Dictionary<string, decimal> stock);

        Dictionary<string,string> GetFamiliasYDepartamentos(string dbcPath);

        List<string> GetUnidadesDeMedida(string dbcPath);
    }
}
