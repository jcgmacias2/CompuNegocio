using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class AbonosDeRemisionRepository : BaseRepository<AbonosDeRemision>, IAbonosDeRemisionRepository
    {
        public AbonosDeRemisionRepository(CNEntities context) : base(context) { }

        public AbonosDeRemision Find(string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.folio.Equals(folio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> List(Cliente client)
        {
            try
            {
                return _dbSet.Where(a => a.Remisione.idCliente.Equals(client.idCliente)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> List(int idBillOfSale)
        {
            try
            {
                return _dbSet.Where(a => a.idRemision.Equals(idBillOfSale)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> List(int idRegister, DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end && a.idEmpresa.Equals(idRegister)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeRemision> List(DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
