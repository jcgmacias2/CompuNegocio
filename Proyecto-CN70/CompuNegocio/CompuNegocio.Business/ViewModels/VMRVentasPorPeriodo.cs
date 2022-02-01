using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRVentasPorPeriodo
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<VMRDetalleVentaPorPeriodo> Detalle { get; set; }

        public VMRVentasPorPeriodo(List<VMRDetalleVentaPorPeriodo> details, DateTime fechaInicio, DateTime fechaFin)
        {
            FechaFin = fechaFin;
            FechaInicio = fechaInicio;
            Detalle = details;
        }
    }
}