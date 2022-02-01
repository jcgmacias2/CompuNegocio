using Aprovi.Business.ViewModels;
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
    /// Interaction logic for OrdersReportView.xaml
    /// </summary>
    public partial class OrdersReportView : BaseView, IOrdersReportView
    {
        public event Action FindCustomer;
        public event Action FindOrder;
        public event Action OpenCustomersList;
        public event Action OpenOrdersList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private VMPedido _order;
        private Cliente _customer;

        public OrdersReportView(VMPedido order)
        {
            _order = order;

            InitializeComponent();
            BindComponents();

            SetReadOnly(true);

            Show(order);
        }

        public OrdersReportView()
        {
            InitializeComponent();
            BindComponents();

            _order = new VMPedido();
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
            btnListarPedidos.Click += btnListarPedidos_Click;
            btnListarClientes.Click += BtnListarClientesOnClick;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            rbCliente.Click += radioButton_Click;
            rbPedido.Click += radioButton_Click;
            rbPendientes.Click += radioButton_Click;
        }

        private void radioButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void SetReadOnly(bool readOnly)
        {
            rbCliente.IsEnabled = !readOnly;
            rbPedido.IsEnabled = !readOnly;
            rbPendientes.IsEnabled = !readOnly;
            rbPedido.IsChecked = readOnly;
            txtFolio.IsReadOnly = readOnly;
            txtCliente.IsReadOnly = readOnly;
            btnListarClientes.IsEnabled = !readOnly;
            btnListarPedidos.IsEnabled = !readOnly;
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenCustomersList.isValid())
                OpenCustomersList();
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindCustomer.isValid())
                FindCustomer();
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

        void btnListarPedidos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenOrdersList.isValid())
                OpenOrdersList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindOrder.isValid())
                FindOrder();
        }

        public VMPedido Order
        {
            get { return _order.isValid() && _order.idPedido.isValid() ? _order : new VMPedido() { folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public Cliente Customer
        {
            get { return _customer.isValid() && _customer.idCliente.isValid() ? _customer : new Cliente() { codigo = txtCliente.Text }; }
        }

        public Reportes_Pedidos ReportType
        {
            get
            {
                if (rbPendientes.IsChecked.HasValue && rbPendientes.IsChecked.Value)
                {
                    return Reportes_Pedidos.Pendientes_De_Surtir;
                }

                if (rbCliente.IsChecked.HasValue && rbCliente.IsChecked.Value)
                {
                    return Reportes_Pedidos.Del_Cliente;
                }

                if (rbPedido.IsChecked.HasValue && rbPedido.IsChecked.Value)
                {
                    return Reportes_Pedidos.Pedido;
                }

                return Reportes_Pedidos.Pendientes_De_Surtir;
            }
        }

        public void Show(VMPedido order)
        {
            txtFolio.Text = order.folio.ToStringOrDefault();

            _order = order;
        }

        public void Show(Cliente customer)
        {
            txtCliente.Text = customer.codigo;

            _customer = customer;
        }

        public void Clear()
        {
            //Limpia los comboboxes
            txtCliente.Clear();
            txtFolio.Clear();

            _customer = new Cliente();
            _order = new VMPedido();
        }
    }
}
