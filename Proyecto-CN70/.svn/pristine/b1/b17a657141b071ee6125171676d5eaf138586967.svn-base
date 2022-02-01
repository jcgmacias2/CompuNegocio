using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewArticulosVendidosRepository : BaseRepository<VwArticulosVendido>, IViewArticulosVendidosRepository
    {
        public ViewArticulosVendidosRepository(CNEntities context) : base(context) { }

        public List<VwArticulosVendido> List(int idCliente)
        {
            try
            {
                return _dbSet.AsNoTracking().Where(x => x.Cliente == idCliente).OrderByDescending(x => x.FechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwArticulosVendido> Like(int idCliente, string value)
        {
            try
            {
                return _dbSet.Where(x => x.Cliente == idCliente && (x.CodigoArticulo.Contains(value) || x.DescripcionArticulo.Contains(value) || x.Fecha.Contains(value))).OrderByDescending(x => x.FechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
