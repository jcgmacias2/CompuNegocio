using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewResumenPorCompraRepository : BaseRepository<VwResumenPorCompra>, IViewResumenPorCompraRepository
    {
        public ViewResumenPorCompraRepository(CNEntities context) : base(context) { }

        public List<VwResumenPorCompra> List(int idProveedor, DateTime inicio, DateTime fin, bool soloConDeuda)
        {
            try
            {
                inicio = inicio.ToLastMidnight();
                fin = fin.ToNextMidnight();

                if (soloConDeuda)
                    return _dbSet.Where(c => c.idProveedor.Equals(idProveedor) && c.fechaHora >= inicio && c.fechaHora <= fin && c.idEstatusDeCompra != 3 && c.abonado.Value < c.subtotal.Value + c.impuestos.Value).ToList();
                else
                    return _dbSet.Where(c => c.idProveedor.Equals(idProveedor) && c.fechaHora >= inicio && c.fechaHora <= fin && c.idEstatusDeCompra != 3).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
