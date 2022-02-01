using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IFacturaService
    {
        /// <summary>
        /// Proporciona el siguiente folio de factura según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio asignado</returns>
        int Next(string serie);

        /// <summary>
        /// Proporciona el último folio utilizado en facturas según la serie
        /// </summary>
        /// <param name="serie">Serie sobre la que se desea obtener folio</param>
        /// <returns>Folio utilizado</returns>
        int Last(string serie);

        /// <summary>
        /// Agrega una factura a la colección de facturas existentes
        /// </summary>
        /// <param name="invoice">Factura a registrar</param>
        /// <returns>Factura registrada</returns>
        Factura Add(VMFactura invoice);

        /// <summary>
        /// Timbra una factura, si es exitoso genera el codigo cbb
        /// </summary>
        /// <param name="invoice">Factura a timbrar</param>
        /// <param name="requiresAddenda">Establece si la factura tiene una addenda</param>
        /// <returns>Factura timbrada</returns>
        Factura Stamp(VMFactura invoice, bool requiresAddenda);

        /// <summary>
        /// Busca una factura a partir de su identificador numérico
        /// </summary>
        /// <param name="idInvoice">Identificador numérico de la factura</param>
        /// <returns>Factura que corresponde al identificador</returns>
        Factura Find(int idInvoice);

        /// <summary>
        /// Busca una factura a partir de su serie y folio
        /// </summary>
        /// <param name="serie">Serie de la factura que se busca</param>
        /// <param name="folio">Folio de la factura que se busca</param>
        /// <returns>Factura que corresponda con la serie y folio</returns>
        Factura Find(string serie, string folio);

        /// <summary>
        /// Enlista las facturas creadas entre las fechas proporcionadas
        /// </summary>
        /// <param name="startDate">fecha inicial</param>
        /// <param name="endDate">fecha final</param>
        /// <param name="user">usuario que emitió las facturas</param>
        /// <returns>lista de facturas creadas entre las fechas proporcionadas</returns>
        List<Factura> ListBySeller(DateTime startDate, DateTime endDate, Usuario user = null);

        /// <summary>
        /// Cancela una factura y todos su abonos registrados
        /// </summary>
        /// <param name="idInvoice">Identificador de la factura a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la factura</param>
        /// <returns>Factura cancelada</returns>
        Factura Cancel(int idInvoice,string reason);

        /// <summary>
        /// Genera los detalles para las ventas por periodo
        /// </summary>
        /// <param name="startDate">Fecha Inicio</param>
        /// <param name="endDate">Fecha Fin</param>
        /// <returns>Detalles</returns>
        List<VMRDetalleVentaPorPeriodo> GetSalesDetailsForPeriod(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Genera los detalles para las comisiones por periodo
        /// </summary>
        /// <param name="startDate">Fecha Inicio</param>
        /// <param name="endDate">Fecha Fin</param>
        /// <param name="user">Usuario</param>
        /// <returns>Detalles</returns>
        List<VMRDetalleComision> GetComissionDetailsForPeriodAndUser(DateTime startDate, DateTime endDate, Usuario user);

        /// <summary>
        /// Proporciona las facturas para la lista de facturas (UI)
        /// </summary>
        /// <param name="value">valor para el filtro</param>
        /// <returns></returns>
        List<VwListaFactura> WithClientOrFolioLike(string value);

        /// <summary>
        /// Proporciona las facturas para la lista de facturas (UI)
        /// </summary>
        /// <returns></returns>
        List<VwListaFactura> List();

        /// <summary>
        /// Permite obtener los documentos con posibilidad de ser abonados en un comprobante de pago
        /// </summary>
        /// <param name="customer">Cliente del cual se desean obtener las facturas</param>
        /// <returns>Lista de facturas con saldo pendiente fitlradas por cliente</returns>
        List<VMFacturaConSaldo> List(Cliente customer);

        /// <summary>
        /// Permite obtener los documentos abonados en un comprobante de pago existente
        /// </summary>
        /// <param name="payment">Comprobante de pago del cual se desean obtener las facturas relacionadas</param>
        /// <returns>Lista de facturas asociadas con el abono registrado en el pago</returns>
        List<VMFacturaConSaldo> List(Pago payment);

        /// <summary>
        /// Obtiene los detalles del reporte de antiguedad de saldo detallado
        /// </summary>
        /// <param name="customer">Cliente para el filtro</param>
        /// <param name="seller">Vendedor para el filtro</param>
        /// <param name="onlyExpired">Solo documentos expirados</param>
        /// <param name="to">Fecha limite</param>
        /// <returns>Lista de detalles</returns>
        List<VMRDetalleAntiguedadSaldos> ListForDetailedCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to);

        /// <summary>
        /// Obtiene los detalles del reporte de antiguedad de saldo totalizado
        /// </summary>
        /// <param name="customer">Cliente para el filtro</param>
        /// <param name="seller">Vendedor para el filtro</param>
        /// <param name="onlyExpired">Solo documentos expirados</param>
        /// <param name="to">Fecha limite</param>
        /// <returns>Lista de detalles</returns>
        List<VMRTotalAntiguedadSaldos> ListForTotalCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to);

        /// <summary>
        /// Actualiza los precios de un detalle de factura a los precios del cliente proporcionado
        /// </summary>
        /// <param name="details">Detalle a actualizar</param>
        /// <param name="customer">Cliente a actualizar</param>
        /// <returns>Detalle con los precios correctos</returns>
        List<VMDetalleDeFactura> UpdatePrices(List<VMDetalleDeFactura> details, Cliente customer);

        /// <summary>
        /// Obtiene las facturas para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListInvoicesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene las cuentas por cobrar para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListCollectableBalancesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
