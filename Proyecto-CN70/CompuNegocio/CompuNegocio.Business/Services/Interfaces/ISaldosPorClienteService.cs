using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface ISaldosPorClienteService
    {
        /// <summary>
        /// Lista de saldos pendientes por proveedor y moneda
        /// </summary>
        /// <returns>Lista de saldos</returns>
        List<VwSaldosPorClientePorMoneda> List();

        /// <summary>
        /// Provee una lista de las cuentas en relación al cliente
        /// </summary>
        /// <param name="client">Cliente sobre el cual obtener las cuentas</param>
        /// <param name="start">Inicio del período de inicio a buscar</param>
        /// <param name="end">Fin del período a buscar</param>
        /// <param name="onlyPendingBalance">True si solo se desean las cuentas con saldo pendiente</param>
        /// <returns>Lista de cuentas del cliente</returns>
        List<VMAbonoCuentaCliente> List(Cliente client, DateTime start, DateTime end, bool onlyPendingBalance);
    }
}
