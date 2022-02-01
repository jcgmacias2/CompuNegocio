using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class CuentasBancariasRepository : BaseRepository<CuentasBancaria>, ICuentasBancariasRepository
    {

        public CuentasBancariasRepository(CNEntities context) : base(context) { }

        public bool CanDelete(CuentasBancaria cuenta)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(c => c.idCuentaBancaria.Equals(cuenta.idCuentaBancaria));

                if (local.AbonosDeFacturas.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasBancaria Find(string numero)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.numeroDeCuenta.Equals(numero, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasBancaria> Like(string value)
        {
            try
            {
                return _dbSet.Where(c => c.Banco.nombre.Contains(value) || c.numeroDeCuenta.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
