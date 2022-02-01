using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IAjustesRepository : IBaseRepository<Ajuste>
    {
        Ajuste Find(string folio);

        List<Ajuste> WithFolioLike(string value);

        List<Ajuste> List(TiposDeAjuste type);

        List<Ajuste> ListByPeriodAndType(DateTime start, DateTime end, TiposDeAjuste type);

        string Next();
    }
}
