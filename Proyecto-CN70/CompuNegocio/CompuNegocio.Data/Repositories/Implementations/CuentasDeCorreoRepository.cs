using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Data.Repositories
{
    public class CuentasDeCorreoRepository : BaseRepository<CuentasDeCorreo>, ICuentasDeCorreoRepository
    {

        public CuentasDeCorreoRepository(CNEntities context) : base(context) { }

        public CuentasDeCorreo Find(int idCliente, string account)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.idCliente.Equals(idCliente) && c.cuenta.Equals(account, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CuentasDeCorreo> List(int idCliente)
        {
            try
            {
                return _dbSet.Where(c => c.idCliente.Equals(idCliente)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
