using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class CuentasGuardianRepository : BaseRepository<CuentasGuardian>, ICuentasGuardianRepository
    {

        public CuentasGuardianRepository(CNEntities context) : base(context) { }

        public CuentasGuardian Find(int idCuenta)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.idCuentaGuardian.Equals(idCuenta));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasGuardian Find(int idConfiguration, string accountAddress)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.idConfiguracion.Equals(idConfiguration) && a.direccion.Equals(accountAddress, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasGuardian> List(int idConfiguracion)
        {
            try
            {
                return _dbSet.Where(a => a.idConfiguracion.Equals(idConfiguracion)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
