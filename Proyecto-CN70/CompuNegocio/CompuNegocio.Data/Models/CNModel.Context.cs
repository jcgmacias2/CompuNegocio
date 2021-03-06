//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aprovi.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CNEntities : DbContext
    {
        public CNEntities()
            : base("name=CNEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AbonosDeCompra> AbonosDeCompras { get; set; }
        public virtual DbSet<AbonosDeFactura> AbonosDeFacturas { get; set; }
        public virtual DbSet<AbonosDeRemision> AbonosDeRemisions { get; set; }
        public virtual DbSet<AddendaDeCliente> AddendaDeClientes { get; set; }
        public virtual DbSet<Addenda> Addendas { get; set; }
        public virtual DbSet<Ajuste> Ajustes { get; set; }
        public virtual DbSet<Articulo> Articulos { get; set; }
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<Bascula> Basculas { get; set; }
        public virtual DbSet<CambioDivisa> CambioDivisas { get; set; }
        public virtual DbSet<CancelacionesDeAjuste> CancelacionesDeAjustes { get; set; }
        public virtual DbSet<CancelacionesDeCompra> CancelacionesDeCompras { get; set; }
        public virtual DbSet<CancelacionesDeCotizacione> CancelacionesDeCotizaciones { get; set; }
        public virtual DbSet<CancelacionesDeFactura> CancelacionesDeFacturas { get; set; }
        public virtual DbSet<CancelacionesDeNotaDeCredito> CancelacionesDeNotaDeCreditoes { get; set; }
        public virtual DbSet<CancelacionesDeNotaDeDescuento> CancelacionesDeNotaDeDescuentoes { get; set; }
        public virtual DbSet<CancelacionesDeOrdenesDeCompra> CancelacionesDeOrdenesDeCompras { get; set; }
        public virtual DbSet<CancelacionesDePago> CancelacionesDePagos { get; set; }
        public virtual DbSet<CancelacionesDePedido> CancelacionesDePedidos { get; set; }
        public virtual DbSet<CancelacionesDeRemisione> CancelacionesDeRemisiones { get; set; }
        public virtual DbSet<CancelacionesDeTimbreDeAbonosDeFactura> CancelacionesDeTimbreDeAbonosDeFacturas { get; set; }
        public virtual DbSet<CancelacionesDeTimbresDeFactura> CancelacionesDeTimbresDeFacturas { get; set; }
        public virtual DbSet<CancelacionesDeTimbresDeNotasDeCredito> CancelacionesDeTimbresDeNotasDeCreditoes { get; set; }
        public virtual DbSet<CancelacionesDeTimbresDePago> CancelacionesDeTimbresDePagos { get; set; }
        public virtual DbSet<Certificado> Certificados { get; set; }
        public virtual DbSet<Clasificacione> Clasificaciones { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<CodigosDeArticuloPorCliente> CodigosDeArticuloPorClientes { get; set; }
        public virtual DbSet<CodigosDeArticuloPorProveedor> CodigosDeArticuloPorProveedors { get; set; }
        public virtual DbSet<ComentariosPorDetalleDeCotizacion> ComentariosPorDetalleDeCotizacions { get; set; }
        public virtual DbSet<ComentariosPorDetalleDeFactura> ComentariosPorDetalleDeFacturas { get; set; }
        public virtual DbSet<ComentariosPorDetalleDeOrdenDeCompra> ComentariosPorDetalleDeOrdenDeCompras { get; set; }
        public virtual DbSet<ComentariosPorDetalleDePedido> ComentariosPorDetalleDePedidoes { get; set; }
        public virtual DbSet<ComentariosPorDetalleDeRemision> ComentariosPorDetalleDeRemisions { get; set; }
        public virtual DbSet<ComisionesPorUsuario> ComisionesPorUsuarios { get; set; }
        public virtual DbSet<Compra> Compras { get; set; }
        public virtual DbSet<ComprobantesEnviado> ComprobantesEnviados { get; set; }
        public virtual DbSet<Configuracione> Configuraciones { get; set; }
        public virtual DbSet<Cotizacione> Cotizaciones { get; set; }
        public virtual DbSet<CuentaPredialPorDetalle> CuentaPredialPorDetalles { get; set; }
        public virtual DbSet<CuentasBancaria> CuentasBancarias { get; set; }
        public virtual DbSet<CuentasDeCorreo> CuentasDeCorreos { get; set; }
        public virtual DbSet<CuentasGuardian> CuentasGuardians { get; set; }
        public virtual DbSet<CuentasPrediale> CuentasPrediales { get; set; }
        public virtual DbSet<DatosExtraPorCompra> DatosExtraPorCompras { get; set; }
        public virtual DbSet<DatosExtraPorCotizacion> DatosExtraPorCotizacions { get; set; }
        public virtual DbSet<DatosExtraPorFactura> DatosExtraPorFacturas { get; set; }
        public virtual DbSet<DatosExtraPorNotaDeCredito> DatosExtraPorNotaDeCreditoes { get; set; }
        public virtual DbSet<DatosExtraPorOrdenDeCompra> DatosExtraPorOrdenDeCompras { get; set; }
        public virtual DbSet<DatosExtraPorPedido> DatosExtraPorPedidoes { get; set; }
        public virtual DbSet<DatosExtraPorRemision> DatosExtraPorRemisions { get; set; }
        public virtual DbSet<DatosExtraPorTraspaso> DatosExtraPorTraspasoes { get; set; }
        public virtual DbSet<DetallesDeAjuste> DetallesDeAjustes { get; set; }
        public virtual DbSet<DetallesDeCompra> DetallesDeCompras { get; set; }
        public virtual DbSet<DetallesDeCotizacion> DetallesDeCotizacions { get; set; }
        public virtual DbSet<DetallesDeFactura> DetallesDeFacturas { get; set; }
        public virtual DbSet<DetallesDeNotaDeCredito> DetallesDeNotaDeCreditoes { get; set; }
        public virtual DbSet<DetallesDeOrdenDeCompra> DetallesDeOrdenDeCompras { get; set; }
        public virtual DbSet<DetallesDePedido> DetallesDePedidoes { get; set; }
        public virtual DbSet<DetallesDeRemision> DetallesDeRemisions { get; set; }
        public virtual DbSet<DetallesDeTraspaso> DetallesDeTraspasoes { get; set; }
        public virtual DbSet<Directorio> Directorios { get; set; }
        public virtual DbSet<Domicilio> Domicilios { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<EmpresasAsociada> EmpresasAsociadas { get; set; }
        public virtual DbSet<Equivalencia> Equivalencias { get; set; }
        public virtual DbSet<Estacione> Estaciones { get; set; }
        public virtual DbSet<EstatusCrediticio> EstatusCrediticios { get; set; }
        public virtual DbSet<EstatusDeAbono> EstatusDeAbonoes { get; set; }
        public virtual DbSet<EstatusDeAjuste> EstatusDeAjustes { get; set; }
        public virtual DbSet<EstatusDeCompra> EstatusDeCompras { get; set; }
        public virtual DbSet<EstatusDeCotizacion> EstatusDeCotizacions { get; set; }
        public virtual DbSet<EstatusDeFactura> EstatusDeFacturas { get; set; }
        public virtual DbSet<EstatusDeNotaDeCredito> EstatusDeNotaDeCreditoes { get; set; }
        public virtual DbSet<EstatusDeNotaDeDescuento> EstatusDeNotaDeDescuentoes { get; set; }
        public virtual DbSet<EstatusDeOrdenDeCompra> EstatusDeOrdenDeCompras { get; set; }
        public virtual DbSet<EstatusDePago> EstatusDePagos { get; set; }
        public virtual DbSet<EstatusDePedido> EstatusDePedidoes { get; set; }
        public virtual DbSet<EstatusDeRemision> EstatusDeRemisions { get; set; }
        public virtual DbSet<EstatusDeTraspaso> EstatusDeTraspasoes { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<FormasPago> FormasPagoes { get; set; }
        public virtual DbSet<Formato> Formatos { get; set; }
        public virtual DbSet<FormatosPorConfiguracion> FormatosPorConfiguracions { get; set; }
        public virtual DbSet<ImpresorasPorEstacion> ImpresorasPorEstacions { get; set; }
        public virtual DbSet<Impuesto> Impuestos { get; set; }
        public virtual DbSet<ListasDePrecio> ListasDePrecios { get; set; }
        public virtual DbSet<MetodosPago> MetodosPagoes { get; set; }
        public virtual DbSet<Moneda> Monedas { get; set; }
        public virtual DbSet<NotasDeCredito> NotasDeCreditoes { get; set; }
        public virtual DbSet<NotasDeDescuento> NotasDeDescuentoes { get; set; }
        public virtual DbSet<OpcionesCosto> OpcionesCostos { get; set; }
        public virtual DbSet<OrdenesDeCompra> OrdenesDeCompras { get; set; }
        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<Pais> Paises { get; set; }
        public virtual DbSet<Pantalla> Pantallas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<PedimentoPorDetalleDeAjuste> PedimentoPorDetalleDeAjustes { get; set; }
        public virtual DbSet<PedimentoPorDetalleDeFactura> PedimentoPorDetalleDeFacturas { get; set; }
        public virtual DbSet<PedimentoPorDetalleDeNotaDeCredito> PedimentoPorDetalleDeNotaDeCreditoes { get; set; }
        public virtual DbSet<PedimentoPorDetalleDeRemision> PedimentoPorDetalleDeRemisions { get; set; }
        public virtual DbSet<PedimentoPorDetalleDeTraspaso> PedimentoPorDetalleDeTraspasoes { get; set; }
        public virtual DbSet<Pedimento> Pedimentos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Precio> Precios { get; set; }
        public virtual DbSet<Privilegio> Privilegios { get; set; }
        public virtual DbSet<ProductosServicio> ProductosServicios { get; set; }
        public virtual DbSet<Proveedore> Proveedores { get; set; }
        public virtual DbSet<Regimene> Regimenes { get; set; }
        public virtual DbSet<Remisione> Remisiones { get; set; }
        public virtual DbSet<Reporte> Reportes { get; set; }
        public virtual DbSet<Seccione> Secciones { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<SolicitudesDeTraspaso> SolicitudesDeTraspasoes { get; set; }
        public virtual DbSet<TimbresDeAbonosDeFactura> TimbresDeAbonosDeFacturas { get; set; }
        public virtual DbSet<TimbresDeFactura> TimbresDeFacturas { get; set; }
        public virtual DbSet<TimbresDeNotasDeCredito> TimbresDeNotasDeCreditoes { get; set; }
        public virtual DbSet<TimbresDePago> TimbresDePagos { get; set; }
        public virtual DbSet<TiposDeAjuste> TiposDeAjustes { get; set; }
        public virtual DbSet<TiposDeComision> TiposDeComisions { get; set; }
        public virtual DbSet<TiposDeComprobante> TiposDeComprobantes { get; set; }
        public virtual DbSet<TiposDeComprobanteFiscal> TiposDeComprobanteFiscals { get; set; }
        public virtual DbSet<TiposDeImpresora> TiposDeImpresoras { get; set; }
        public virtual DbSet<TiposDeImpuesto> TiposDeImpuestoes { get; set; }
        public virtual DbSet<TiposFactor> TiposFactors { get; set; }
        public virtual DbSet<TiposRelacion> TiposRelacions { get; set; }
        public virtual DbSet<Traspaso> Traspasos { get; set; }
        public virtual DbSet<UnidadesDeMedida> UnidadesDeMedidas { get; set; }
        public virtual DbSet<UsosCFDI> UsosCFDIs { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<VwArticulosOrdenado> VwArticulosOrdenados { get; set; }
        public virtual DbSet<VwArticulosVendido> VwArticulosVendidos { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorCompra> VwBaseGravableDeImpuestosPorCompras { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorDetalleDeCompra> VwBaseGravableDeImpuestosPorDetalleDeCompras { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorDetalleDeFactura> VwBaseGravableDeImpuestosPorDetalleDeFacturas { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorDetalleDeNotaDeCredito> VwBaseGravableDeImpuestosPorDetalleDeNotaDeCreditoes { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorDetalleDePedido> VwBaseGravableDeImpuestosPorDetalleDePedidoes { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorDetalleDeRemision> VwBaseGravableDeImpuestosPorDetalleDeRemisions { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorFactura> VwBaseGravableDeImpuestosPorFacturas { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorNotasDeCredito> VwBaseGravableDeImpuestosPorNotasDeCreditoes { get; set; }
        public virtual DbSet<VwBaseGravableDeImpuestosPorRemisione> VwBaseGravableDeImpuestosPorRemisiones { get; set; }
        public virtual DbSet<VwClientePorMoneda> VwClientePorMonedas { get; set; }
        public virtual DbSet<VwCodigosDeArticuloPorCliente> VwCodigosDeArticuloPorClientes { get; set; }
        public virtual DbSet<VwCodigosDeArticuloPorProveedor> VwCodigosDeArticuloPorProveedors { get; set; }
        public virtual DbSet<VwComprasPorArticulo> VwComprasPorArticuloes { get; set; }
        public virtual DbSet<VwComprobantesCancelado> VwComprobantesCancelados { get; set; }
        public virtual DbSet<VwCotizacionesPorPeriodo> VwCotizacionesPorPeriodoes { get; set; }
        public virtual DbSet<VwDetallesDePedidosFacturado> VwDetallesDePedidosFacturados { get; set; }
        public virtual DbSet<VwDetallesDePedidosPorSurtir> VwDetallesDePedidosPorSurtirs { get; set; }
        public virtual DbSet<VwDetallesDePedidosRemisionado> VwDetallesDePedidosRemisionados { get; set; }
        public virtual DbSet<VwEntradasConPedimento> VwEntradasConPedimentos { get; set; }
        public virtual DbSet<VwEntradasPorAjuste> VwEntradasPorAjustes { get; set; }
        public virtual DbSet<VwEntradasPorAjustesCancelado> VwEntradasPorAjustesCancelados { get; set; }
        public virtual DbSet<VwEntradasPorArticulo> VwEntradasPorArticuloes { get; set; }
        public virtual DbSet<VwEntradasPorCompra> VwEntradasPorCompras { get; set; }
        public virtual DbSet<VwEntradasPorFacturasCancelada> VwEntradasPorFacturasCanceladas { get; set; }
        public virtual DbSet<VwEntradasPorNotasDeCredito> VwEntradasPorNotasDeCreditoes { get; set; }
        public virtual DbSet<VwEntradasPorRemisionesCancelada> VwEntradasPorRemisionesCanceladas { get; set; }
        public virtual DbSet<VwEntradasPorTraspaso> VwEntradasPorTraspasos { get; set; }
        public virtual DbSet<VwEntradasTotalesPorAjuste> VwEntradasTotalesPorAjustes { get; set; }
        public virtual DbSet<VwEntradasTotalesPorAjustesCancelado> VwEntradasTotalesPorAjustesCancelados { get; set; }
        public virtual DbSet<VwEntradasTotalesPorCompra> VwEntradasTotalesPorCompras { get; set; }
        public virtual DbSet<VwEntradasTotalesPorFacturasCancelada> VwEntradasTotalesPorFacturasCanceladas { get; set; }
        public virtual DbSet<VwEntradasTotalesPorNotasDeCredito> VwEntradasTotalesPorNotasDeCreditoes { get; set; }
        public virtual DbSet<VwEntradasTotalesPorRemisionesCancelada> VwEntradasTotalesPorRemisionesCanceladas { get; set; }
        public virtual DbSet<VwEntradasTotalesPorTraspaso> VwEntradasTotalesPorTraspasos { get; set; }
        public virtual DbSet<VwEntradasTotalesPorTraspasosRechazado> VwEntradasTotalesPorTraspasosRechazados { get; set; }
        public virtual DbSet<VwExistencia> VwExistencias { get; set; }
        public virtual DbSet<VwExistenciasConPedimento> VwExistenciasConPedimentos { get; set; }
        public virtual DbSet<VwFoliosPorAbono> VwFoliosPorAbonos { get; set; }
        public virtual DbSet<VwFoliosPorFactura> VwFoliosPorFacturas { get; set; }
        public virtual DbSet<VwFoliosPorNotasDeCredito> VwFoliosPorNotasDeCreditoes { get; set; }
        public virtual DbSet<VwFoliosPorPago> VwFoliosPorPagos { get; set; }
        public virtual DbSet<VwFoliosPorParcialidade> VwFoliosPorParcialidades { get; set; }
        public virtual DbSet<VwFoliosPorSerie> VwFoliosPorSeries { get; set; }
        public virtual DbSet<VwInformacionTotalDeImpuestosPorDetalleDeCompra> VwInformacionTotalDeImpuestosPorDetalleDeCompras { get; set; }
        public virtual DbSet<VwInformacionTotalDeImpuestosPorDetalleDeFactura> VwInformacionTotalDeImpuestosPorDetalleDeFacturas { get; set; }
        public virtual DbSet<VwInformacionTotalDeImpuestosPorDetalleDeNotaDeCredito> VwInformacionTotalDeImpuestosPorDetalleDeNotaDeCreditoes { get; set; }
        public virtual DbSet<VwInformacionTotalDeImpuestosPorDetalleDePedido> VwInformacionTotalDeImpuestosPorDetalleDePedidoes { get; set; }
        public virtual DbSet<VwInformacionTotalDeImpuestosPorDetalleDeRemision> VwInformacionTotalDeImpuestosPorDetalleDeRemisions { get; set; }
        public virtual DbSet<VwInventario> VwInventarios { get; set; }
        public virtual DbSet<VwInventarioPorClasificacion> VwInventarioPorClasificacions { get; set; }
        public virtual DbSet<VwListaArticulo> VwListaArticulos { get; set; }
        public virtual DbSet<VwListaFactura> VwListaFacturas { get; set; }
        public virtual DbSet<VwListaPago> VwListaPagos { get; set; }
        public virtual DbSet<VwListaParcialidade> VwListaParcialidades { get; set; }
        public virtual DbSet<VwListasDePrecioPorArticulo> VwListasDePrecioPorArticuloes { get; set; }
        public virtual DbSet<VwListasDePrecioPorArticuloConExistencia> VwListasDePrecioPorArticuloConExistencias { get; set; }
        public virtual DbSet<VwReporteAntiguedadSaldosFactura> VwReporteAntiguedadSaldosFacturas { get; set; }
        public virtual DbSet<VwReporteAntiguedadSaldosRemisione> VwReporteAntiguedadSaldosRemisiones { get; set; }
        public virtual DbSet<VwReporteAvaluo> VwReporteAvaluos { get; set; }
        public virtual DbSet<VwReporteCostoDeLoVendido> VwReporteCostoDeLoVendidoes { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaAjustesEntrada> VwReporteEstatusDeLaEmpresaAjustesEntradas { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaAjustesSalida> VwReporteEstatusDeLaEmpresaAjustesSalidas { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaAvaluo> VwReporteEstatusDeLaEmpresaAvaluos { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaCompra> VwReporteEstatusDeLaEmpresaCompras { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaCuentasPorCobrar> VwReporteEstatusDeLaEmpresaCuentasPorCobrars { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaCuentasPorPagar> VwReporteEstatusDeLaEmpresaCuentasPorPagars { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaFactura> VwReporteEstatusDeLaEmpresaFacturas { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaNotasDeCredito> VwReporteEstatusDeLaEmpresaNotasDeCreditoes { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaNotasDeDescuento> VwReporteEstatusDeLaEmpresaNotasDeDescuentoes { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaPedido> VwReporteEstatusDeLaEmpresaPedidos { get; set; }
        public virtual DbSet<VwReporteEstatusDeLaEmpresaRemisione> VwReporteEstatusDeLaEmpresaRemisiones { get; set; }
        public virtual DbSet<VwReporteImpuestosPorPeriodo> VwReporteImpuestosPorPeriodoes { get; set; }
        public virtual DbSet<VwReporteInventarioFisico> VwReporteInventarioFisicoes { get; set; }
        public virtual DbSet<VwReporteNotasDeDescuento> VwReporteNotasDeDescuentoes { get; set; }
        public virtual DbSet<VwReporteVentasPorArticulo> VwReporteVentasPorArticuloes { get; set; }
        public virtual DbSet<VwResumenPorCompra> VwResumenPorCompras { get; set; }
        public virtual DbSet<VwResumenPorFactura> VwResumenPorFacturas { get; set; }
        public virtual DbSet<VwResumenPorNotaDeCredito> VwResumenPorNotaDeCreditoes { get; set; }
        public virtual DbSet<VwResumenPorOrdenDeCompra> VwResumenPorOrdenDeCompras { get; set; }
        public virtual DbSet<VwResumenPorRemision> VwResumenPorRemisions { get; set; }
        public virtual DbSet<VwSaldoDeudorPorClientePorMoneda> VwSaldoDeudorPorClientePorMonedas { get; set; }
        public virtual DbSet<VwSaldosPorClientePorMoneda> VwSaldosPorClientePorMonedas { get; set; }
        public virtual DbSet<VwSaldosPorProveedorPorMoneda> VwSaldosPorProveedorPorMonedas { get; set; }
        public virtual DbSet<VwSalidasConPedimento> VwSalidasConPedimentos { get; set; }
        public virtual DbSet<VwSalidasPorAjuste> VwSalidasPorAjustes { get; set; }
        public virtual DbSet<VwSalidasPorAjustesCancelado> VwSalidasPorAjustesCancelados { get; set; }
        public virtual DbSet<VwSalidasPorArticulo> VwSalidasPorArticuloes { get; set; }
        public virtual DbSet<VwSalidasPorComprasCancelada> VwSalidasPorComprasCanceladas { get; set; }
        public virtual DbSet<VwSalidasPorFactura> VwSalidasPorFacturas { get; set; }
        public virtual DbSet<VwSalidasPorNotasDeCreditoCancelada> VwSalidasPorNotasDeCreditoCanceladas { get; set; }
        public virtual DbSet<VwSalidasPorRemisione> VwSalidasPorRemisiones { get; set; }
        public virtual DbSet<VwSalidasPorTraspaso> VwSalidasPorTraspasos { get; set; }
        public virtual DbSet<VwSalidasTotalesPorAjuste> VwSalidasTotalesPorAjustes { get; set; }
        public virtual DbSet<VwSalidasTotalesPorAjustesCancelado> VwSalidasTotalesPorAjustesCancelados { get; set; }
        public virtual DbSet<VwSalidasTotalesPorComprasCancelada> VwSalidasTotalesPorComprasCanceladas { get; set; }
        public virtual DbSet<VwSalidasTotalesPorFactura> VwSalidasTotalesPorFacturas { get; set; }
        public virtual DbSet<VwSalidasTotalesPorRemisione> VwSalidasTotalesPorRemisiones { get; set; }
        public virtual DbSet<VwSalidasTotalesPorTraspaso> VwSalidasTotalesPorTraspasos { get; set; }
        public virtual DbSet<VwTimbresPorFactura> VwTimbresPorFacturas { get; set; }
        public virtual DbSet<VwTimbresPorNotasDeCredito> VwTimbresPorNotasDeCreditoes { get; set; }
        public virtual DbSet<VwTimbresPorPago> VwTimbresPorPagos { get; set; }
        public virtual DbSet<VwTimbresPorParcialidade> VwTimbresPorParcialidades { get; set; }
        public virtual DbSet<VwVentasActivasPorCliente> VwVentasActivasPorClientes { get; set; }
        public virtual DbSet<Periodicidad> Periodicidads { get; set; }
        public virtual DbSet<ImpuestoPorFactura> ImpuestoPorFacturas { get; set; }
    }
}
