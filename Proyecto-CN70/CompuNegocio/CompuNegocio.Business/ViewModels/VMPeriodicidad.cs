using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMPeriodicidad
    {
        public VMPeriodicidad(bool global, string periodicidad, string mes, string year)
        {
            this.EsFacturaGlobal = global;
            this.CodigoPeriodicidad = periodicidad;
            this.Mes = mes;
            this.Year = year;
        }

        public bool EsFacturaGlobal { get; set; }
        public string CodigoPeriodicidad { get; set; }
        public string Mes { get; set; }
        public string Year { get; set; }

    }
}