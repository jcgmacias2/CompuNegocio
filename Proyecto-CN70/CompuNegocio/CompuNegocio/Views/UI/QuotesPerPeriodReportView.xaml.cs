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
    /// Interaction logic for QuotesPerPeriodReportView.xaml
    /// </summary>
    public partial class QuotesPerPeriodReportView : BaseView, IQuotesPerPeriodReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action OpenCustomersList;
        public event Action FindCustomer;

        private Cliente _customer;

        public QuotesPerPeriodReportView()
        {
            InitializeComponent();
            BindComponents();

            _customer = new Cliente();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnOpenCustomersList.Click += BtnOpenCustomersListOnClick;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindCustomer.isValid())
            {
                FindCustomer();
            }
        }

        private void BtnOpenCustomersListOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenCustomersList.isValid())
            {
                OpenCustomersList();
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

        public void SetDates(DateTime startDate, DateTime endDate)
        {
            dpFechaInicio.SelectedDate = startDate;
            dpFechaFin.SelectedDate = endDate;
        }

        public void Show(Cliente customer)
        {
            txtCliente.Text = customer.codigo;
            _customer = customer;
        }

        public DateTime StartDate { get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today); } }
        public DateTime EndDate { get { return dpFechaFin.SelectedDate.GetValueOrDefault(DateTime.Today); } }

        public Cliente Customer => new Cliente(){codigo = txtCliente.Text, idCliente = _customer.idCliente};
        public bool OnlySalePending => cbIncluirSoloPendientesDeVenta.IsChecked.GetValueOrDefault(false);
    }
}
