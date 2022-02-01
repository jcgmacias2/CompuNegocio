using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class CatalogosEstaticosService: ICatalogosEstaticosService
    {
        private IUnitOfWork _UOW;
        private ITiposDeImpuestoRepository _tiposDeImpuesto;
        private IMonedasRepository _monedas;
        private IPermisosRepository _permisos;
        private IPantallasRepository _pantallas;
        private IFormasPagoRepository _formasDePago;
        private ITiposDeComprobanteRepository _tiposDeComprobante;
        private IMetodosPagoRepository _metodosDePago;
        private ITiposDeAjustesRepository _tiposDeAjustes;
        private ITiposFactorRepository _tiposFactor;
        private IPaisesRepository _paises;
        private ITiposRelacionRepository _tiposRelacion;
        private IDirectorioRepository _directorio;
        private ISeccionesRepository _secciones;
        private IComisionesRepository _comisiones;
        private IReportesRepository _reportes;

        public CatalogosEstaticosService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _tiposDeImpuesto = _UOW.TiposDeImpuesto;
            _monedas = _UOW.Monedas;
            _pantallas = _UOW.Pantallas;
            _permisos = _UOW.Permisos;
            _formasDePago = _UOW.FormasDePago;
            _tiposDeComprobante = _UOW.TiposDeComprobante;
            _metodosDePago = _UOW.MetodosDePago;
            _tiposDeAjustes = _UOW.TiposDeAjuste;
            _tiposFactor = _UOW.TiposFactor;
            _paises = _UOW.Paises;
            _tiposRelacion = _UOW.TiposRelacion;
            _directorio = _UOW.Directorio;
            _secciones = _UOW.Secciones;
            _comisiones = _UOW.Comisiones;
            _reportes = _UOW.Reportes;
        }

        public List<TiposDeImpuesto> ListTiposDeImpuesto()
        {
            try
            {
                return _tiposDeImpuesto.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Moneda> ListMonedas()
        {
            try
            {
                return _monedas.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Permiso> ListPermisos()
        {
            try
            {
                return _permisos.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pantalla> ListPantallas()
        {
            try
            {
                return _pantallas.List().OrderBy(p => p.nombre).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FormasPago> ListFormasDePago()
        {
            try
            {
                return _formasDePago.List().OrderBy(m => m.descripcion).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TiposDeComprobante> ListTiposDeComprobante()
        {
            try
            {
                return _tiposDeComprobante.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> ListImpresoras()
        {
            try
            {
                return PrinterSettings.InstalledPrinters.Cast<string>().ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MetodosPago> ListMetodosDePago()
        {
            try
            {
                return _metodosDePago.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TiposDeAjuste> ListTiposDeAjuste()
        {
            try
            {
                return _tiposDeAjustes.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TiposFactor> ListTiposFactor()
        {
            try
            {
                return _tiposFactor.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pais> ListPaises()
        {
            try
            {
                return _paises.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TiposRelacion> ListTiposRelacion()
        {
            try
            {
                return _tiposRelacion.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Directorio> ListDirectorio()
        {
            try
            {
                return _directorio.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Seccione> ListSecciones()
        {
            try
            {
                return _secciones.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<object> ListTiposReporteRemision()
        {
            try
            {
                return Enum.GetValues(typeof(Tipos_Reporte_Remisiones)).Cast<Tipos_Reporte_Remisiones>().Select(x => (object)new { Value = x, Text = x.ToString().Replace("_", " ") }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TiposDeComision> ListComisiones()
        {
            try
            {
                return _comisiones.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Opciones_Pedido> ListOpcionesPedido()
        {
            try
            {
                return Enum.GetValues(typeof(Opciones_Pedido)).Cast<Opciones_Pedido>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Reporte> ListReportes()
        {
            try
            {
                return _reportes.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<object> ListOpcionesCostos()
        {
            try
            {
                return Enum.GetValues(typeof(Opciones_Costos)).Cast<Opciones_Costos>().Select(x => (object)new { Value = (int)x, Text = x.ToString().Replace("_", " ") }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
