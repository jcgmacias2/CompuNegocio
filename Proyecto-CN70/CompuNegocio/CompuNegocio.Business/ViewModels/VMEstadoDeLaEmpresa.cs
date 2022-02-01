using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.ViewModels
{
    public class VMEstadoDeLaEmpresa
    {
        public decimal TotalPesosVentas { get; set; }
        public decimal TotalPesosVentasImpuestosTrasladados { get; set; }
        public decimal TotalPesosVentasImpuestosRetenidos { get; set; }
        public decimal TotalDolaresVentas { get; set; }
        public decimal TotalDolaresVentasImpuestosTrasladados { get; set; }
        public decimal TotalDolaresVentasImpuestosRetenidos { get; set; }

        public decimal TotalPesosPedidos { get; set; }
        public decimal TotalPesosPedidosImpuestosTrasladados { get; set; }
        public decimal TotalPesosPedidosImpuestosRetenidos { get; set; }
        public decimal TotalDolaresPedidos { get; set; }
        public decimal TotalDolaresPedidosImpuestosTrasladados { get; set; }
        public decimal TotalDolaresPedidosImpuestosRetenidos { get; set; }

        public decimal TotalPesosNotasDeCredito { get; set; }
        public decimal TotalPesosNotasDeCreditoImpuestosTrasladados { get; set; }
        public decimal TotalPesosNotasDeCreditoImpuestosRetenidos { get; set; }
        public decimal TotalDolaresNotasDeCredito { get; set; }
        public decimal TotalDolaresNotasDeCreditoImpuestosTrasladados { get; set; }
        public decimal TotalDolaresNotasDeCreditoImpuestosRetenidos { get; set; }

        public decimal TotalPesosNotasDeDescuento { get; set; }
        public decimal TotalPesosNotasDeDescuentoImpuestosTrasladados { get; set; }
        public decimal TotalPesosNotasDeDescuentoImpuestosRetenidos { get; set; }
        public decimal TotalDolaresNotasDeDescuento { get; set; }
        public decimal TotalDolaresNotasDeDescuentoImpuestosTrasladados { get; set; }
        public decimal TotalDolaresNotasDeDescuentoImpuestosRetenidos { get; set; }

        public decimal TotalPesosCompras { get; set; }
        public decimal TotalPesosComprasImpuestosTrasladados { get; set; }
        public decimal TotalPesosComprasImpuestosRetenidos { get; set; }
        public decimal TotalDolaresCompras { get; set; }
        public decimal TotalDolaresComprasImpuestosTrasladados { get; set; }
        public decimal TotalDolaresComprasImpuestosRetenidos { get; set; }

        public decimal TotalPesosCuentasPorCobrar { get; set; }
        public decimal TotalPesosCuentasPorCobrarImpuestosTrasladados { get; set; }
        public decimal TotalPesosCuentasPorCobrarImpuestosRetenidos { get; set; }
        public decimal TotalDolaresCuentasPorCobrar { get; set; }
        public decimal TotalDolaresCuentasPorCobrarImpuestosTrasladados { get; set; }
        public decimal TotalDolaresCuentasPorCobrarImpuestosRetenidos { get; set; }

        public decimal TotalPesosCuentasPorPagar { get; set; }
        public decimal TotalPesosCuentasPorPagarImpuestosTrasladados { get; set; }
        public decimal TotalPesosCuentasPorPagarImpuestosRetenidos { get; set; }
        public decimal TotalDolaresCuentasPorPagar { get; set; }
        public decimal TotalDolaresCuentasPorPagarImpuestosTrasladados { get; set; }
        public decimal TotalDolaresCuentasPorPagarImpuestosRetenidos { get; set; }

        public decimal TotalPesosAvaluo { get; set; }
        public decimal TotalDolaresAvaluo { get; set; }

        public decimal TotalPesosAjusteEntrada { get; set; }
        public decimal TotalDolaresAjusteEntrada { get; set; }

        public decimal TotalPesosAjusteSalida { get; set; }
        public decimal TotalDolaresAjusteSalida { get; set; }

        public bool IncluirRemisiones { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TipoDeCambio { get; set; }
    }
}
