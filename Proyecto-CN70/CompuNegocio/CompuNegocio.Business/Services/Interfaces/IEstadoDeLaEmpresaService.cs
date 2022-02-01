using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IEstadoDeLaEmpresaService
    {
        /// <summary>
        /// Genera el reporte de estado de la empresa
        /// </summary>
        /// <param name="vm">ViewModel a llenar con el reporte</param>
        /// <param name="startDate">Fecha de inicio para el filtro</param>
        /// <param name="endDate">Fecha de fin para el filtro</param>
        /// <returns>Reporte de Estado de la empresa</returns>
        VMEstadoDeLaEmpresa List(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate, bool includeBillsOfSale);
    }
}
