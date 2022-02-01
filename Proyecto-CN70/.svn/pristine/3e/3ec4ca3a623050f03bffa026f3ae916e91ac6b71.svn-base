using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ComprasRepository : BaseRepository<Compra>, IComprasRepository
    {
        public ComprasRepository(CNEntities context) : base(context) { }

        public Compra Find(int idSupplier, string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.idProveedor.Equals(idSupplier) && c.folio.Equals(folio, StringComparison.InvariantCultureIgnoreCase) && !c.idEstatusDeCompra.Equals((int)StatusDeCompra.Cancelada));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compra> WithFolioOrSupplierLike(string search)
        {
            try
            {
                return _dbSet.Where(c => c.folio.Contains(search) || c.Proveedore.codigo.Contains(search)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Compra Last(int idSupplier)
        {
            try
            {
                var last = _dbSet.SqlQuery(string.Format("SELECT TOP 1 Compras.* FROM Compras WHERE idProveedor = {0} ORDER BY fechaHora DESC", idSupplier));

                if (!last.isValid())
                    throw new Exception("No existen compras registradas al proveedor especificado");

                return last.First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compra> List(DateTime start, DateTime end)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();
                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compra> List(DateTime start, DateTime end, Proveedore supplier)
        {
            start = start.ToLastMidnight();
            end = end.ToNextMidnight();
            return _dbSet.Where(a => a.idProveedor.Equals(supplier.idProveedor) && a.fechaHora >= start && a.fechaHora <= end).ToList();
        }

        public List<DetallesDeCompra> Find(OrdenesDeCompra order)
        {
            try
            {
                return _dbSet
                    .Where(x => x.idOrdenDeCompra == order.idOrdenDeCompra && x.idEstatusDeCompra != (int)StatusDeCompra.Cancelada)
                    .ToList().SelectMany(x=>x.DetallesDeCompras).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
