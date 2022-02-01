using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;
using Aprovi.Data.Core;
using Aprovi.Data.Repositories;

namespace Aprovi.Business.Services
{
    public class ProductoServicioService : IProductoServicioService
    {
        private IUnitOfWork _UOW;
        private IProductosServiciosRepository _productosServicios;

        public ProductoServicioService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _productosServicios = _UOW.ProductosYServicios;
        }

        public ProductosServicio Add(ProductosServicio productoServicio)
        {
            try
            {
                if (!productoServicio.codigo.isValid())
                    throw new Exception("Debe capturar el código del producto o servicio");

                if (!productoServicio.descripcion.isValid())
                    throw new Exception("Debe capturar la descripción del producto o servicio");

                _productosServicios.Add(productoServicio);
                _UOW.Save();

                return productoServicio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductosServicio Find(int idProductoServicio)
        {
            try
            {
                return _productosServicios.Find(idProductoServicio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductosServicio Find(string codigo)
        {
            try
            {
                return _productosServicios.Find(codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProductosServicio> List()
        {
            try
            {
                return _productosServicios.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ProductosServicio> List(string value)
        {
            try
            {
                return _productosServicios.List(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(int idProductoServicio)
        {
            try
            {
                var local = _productosServicios.Find(idProductoServicio);

                if (local.Articulos.Count > 0)
                    throw new Exception("Tiene artículos relacionados, por lo que no puede ser eliminado");

                if (local.idProductoServicio.Equals(1))
                    throw new Exception("Este código forma parte de los defaults del sistema, por lo que no puede ser eliminado");

                _productosServicios.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ProductosServicio Update(ProductosServicio productoServicio)
        {
            try
            {
                var local = _productosServicios.Find(productoServicio.idProductoServicio);
                local.descripcion = productoServicio.descripcion;
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
