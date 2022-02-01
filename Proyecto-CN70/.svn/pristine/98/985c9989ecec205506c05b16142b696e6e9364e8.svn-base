using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface ICuentaBancariaService
    {
        CuentasBancaria Add(CuentasBancaria cuenta);

        CuentasBancaria Find(string numero);

        CuentasBancaria Find(int idCuenta);

        List<CuentasBancaria> List();

        List<CuentasBancaria> List(string value);

        CuentasBancaria Update(CuentasBancaria cuenta);

        bool CanDelete(CuentasBancaria cuenta);

        void Delete(int idCuenta);
    }
}
