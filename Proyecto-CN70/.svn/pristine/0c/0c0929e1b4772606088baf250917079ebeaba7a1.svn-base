using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IAbonosService
    {
        /// <summary>
        /// Provee una lista de los abonos de factura y remisión en un periodo de tiempo
        /// </summary>
        /// <param name="start">Fecha de inicio del periodo</param>
        /// <param name="end">Fecha de terminación del periodo</param>
        /// <returns>Lista de Abonos</returns>
        List<VMAbono> ByPeriod(DateTime start, DateTime end);

        /// <summary>
        /// Provee una lista de los abonos de factura y remisión en un periodo de tiempo realizados en la caja especificada
        /// </summary>
        /// <param name="start">Fecha de inicio del periodo</param>
        /// <param name="end">Fecha de terminación del periodo</param>
        /// <returns>Lista de Abonos</returns>
        List<VMAbono> ByPeriod(Empresa business, DateTime start, DateTime end);
    }
}
