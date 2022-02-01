using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IImpuestosRepository : IBaseRepository<Impuesto>
    {
        bool CanDelete(int idTax);

        Impuesto Find(string code, decimal rate, TiposDeImpuesto type);

        List<Impuesto> WithNameLike(string name);

        Impuesto Find(string description, int type, decimal rate);

        Impuesto SearchAll(string description, int type, decimal rate);

        List<VwReporteImpuestosPorPeriodo> List(DateTime startDate, DateTime endDate);
    }
}
