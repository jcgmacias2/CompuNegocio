using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface ICuentasDeCorreoRepository : IBaseRepository<CuentasDeCorreo>
    {
        CuentasDeCorreo Find(int idCliente, string account);

        List<CuentasDeCorreo> List(int idCliente);
    }
}
