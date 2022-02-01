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
    /// Interaction logic for PriceListsReportView.xaml
    /// </summary>
    public partial class PriceListsReportView : BaseView, IPriceListsReportView
    {
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        public event Action ReportTypeChanged;
        public event Action Load;
        public event Action OpenClassificationsList;
        public event Action FindClassification;

        private Clasificacione _classification;

        public PriceListsReportView()
        {
            InitializeComponent();
            BindComponents();

            _classification = new Clasificacione();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            this.Loaded += OnLoaded;
            txtClasificacion.LostFocus += TxtClasificacionOnLostFocus;
            btnOpenClassificationsList.Click += BtnOpenClassificationsListOnClick;

            rbTodosLosArticulos.Click += FilterChanged;
            rbConClasificacion.Click += FilterChanged;
        }

        private void BtnOpenClassificationsListOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenClassificationsList.isValid())
            {
                OpenClassificationsList();
            }
        }

        private void TxtClasificacionOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindClassification.isValid())
            {
                FindClassification();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Load.isValid())
            {
                Load();
            }
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if (ReportTypeChanged.isValid())
            {
                ReportTypeChanged();
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMRListaDePrecios Report
        {
            get
            {
                return new VMRListaDePrecios()
                {
                    Moneda = cmbMonedas.SelectedItem as Moneda,
                    TipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    Clasificacion = new Clasificacione()
                    {
                        idClasificacion = _classification.idClasificacion, descripcion = txtClasificacion.Text
                    },
                    IncluirImpuestos = chkIncluirImpuestos.IsChecked.GetValueOrDefault(false),
                    IncluirNoInventariados = rbIncludeNonStocked.IsChecked.GetValueOrDefault(false),
                    SoloConInventario = rbOnlyWithStock.IsChecked.GetValueOrDefault(false),
                    FilterType = Filter,
                    ReportType = ReportType
                };
            }
        }

        private TiposFiltroReporteListaDePrecios Filter
        {
            get
            {
                return rbTodosLosArticulos.IsChecked.GetValueOrDefault(false) ? TiposFiltroReporteListaDePrecios.Todos_Los_Articulos : TiposFiltroReporteListaDePrecios.Clasificacion;
            }
        }

        private ReportesListaDePrecios ReportType
        {
            get
            {
                if (rbTodosLosPrecios.IsChecked.GetValueOrDefault(false))
                {
                    return ReportesListaDePrecios.Todos_Los_Precios;
                }

                if (rbPrecioA.IsChecked.GetValueOrDefault(false))
                {
                    return ReportesListaDePrecios.Precio_A;
                }

                if (rbPrecioB.IsChecked.GetValueOrDefault(false))
                {
                    return ReportesListaDePrecios.Precio_B;
                }

                if (rbPrecioC.IsChecked.GetValueOrDefault(false))
                {
                    return ReportesListaDePrecios.Precio_C;
                }

                if (rbPrecioD.IsChecked.GetValueOrDefault(false))
                {
                    return ReportesListaDePrecios.Precio_D;
                }

                return ReportesListaDePrecios.Todos_Los_Precios;
            }
        }

        public void Show(VMRListaDePrecios vm)
        {
            txtTipoDeCambio.Text = vm.TipoDeCambio.ToDecimalString();
            txtClasificacion.Text = vm.Clasificacion.descripcion;


            _classification = vm.Clasificacion;
            if (vm.Moneda.isValid())
            {
                cmbMonedas.SelectedItem = vm.Moneda;
            }

            SetEnvironment(vm.FilterType);
        }

        public void FillCombos(List<Moneda> currencies)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
        }

        private void SetEnvironment(TiposFiltroReporteListaDePrecios filter)
        {
            txtClasificacion.IsEnabled = filter.Equals(TiposFiltroReporteListaDePrecios.Clasificacion);
            btnOpenClassificationsList.IsEnabled = filter.Equals(TiposFiltroReporteListaDePrecios.Clasificacion);
        }
    }
}
