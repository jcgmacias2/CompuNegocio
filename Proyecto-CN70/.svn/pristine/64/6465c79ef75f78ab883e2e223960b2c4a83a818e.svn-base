using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface ICuentasBancariasRepository : IBaseRepository<CuentasBancaria>
    {
        bool CanDelete(CuentasBancaria cuenta);

        List<CuentasBancaria> Like(string value);

        CuentasBancaria Find(string numero);
    }
}
