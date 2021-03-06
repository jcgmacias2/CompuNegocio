using Aprovi.Business.Services;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Application.Helpers
{
    public class AproviContainer
    {
        #region Variables privadas

        private UnityContainer _container;

        #endregion

        #region Constructores

        /// <summary>
        /// Inicializa el container en base al codigo que se le pasa como parametro, cargando así las personalizaciones del cliente cuando existan
        /// </summary>
        /// <param name="code">Código de referencia del cliente</param>
        public AproviContainer(Customization code)
        {
            try
            {
                _container = new UnityContainer();

                //El registro de las clases default se debe hacer siempre
                Default();

                //Ahora en caso de que exista un metodo que construya instancias personalizadas para el código lo ejecuto
                LoadCustomization(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa el container con el registro de tipos base por defecto
        /// </summary>
        public AproviContainer()
        {
            try
            {
                _container = new UnityContainer();
                Default();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Inicializa el container utilizando la base de datos proporcionada
        /// </summary>
        /// <param name="connectionString"></param>
        public AproviContainer(string connectionString)
        {
            _container = new UnityContainer();
            Default(connectionString);
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que exponer el container para su utilización
        /// </summary>
        public UnityContainer Container { get { return _container; } }

        #endregion

        #region Metodos públicos

        public void LoadCustomization(Customization code)
        {
            try
            {
                //Ahora en caso de que exista un metodo que construya instancias personalizadas para el código lo ejecuto
                //Obtiene el metodo que corresponda con el código de Customization
                MethodInfo method = this.GetType().GetMethod(code.ToString());
                if (method.isValid())
                    method.Invoke(this, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Registros de tipos en el container

        /// <summary>
        /// Realiza el registro de los servicios base
        /// </summary>
        private void Default(string connectionString = null)
        {
            //MT: Core configurations
            //Permite crear un contenedor utilizando la base de datos especificada
            if (connectionString.isValid())
            {
                _container.RegisterType<CNEntities>(new PerThreadLifetimeManager(), new InjectionConstructor(connectionString));
            }
            else
            {
                _container.RegisterInstance<CNEntities>(new CNEntities());
            }
            _container.RegisterType<IUnitOfWork, UnitOfWork>(new PerThreadLifetimeManager());

            //MT: Register Services
            _container.RegisterType<IUnidadDeMedidaService, AproviUnidadDeMedidaService>(new PerThreadLifetimeManager());
            _container.RegisterType<IImpuestoService, AproviImpuestoService>(new PerThreadLifetimeManager());
            _container.RegisterType<ICatalogosEstaticosService, AproviCatalogosEstaticosService>(new PerThreadLifetimeManager());
            _container.RegisterType<IArticuloService, AproviArticuloService>(new PerThreadLifetimeManager());
            _container.RegisterType<IUsuarioService, AproviUsuarioService>(new PerResolveLifetimeManager());
            _container.RegisterType<IPrivilegioService, AproviPrivilegioService>(new PerResolveLifetimeManager());
            _container.RegisterType<IClienteService, AproviClienteService>(new PerResolveLifetimeManager());
            _container.RegisterType<IClasificacionService, AproviClasificacionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IListaDePrecioService, AproviListaDePrecioService>(new PerResolveLifetimeManager());
            _container.RegisterType<IFormaPagoService, AproviFormaPagoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEquivalenciaService, AproviEquivalenciaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IProveedorService, AproviProveedorService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICompraService, AproviCompraService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAbonoDeCompraService, AproviAbonoDeCompraService>(new PerResolveLifetimeManager());
            _container.RegisterType<IConfiguracionService, AproviConfiguracionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmpresaService, AproviEmpresaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IRegimenService, AproviRegimenService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISeguridadService, AproviSeguridadService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICertificadoService, AproviCertificadoService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISerieService, AproviSerieService>(new PerResolveLifetimeManager());
            _container.RegisterType<ILicenciaService, AproviLicenciaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEstacionService, AproviEstacionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IFacturaService, AproviFacturaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IComprobantFiscaleService, AproviComprobanteFiscalService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAbonoDeFacturaService, AproviAbonoDeFacturaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IRemisionService, AproviRemisionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAbonoDeRemisionService, AproviAbonoDeRemisionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAbonosService, AproviAbonosService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAjusteService, AproviAjusteService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISaldosPorProveedorService, AproviSaldosPorProveedorService>(new PerResolveLifetimeManager());
            _container.RegisterType<IDispositivoService, AproviDispositivoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IBasculaService, AproviBasculaService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISaldosPorClienteService, AproviSaldosPorClienteService>(new PerResolveLifetimeManager());
            _container.RegisterType<IBancoService, AproviBancoService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICuentaBancariaService, AproviCuentaBancariaService>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductoServicioService, AproviProductoServicioService>(new PerResolveLifetimeManager());
            _container.RegisterType<IUsosCFDIService, AproviUsosCFDIService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICuentaPredialService, AproviCuentaPredialService>(new PerResolveLifetimeManager());
            _container.RegisterType<IMigrationDataService, MigrationDataService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICuentaDeCorreoService, AproviCuentaDeCorreoService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICuentaGuardianService, AproviCuentaGuardianService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEnvioDeCorreoService, AproviEnvioDeCorreoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IComprobanteEnviadoService, AproviComprobanteEnviadoService>(new PerResolveLifetimeManager());
			_container.RegisterType<ICotizacionService, AproviCotizacionService>(new PerResolveLifetimeManager());
            _container.RegisterType<IPedimentoService, AproviPedimentoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IPedidoService, AproviPedidoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IOrdenDeCompraService, AproviOrdenDeCompraService>(new PerResolveLifetimeManager());
            _container.RegisterType<IPagoService, AproviPagoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmpresaAsociadaService, AproviEmpresaAsociadaService>(new PerResolveLifetimeManager());
            _container.RegisterType<ITraspasoService, AproviTraspasoService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISolicitudDeTraspasoService, AproviSolicitudDeTraspasoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IVentasPorArticuloService, AproviVentasPorArticuloService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICodigoDeArticuloPorProveedorService, AproviCodigoDeArticuloPorProveedorService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICodigoDeArticuloPorClienteService, AproviCodigoDeArticuloPorClienteService>(new PerResolveLifetimeManager());
            _container.RegisterType<INotaDeCreditoService, AproviNotaDeCreditoService>(new PerResolveLifetimeManager());
            _container.RegisterType<INotaDeDescuentoService, AproviNotaDeDescuentoService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEstadoDeLaEmpresaService, AproviEstadoDeLaEmpresaService>(new PerResolveLifetimeManager());
            _container.RegisterType<ICostoDeLoVendidoService, AproviCostoDeLoVendidoService>(new PerResolveLifetimeManager());
            //_container.RegisterType<IListasDePreciosService, AproviListasDePreciosService>(new PerResolveLifetimeManager());
        }

        /// <summary>
        /// Reemplaza los registros por las implementaciones personalizadas de MDK
        /// </summary>
        private void Mdk()
        {
            
        }

        /// <summary>
        /// Reemplaza los registros por las implementaciones personalizadas de Kowi
        /// </summary>
        private void Kowi()
        {

        }

        #endregion

    }
}
