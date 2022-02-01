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
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for StockReportView.xaml
    /// </summary>
    public partial class StockReportView : BaseView, IStockReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action AddClassification;
        public event Action OpenClassificationsList;

        private Clasificacione _clasification;

        public StockReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnAgregarClasificacion.Click += BtnAgregarClasificacionOnClick;
            btnListarClasificaciones.Click += BtnListarClasificacionesOnClick;
        }

        private void BtnListarClasificacionesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenClassificationsList.isValid())
            {
                OpenClassificationsList();
            }
        }

        private void BtnAgregarClasificacionOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AddClassification.isValid())
            {
                AddClassification();
            }
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Print.isValid())
                Print();
            Mouse.OverrideCursor = null;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public void Show(Clasificacione clasification)
        {
            _clasification = clasification;

            txtClasificacion.Text = clasification.descripcion;
        }

        public void Show(List<Clasificacione> clasifications)
        {
            dgClasificaciones.ItemsSource = null;
            dgClasificaciones.ItemsSource = clasifications;
        }

        public void Clear()
        {
            txtClasificacion.Clear();
        }

        public bool OnlyWithStock => cbSoloExistencia.IsChecked.GetValueOrDefault(false);
        public Clasificacione Classification { get { return _clasification; } }
        public List<Clasificacione> SelectedClassifications { get{ return dgClasificaciones.Items.Cast<Clasificacione>().ToList(); } }
    }
}
