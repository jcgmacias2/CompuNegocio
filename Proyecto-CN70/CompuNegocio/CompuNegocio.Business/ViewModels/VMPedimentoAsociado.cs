using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMPedimentoAsociado
    {
        public VMPedimentoAsociado() { }

        public Articulo Articulo { get; set; }

        public int IdPedimento { get; set; }

        public string NumeroDePedimento { get; set; }

        public decimal Cantidad { get; set; }
    }
}
