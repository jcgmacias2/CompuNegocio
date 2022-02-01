using Aprovi.Business.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for DiscountNotesReportView.xaml
    /// </summary>
    public partial class DiscountNotesPerPeriodReportView : BaseView, IDiscountNotesPerPeriodReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        
        public event Action OpenCustomersList;
        public event Action FindCustomer;

        private Cliente _customer;

        public DiscountNotesPerPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;

            btnListarCliente.Click += BtnListarClientesOnClick;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
            rbSoloCliente.Click += UpdateReportType;
            rbTodosLosClientes.Click += UpdateReportType;

            _customer = new Cliente();
        }

        private void UpdateReportType(object sender, RoutedEventArgs routedEventArgs)
        {
            SetEnvironment(rbTodosLosClientes.IsChecked.GetValueOrDefault(false) ? TiposFiltroReporteNotasDeDescuento.Todos_Los_Clientes : TiposFiltroReporteNotasDeDescuento.Cliente);
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindCustomer.isValid())
            {
                FindCustomer();
            }
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
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

        public void Show(VMReporteNotasDeDescuento vm)
        {
            txtCliente.Text = vm.Customer.codigo;
            _customer = vm.Customer;

            dpFechaInicio.SelectedDate = vm.StartDate;
            dpFechaFin.SelectedDate = vm.EndDate;
        }

        public VMReporteNotasDeDescuento Report => new VMReporteNotasDeDescuento()
        {
            Customer = new Cliente() { idCliente = _customer.idCliente, codigo = txtCliente.Text },
            IncludeOnlyApplied = rbSoloAplicadas.IsChecked.GetValueOrDefault(false),
            IncludeOnlyPending = rbSoloPorAplicar.IsChecked.GetValueOrDefault(false),
            Filter = rbTodosLosClientes.IsChecked.GetValueOrDefault(false) ? TiposFiltroReporteNotasDeDescuento.Todos_Los_Clientes : TiposFiltroReporteNotasDeDescuento.Cliente,
            StartDate = dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today),
            EndDate = dpFechaFin.SelectedDate.GetValueOrDefault(DateTime.Today)
        };

        private void SetEnvironment(TiposFiltroReporteNotasDeDescuento filterType)
        {
            txtCliente.IsEnabled = filterType.Equals(TiposFiltroReporteNotasDeDescuento.Cliente);
            btnListarCliente.IsEnabled = filterType.Equals(TiposFiltroReporteNotasDeDescuento.Cliente);

            txtCliente.Clear();

            _customer = new Cliente();
        }
    }
}
