using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IAbonosDeRemisionRepository : IBaseRepository<AbonosDeRemision>
    {
        AbonosDeRemision Find(string folio);

        List<AbonosDeRemision> List(Cliente client);

        List<AbonosDeRemision> List(int idBillOfSale);

        List<AbonosDeRemision> List(int idRegister, DateTime start, DateTime end);

        List<AbonosDeRemision> List(DateTime start, DateTime end);
    }
}
