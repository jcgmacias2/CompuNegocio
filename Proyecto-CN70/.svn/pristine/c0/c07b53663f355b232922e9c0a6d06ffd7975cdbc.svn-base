using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class DispositivoService : IDispositivoService
    {
        public DispositivoService() { }

        public List<StopBits> ListStopBits()
        {
            try
            {
                return Enum.GetValues(typeof(StopBits)).Cast<StopBits>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Parity> ListParities()
        {
            try
            {
                return Enum.GetValues(typeof(Parity)).Cast<Parity>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> ListPorts()
        {
            try
            {
                return SerialPort.GetPortNames().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
