using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Data.Entity;

namespace Aprovi.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(CNEntities context)
        {
            _context = context;
        }

        #region Private Instances
        
        private CNEntities _context;
        private IUnidadesDeMedidaRepository _unidadesDeMedida;
        private IImpuestosRepository _impuestos;
        private ITiposDeImpuestoRepository _tiposDeImpuesto;
        private IArticulosRepository _articulos;
        private IMonedasRepository _monedas;
        private IUsuariosRepository _usuarios;
        private IPrivilegiosRepository _privilegios;
        private IPermisosRepository _permisos;
        private IPantallasRepository _pantallas;
        private IClientesRepository _clientes;
        private IDomiciliosRepository _domicilios;
        private IClasificacionesRepository _clasificaciones;
        private IListasDePrecioRepository _listasDePrecio;
        private IPreciosRepository _precios;
        private IFormasPagoRepository _formasDePago;
        private IEquivalenciasRepository _equivalencias;
        private IProveedoresRepository _proveedores;
        private IComprasRepository _compras;
        private IAbonosDeCompraRepository _abonosDeCompra;
        private IMetodosPagoRepository _metodosDePago;
        private IConfiguracionesRepository _configuraciones;
        private IAplicacionesRepository _aplicaciones;
        private IBasculasRepository _basculas;
        private IViewExistenciasRepository _existencias;
        private IRegimenesRepository _regimenes;
        private ICertificadosRepository _certificados;
        private ISeriesRepository _series;
        private ITiposDeComprobanteRepository _comprobantes;
        private IEstacionesRepository _estaciones;
        private IImpresorasRepository _impresoras;
        private IFacturasRepository _facturas;
        private IViewSeriesRepository _folios;
        private IAbonosDeFacturaRepository _abonosDeFactura;
        private IRemisionesRepository _remisiones;
        private IViewFoliosPorAbonosRepository _foliosPorAbonos;
        private IAbonosDeRemisionRepository _abonosDeRemision;
        private IAjustesRepository _ajustes;
        private ITiposDeAjustesRepository _tiposDeAjustes;
        private ITimbresDeAbonosDeFacturaRepository _timbresParcialidades;
        private IViewSaldosPorProveedorPorMonedaRepository _saldosPorProveedor;
        private IViewResumenPorCompraRepository _resumenDeCompras;
        private IViewEntradasPorComprasRepository _entradasPorCompras;
        private IViewSalidasPorComprasCanceladasRepository _salidasPorComprasCanceladas;
        private IViewEntradasPorAjustesRepository _entradasPorAjustes;
        private IViewSalidasPorAjustesCanceladosRepository _salidasPorAjustesCancelados;
        private IViewSalidasPorAjustesRepository _salidasPorAjustes;
        private IViewEntradasPorAjustesCanceladosRepository _entradasPorAjustesCancelados;
        private IViewSalidasPorFacturasRepository _salidasPorFacturas;
        private IViewEntradasPorFacturasCanceladasRepository _entradasPorFacturasCanceladas;
        private IViewSalidasPorRemisionesRepository _salidasPorRemisiones;
        private IViewEntradasPorRemisionesCanceladasRepository _entradasPorRemisionesCanceladas;
        private IViewSaldosPorClientePorMonedaRepository _saldosPorCiente;
        private IViewResumenPorFacturaRepository _resumenDeFacturas;
        private IViewResumenPorRemisionRepository _resumenDeRemisiones;
        private IBancosRepository _bancos;
        private ICuentasBancariasRepository _cuentasBancarias;
        private ITiposFactorRepository _tiposFactor;
        private IProductosServiciosRepository _productosServicios;
        private IPaisesRepository _paises;
        private IUsosCFDIRepository _usosCFDI;
        private ITiposRelacionRepository _tiposRelacion;
        private ICuentasPredialesRepository _cuentasPrediales;
        private IEmpresasRepository _empresas;
        private IVFPDataExtractorRepository _vfpData;
        private ICuentasDeCorreoRepository _cuentasCorreo;
        private ICuentasGuardianRepository _cuentasGuardian;
        private IComprobantesEnviadosRepository _comprobantesEnviados;
        private IDirectorioRepository _directorio;
        private ISeccionesRepository _secciones;
        private ICotizacionesRepository _cotizaciones;
        private IViewPedimentosRepository _pedimentosPorArticulo;
        private IViewReporteInventarioFisicoRepository _inventarioFisico;
        private IComisionesRepository _comisiones;
        private IPedidosRepository _pedidos;
        private IPedimentosRepository _pedimentos;
        private IOpcionesCostosRepository _opcionesCostos;
        private IOrdenesDeCompraRepository _ordenesDeCompra;
        private IViewArticulosVendidosRepository _articulosVendidos;
        private IViewVentasActivasPorClienteRepository _ventasPorCliente;
        private IViewSaldoDeudorPorClientePorMonedaRepository _saldoDeudorPorClientePorMoneda;
        private IViewListaFacturasRepository _listaFacturas;
        private IReportesRepository _reportes;
        private IFormatosRepository _formatos;
        private IPagosRepository _pagos;
        private IViewListaParcialidadesRepository _listaParcialidades;
        private IViewCotizacionesPorPeriodoRepository _cotizacionesPorPeriodo;
        private IViewReporteAntiguedadSaldosFacturasRepository _antiguedadSaldosFacturas;
        private IViewReporteAntiguedadSaldosRemisionesRepository _antiguedadSaldosRemisiones;
        private IEmpresasAsociadasRepository _empresasAsociadas;
        private IViewListaPagosRepository _listaPagos;
        private ITraspasosRepository _traspasos;
        private ISolicitudesDeTraspasoRepository _solicitudesDeTraspaso;
        private IViewSalidasPorTraspasosRepository _salidasPorTraspasos;
        private IViewEntradasPorTraspasosRepository _entradasPorTraspaso;
        //private IViewEntradasPorTraspasosRechazadosRepository _entradasPorTraspasosRechazados;
        private IViewReporteVentasPorArticuloRepository _ventasPorArticulo;
        private ICodigosDeArticuloPorProveedorRepository _codigosDeArticuloPorProveedor;
        private ICodigosDeArticuloPorClienteRepository _codigosDeArticuloPorCliente;
        private IViewCodigosDeArticuloPorProveedorRepository _codigosDeArticulosPorProveedor;
        private IViewCodigosDeArticuloPorClienteRepository _codigosDeArticulosPorCliente;
        private INotasDeCreditoRepository _notasDeCredito;
        private INotasDeDescuentoRepository _notasDeDescuento;
        private IViewEntradasPorNotasDeCreditoRepository _entradasNotasDeCredito;
        private IViewSalidasPorNotasDeCreditoCanceladasRepository _salidasNotasDeCredito;
        private IViewReporteAvaluoRepository _avaluos;
        private IViewReporteEstatusDeLaEmpresaAjustesEntradaRepository _estatusDeLaEmpresaAjustesEntrada;
        private IViewReporteEstatusDeLaEmpresaAjustesSalidaRepository _estatusDeLaEmpresaAjustesSalida;
        private IViewReporteEstatusDeLaEmpresaAvaluoRepository _estatusDeLaEmpresaAvaluo;
        private IViewReporteEstatusDeLaEmpresaComprasRepository _estatusDeLaEmpresaCompras;
        private IViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository _estatusDeLaEmpresaCuentasPorCobrar;
        private IViewReporteEstatusDeLaEmpresaCuentasPorPagarRepository _estatusDeLaEmpresaCuentasPorPagar;
        private IViewReporteEstatusDeLaEmpresaFacturasRepository _estatusDeLaEmpresaFacturas;
        private IViewReporteEstatusDeLaEmpresaNotasDeCreditoRepository _estatusDeLaEmpresaNotasDeCredito;
        private IViewReporteEstatusDeLaEmpresaNotasDeDescuentoRepository _estatusDeLaEmpresaNotasDeDescuento;
        private IViewReporteEstatusDeLaEmpresaPedidosRepository _estatusDeLaEmpresaPedidos;
        private IViewReporteEstatusDeLaEmpresaRemisionesRepository _estatusDeLaEmpresaRemisiones;
        private IViewReporteCostoDeLoVendidoRepository _costoDeLoVendido;
        private IViewReporteListaDePreciosRepository _listaDePrecios;
        private IViewReporteListaDePreciosPorListaRepository _listaDePreciosPorLista;
        private IViewReporteListaDePreciosConImpuestosRepository _listaDePreciosConImpuestos;
        private IViewReporteListaDePreciosPorListaConImpuestosRepository _listaDePreciosPorListaConImpuestos;
        private IViewReporteNotasDeDescuentoRepository _reporteNotasDeDescuento;

        #endregion

        #region Properties / Repositories

        public IUnidadesDeMedidaRepository UnidadesDeMedida
        {
            get
            {
                if (_unidadesDeMedida == null)
                    _unidadesDeMedida = new UnidadesDeMedidaRepository(_context);
                return _unidadesDeMedida;
            }
        }

        public IImpuestosRepository Impuestos
        {
            get
            {
                if (_impuestos == null)
                    _impuestos = new ImpuestosRepository(_context);
                return _impuestos;
            }
        }

        public ITiposDeImpuestoRepository TiposDeImpuesto
        {
            get
            {
                if (_tiposDeImpuesto == null)
                    _tiposDeImpuesto = new TiposDeImpuestoRepository(_context);
                return _tiposDeImpuesto;
            }
        }

        public IArticulosRepository Articulos
        {
            get
            {
                if (_articulos == null)
                    _articulos = new ArticulosRepository(_context);
                return _articulos;
            }
        }

        public IMonedasRepository Monedas
        {
            get
            {
                if (_monedas == null)
                    _monedas = new MonedasRepository(_context);
                return _monedas;
            }
        }

        public IUsuariosRepository Usuarios
        {
            get
            {
                if (_usuarios == null)
                    _usuarios = new UsuariosRepository(_context);
                return _usuarios;
            }
        }

        public IPrivilegiosRepository Privilegios
        {
            get
            {
                if (_privilegios == null)
                    _privilegios = new PrivilegiosRepository(_context);
                return _privilegios;
            }
        }

        public IPermisosRepository Permisos
        {
            get
            {
                if (_permisos == null)
                    _permisos = new PermisosRepository(_context);
                return _permisos;
            }
        }

        public IPantallasRepository Pantallas
        {
            get
            {
                if (_pantallas == null)
                    _pantallas = new PantallasRepository(_context);
                return _pantallas;
            }
        }

        public IClientesRepository Clientes
        {
            get
            {
                if (_clientes == null)
                    _clientes = new ClientesRepository(_context);
                return _clientes;
            }
        }

        public IDomiciliosRepository Domicilios
        {
            get
            {
                if (_domicilios == null)
                    _domicilios = new DomiciliosRepository(_context);
                return _domicilios;
            }
        }

        public IClasificacionesRepository Clasificaciones
        {
            get
            {
                if (_clasificaciones == null)
                    _clasificaciones = new ClasificacionesRepository(_context);
                return _clasificaciones;
            }
        }

        public IListasDePrecioRepository ListasDePrecio
        {
            get
            {
                if (_listasDePrecio == null)
                    _listasDePrecio = new ListasDePrecioRepository(_context);
                return _listasDePrecio;
            }
        }

        public IPreciosRepository Precios
        {
            get
            {
                if (_precios == null)
                    _precios = new PreciosRepository(_context);
                return _precios;
            }
        }

        public IFormasPagoRepository FormasDePago
        {
            get
            {
                if (_formasDePago == null)
                    _formasDePago = new FormasPagoRepository(_context);
                return _formasDePago;
            }
        }

        public IEquivalenciasRepository Equivalencias
        {
            get
            {
                if (_equivalencias == null)
                    _equivalencias = new EquivalenciasRepository(_context);
                return _equivalencias;
            }
        }

        public IProveedoresRepository Proveedores
        {
            get
            {
                if (_proveedores == null)
                    _proveedores = new ProveedoresRepository(_context);
                return _proveedores;
            }
        }

        public IComprasRepository Compras
        {
            get
            {
                if (_compras == null)
                    _compras = new ComprasRepository(_context);
                return _compras;
            }
        }

        public IAbonosDeCompraRepository AbonosDeCompra
        {
            get
            {
                if (_abonosDeCompra == null)
                    _abonosDeCompra = new AbonosDeCompraRepository(_context);
                return _abonosDeCompra;
            }
        }

        public IMetodosPagoRepository MetodosDePago
        {
            get
            {
                if (_metodosDePago == null)
                    _metodosDePago = new MetodosDePagoRepository(_context);
                return _metodosDePago;
            }
        }

        public IConfiguracionesRepository Configuraciones
        {
            get
            {
                if (_configuraciones == null)
                    _configuraciones = new ConfiguracionesRepository(_context);
                return _configuraciones;
            }
        }

        public IAplicacionesRepository Aplicaciones
        {
            get
            {
                if (_aplicaciones == null)
                    _aplicaciones = new AplicacionesRepository();
                return _aplicaciones;
            }
        }

        public IBasculasRepository Basculas
        {
            get
            {
                if (_basculas == null)
                    _basculas = new BasculasRepository(_context);
                return _basculas;
            }
        }

        public IViewExistenciasRepository Existencias
        {
            get
            {
                if (_existencias == null)
                    _existencias = new ViewExistenciasRepository(_context);
                return _existencias;
            }
        }

        public IRegimenesRepository Regimenes
        {
            get
            {
                if (_regimenes == null)
                    _regimenes = new RegimenesRepository(_context);
                return _regimenes;
            }
        }

        public ICertificadosRepository Certificados
        {
            get
            {
                if (_certificados == null)
                    _certificados = new CertificadosRepository(_context);
                return _certificados;
            }
        }

        public ISeriesRepository Series
        {
            get
            {
                if (_series == null)
                    _series = new SeriesRepository(_context);
                return _series;
            }
        }

        public ITiposDeComprobanteRepository TiposDeComprobante
        {
            get
            {
                if (_comprobantes == null)
                    _comprobantes = new TiposDeComprobanteRepository(_context);
                return _comprobantes;
            }
        }

        public IEstacionesRepository Estaciones
        {
            get
            {
                if (_estaciones == null)
                    _estaciones = new EstacionesRepository(_context);
                return _estaciones;
            }
        }

        public IImpresorasRepository Impresoras
        {
            get
            {
                if (_impresoras == null)
                    _impresoras = new ImpresorasRepository(_context);
                return _impresoras;
            }
        }

        public IFacturasRepository Facturas
        {
            get
            {
                if (_facturas == null)
                    _facturas = new FacturasRepository(_context);
                return _facturas;
            }
        }

        public IViewSeriesRepository Folios
        {
            get
            {
                if (_folios == null)
                    _folios = new ViewSeriesRepository(_context);
                return _folios;
            }
        }

        public IAbonosDeFacturaRepository AbonosDeFactura
        {
            get
            {
                return _abonosDeFactura.isValid() ? _abonosDeFactura : _abonosDeFactura = new AbonosDeFacturaRepository(_context);
            }
        }

        public IRemisionesRepository Remisiones
        {
            get { return _remisiones.isValid() ? _remisiones : _remisiones = new RemisionesRepository(_context); }
        }

        public IViewFoliosPorAbonosRepository FoliosPorAbono
        {
            get { return _foliosPorAbonos.isValid() ? _foliosPorAbonos : _foliosPorAbonos = new ViewFoliosPorAbonos(_context); }
        }

        public IAbonosDeRemisionRepository AbonosDeRemision
        {
            get { return _abonosDeRemision.isValid() ? _abonosDeRemision : _abonosDeRemision = new AbonosDeRemisionRepository(_context); }
        }

        public IAjustesRepository Ajustes
        {
            get { return _ajustes.isValid() ? _ajustes : _ajustes = new AjustesRepository(_context); }
        }

        public ITiposDeAjustesRepository TiposDeAjuste
        {
            get { return _tiposDeAjustes.isValid() ? _tiposDeAjustes : _tiposDeAjustes = new TiposDeAjusteRepository(_context); }
        }

        public ITimbresDeAbonosDeFacturaRepository TimbresParcialidades
        {
            get { return _timbresParcialidades.isValid() ? _timbresParcialidades : _timbresParcialidades = new TimbresDeAbonosDeFacturasRepository(_context); }
        }

        public IViewSaldosPorProveedorPorMonedaRepository SaldosPorProveedor
        {
            get { return _saldosPorProveedor.isValid() ? _saldosPorProveedor : _saldosPorProveedor = new ViewSaldosPorProveedorPorMonedaRepository(_context); }
        }

        public IViewResumenPorCompraRepository ResumenDeCompras
        {
            get { return _resumenDeCompras.isValid() ? _resumenDeCompras : _resumenDeCompras = new ViewResumenPorCompraRepository(_context); }
        }

        public IViewEntradasPorComprasRepository EntradasPorCompras
        {
            get { return _entradasPorCompras.isValid() ? _entradasPorCompras : _entradasPorCompras = new ViewEntradasPorComprasRepository(_context); }
        }

        public IViewSalidasPorComprasCanceladasRepository SalidasPorComprasCanceladas
        {
            get { return _salidasPorComprasCanceladas.isValid() ? _salidasPorComprasCanceladas : _salidasPorComprasCanceladas = new ViewSalidasPorComprasCanceladasRepository(_context); }
        }
        
        public IViewEntradasPorAjustesRepository EntradasPorAjustes
        {
            get { return _entradasPorAjustes.isValid() ? _entradasPorAjustes : _entradasPorAjustes = new ViewEntradasPorAjustesRepository(_context); }
        }

        public IViewSalidasPorAjustesCanceladosRepository SalidasPorAjustesCancelados
        {
            get { return _salidasPorAjustesCancelados.isValid() ? _salidasPorAjustesCancelados : _salidasPorAjustesCancelados = new ViewSalidasPorAjustesCanceladosRepository(_context); }
        }

        public IViewSalidasPorAjustesRepository SalidasPorAjustes
        {
            get { return _salidasPorAjustes.isValid() ? _salidasPorAjustes : _salidasPorAjustes = new ViewSalidasPorAjustesRepository(_context); }
        }

        public IViewEntradasPorAjustesCanceladosRepository EntradasPorAjustesCancelados
        {
            get { return _entradasPorAjustesCancelados.isValid() ? _entradasPorAjustesCancelados : _entradasPorAjustesCancelados = new ViewEntradasPorAjustesCanceladosRepository(_context); }
        }

        public IViewSalidasPorFacturasRepository SalidasPorFacturas
        {
            get { return _salidasPorFacturas.isValid() ? _salidasPorFacturas : _salidasPorFacturas = new ViewSalidasPorFacturasRepository(_context); }
        }

        public IViewEntradasPorFacturasCanceladasRepository EntradasPorFacturasCanceladas
        {
            get { return _entradasPorFacturasCanceladas.isValid() ? _entradasPorFacturasCanceladas : _entradasPorFacturasCanceladas = new ViewEntradasPorFacturasCanceladasRepository(_context); }
        }

        public IViewSalidasPorRemisionesRepository SalidasPorRemisiones
        {
            get { return _salidasPorRemisiones.isValid() ? _salidasPorRemisiones : _salidasPorRemisiones = new ViewSalidasPorRemisionesRepository(_context); }
        }

        public IViewEntradasPorRemisionesCanceladasRepository EntradasPorRemisionesCanceladas
        {
            get { return _entradasPorRemisionesCanceladas.isValid() ? _entradasPorRemisionesCanceladas : _entradasPorRemisionesCanceladas = new ViewEntradasPorRemisionesCanceladasRepository(_context); }
        }

        public IViewSaldosPorClientePorMonedaRepository SaldosPorCliente
        {
            get { return _saldosPorCiente.isValid() ? _saldosPorCiente : _saldosPorCiente = new ViewSaldosPorClientePorMonedaRepository(_context); }
        }

        public IViewResumenPorFacturaRepository ResumenDeFacturas
        {
            get { return _resumenDeFacturas.isValid() ? _resumenDeFacturas : _resumenDeFacturas = new ViewResumenPorFacturaRepository(_context); }
        }

        public IViewResumenPorRemisionRepository ResumenDeRemisiones
        {
            get { return _resumenDeRemisiones.isValid() ? _resumenDeRemisiones : _resumenDeRemisiones = new ViewResumenPorRemisionRepository(_context); }
        }

        public IBancosRepository Bancos
        {
            get { return _bancos.isValid() ? _bancos : _bancos = new BancosRepository(_context); }
        }

        public ICuentasBancariasRepository CuentasBancarias
        {
            get { return _cuentasBancarias.isValid() ? _cuentasBancarias : _cuentasBancarias = new CuentasBancariasRepository(_context); }
        }

        public ITiposFactorRepository TiposFactor => _tiposFactor.isValid() ? _tiposFactor : _tiposFactor = new TiposFactorRepository(_context);

        public IProductosServiciosRepository ProductosYServicios => _productosServicios.isValid() ? _productosServicios : _productosServicios = new ProductosServiciosRepository(_context);

        public IPaisesRepository Paises => _paises.isValid() ? _paises : _paises = new PaisesRepository(_context);

        public IUsosCFDIRepository UsosCFDI => _usosCFDI.isValid() ? _usosCFDI : _usosCFDI = new UsosCFDIRepository(_context);

        public ITiposRelacionRepository TiposRelacion => _tiposRelacion.isValid() ? _tiposRelacion : _tiposRelacion = new TiposRelacionRepository(_context);

        public ICuentasPredialesRepository CuentasPrediales => _cuentasPrediales.isValid() ? _cuentasPrediales : _cuentasPrediales = new CuentasPredialesRepository(_context);

        public IEmpresasRepository Empresas => _empresas.isValid() ? _empresas : _empresas = new EmpresasRepository(_context);

        public IVFPDataExtractorRepository VFPDataExtractor => _vfpData.isValid() ? _vfpData : _vfpData = new VFPDataExtractorRepository();

        public ICuentasDeCorreoRepository CuentasDeCorreo => _cuentasCorreo.isValid() ? _cuentasCorreo : _cuentasCorreo = new CuentasDeCorreoRepository(_context);

        public ICuentasGuardianRepository CuentasGuardian => _cuentasGuardian.isValid() ? _cuentasGuardian : _cuentasGuardian = new CuentasGuardianRepository(_context);

        public IComprobantesEnviadosRepository ComprobantesEnviados => _comprobantesEnviados.isValid() ? _comprobantesEnviados : _comprobantesEnviados = new ComprobantesEnviadosRepository(_context);

        public IDirectorioRepository Directorio => _directorio.isValid() ? _directorio : _directorio = new DirectorioRepository(_context);
        
        public ISeccionesRepository Secciones => _secciones.isValid() ? _secciones : _secciones = new SeccionesRepository(_context);

        public ICotizacionesRepository Cotizaciones => _cotizaciones.isValid() ? _cotizaciones : _cotizaciones = new CotizacionesRepository(_context);

        public IViewPedimentosRepository PedimentosPorArticulo => _pedimentosPorArticulo.isValid() ? _pedimentosPorArticulo : _pedimentosPorArticulo = new ViewPedimentosRepository(_context);

        public IViewReporteInventarioFisicoRepository InventarioFisico => _inventarioFisico.isValid() ? _inventarioFisico : _inventarioFisico = new ViewReporteInventarioFisicoRepository(_context);

        public IComisionesRepository Comisiones => _comisiones.isValid() ? _comisiones : _comisiones = new ComisionesRepository(_context);

        public IPedidosRepository Pedidos => _pedidos.isValid() ? _pedidos : _pedidos = new PedidosRepository(_context);

        public IPedimentosRepository Pedimentos => _pedimentos.isValid() ? _pedimentos : _pedimentos = new PedimentosRepository(_context);

        public IOpcionesCostosRepository OpcionesCostos => _opcionesCostos.isValid() ? _opcionesCostos : _opcionesCostos = new OpcionesCostosRepository(_context);
        
        public IOrdenesDeCompraRepository OrdenesDeCompra => _ordenesDeCompra.isValid() ? _ordenesDeCompra : _ordenesDeCompra = new OrdenesDeCompraRepository(_context);

        public IViewArticulosVendidosRepository ArticulosVendidos => _articulosVendidos.isValid() ? _articulosVendidos : _articulosVendidos = new ViewArticulosVendidosRepository(_context);

        public IViewVentasActivasPorClienteRepository VentasPorCliente => _ventasPorCliente.isValid() ? _ventasPorCliente : _ventasPorCliente = new ViewVentasActivasPorClienteRepository(_context);
        
        public IViewSaldoDeudorPorClientePorMonedaRepository SaldoDeudorPorClientePorMoneda => _saldoDeudorPorClientePorMoneda.isValid() ? _saldoDeudorPorClientePorMoneda : _saldoDeudorPorClientePorMoneda = new ViewSaldoDeudorPorClientePorMonedaRepository(_context);
        
        public IViewListaFacturasRepository ListaFacturas => _listaFacturas.isValid() ? _listaFacturas : _listaFacturas = new ViewListaFacturasRepository(_context);

        public IReportesRepository Reportes => _reportes.isValid() ? _reportes : _reportes = new ReportesRepository(_context);

        public IFormatosRepository Formatos => _formatos.isValid() ? _formatos : _formatos = new FormatosRepository(_context);

        public IPagosRepository Pagos => _pagos.isValid() ? _pagos : _pagos = new PagosRepository(_context);

        public IViewListaParcialidadesRepository ListaParcialidades => _listaParcialidades.isValid() ? _listaParcialidades : _listaParcialidades = new ViewListaParcialidadesRepository(_context);

        public IViewCotizacionesPorPeriodoRepository CotizacionesPorPeriodo => _cotizacionesPorPeriodo.isValid() ? _cotizacionesPorPeriodo : _cotizacionesPorPeriodo = new ViewCotizacionesPorPeriodoRepository(_context);

        public IViewReporteAntiguedadSaldosFacturasRepository AntiguedadSaldosFacturas => _antiguedadSaldosFacturas.isValid() ? _antiguedadSaldosFacturas : _antiguedadSaldosFacturas = new ViewReporteAntiguedadSaldosFacturas(_context);

        public IViewReporteAntiguedadSaldosRemisionesRepository AntiguedadSaldosRemisiones => _antiguedadSaldosRemisiones.isValid() ? _antiguedadSaldosRemisiones : _antiguedadSaldosRemisiones = new ViewReporteAntiguedadSaldosRemisionesRepository(_context);
        
        public IEmpresasAsociadasRepository EmpresasAsociadas => _empresasAsociadas.isValid() ? _empresasAsociadas : _empresasAsociadas = new EmpresasAsociadasRepository(_context);

        public IViewListaPagosRepository ListaDePagos => _listaPagos.isValid() ? _listaPagos : _listaPagos = new ViewListaPagosRepository(_context);

        public ITraspasosRepository Traspasos => _traspasos.isValid() ? _traspasos : _traspasos = new TraspasosRepository(_context);

        public ISolicitudesDeTraspasoRepository SolicitudesDeTraspaso => _solicitudesDeTraspaso.isValid() ? _solicitudesDeTraspaso : _solicitudesDeTraspaso = new SolicitudesDeTraspasoRepository(_context);

        public IViewSalidasPorTraspasosRepository SalidasPorTraspasos => _salidasPorTraspasos.isValid() ? _salidasPorTraspasos : _salidasPorTraspasos = new ViewSalidasPorTraspasosRepository(_context);

        public IViewEntradasPorTraspasosRepository EntradasPorTraspasos => _entradasPorTraspaso.isValid() ? _entradasPorTraspaso : _entradasPorTraspaso = new ViewEntradasPorTraspasosRepository(_context);

        //public IViewEntradasPorTraspasosRechazadosRepository EntradasPorTraspasosRechazados => _entradasPorTraspasosRechazados.isValid() ? _entradasPorTraspasosRechazados : _entradasPorTraspasosRechazados = new ViewEntradasPorTraspasosRechazadosRepository(_context);

        public IViewReporteVentasPorArticuloRepository VentasPorArticulo => _ventasPorArticulo.isValid() ? _ventasPorArticulo : _ventasPorArticulo = new ViewReporteVentasPorArticuloRepository(_context);

        public ICodigosDeArticuloPorProveedorRepository CodigosDeArticuloPorProveedor => _codigosDeArticuloPorProveedor.isValid() ? _codigosDeArticuloPorProveedor : _codigosDeArticuloPorProveedor = new CodigosDeArticuloPorProveedorRepository(_context);

        public ICodigosDeArticuloPorClienteRepository CodigosDeArticuloPorCliente => _codigosDeArticuloPorCliente.isValid() ? _codigosDeArticuloPorCliente : _codigosDeArticuloPorCliente = new CodigosDeArticuloPorClienteRepository(_context);

        public IViewCodigosDeArticuloPorProveedorRepository CodigosDeArticulosPorProveedor => _codigosDeArticulosPorProveedor.isValid() ? _codigosDeArticulosPorProveedor : _codigosDeArticulosPorProveedor = new ViewCodigosDeArticuloPorProveedorRepository(_context);

        public IViewCodigosDeArticuloPorClienteRepository CodigosDeArticulosPorCliente => _codigosDeArticulosPorCliente.isValid() ? _codigosDeArticulosPorCliente : _codigosDeArticulosPorCliente = new ViewCodigosDeArticuloPorClienteRepository(_context);

        public INotasDeCreditoRepository NotasDeCredito => _notasDeCredito.isValid() ? _notasDeCredito : _notasDeCredito = new NotasDeCreditoRepository(_context);

        public INotasDeDescuentoRepository NotasDeDescuento => _notasDeDescuento.isValid() ? _notasDeDescuento : _notasDeDescuento = new NotasDeDescuentoRepository(_context);

        public IViewEntradasPorNotasDeCreditoRepository EntrasPorNotasDeCredito => _entradasNotasDeCredito.isValid() ? _entradasNotasDeCredito : _entradasNotasDeCredito = new ViewEntradasPorNotasDeCreditoRepository(_context);

        public IViewSalidasPorNotasDeCreditoCanceladasRepository SalidasPorNotasDeCredito => _salidasNotasDeCredito.isValid() ? _salidasNotasDeCredito : _salidasNotasDeCredito = new ViewSalidasPorNotasDeCreditoCanceladasRepository(_context);

        public IViewReporteAvaluoRepository Avaluo => _avaluos.isValid() ? _avaluos : _avaluos = new ViewReporteAvaluoRepository(_context);

        public IViewReporteEstatusDeLaEmpresaAjustesEntradaRepository EstatusDeLaEmpresaAjustesEntrada => _estatusDeLaEmpresaAjustesEntrada.isValid() ? _estatusDeLaEmpresaAjustesEntrada : _estatusDeLaEmpresaAjustesEntrada = new ViewReporteEstatusDeLaEmpresaAjustesEntradaRepository(_context);

        public IViewReporteEstatusDeLaEmpresaAjustesSalidaRepository EstatusDeLaEmpresaAjustesSalida => _estatusDeLaEmpresaAjustesSalida.isValid() ? _estatusDeLaEmpresaAjustesSalida : _estatusDeLaEmpresaAjustesSalida = new ViewReporteEstatusDeLaEmpresaAjustesSalidaRepository(_context);

        public IViewReporteEstatusDeLaEmpresaAvaluoRepository EstatusDeLaEmpresaAvaluo => _estatusDeLaEmpresaAvaluo.isValid() ? _estatusDeLaEmpresaAvaluo : _estatusDeLaEmpresaAvaluo = new ViewReporteEstatusDeLaEmpresaAjustesAvaluoRepository(_context);

        public IViewReporteEstatusDeLaEmpresaComprasRepository EstatusDeLaEmpresaCompras => _estatusDeLaEmpresaCompras.isValid() ? _estatusDeLaEmpresaCompras : _estatusDeLaEmpresaCompras = new ViewReporteEstatusDeLaEmpresaComprasRepository(_context);

        public IViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository EstatusDeLaEmpresaCuentasPorCobrar => _estatusDeLaEmpresaCuentasPorCobrar.isValid() ? _estatusDeLaEmpresaCuentasPorCobrar : _estatusDeLaEmpresaCuentasPorCobrar = new ViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository(_context);

        public IViewReporteEstatusDeLaEmpresaCuentasPorPagarRepository EstatusDeLaEmpresaCuentasPorPagar => _estatusDeLaEmpresaCuentasPorPagar.isValid() ? _estatusDeLaEmpresaCuentasPorPagar : _estatusDeLaEmpresaCuentasPorPagar = new ViewReporteEstatusDeLaEmpresaCuentasPorPagarRepository(_context);

        public IViewReporteEstatusDeLaEmpresaFacturasRepository EstatusDeLaEmpresaFacturas => _estatusDeLaEmpresaFacturas.isValid() ? _estatusDeLaEmpresaFacturas : _estatusDeLaEmpresaFacturas = new ViewReporteEstatusDeLaEmpresaFacturasRepository(_context);

        public IViewReporteEstatusDeLaEmpresaNotasDeCreditoRepository EstatusDeLaEmpresaNotasDeCredito => _estatusDeLaEmpresaNotasDeCredito.isValid() ? _estatusDeLaEmpresaNotasDeCredito : _estatusDeLaEmpresaNotasDeCredito = new ViewReporteEstatusDeLaEmpresaNotasDeCreditoRepository(_context);

        public IViewReporteEstatusDeLaEmpresaNotasDeDescuentoRepository EstatusDeLaEmpresaNotasDeDescuento => _estatusDeLaEmpresaNotasDeDescuento.isValid() ? _estatusDeLaEmpresaNotasDeDescuento : _estatusDeLaEmpresaNotasDeDescuento = new ViewReporteEstatusDeLaEmpresaNotasDeDescuentoRepository(_context);

        public IViewReporteEstatusDeLaEmpresaPedidosRepository EstatusDeLaEmpresaPedidos => _estatusDeLaEmpresaPedidos.isValid() ? _estatusDeLaEmpresaPedidos : _estatusDeLaEmpresaPedidos = new ViewReporteEstatusDeLaEmpresaPedidosRepository(_context);
        
        public IViewReporteEstatusDeLaEmpresaRemisionesRepository EstatusDeLaEmpresaRemisiones => _estatusDeLaEmpresaRemisiones.isValid() ? _estatusDeLaEmpresaRemisiones : _estatusDeLaEmpresaRemisiones = new ViewReporteEstatusDeLaEmpresaRemisionesRepository(_context);

        public IViewReporteCostoDeLoVendidoRepository CostoDeLoVendido => _costoDeLoVendido.isValid() ? _costoDeLoVendido : _costoDeLoVendido = new ViewReporteCostoDeLoVendidoRepository(_context);

        public IViewReporteListaDePreciosRepository ListaDePrecios => _listaDePrecios.isValid() ? _listaDePrecios : _listaDePrecios = new ViewReporteListaDePreciosRepository(_context);

        public IViewReporteListaDePreciosConImpuestosRepository ListaDePreciosConImpuestos => _listaDePreciosConImpuestos.isValid() ? _listaDePreciosConImpuestos : _listaDePreciosConImpuestos = new ViewReporteListaDePreciosConImpuestosRepository(_context);

        public IViewReporteListaDePreciosPorListaRepository ListaDePreciosPorLista => _listaDePreciosPorLista.isValid() ? _listaDePreciosPorLista : _listaDePreciosPorLista = new ViewReporteListaDePreciosPorListaRepository(_context);

        public IViewReporteListaDePreciosPorListaConImpuestosRepository ListaDePreciosPorListaConImpuestos => _listaDePreciosPorListaConImpuestos.isValid() ? _listaDePreciosPorListaConImpuestos : _listaDePreciosPorListaConImpuestos = new ViewReporteListaDePreciosPorListaConImpuestosRepository(_context);
        
        public IViewReporteNotasDeDescuentoRepository ReporteNotasDeDescuento => _reporteNotasDeDescuento.isValid() ? _reporteNotasDeDescuento : _reporteNotasDeDescuento = new ViewReporteNotasDeDescuentoRepository(_context);
        #endregion

        #region Generic Methods

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public void Reload()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        {
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Deleted:
                        {
                            entry.State = EntityState.Unchanged;
                            break;
                        }
                    case EntityState.Added:
                        {
                            entry.State = EntityState.Detached;
                            break;
                        }
                }
            }
        }

        #endregion

    }
}
