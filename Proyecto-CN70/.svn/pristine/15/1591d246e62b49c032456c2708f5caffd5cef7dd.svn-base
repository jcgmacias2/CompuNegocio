using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteVentasPorArticuloRepository : IBaseRepository<VwReporteVentasPorArticulo>
    {
        List<VwReporteVentasPorArticulo> ListDetailed(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled);
        List<VwReporteVentasPorArticulo> ListTotals(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled);
    }
}