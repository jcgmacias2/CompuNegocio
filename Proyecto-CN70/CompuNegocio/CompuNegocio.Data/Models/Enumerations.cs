using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Models
{

    /// <summary>
    /// Enum designed to active options on Demo Mode
    /// </summary>
    public enum Ambiente { Production, Demo, Development, Configuration };

    public enum Customization
    {
        Default,
        Aprovi,
        Mdk,
        Kowi,
        Integra,
        Oteiza
    }

    public enum Modulos
    {
        Envio_De_Correos = 1,
        Cancelacion_De_Facturas = 2,
        IEPS = 3,
        Predial = 4,
        ISR = 5,
        Addendas = 6,
        Traspasos = 7,
        Control_de_Inventario = 8
    }

    /// <summary>
    /// Enumeración de los privilegios que se pueden solicitar
    /// </summary>
    public enum AccesoRequerido
    {
        Ver = 1,
        Ver_y_Agregar = 2,
        Total = 3
    }

    public enum Paises
    {
        México = 1,
        Estados_Unidos = 2
    }

    /// <summary>
    /// Enumeración de Tipos de impuesto
    /// </summary>
    public enum TipoDeImpuesto
    {
        Trasladado = 1,
        Retenido = 2
    }

    /// <summary>
    /// Enumeración de los tipos de comprobantes fiscales
    /// </summary>
    public enum TipoDeComprobante
    {
        Factura = 1,
        Parcialidad = 2,
        Nota_De_Credito = 3
    }

    /// <summary>
    /// Enumeración que representa los valores de distintos tipos de ajustes
    /// </summary>
    public enum TipoDeAjuste
    {
        Entrada = 1,
        Salida = 2
    }

    /// <summary>
    /// Enumeración de impuestos validos
    /// </summary>
    public enum Impuestos
    {
        IVA = 2,
        IEPS = 3,
        ISR = 1
    }

    public enum TipoDeBusqueda
    {
        Lista_De_Precios = 0,
        Artículos = 1,
        Clientes = 2
    }

    public enum StatusDeCompra
    {
        Crédito = 1,
        Pagada = 2,
        Cancelada = 3
    }

    public enum MetodoDePago
    {
        Pago_en_una_sola_exhibicion = 1,
        Pago_en_parcialidades_o_diferido = 2
    }

    public enum Monedas
    {
        Pesos = 1,
        Dólares = 2
    }

    public enum TipoDeImpresora
    {
        Recibos = 1,
        Reportes = 2
    }

    public enum StatusDeFactura
    {
        Nueva = 0,
        Pendiente_de_timbrado = 1,
        Timbrada = 2,
        Anulada = 3,
        Cancelada = 4
    }

    public enum StatusDeAbono
    {
        Registrado = 1,
        Cancelado = 2,
    }

    public enum StatusDeRemision
    {
        Nueva = 0,
        Registrada = 1,
        Facturada = 2,
        Cancelada = 3
    }

    public enum StatusDeAjuste
    {
        Registrado = 1,
        Cancelado = 2
    }

    public enum ReporteDeFlujoPor
    {
        TodosLosArticulos,
        PorUnArticulo,
        PorClasificacion
    }

    public enum List
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }

    public enum ProductosServicios
    {
        No_Existe_En_El_Catalogo = 1
    }

    public enum EnumSistemas
    {
        CN = 1,
        CR = 2,
        CS = 3,
        CG = 4
    }

    public enum Addendas
    {
        Gayosso = 1,
        Jardines = 2,
        Calimax = 3,
        ComercialMexicana = 4
    }

    public enum DatoExtra
    {
        NumeroRM,
        FechaDeEntrega,
        Sucursal,
        Seccion,
        Nota
    }

    public enum StatusDeCotizacion
    {
        Nueva = 0,
        Registrada = 1,
        Facturada = 2,
        Remisionada = 3,
        Cancelada = 4
    }

    public enum StatusDePedido
    {
        Nuevo = 0,
        Registrado = 1,
        Surtido_Parcial = 2,
        Surtido_Total = 3,
        Cancelado = 4
    }

    public enum StatusDeTraspaso
    {
        Nuevo = 0,
        Registrado = 1,
        Parcial = 2,
        Total = 3,
        Rechazado = 4
    }

    public enum StatusDeOrdenDeCompra
    {
        Nuevo = 0,
        Registrado = 1,
        Surtido_Parcial = 2,
        Surtido_Total = 3,
        Cancelado = 4
    }

    public enum Tipos_Reporte_Remisiones
    {
        Todas = 0,
        Pendientes_de_Facturar = 1,
        Solo_Facturadas = 2
    }

    public enum Comision
    {
        Ninguna = 0,
        Minima = 1,
        Media = 2,
        Maxima = 3
    }

    public enum Opciones_Pedido
    {
        Facturar,
        Remisionar
    }

    public enum Formas_De_Pago
    {
        Efectivo = 1,
        Cheque_Nominativo = 2,
        Transferencia_Electronica_De_Fondos = 3,
        Tarjeta_De_Credito = 4,
        Monedero_Electronico = 5,
        Dinero_Electronico = 6,
        Vales_De_Despensa = 7,
        Dacion_En_Pago = 8,
        Pago_Por_Subrogacion = 9,
        Pago_Por_Consignacion = 10,
        Condonacion = 11,
        Compensacion = 12,
        Novacion = 13,
        Confusion = 14,
        Remision_De_Deuda = 15,
        Prescripcion_O_Caducidad = 16,
        A_Satisfaccion_Del_Acreedor = 17,
        Tarjeta_De_Debito = 18,
        Tarjeta_De_Servicios = 19,
        Por_Definir = 20
    }

    public enum Reportes_Pedidos
    {
        Pendientes_De_Surtir,
        Del_Cliente,
        Pedido
    }

    public enum Opciones_Costos
    {
        Precio_de_venta = 1,
        Utilidad_bruta = 2
    }


    public enum Opciones_Envio_Correo
    {
        Configuradas,
        Proporcionada
    }

    public enum StatusDePago
    {
        Nuevo = 0,
        Pendiente_de_timbrado = 1,
        Timbrado = 2,
        Anulado = 3,
        Cancelado = 4
    }

    public enum UsoCFDI
    {
        Adquisicion_de_mercancias = 1,
        Devoluciones_descuentos_o_bonificaciones = 2,
        Gastos_en_general = 3,
        Construcciones = 4,
        Mobiliario_y_equipo_de_oficina_por_inversiones = 5,
        Equipo_de_transporte = 6,
        Equipo_de_computo_y_accesorios = 7,
        Dados_troqueles_moldes_matrices_y_herramental = 8,
        Comunicaciones_telefonicas = 9,
        Comunicaciones_satelitales = 10,
        Otra_maquinaria_y_equipo = 11,
        Honorarios_medicos_dentales_y_gastos_hospitalarios = 12,
        Gastos_medicos_por_incapacidad_o_discapacidad = 13,
        Gastos_funerales = 14,
        Donativos = 15,
        Intereses_reales_efectivamente_pagados_por_creditos_hipotecarios_Casa_Habitacion = 16,
        Aporataciones_voluntarias_al_SAR = 17,
        Primas_por_seguros_de_gastos_medicos = 18,
        Gastos_de_transportacion_escolar_obligatoria = 19,
        Depositos_en_cuentas_para_el_ahorro_primas_que_tengan_como_base_planes_de_pensiones = 20,
        Pagos_por_servicios_educativos_colegiaturas = 21,
        Por_Definir = 22
    }

    public enum TiposParcialidad
    {
        Simple = 1,
        Multiple = 2
    }

    public enum FiltroClientes
    {
        Todos,
        Vendedor,
        Cliente
    }

    public enum TiposDeReporteAntiguedadDeSaldos
    {
        Detallado,
        Totales
    }

    public enum TiposDeReporteTraspasos
    {
        Todos,
        Traspaso
    }

    public enum TiposDeFiltroVentasPorArticulo
    {
        Todos,
        Articulos_En_Clasificacion,
        Unicamente_Articulo
    }

    public enum TiposDeReporteVentasPorArticulo
    {
        Totalizado,
        Detallado,
        Detallado_Con_Datos_Del_Cliente
    }

    public enum StatusDeNotaDeCredito
    {
        Nueva = 0,
        Pendiente_De_Timbrado = 1,
        Timbrada = 2,
        Anulada = 3,
        Cancelada = 4
    }

    public enum StatusDeNotaDeDescuento
    {
        Nueva = 0,
        Registrada = 1,
        Aplicada = 2,
        Cancelada = 3
    }

    public enum FiltroReporteAvaluo
    {
        Todos_Los_Articulos,
        Clasificacion,
        Articulos_Nacionales,
        Articulos_Extranjeros
    }

    public enum StatusCrediticio
    {
        Saldo_pendiente = 1,
        Saldado = 2
    }

    public enum ReportesListaDePrecios
    {
        Todos_Los_Precios = 0,
        Precio_A = 1,
        Precio_B = 2,
        Precio_C = 3,
        Precio_D = 4,
    }

    public enum TiposFiltroReporteListaDePrecios
    {
        Todos_Los_Articulos,
        Clasificacion
    }

    public enum TiposFiltroReporteNotasDeDescuento
    {
        Todos_Los_Clientes,
        Cliente
    }
}
