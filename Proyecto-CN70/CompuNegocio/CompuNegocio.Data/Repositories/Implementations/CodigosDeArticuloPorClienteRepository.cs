using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class CodigosDeArticuloPorClienteRepository : BaseRepository<CodigosDeArticuloPorCliente>, ICodigosDeArticuloPorClienteRepository
    {

        public CodigosDeArticuloPorClienteRepository(CNEntities context) : base(context) { }

        public CodigosDeArticuloPorCliente Find(int idCodigoDeArticuloPorCliente)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.idCodigoDeArticuloPorCliente == idCodigoDeArticuloPorCliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorCliente> List(string codigo)
        {
            try
            {
                return _dbSet.Where(a => a.codigo.Equals(codigo, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorCliente> ListByItem(int idArticulo)
        {
            try
            {
                return _dbSet.Where(a => a.idArticulo == idArticulo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CodigosDeArticuloPorCliente> ListByCustomer(int idCliente)
        {
            try
            {
                return _dbSet.Where(a => a.idCliente == idCliente).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
