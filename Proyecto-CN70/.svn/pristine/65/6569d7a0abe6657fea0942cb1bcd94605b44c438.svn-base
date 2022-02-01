using System;
using System.Collections.Generic;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRImpuestosPorPeriodo
    {
        public List<Impuesto> Impuestos { get; set; }
        public List<VMRDetalleImpuestosPorPeriodo> DetalleIngresos { get; set; }
        public List<VMRDetalleImpuestosPorPeriodo> DetalleEgresos { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public VMRImpuestosPorPeriodo(List<Impuesto> impuestos, List<VMRDetalleImpuestosPorPeriodo> detalleIngresos, List<VMRDetalleImpuestosPorPeriodo> detalleEgresos, DateTime fechaInicio, DateTime fechaFin)
        {
            Impuestos = impuestos;
            DetalleEgresos = detalleEgresos;
            DetalleIngresos = detalleIngresos;
            StartDate = fechaInicio;
            EndDate = fechaFin;
        }
    }
}