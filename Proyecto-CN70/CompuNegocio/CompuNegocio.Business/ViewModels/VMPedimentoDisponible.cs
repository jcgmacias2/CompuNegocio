using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMPedimentoDisponible
    {
        public VMPedimentoDisponible() { }

        public int IdPedimento { get; set; }

        public string NumeroDePedimento { get; set; }

        public decimal Existencia { get; set; }

        public decimal Asociar { get; set; }

    }
}
