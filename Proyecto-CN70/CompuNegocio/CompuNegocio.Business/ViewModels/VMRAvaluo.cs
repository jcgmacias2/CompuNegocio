using System;
using System.Collections.Generic;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRAvaluo
    {
        public FiltroReporteAvaluo Filtro { get; set; }
        public bool SoloExistencias { get; set; }
        public decimal TipoDeCambio { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal PorcentajeTotal { get; set; }

        public Clasificacione Clasificacion { get; set; }

        public List<VMRDetalleAvaluo> Detalle { get; set; }
    }
}