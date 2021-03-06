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
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ItemsAppraisalReportView.xaml
    /// </summary>
    public partial class ItemsAppraisalReportView : BaseView, IItemsAppraisalReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action OpenClassificationsList;
        public event Action FindClassification;
        public event Action SelectedFilterChanged;
        public event Action Load;

        private Clasificacione _classification;
        
        public ItemsAppraisalReportView()
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
            rbTodosLosArticulos.Click += FilterChangeEvent;
            rbArticulosClasificacion.Click += FilterChangeEvent;
            rbArticulosExtranjeros.Click += FilterChangeEvent;
            rbArticulosNacionales.Click += FilterChangeEvent;
            this.Loaded += OnLoaded;

            txtClasificacion.LostFocus += TxtClasificacionOnLostFocus;
            btnListarClasificaciones.Click += BtnListarClasificacionesOnClick;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
            {
                Load();
            }
        }

        private void FilterChangeEvent(object sender, RoutedEventArgs e)
        {
            if (SelectedFilterChanged.isValid())
            {
                SelectedFilterChanged();
            }
        }

        private void BtnListarClasificacionesOnClick(object sender, RoutedEventArgs routedEventArgs)
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

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMRAvaluo Report
        {
            get
            {
                return new VMRAvaluo()
                {
                    Clasificacion = new Clasificacione(){idClasificacion = _classification.idClasificacion, descripcion = txtClasificacion.Text},
                    Fecha = dpFecha.SelectedDate.GetValueOrDefault(DateTime.Today),
                    Filtro = Filter,
                    SoloExistencias = cbIncluirSoloConExistencia.IsChecked.GetValueOrDefault(false),
                    TipoDeCambio = txtTipoDeCambio.Text.ToValidatedDecimal().GetValueOrDefault(0m)
                };
            }
        }

        private FiltroReporteAvaluo Filter
        {
            get
            {
                if (rbTodosLosArticulos.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroReporteAvaluo.Todos_Los_Articulos;
                }

                if (rbArticulosClasificacion.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroReporteAvaluo.Clasificacion;
                }

                if (rbArticulosNacionales.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroReporteAvaluo.Articulos_Nacionales;
                }

                if (rbArticulosExtranjeros.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroReporteAvaluo.Articulos_Extranjeros;
                }

                return FiltroReporteAvaluo.Todos_Los_Articulos;
            }
        }

        public void Show(Clasificacione classification)
        {
            txtClasificacion.Text = classification.descripcion;

            _classification = classification;
        }

        public void Show(VMRAvaluo appraisal)
        {
            txtTipoDeCambio.Text = appraisal.TipoDeCambio.ToDecimalString();
            cbIncluirSoloConExistencia.IsChecked = appraisal.SoloExistencias;
            dpFecha.SelectedDate = appraisal.Fecha;

            Show(appraisal.Clasificacion);
            SetEnvironment(appraisal.Filtro);
        }

        private void SetEnvironment(FiltroReporteAvaluo appraisalFilter)
        {
            rbTodosLosArticulos.IsChecked = appraisalFilter.Equals(FiltroReporteAvaluo.Todos_Los_Articulos);
            rbArticulosClasificacion.IsChecked = appraisalFilter.Equals(FiltroReporteAvaluo.Clasificacion);
            rbArticulosNacionales.IsChecked = appraisalFilter.Equals(FiltroReporteAvaluo.Articulos_Nacionales);
            rbArticulosExtranjeros.IsChecked = appraisalFilter.Equals(FiltroReporteAvaluo.Articulos_Extranjeros);
            txtClasificacion.IsEnabled = appraisalFilter.Equals(FiltroReporteAvaluo.Clasificacion);
            btnListarClasificaciones.IsEnabled = appraisalFilter.Equals(FiltroReporteAvaluo.Clasificacion);
        }
    }
}
