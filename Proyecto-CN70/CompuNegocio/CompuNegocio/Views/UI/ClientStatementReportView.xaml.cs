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
    /// Interaction logic for ClientStatementReportView.xaml
    /// </summary>
    public partial class ClientStatementReportView : BaseView, IClientStatementReportView
    {
        public event Action FindClient;
        public event Action OpenClientsList;
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        private Cliente _client;

        public ClientStatementReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCliente.LostFocus += txtCliente_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            _client = new Cliente();
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

        void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenClientsList.isValid())
                OpenClientsList();
        }

        void txtCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindClient.isValid())
                FindClient();
        }

        public Cliente Client
        {
            get { _client.codigo = txtCliente.Text; return _client; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public bool OnlyPendingBalance
        {
            get { return chkSoloConDeuda.IsChecked.GetValueOrDefault(); }
        }

        public void Show(Cliente client)
        {
            txtCliente.Text = client.codigo;
            _client = client;
        }
    }
}
