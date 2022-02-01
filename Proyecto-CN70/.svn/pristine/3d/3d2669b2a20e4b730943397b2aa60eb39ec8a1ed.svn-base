using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSaldosPorClientePorMonedaRepository : BaseRepository<VwSaldosPorClientePorMoneda>, IViewSaldosPorClientePorMonedaRepository
    {
        public ViewSaldosPorClientePorMonedaRepository(CNEntities context) : base(context) { }

        public new List<VwSaldosPorClientePorMoneda> List()
        {
            try
            {
                return _dbSet.Where(c => c.activo && c.total > c.abonado).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwSaldosPorClientePorMoneda> List(int idClient)
        {
            try
            {
                return _dbSet.Where(c => c.idCliente.Equals(idClient) && c.total > c.abonado).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
