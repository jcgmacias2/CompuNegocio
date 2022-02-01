using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewReporteListaDePreciosPorListaConImpuestosRepository : IBaseRepository<VwReporteListaDePreciosPorListaConImpuesto>
    {
        List<VwReporteListaDePreciosPorListaConImpuesto> WithClassification(ListasDePrecio priceList, Clasificacione classification, bool includeOnlyWithStock, bool includeNonStockedItems);
    }
}
