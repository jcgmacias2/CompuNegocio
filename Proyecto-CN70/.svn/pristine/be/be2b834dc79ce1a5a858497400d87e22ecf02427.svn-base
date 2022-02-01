using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMRComisiones
    {
        public string NombreUsuario { get; set; }
        public string ComisionesDolares { get; set; }
        public string ComisionesPesos { get; set; }
        public string AbonosPesos { get; set; }
        public string AbonosDolares { get; set; }

        public List<VMRDetalleComision> Detalles { get; set; }

        public VMRComisiones(List<VMRDetalleComision> detail, string nombreUsuario)
        {
            NombreUsuario = nombreUsuario;

            ComisionesDolares = detail.Where(x=>x.idMoneda == (int)Monedas.Dólares && !x.FechaCancelacion.isValid()).Sum(x=>x.Comision).ToCurrencyString();
            ComisionesPesos = detail.Where(x=>x.idMoneda == (int)Monedas.Pesos && !x.FechaCancelacion.isValid()).Sum(x=>x.Comision).ToCurrencyString();
            AbonosDolares = detail.Where(x=>x.idMoneda == (int)Monedas.Dólares && !x.FechaCancelacion.isValid()).Sum(x=>x.Abonado).ToCurrencyString();
            AbonosPesos = detail.Where(x=>x.idMoneda == (int)Monedas.Pesos && !x.FechaCancelacion.isValid()).Sum(x=>x.Abonado).ToCurrencyString();

            Detalles = detail;
        }
    }
}