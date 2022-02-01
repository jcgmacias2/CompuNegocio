using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ICostoDeLoVendidoService
    {
        /// <summary>
        /// Provee una lista del costo de lo vendido
        /// </summary>
        /// <param name="startDate">Fecha de inicio para el filtro</param>
        /// <param name="endDate">Fecha de fin para el filtro</param>
        /// <param name="includeBillsOfSale">True si se desea incluir las remisiones</param>
        /// <returns>Lista de costo de lo vendido</returns>
        List<VMRDetalleCostoDeLoVendido> List(DateTime startDate, DateTime endDate, bool includeBillsOfSale);
    }
}
