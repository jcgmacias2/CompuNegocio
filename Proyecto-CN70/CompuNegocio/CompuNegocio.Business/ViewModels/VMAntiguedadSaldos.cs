using System;
using Aprovi.Data.Models;

namespace Aprovi.Business.ViewModels
{
    public class VMAntiguedadSaldos
    {
        public FiltroClientes FiltroClientes { get; set; }
        public TiposDeReporteAntiguedadDeSaldos TipoDeReporte { get; set; }
        public bool IncluirRemisiones { get; set; }
        public bool SoloVencidos { get; set; }
        public DateTime Fecha { get; set; }
        public int Periodo { get; set; }

        public Usuario Vendedor { get; set; }
        public Cliente Cliente { get; set; }
    }
}