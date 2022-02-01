using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewVentasActivasPorClienteRepository : BaseRepository<VwVentasActivasPorCliente>, IViewVentasActivasPorClienteRepository
    {
        public ViewVentasActivasPorClienteRepository(CNEntities context) : base(context) { }

        public List<VwVentasActivasPorCliente> List(int idCliente, bool withDebtOnly)
        {
            try
            {
                var query = _dbSet.AsNoTracking().Where(x => x.idCliente == idCliente);

                if (withDebtOnly)
                {
                    query = query.Where(x => x.Saldo != null && x.Saldo.Value > 0m);
                }

                return query.OrderByDescending(x => x.FechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwVentasActivasPorCliente> Like(int idCliente, string value, bool withDebtOnly)
        {
            try
            {
                var query = _dbSet.AsNoTracking().Where(x => x.idCliente == idCliente && (x.Folio.Contains(value) || x.Fecha.Contains(value)));

                if (withDebtOnly)
                {
                    query = query.Where(x => x.Saldo != null && x.Saldo.Value > 0m);
                }

                return query.OrderByDescending(x => x.FechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
