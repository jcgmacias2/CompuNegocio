using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        //Model Repositories Interfaces

        IEmpresasRepository Empresas { get; }

        IUnidadesDeMedidaRepository UnidadesDeMedida { get; }

        IImpuestosRepository Impuestos { get; }

        ITiposDeImpuestoRepository TiposDeImpuesto { get; }

        IArticulosRepository Articulos { get; }

        IMonedasRepository Monedas { get; }

        IUsuariosRepository Usuarios { get; }

        IPrivilegiosRepository Privilegios { get; }

        IPermisosRepository Permisos { get; }

        IPantallasRepository Pantallas { get; }

        IClientesRepository Clientes { get; }

        IDomiciliosRepository Domicilios { get; }

        IClasificacionesRepository Clasificaciones { get; }

        IListasDePrecioRepository ListasDePrecio { get; }

        IPreciosRepository Precios { get; }

        IFormasPagoRepository FormasDePago { get; }

        IEquivalenciasRepository Equivalencias { get; }

        IProveedoresRepository Proveedores { get; }

        IComprasRepository Compras { get; }

        IAbonosDeCompraRepository AbonosDeCompra { get; }

        IMetodosPagoRepository MetodosDePago { get; }

        IConfiguracionesRepository Configuraciones { get; }

        IAplicacionesRepository Aplicaciones { get; }

        IBasculasRepository Basculas { get; }

        IViewExistenciasRepository Existencias { get; }

        IRegimenesRepository Regimenes { get; }

        ICertificadosRepository Certificados { get; }

        ISeriesRepository Series { get; }

        ITiposDeComprobanteRepository TiposDeComprobante { get; }

        IEstacionesRepository Estaciones { get; }

        IImpresorasRepository Impresoras { get; }

        IFacturasRepository Facturas { get; }

        IViewSeriesRepository Folios { get; }

        IAbonosDeFacturaRepository AbonosDeFactura { get; }

        IRemisionesRepository Remisiones { get; }

        IViewFoliosPorAbonosRepository FoliosPorAbono { get; }

        IAbonosDeRemisionRepository AbonosDeRemision { get; }

        IAjustesRepository Ajustes { get; }

        ITiposDeAjustesRepository TiposDeAjuste { get; }

        ITimbresDeAbonosDeFacturaRepository TimbresParcialidades { get; }

        IViewSaldosPorProveedorPorMonedaRepository SaldosPorProveedor { get; }

        IViewResumenPorCompraRepository ResumenDeCompras { get; }

        IViewEntradasPorComprasRepository EntradasPorCompras { get; }

        IViewSalidasPorComprasCanceladasRepository SalidasPorComprasCanceladas { get; }

        IViewEntradasPorAjustesRepository EntradasPorAjustes { get; }

        IViewSalidasPorAjustesCanceladosRepository SalidasPorAjustesCancelados { get; }

        IViewSalidasPorAjustesRepository SalidasPorAjustes { get; }

        IViewEntradasPorAjustesCanceladosRepository EntradasPorAjustesCancelados { get; }

        IViewSalidasPorFacturasRepository SalidasPorFacturas { get; }

        IViewEntradasPorFacturasCanceladasRepository EntradasPorFacturasCanceladas { get; }

        IViewSalidasPorRemisionesRepository SalidasPorRemisiones { get; }

        IViewEntradasPorRemisionesCanceladasRepository EntradasPorRemisionesCanceladas { get; }

        IViewSaldosPorClientePorMonedaRepository SaldosPorCliente { get; }

        IViewResumenPorFacturaRepository ResumenDeFacturas { get; }

        IViewResumenPorRemisionRepository ResumenDeRemisiones { get; }

        IBancosRepository Bancos { get; }

        ICuentasBancariasRepository CuentasBancarias { get; }

        ITiposFactorRepository TiposFactor { get; }

        IProductosServiciosRepository ProductosYServicios { get; }

        IPaisesRepository Paises { get; }

        IUsosCFDIRepository UsosCFDI { get; }

        ITiposRelacionRepository TiposRelacion { get; }

        ICuentasPredialesRepository CuentasPrediales { get; }

        IVFPDataExtractorRepository VFPDataExtractor { get; }

        ICuentasDeCorreoRepository CuentasDeCorreo { get; }

        ICuentasGuardianRepository CuentasGuardian { get; }

        IComprobantesEnviadosRepository ComprobantesEnviados { get; }

        IDirectorioRepository Directorio { get; }

        ISeccionesRepository Secciones { get; }

        ICotizacionesRepository Cotizaciones { get; }

        IViewPedimentosRepository PedimentosPorArticulo { get; }

        IPedimentosRepository Pedimentos { get; }

        IViewReporteInventarioFisicoRepository InventarioFisico { get; }

        IComisionesRepository Comisiones { get; }

        IPedidosRepository Pedidos { get; }

        IOpcionesCostosRepository OpcionesCostos { get; }

        IOrdenesDeCompraRepository OrdenesDeCompra { get; }

        IViewArticulosVendidosRepository ArticulosVendidos { get; }

        IViewVentasActivasPorClienteRepository VentasPorCliente { get; }

        IViewSaldoDeudorPorClientePorMonedaRepository SaldoDeudorPorClientePorMoneda { get; }

        IViewListaFacturasRepository ListaFacturas { get; }

        IReportesRepository Reportes { get; }

        IFormatosRepository Formatos { get; }

        IPagosRepository Pagos { get; }

        IViewListaParcialidadesRepository ListaParcialidades { get; }

        IViewCotizacionesPorPeriodoRepository CotizacionesPorPeriodo { get; }

        IViewReporteAntiguedadSaldosFacturasRepository AntiguedadSaldosFacturas { get; }

        IViewReporteAntiguedadSaldosRemisionesRepository AntiguedadSaldosRemisiones { get; }

        IEmpresasAsociadasRepository EmpresasAsociadas { get; }

        IViewListaPagosRepository ListaDePagos { get; }

        ITraspasosRepository Traspasos { get; }

        ISolicitudesDeTraspasoRepository SolicitudesDeTraspaso { get; }

        IViewSalidasPorTraspasosRepository SalidasPorTraspasos { get; }

        IViewEntradasPorTraspasosRepository EntradasPorTraspasos { get; }

        //IViewEntradasPorTraspasosRechazadosRepository EntradasPorTraspasosRechazados { get; }

        IViewReporteVentasPorArticuloRepository VentasPorArticulo { get; }

        ICodigosDeArticuloPorProveedorRepository CodigosDeArticuloPorProveedor { get; }

        ICodigosDeArticuloPorClienteRepository CodigosDeArticuloPorCliente { get; }

        IViewCodigosDeArticuloPorProveedorRepository CodigosDeArticulosPorProveedor { get; }

        IViewCodigosDeArticuloPorClienteRepository CodigosDeArticulosPorCliente { get; }

        INotasDeCreditoRepository NotasDeCredito { get; }

        INotasDeDescuentoRepository NotasDeDescuento { get; }

        IViewEntradasPorNotasDeCreditoRepository EntrasPorNotasDeCredito { get; }

        IViewSalidasPorNotasDeCreditoCanceladasRepository SalidasPorNotasDeCredito { get; }

        IViewReporteAvaluoRepository Avaluo { get; }

        IViewReporteEstatusDeLaEmpresaAjustesEntradaRepository EstatusDeLaEmpresaAjustesEntrada { get; }

        IViewReporteEstatusDeLaEmpresaAjustesSalidaRepository EstatusDeLaEmpresaAjustesSalida { get; }

        IViewReporteEstatusDeLaEmpresaAvaluoRepository EstatusDeLaEmpresaAvaluo { get; }

        IViewReporteEstatusDeLaEmpresaComprasRepository EstatusDeLaEmpresaCompras { get; }

        IViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository EstatusDeLaEmpresaCuentasPorCobrar { get; }

        IViewReporteEstatusDeLaEmpresaCuentasPorPagarRepository EstatusDeLaEmpresaCuentasPorPagar { get; }

        IViewReporteEstatusDeLaEmpresaFacturasRepository EstatusDeLaEmpresaFacturas { get; }

        IViewReporteEstatusDeLaEmpresaNotasDeCreditoRepository EstatusDeLaEmpresaNotasDeCredito { get; }

        IViewReporteEstatusDeLaEmpresaNotasDeDescuentoRepository EstatusDeLaEmpresaNotasDeDescuento { get; }

        IViewReporteEstatusDeLaEmpresaPedidosRepository EstatusDeLaEmpresaPedidos { get; }

        IViewReporteEstatusDeLaEmpresaRemisionesRepository EstatusDeLaEmpresaRemisiones { get; }

        IViewReporteCostoDeLoVendidoRepository CostoDeLoVendido { get; }

        IViewReporteListaDePreciosRepository ListaDePrecios { get; }

        IViewReporteListaDePreciosPorListaRepository ListaDePreciosPorLista { get; }

        IViewReporteListaDePreciosConImpuestosRepository ListaDePreciosConImpuestos { get; }

        IViewReporteListaDePreciosPorListaConImpuestosRepository ListaDePreciosPorListaConImpuestos { get; }

        IViewReporteNotasDeDescuentoRepository ReporteNotasDeDescuento { get; }

        /// <summary>
        /// Commits the changes to the database
        /// </summary>
        void Save();

        /// <summary>
        /// Rejectes all the non commited changes to the context
        /// </summary>
        void Reload();
    }
}
