using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class ArticulosRepository : BaseRepository<Articulo>, IArticulosRepository
    {
        public ArticulosRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idItem)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(a => a.idArticulo.Equals(idItem));

                if (local.DetallesDeCompras.Count > 0)
                    return false;

                if (local.DetallesDeFacturas.Count > 0)
                    return false;

                if (local.Precios.Count > 0)
                    return false;

                if (local.DetallesDeAjustes.Count > 0)
                    return false;

                if (local.DetallesDePedidoes.Count > 0)
                    return false;

                if (local.DetallesDeCotizacions.Count > 0)
                    return false;

                if (local.DetallesDeRemisions.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Articulo Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> WithNameLike(string name)
        {
            try
            {
                return _dbSet.Where(a => a.codigo.Contains(name) || a.descripcion.Contains(name)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> List(int idClassification)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT A.* FROM Articulos AS A, ClasificacionesPorArticulo AS CxA WHERE CxA.idClasificacion = {0} AND CxA.idArticulo = A.idArticulo AND A.activo = 1", idClassification)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Articulo SearchAll(string code)
        {
            try
            {
                //Lo busco en la base de datos
                var item = _dbSet.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Si existe lo regreso
                if (item.isValid())
                    return item;

                //Si no existe en la base de datos, reviso el store local
                item = _dbSet.Local.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Regreso lo que haya obtenido, ya sea un null o lo que encontré localmente
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaArticulo> GetItemsForPagedList(int itemsAmount, int pageNumber)
        {
            try
            {
                return _context.VwListaArticulos.AsNoTracking().OrderBy(x => x.idArticulo).Skip((itemsAmount * pageNumber) - itemsAmount).Take(itemsAmount).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaArticulo> GetItemsForList()
        {
            try
            {
                return _context.VwListaArticulos.AsNoTracking().OrderBy(x => x.idArticulo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaArticulo> GetItemsForListWithNameLike(string name)
        {
            try
            {
                return _context.VwListaArticulos.AsNoTracking().Where(x => x.Codigo.Contains(name) || x.Descripcion.Contains(name)).OrderBy(x=>x.idArticulo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> FindAllForProvider(string code, int idSupplier)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT A.* FROM Articulos AS A WHERE idArticulo IN (SELECT idArticulo FROM VwCodigosDeArticuloPorProveedor WHERE codigoAlterno = '{0}' AND (idProveedor = {1} OR idProveedor IS NULL))", code, idSupplier)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> FindAllForCustomer(string code, int idCliente)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT A.* FROM Articulos AS A WHERE idArticulo IN (SELECT idArticulo FROM VwCodigosDeArticuloPorCliente WHERE codigoAlterno = '{0}' AND (idCliente = {1} OR idCliente IS NULL))", code, idCliente)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Articulo> WithFlow(DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT A.* FROM Articulos AS A WHERE A.idArticulo IN ( SELECT idArticulo FROM VwSalidasPorAjustes WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwSalidasPorAjustesCancelados WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwSalidasPorComprasCanceladas WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwSalidasPorFacturas WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwSalidasPorRemisiones WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwEntradasPorAjustes WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwEntradasPorAjustesCancelados WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwEntradasPorCompras WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwEntradasPorFacturasCanceladas WHERE fechaHora >= '{0}' AND fechaHora <= '{1}' UNION SELECT idArticulo FROM VwEntradasPorRemisionesCanceladas WHERE fechaHora >= '{0}' AND fechaHora <= '{1}') AND A.inventariado = 1", start, end)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
