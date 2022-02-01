using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSaldoDeudorPorClientePorMonedaRepository : BaseRepository<VwSaldoDeudorPorClientePorMoneda>, IViewSaldoDeudorPorClientePorMonedaRepository
    {
        public ViewSaldoDeudorPorClientePorMonedaRepository(CNEntities context) : base(context) { }

        public VwSaldoDeudorPorClientePorMoneda FindByCustomerAndCurrency(Cliente customer, Moneda currency)
        {
            try
            {
                return _dbSet.AsNoTracking().FirstOrDefault(x => x.idCliente == customer.idCliente && x.idMoneda == currency.idMoneda);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
