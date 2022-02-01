using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class CuentasPredialesRepository : BaseRepository<CuentasPrediale>, ICuentasPredialesRepository
    {

        public CuentasPredialesRepository(CNEntities context) : base(context) { }

        public CuentasPrediale Find(int idCuenta)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.idCuentaPredial.Equals(idCuenta));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CuentasPrediale Find(string cuenta)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.cuenta.Equals(cuenta));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasPrediale> Like(string value)
        {
            try
            {
                return _dbSet.Where(c => c.cuenta.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
