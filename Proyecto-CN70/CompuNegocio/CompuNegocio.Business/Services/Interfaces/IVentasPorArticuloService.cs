using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IVentasPorArticuloService
    {
        /// <summary>
        /// Provee una lista de las ventas por articulo
        /// </summary>
        /// <param name="item">Articulo por el que se filtraran las ventas</param>
        /// <param name="classification">Clasificacion por la que se filtraran las ventas</param>
        /// <param name="startDate">Fecha de inicio por la que se filtraran las ventas</param>
        /// <param name="endDate">Fecha de fin por la que se filtraran las ventas</param>
        /// <param name="includeInvoices">True si se desea incluir las facturas</param>
        /// <param name="includeBillsOfSale">True si se desea incluir las remisiones</param>
        /// <param name="includeCancelled">True si se desea incluir ventas canceladas</param>
        /// <returns>Lista de ventas por articulo</returns>
        List<VMRDetalleVentasPorArticulo> ListDetailed(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled);

        /// <summary>
        /// Provee una lista de las ventas por articulo totalizadas
        /// </summary>
        /// <param name="item">Articulo por el que se filtraran las ventas</param>
        /// <param name="classification">Clasificacion por la que se filtraran las ventas</param>
        /// <param name="startDate">Fecha de inicio por la que se filtraran las ventas</param>
        /// <param name="endDate">Fecha de fin por la que se filtraran las ventas</param>
        /// <param name="includeInvoices">True si se desea incluir las facturas</param>
        /// <param name="includeBillsOfSale">True si se desea incluir las remisiones</param>
        /// <param name="includeCancelled">True si se desea incluir ventas canceladas</param>
        /// <returns>Lista de ventas por articulo totalizadas</returns>
        List<VMRDetalleVentasPorArticulo> ListTotals(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled);
    }
}
