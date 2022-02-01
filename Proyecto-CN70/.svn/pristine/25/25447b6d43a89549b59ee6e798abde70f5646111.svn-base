using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ICotizacionService
    {
        /// <summary>
        /// Proporciona el siguiente folio de cotizacion
        /// </summary>
        /// <returns>Folio asignado</returns>
        int Next();

        /// <summary>
        /// Proporciona el último folio utilizado en cotizaciones
        /// </summary>
        /// <returns>Folio utilizado</returns>
        int Last();

        /// <summary>
        /// Agrega una cotizacion a la colección de cotizaciones existentes
        /// </summary>
        /// <param name="quote">Cotizacion a registrar</param>
        /// <returns>Cotizacion registrada</returns>
        Cotizacione Add(VMCotizacion quote);

        /// <summary>
        /// Actualiza una cotizacion
        /// </summary>
        /// <param name="quote">cotizacion a actualizar</param>
        /// <returns>cotizacion actualizada</returns>
        Cotizacione Update(VMCotizacion quote);

        /// <summary>
        /// Busca una cotizacion a partir de su identificador numérico
        /// </summary>
        /// <param name="idQuote">Identificador numérico de la cotizacion</param>
        /// <returns>Cotizacion que corresponde al identificador</returns>
        Cotizacione Find(int idQuote);

        /// <summary>
        /// Busca una cotizacion a partir de su  folio
        /// </summary>
        /// <param name="folio">Folio de la cotizacion que se busca</param>
        /// <returns>Cotizacion que corresponda con él folio</returns>
        Cotizacione FindByFolio(int folio);

        /// <summary>
        /// Enlista todas las cotizaciones existentes
        /// </summary>
        /// <returns>Lista de cotizaciones</returns>
        List<Cotizacione> List();

        /// <summary>
        /// Enlista todas las cotizaciones que coinciden total o parcialmente en su folio, o razón social del cliente, con el valor que se especifíca
        /// </summary>
        /// <param name="value">Valor a buscar en coincidencia</param>
        /// <returns>Lista de cotizaciones que coincidan con la búsqueda</returns>
        List<Cotizacione> WithFolioOrClientLike(string value);

        /// <summary>
        /// Cancela una cotizacion
        /// </summary>
        /// <param name="idQuote">Identificador de la cotizacion a cancelar</param>
        /// <param name="reason">Motivo por el que se cancela la cotizacion</param>
        /// <returns>Cotizacion cancelada</returns>
        Cotizacione Cancel(int idQuote,string reason);

        /// <summary>
        /// Establece la factura producto de la cotizacion
        /// </summary>
        /// <param name="idQuote">Identificador de la cotizacion</param>
        /// <param name="idInvoice">Identificador de la factura</param>
        /// <returns>Cotizacion asociada</returns>
        Cotizacione Invoiced(int idQuote, int idInvoice);

        /// <summary>
        /// Establece la remision producto de la cotizacion
        /// </summary>
        /// <param name="idQuote">Identificador de la cotizacion</param>
        /// <param name="idRemision">Identificador de la remision</param>
        /// <returns>Cotizacion asociada</returns>
        Cotizacione Remitted(int idQuote, int idRemision);

        /// <summary>
        /// Reporte de cotizaciones por periodo
        /// </summary>
        /// <param name="from">Fecha de inicio</param>
        /// <param name="to">Fecha fin</param>
        /// <param name="onlySalePending">Solo pendientes de venta</param>
        /// <returns>Lista de detalles de cotizacion</returns>
        List<VMRDetalleDeCotizacion> GetDetailsForReport(DateTime from, DateTime to, bool onlySalePending);

        /// <summary>
        /// Reporte de cotizaciones por periodo
        /// </summary>
        /// <param name="from">Fecha de inicio</param>
        /// <param name="to">Fecha fin</param>
        /// <param name="customer">Cliente</param>
        /// <param name="onlySalePending">Solo pendientes de venta</param>
        /// <returns>Lista de detalles de cotizacion</returns>
        List<VMRDetalleDeCotizacion> GetDetailsForReport(DateTime from, DateTime to, Cliente customer, bool onlySalePending);

        /// <summary>
        /// Desvincula una cotizacion de la factura o remision vinculada
        /// </summary>
        /// <param name="quote">Cotizacion a desvincular</param>
        /// <returns></returns>
        Cotizacione Unlink(Cotizacione quote);
    }
}
