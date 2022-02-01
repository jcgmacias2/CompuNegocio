using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IListasDePreciosService
    {
        /// <summary>
        /// Provee una lista de precios
        /// </summary>
        /// <param name="priceList">Lista de precios para la que se generara el reporte</param>
        /// <param name="onlyWithStock">true si solo se deben incluir articulos con existencia</param>
        /// <param name="classification">Clasificacion por la que se filtraran las ventas</param>
        /// <param name="includeNonStocked">true si se deben incluir articulos no inventariados</param>
        /// <param name="includeTaxes">True si se desea obtener precios con impuestos incluidos</param>
        /// <param name="currency">Moneda para los precios</param>
        /// <param name="exchangeRate">Tipo de cambio para los precios</param>
        /// <returns>Lista de precios</returns>
        VMRListaDePrecios List(ListasDePrecio priceList, Clasificacione classification, bool onlyWithStock, bool includeNonStocked, bool includeTaxes, Moneda currency, decimal exchangeRate);
    }
}
