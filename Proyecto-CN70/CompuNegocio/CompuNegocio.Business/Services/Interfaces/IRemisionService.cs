using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IRemisionService
    {
        /// <summary>
        /// Proporciona el siguiente folio de remision
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en remisiones
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega una remisión a la colección de remisiones existentes
        /// </summary>
        /// <param name="billOfSale">Remisión a registrar</param>
        /// <returns>Remisión registrada</returns>
        Remisione Add(VMRemision billOfSale);

        /// <summary>
        /// Actualiza una remisión
        /// </summary>
        /// <param name="billOfSale">Remisión a actualizar</param>
        /// <returns>Remisión actualizada</returns>
        Remisione Update(VMRemision billOfSale);

        /// <summary>
        /// Busca una remisión a partir de su identificador numérico
        /// </summary>
        /// <param name="idBillOfSale">Identificador numérico de la remisión</param>
        /// <returns>Remisión que corresponde al identificador</returns>
        Remisione Find(int idBillOfSale);

        /// <summary>
        /// Busca una remisión a partir de su  folio
        /// </summary>
        /// <param name="folio">Folio de la remisión que se busca</param>
        /// <returns>Remisión que corresponda con él folio</returns>
        Remisione FindByFolio(int folio);

        /// <summary>
        /// Enlista todas las facturas existentes
        /// </summary>
        /// <returns>Lista de facturas</returns>
        List<Remisione> List();

        /// <summary>
        /// Enlista las remisiones creadas entre las fechas proporcionadas
        /// </summary>
        /// <param name="startDate">fecha inicial</param>
        /// <param name="endDate">fecha final</param>
        /// <param name="user">Usuario que vendio la remision</param>
        /// <returns>lista de remisiones creadas entre las fechas proporcionadas</returns>
        List<Remisione> ListBySeller(DateTime startDate, DateTime endDate, Usuario user = null);

        /// <summary>
        /// Enlista todas las remisiones que coinciden total o parcialmente en su folio, o razón social del cliente, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de remisiones que coincidan con la búsqueda</returns>
        List<Remisione> WithFolioOrClientLike(string value);

        /// <summary>
        /// Cancela una remisión y todos su abonos registrados
        /// </summary>
        /// <param name="idBillOfSale">Identificador de la remisión a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la remision</param>
        /// <returns>Remisión cancelada</returns>
        Remisione Cancel(int idBillOfSale, string reason);

        /// <summary>
        /// Establece la factura producto de la remisión
        /// </summary>
        /// <param name="idBillOfSale">Identificador de la remisión</param>
        /// <param name="idInvoice">Identificador de la factura</param>
        /// <returns>Remisión asociada</returns>
        Remisione Invoiced(int idBillOfSale, int idInvoice);

        /// <summary>
        /// Obtiene las remisiones creadas entre las fechas y con el filtro proporcionado
        /// </summary>
        /// <param name="startDate">fecha de inicio</param>
        /// <param name="endDate">fecha de fin</param>
        /// <param name="filter">filtro a aplicar</param>
        /// <returns>lista con las remisiones obtenidas</returns>
        List<Remisione> List(DateTime startDate, DateTime endDate, Tipos_Reporte_Remisiones filter);

        /// <summary>
        /// Obtiene las remisiones vigentes y sin facturar
        /// </summary>
        /// <returns>lista con las remisiones obtenidas</returns>
        List<VwResumenPorRemision> ListActive();

        /// <summary>
        /// Enlista todas las remisiones que coinciden total o parcialmente en su folio, o razón social del cliente, con el valor que se especifíca y estan vigentes sin facturar
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de remisiones que coincidan con la búsqueda</returns>
        List<VwResumenPorRemision> ActiveWithFolioOrClientLike(string value);

        /// <summary>
        /// Enlista todas las remisiones por un rango de fecha y estan vigentes sin facturar
        /// </summary>
        /// <param name="start">Fecha inicial a buscar</param>
        /// <param name="end">Fecha final a buscar </param>
        /// <returns>Lista de remisiones que coincidan con la búsqueda</returns>
        List<VwResumenPorRemision> ActiveWithDateLike(DateTime start, DateTime end);

        /// <summary>
        /// Convierte una lista de remisiones a una sola factura
        /// </summary>
        /// <param name="billsOfSale"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="customer"></param>
        /// <param name="currency"></param>
        /// <param name="regimen"></param>
        /// <param name="paymentMethod"></param>
        /// <param name="cfdiUsage"></param>
        /// <param name="company"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        VMFactura ToInvoice(ICollection<VMRemision> billsOfSale, string serie, int folio, decimal exchangeRate, Cliente customer, Moneda currency, Regimene regimen, MetodosPago paymentMethod, UsosCFDI cfdiUsage, Empresa company, Usuario user, FormasPago formaPago);

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
        /// Obtiene los detalles del reporte de antiguedad de saldo detallado
        /// </summary>
        /// <param name="customer">Cliente para el filtro</param>
        /// <param name="seller">Vendedor para el filtro</param>
        /// <param name="onlyExpired">Solo documentos expirados</param>
        /// <param name="to">Fecha limite</param>
        /// <returns>Lista de detalles</returns>
        List<VMRDetalleAntiguedadSaldos> ListForDetailedCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to);

        /// <summary>
        /// Obtiene los detalles del reporte de antiguedad de saldo total
        /// </summary>
        /// <param name="customer">Cliente para el filtro</param>
        /// <param name="seller">Vendedor para el filtro</param>
        /// <param name="onlyExpired">Solo documentos expirados</param>
        /// <param name="to">Fecha limite</param>
        /// <returns>Lista de detalles</returns>
        List<VMRTotalAntiguedadSaldos> ListForTotalCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to);

        /// <summary>
        /// Actualiza los precios de un detalle de remision a los precios del cliente proporcionado
        /// </summary>
        /// <param name="details">Detalle a actualizar</param>
        /// <param name="customer">Cliente a actualizar</param>
        /// <returns>Detalle con los precios correctos</returns>
        List<VMDetalleDeRemision> UpdatePrices(List<VMDetalleDeRemision> details, Cliente customer);

        /// <summary>
        /// Obtiene las remisiones para el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">objeto a llenar con los totales</param>
        /// <param name="startDate">Fecha de inicio para el reporte</param>
        /// <param name="endDate">Fecha de fin para el reporte</param>
        /// <returns></returns>
        VMEstadoDeLaEmpresa ListForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate);
    }
}
