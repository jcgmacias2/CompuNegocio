using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ISaldosPorProveedorService
    {
        /// <summary>
        /// Provee una lista de saldos por proveedor por moneda
        /// </summary>
        /// <returns>Lista de saldos por proveedor</returns>
        List<VwSaldosPorProveedorPorMoneda> List();

        /// <summary>
        /// Provee una lista de las cuentas en relación al proveedor
        /// </summary>
        /// <param name="supplier">Proveedor sobre el cual obtener las cuentas</param>
        /// <param name="start">Inicio del período de inicio a buscar</param>
        /// <param name="end">Fin del período a buscar</param>
        /// <param name="onlyPendingBalance">True si solo se desean las cuentas con saldo pendiente</param>
        /// <returns>Lista de cuentas con el proveedor</returns>
        List<VMAbonoCuentaProveedor> List(Proveedore supplier, DateTime start, DateTime end, bool onlyPendingBalance);
    }
}
