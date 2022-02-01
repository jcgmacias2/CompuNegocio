using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IEquivalenciasRepository : IBaseRepository<Equivalencia>
    {
        Equivalencia Find(int idItem, int idUnitOfMeasure);

        List<Equivalencia> List(int idItem);

        bool HasOperations(int idEquivalence);
    }
}
