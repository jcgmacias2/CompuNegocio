using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IMigrationDataService
    {
        List<Articulo> GetArticulos(string dbcPath, out Dictionary<string, decimal> stock);

        List<VMEquivalenciaClasificacion> GetClasificaciones(string dbcPath);

        List<VMEquivalenciaUnidades> GetUnidadesDeMedida(string dbcPath);

        int Homologate(string excelFile, string CNStartCell, string SATStartCell);
    }
}
