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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for PaymentsByPeriodReportView.xaml
    /// </summary>
    public partial class PaymentsByPeriodReportView : BaseView, IPaymentsByPeriodReportView
    {
        public event Action Quit;
        public event Action Print;
        public event Action Preview;

        public PaymentsByPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public Empresa Business
        {
            get { return cmbEmpresas.SelectedValue.isValid() ? (Empresa)cmbEmpresas.SelectedItem : null; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public void FillCombo(List<Empresa> businesses)
        {
            cmbEmpresas.ItemsSource = businesses;
            cmbEmpresas.SelectedValuePath = "idEmpresa";
            cmbEmpresas.DisplayMemberPath = "descripcion";
        }
    }
}
