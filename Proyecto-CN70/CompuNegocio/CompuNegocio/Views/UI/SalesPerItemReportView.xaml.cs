using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for SalesPerItemReportView.xaml
    /// </summary>
    public partial class SalesPerItemReportView : BaseView, ISalesPerItemReportView
    {
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action FindClassification;
        public event Action OpenClassificationsList;

        private int _idItem;
        private int _idClassification;

        public SalesPerItemReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            txtArticulo.LostFocus += TxtArticulo_LostFocus;
            btnListarArticulos.Click += BtnListarArticulos_Click;
            txtClasificacion.LostFocus += TxtClasificacion_LostFocus;
            btnListarClasificaciones.Click += BtnListarClasificaciones_Click;

            rbTodos.Click += UpdateEnvironment;
            rbSoloUnaClasificacion.Click += UpdateEnvironment;
            rbSoloUnArticulo.Click += UpdateEnvironment;
            rbReporteDetallado.Click += UpdateEnvironment;
            rbReporteDetalladoCliente.Click += UpdateEnvironment;
            rbReporteTotalizado.Click += UpdateEnvironment;
        }

        private void UpdateEnvironment(object sender, RoutedEventArgs e)
        {
            SetEnvironment(FilterType, ReportType);
        }

        private void BtnListarClasificaciones_Click(object sender, RoutedEventArgs e)
        {
            if (OpenClassificationsList.isValid())
                OpenClassificationsList();
        }

        private void TxtClasificacion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindClassification.isValid())
                FindClassification();
        }

        private void BtnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        private void TxtArticulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Print.isValid())
                Print();
            Mouse.OverrideCursor = null;
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMVentasPorArticulo Report
        {
            get
            {
                return new VMVentasPorArticulo()
                {
                    Item = new Articulo() {codigo = txtArticulo.Text, idArticulo = _idItem},
                    Classification = new Clasificacione()
                    {
                        descripcion = txtClasificacion.Text,
                        idClasificacion = _idClassification
                    },
                    StartDate = dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today),
                    EndDate = dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Today),
                    IncludeBillsOfSale = cbIncluirRemisiones.IsChecked.GetValueOrDefault(false),
                    IncludeCancellations = cbIncluirCancelaciones.IsChecked.GetValueOrDefault(false),
                    IncludeInvoices = cbIncluirFacturas.IsChecked.GetValueOrDefault(false),
                    IncludePorcentages = cbIncluirPorcentaje.IsChecked.GetValueOrDefault(false),
                    ReportType = ReportType,
                    FilterType = FilterType
                };
            }
        }

        private TiposDeReporteVentasPorArticulo ReportType {
            get
            {
                if (rbReporteTotalizado.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeReporteVentasPorArticulo.Totalizado;
                }

                if (rbReporteDetallado.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeReporteVentasPorArticulo.Detallado;
                }

                if (rbReporteDetalladoCliente.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeReporteVentasPorArticulo.Detallado_Con_Datos_Del_Cliente;
                }

                return default(TiposDeReporteVentasPorArticulo);
            }
        }

        private TiposDeFiltroVentasPorArticulo FilterType
        {
            get
            {
                if (rbTodos.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeFiltroVentasPorArticulo.Todos;
                }

                if (rbSoloUnArticulo.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeFiltroVentasPorArticulo.Unicamente_Articulo;
                }

                if (rbSoloUnaClasificacion.IsChecked.GetValueOrDefault(false))
                {
                    return TiposDeFiltroVentasPorArticulo.Articulos_En_Clasificacion;
                }

                return default(TiposDeFiltroVentasPorArticulo);
            }
        }

        public void Show(VMVentasPorArticulo vm)
        {
            if (vm.Classification.isValid())
            {
                txtClasificacion.Text = vm.Classification.descripcion;
                _idClassification = vm.Classification.idClasificacion;
            }

            if (vm.Item.isValid())
            {
                txtArticulo.Text = vm.Item.codigo;
                _idItem = vm.Item.idArticulo;
            }

            dpFechaInicio.SelectedDate = vm.StartDate;
            dpFechaFinal.SelectedDate = vm.EndDate;
        }

        private void SetEnvironment(TiposDeFiltroVentasPorArticulo filter, TiposDeReporteVentasPorArticulo type)
        {
            txtClasificacion.IsEnabled = filter.Equals(TiposDeFiltroVentasPorArticulo.Articulos_En_Clasificacion);
            btnListarClasificaciones.IsEnabled = filter.Equals(TiposDeFiltroVentasPorArticulo.Articulos_En_Clasificacion);

            txtArticulo.IsEnabled = filter.Equals(TiposDeFiltroVentasPorArticulo.Unicamente_Articulo);
            btnListarArticulos.IsEnabled = filter.Equals(TiposDeFiltroVentasPorArticulo.Unicamente_Articulo);

            cbIncluirPorcentaje.IsEnabled = type.Equals(TiposDeReporteVentasPorArticulo.Totalizado);

            txtClasificacion.Clear();
            txtArticulo.Clear();
            _idItem = 0;
            _idClassification = 0;
        }
    }
}
