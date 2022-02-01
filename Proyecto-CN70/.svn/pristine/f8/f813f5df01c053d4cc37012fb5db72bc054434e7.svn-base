using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class OrdenesDeCompraRepository : BaseRepository<OrdenesDeCompra>, IOrdenesDeCompraRepository
    {
        public OrdenesDeCompraRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var orden = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(orden.isValid())
                    return orden.folio + 1;
                else
                    return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                var orden = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (orden.isValid())
                    return orden.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra Find(int folio, int idProveedor)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.folio.Equals(folio) && r.idProveedor == idProveedor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra FindById(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idOrdenDeCompra.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrdenesDeCompra> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeOrdenDeCompra.Equals(idEstatus)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra FindByFolio(int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(x => x.folio == folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrdenesDeCompra> WithFolioOrProviderLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeOrdenDeCompra.Equals(idEstatus)).Where(r => r.folio.ToString().Contains(value) || r.Proveedore.razonSocial.Contains(value)).ToList();
                else
                    return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.Proveedore.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrdenesDeCompra> WithStatus(StatusDeOrdenDeCompra status)
        {
            try
            {
                return _dbSet.Where(x => x.idEstatusDeOrdenDeCompra == (int) status).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrdenesDeCompra> WithProvider(Proveedore proveedor)
        {
            try
            {
                return _dbSet.Where(x => x.idProveedor == proveedor.idProveedor).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDeOrdenDeCompra detail)
        {
            var detailDb = _context.DetallesDeOrdenDeCompras.Find(detail.idDetalleDeOrdenDeCompra);
            _context.DetallesDeOrdenDeCompras.Remove(detailDb);
        }
    }
}
