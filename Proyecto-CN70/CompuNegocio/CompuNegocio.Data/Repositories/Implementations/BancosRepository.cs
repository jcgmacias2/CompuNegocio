using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class BancosRepository : BaseRepository<Banco>, IBancosRepository
    {

        public BancosRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idBank)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(b => b.idBanco.Equals(idBank));

                if (local.CuentasBancarias.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Banco Find(int idBank)
        {
            try
            {
                return _dbSet.FirstOrDefault(b => b.idBanco.Equals(idBank));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Banco Find(string name)
        {
            try
            {
                return _dbSet.FirstOrDefault(b => b.nombre.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Banco> Like(string value)
        {
            try
            {
                return _dbSet.Where(b => b.nombre.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
