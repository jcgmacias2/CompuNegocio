using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for CompanyStatusReportView.xaml
    /// </summary>
    public partial class CompanyStatusReportView : BaseView, ICompanyStatusReportView
    {
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        public event Action Load;
        public event Action FilterChanged;

        public CompanyStatusReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            Loaded += OnLoaded;
            dpFechaFinal.SelectedDateChanged += DpFechaSelectedDateChanged;
            dpFechaInicio.SelectedDateChanged += DpFechaSelectedDateChanged;
            cbIncluirRemisiones.Click += CbIncluirRemisionesOnChecked;
        }

        private void CbIncluirRemisionesOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FilterChanged.isValid())
            {
                FilterChanged();
            }
        }

        private void DpFechaSelectedDateChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (FilterChanged.isValid())
            {
                FilterChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Load.isValid())
            {
                Load();
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (Print.isValid())
                Print();

            Mouse.OverrideCursor = null;
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMEstadoDeLaEmpresa Report
        {
            get
            {
                return new VMEstadoDeLaEmpresa()
                {
                    FechaInicio = dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today),
                    FechaFin = dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Today),
                    IncluirRemisiones = cbIncluirRemisiones.IsChecked.GetValueOrDefault(false),
                    TipoDeCambio = lblTipoDeCambio.Content.ToDecimal(),
                    TotalPesosVentas = txtVentasPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresVentas = txtVentasDolares.Text.ToDecimalOrDefault(),
                    TotalPesosVentasImpuestosRetenidos = txtVentasPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosVentasImpuestosTrasladados = txtVentasPesosImpuestosTrasladados.ToIntOrDefault(),
                    TotalDolaresVentasImpuestosRetenidos = txtVentasDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresVentasImpuestosTrasladados = txtVentasDolaresImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalPesosAjusteEntrada = txtAjustesEntradaPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresAjusteEntrada = txtAjustesEntradaDolares.Text.ToDecimalOrDefault(),
                    TotalPesosAjusteSalida = txtAjustesDeSalidaPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresAjusteSalida = txtAjustesDeSalidaDolares.Text.ToDecimalOrDefault(),
                    TotalPesosAvaluo = txtAvaluoPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresAvaluo = txtAvaluoDolares.Text.ToDecimalOrDefault(),
                    TotalPesosCompras = txtComprasPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresCompras = txtComprasDolares.Text.ToDecimalOrDefault(),
                    TotalPesosComprasImpuestosRetenidos = txtComprasPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosComprasImpuestosTrasladados = txtComprasPesosImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalDolaresComprasImpuestosRetenidos = txtComprasDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresComprasImpuestosTrasladados = txtComprasDolaresImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorCobrar = txtCuentasPorCobrarPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorCobrar = txtCuentasPorCobrarDolares.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorCobrarImpuestosRetenidos = txtCuentasPorCobrarPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorCobrarImpuestosTrasladados = txtCuentasPorCobrarPesosImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorCobrarImpuestosRetenidos = txtCuentasPorCobrarDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorCobrarImpuestosTrasladados = txtCuentasPorCobrarDolaresImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorPagar = txtCuentasPorPagarPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorPagar = txtCuentasPorPagarDolares.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorPagarImpuestosRetenidos = txtCuentasPorPagarPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosCuentasPorPagarImpuestosTrasladados = txtCuentasPorPagarPesosImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorPagarImpuestosRetenidos = txtCuentasPorPagarDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresCuentasPorPagarImpuestosTrasladados = txtCuentasPorPagarDolaresImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalPesosNotasDeCredito = txtNotasDeCreditoPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresNotasDeCredito = txtNotasDeCreditoDolares.Text.ToDecimalOrDefault(),
                    TotalPesosNotasDeCreditoImpuestosRetenidos = txtNotasDeCreditoPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosNotasDeCreditoImpuestosTrasladados = txtNotasDeCreditoPesosImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalDolaresNotasDeCreditoImpuestosRetenidos = txtNotasDeCreditoDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresNotasDeCreditoImpuestosTrasladados = txtNotasDeCreditoDolaresImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalPesosNotasDeDescuento = txtNotasDeDescuentoPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresNotasDeDescuento = txtNotasDeDescuentoDolares.Text.ToDecimalOrDefault(),
                    TotalPesosPedidos = txtPedidosPesos.Text.ToDecimalOrDefault(),
                    TotalDolaresPedidos = txtPedidosDolares.Text.ToDecimalOrDefault(),
                    TotalPesosPedidosImpuestosRetenidos = txtPedidosPesosImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalPesosPedidosImpuestosTrasladados = txtPedidosPesosImpuestosTrasladados.Text.ToDecimalOrDefault(),
                    TotalDolaresPedidosImpuestosRetenidos = txtPedidosDolaresImpuestosRetenidos.Text.ToDecimalOrDefault(),
                    TotalDolaresPedidosImpuestosTrasladados = txtPedidosDolaresImpuestosTrasladados.Text.ToDecimalOrDefault()
                };
            }
        }

        public void Show(VMEstadoDeLaEmpresa vm)
        {
            dpFechaFinal.SelectedDate = vm.FechaFin;
            dpFechaInicio.SelectedDate = vm.FechaInicio;
            cbIncluirRemisiones.IsChecked = vm.IncluirRemisiones;
            lblTipoDeCambio.Content = vm.TipoDeCambio.ToDecimalString();

            txtAjustesEntradaPesos.Text = vm.TotalPesosAjusteEntrada.ToDecimalString();
            txtAjustesEntradaDolares.Text = vm.TotalDolaresAjusteEntrada.ToDecimalString();
            txtAjustesEntradaPesosImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAjustesEntradaPesosImpuestosTrasladados.Text = 0.0m.ToDecimalString();
            txtAjustesEntradaDolaresImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAjustesEntradaDolaresImpuestosTrasladados.Text = 0.0m.ToDecimalString();

            txtAjustesDeSalidaPesos.Text = vm.TotalPesosAjusteSalida.ToDecimalString();
            txtAjustesDeSalidaDolares.Text = vm.TotalDolaresAjusteSalida.ToDecimalString();
            txtAjustesDeSalidaPesosImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAjustesDeSalidaPesosImpuestosTrasladados.Text = 0.0m.ToDecimalString();
            txtAjustesDeSalidaDolaresImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAjustesDeSalidaDolaresImpuestosTrasladados.Text = 0.0m.ToDecimalString();

            txtAvaluoPesos.Text = vm.TotalPesosAvaluo.ToDecimalString();
            txtAvaluoDolares.Text = vm.TotalDolaresAvaluo.ToDecimalString();
            txtAvaluoPesosImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAvaluoPesosImpuestosTrasladados.Text = 0.0m.ToDecimalString();
            txtAvaluoDolaresImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtAvaluoDolaresImpuestosTrasladados.Text = 0.0m.ToDecimalString();

            txtComprasPesos.Text = vm.TotalPesosCompras.ToDecimalString();
            txtComprasDolares.Text = vm.TotalDolaresCompras.ToDecimalString();
            txtComprasPesosImpuestosRetenidos.Text = vm.TotalPesosComprasImpuestosRetenidos.ToDecimalString();
            txtComprasPesosImpuestosTrasladados.Text = vm.TotalPesosComprasImpuestosTrasladados.ToDecimalString();
            txtComprasDolaresImpuestosRetenidos.Text = vm.TotalDolaresComprasImpuestosRetenidos.ToDecimalString();
            txtComprasDolaresImpuestosTrasladados.Text = vm.TotalDolaresComprasImpuestosTrasladados.ToDecimalString();

            txtCuentasPorCobrarPesos.Text = vm.TotalPesosCuentasPorCobrar.ToDecimalString();
            txtCuentasPorCobrarDolares.Text = vm.TotalDolaresCuentasPorCobrar.ToDecimalString();
            txtCuentasPorCobrarPesosImpuestosRetenidos.Text = vm.TotalPesosCuentasPorCobrarImpuestosRetenidos.ToDecimalString();
            txtCuentasPorCobrarPesosImpuestosTrasladados.Text = vm.TotalPesosCuentasPorCobrarImpuestosTrasladados.ToDecimalString();
            txtCuentasPorCobrarDolaresImpuestosRetenidos.Text = vm.TotalDolaresCuentasPorCobrarImpuestosRetenidos.ToDecimalString();
            txtCuentasPorCobrarDolaresImpuestosTrasladados.Text = vm.TotalDolaresCuentasPorCobrarImpuestosTrasladados.ToDecimalString();

            txtCuentasPorPagarPesos.Text = vm.TotalPesosCuentasPorPagar.ToDecimalString();
            txtCuentasPorPagarDolares.Text = vm.TotalDolaresCuentasPorPagar.ToDecimalString();
            txtCuentasPorPagarPesosImpuestosRetenidos.Text = vm.TotalPesosCuentasPorPagarImpuestosRetenidos.ToDecimalString();
            txtCuentasPorPagarPesosImpuestosTrasladados.Text = vm.TotalPesosCuentasPorPagarImpuestosTrasladados.ToDecimalString();
            txtCuentasPorPagarDolaresImpuestosRetenidos.Text = vm.TotalDolaresCuentasPorPagarImpuestosRetenidos.ToDecimalString();
            txtCuentasPorPagarDolaresImpuestosTrasladados.Text = vm.TotalDolaresCuentasPorPagarImpuestosTrasladados.ToDecimalString();

            txtVentasPesos.Text = vm.TotalPesosVentas.ToDecimalString();
            txtVentasDolares.Text = vm.TotalDolaresVentas.ToDecimalString();
            txtVentasPesosImpuestosRetenidos.Text = vm.TotalPesosVentasImpuestosRetenidos.ToDecimalString();
            txtVentasPesosImpuestosTrasladados.Text = vm.TotalPesosVentasImpuestosTrasladados.ToDecimalString();
            txtVentasDolaresImpuestosRetenidos.Text = vm.TotalDolaresVentasImpuestosRetenidos.ToDecimalString();
            txtVentasDolaresImpuestosTrasladados.Text = vm.TotalDolaresVentasImpuestosTrasladados.ToDecimalString();

            txtNotasDeCreditoPesos.Text = vm.TotalPesosNotasDeCredito.ToDecimalString();
            txtNotasDeCreditoDolares.Text = vm.TotalDolaresNotasDeCredito.ToDecimalString();
            txtNotasDeCreditoPesosImpuestosRetenidos.Text = vm.TotalPesosNotasDeCreditoImpuestosRetenidos.ToDecimalString();
            txtNotasDeCreditoPesosImpuestosTrasladados.Text = vm.TotalPesosNotasDeCreditoImpuestosTrasladados.ToDecimalString();
            txtNotasDeCreditoDolaresImpuestosRetenidos.Text = vm.TotalDolaresNotasDeCreditoImpuestosRetenidos.ToDecimalString();
            txtNotasDeCreditoDolaresImpuestosTrasladados.Text = vm.TotalDolaresNotasDeCreditoImpuestosTrasladados.ToDecimalString();

            txtNotasDeDescuentoPesos.Text = vm.TotalPesosNotasDeDescuento.ToDecimalString();
            txtNotasDeDescuentoDolares.Text = vm.TotalDolaresNotasDeDescuento.ToDecimalString();
            txtNotasDeDescuentoPesosImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtNotasDeDescuentoPesosImpuestosTrasladados.Text = 0.0m.ToDecimalString();
            txtNotasDeDescuentoDolaresImpuestosRetenidos.Text = 0.0m.ToDecimalString();
            txtNotasDeDescuentoDolaresImpuestosTrasladados.Text = 0.0m.ToDecimalString();

            txtPedidosPesos.Text = vm.TotalPesosPedidos.ToDecimalString();
            txtPedidosDolares.Text = vm.TotalDolaresPedidos.ToDecimalString();
            txtPedidosPesosImpuestosRetenidos.Text = vm.TotalPesosPedidosImpuestosRetenidos.ToDecimalString();
            txtPedidosPesosImpuestosTrasladados.Text = vm.TotalPesosPedidosImpuestosTrasladados.ToDecimalString();
            txtPedidosDolaresImpuestosRetenidos.Text = vm.TotalDolaresPedidosImpuestosRetenidos.ToDecimalString();
            txtPedidosDolaresImpuestosTrasladados.Text = vm.TotalDolaresPedidosImpuestosTrasladados.ToDecimalString();
        }
    }
}
