using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public interface IDispositivoService
    {
        /// <summary>
        /// Creates a list from the StopBits enum
        /// </summary>
        /// <returns>List of StopBits</returns>
        List<StopBits> ListStopBits();

        /// <summary>
        /// Creates a list from the Parity enum
        /// </summary>
        /// <returns>List of Parities</returns>
        List<Parity> ListParities();

        /// <summary>
        /// Creates a list from the available serial ports on the pc
        /// </summary>
        /// <returns>List of available ports</returns>
        List<string> ListPorts();
    }
}
