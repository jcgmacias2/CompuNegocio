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
    /// Interaction logic for CollectableBalancesReportView.xaml
    /// </summary>
    public partial class CollectableBalancesReportView : BaseView, ICollectableBalancesReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action OpenSellersList;
        public event Action OpenCustomersList;
        public event Action FindSeller;
        public event Action FindCustomer;

        private Usuario _seller;
        private Cliente _customer;
        
        public CollectableBalancesReportView()
        {
            InitializeComponent();
            BindComponents();

            _seller = new Usuario();
            _customer = new Cliente();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;

            txtVendedor.LostFocus += TxtVendedorOnLostFocus;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
            btnListarVendedores.Click += BtnListarVendedoresOnClick;
            btnListarClientes.Click += BtnListarClientesOnClick;
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenCustomersList.isValid())
            {
                OpenCustomersList();
            }
        }

        private void BtnListarVendedoresOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenSellersList.isValid())
            {
                OpenSellersList();
            }
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindCustomer.isValid())
            {
                FindCustomer();
            }
        }

        private void TxtVendedorOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindSeller.isValid())
            {
                FindSeller();
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

        public VMAntiguedadSaldos Report
        {
            get
            {
                return new VMAntiguedadSaldos()
                {
                    Cliente = CustomerFilter == FiltroClientes.Cliente ? new Cliente() { codigo = txtCliente.Text, idCliente = _customer.idCliente } : null,
                    Vendedor = CustomerFilter == FiltroClientes.Vendedor ? new Usuario() { nombreDeUsuario = txtVendedor.Text, idUsuario = _seller.idUsuario } : null,
                    Fecha = dpFecha.SelectedDate.GetValueOrDefault(DateTime.Today),
                    IncluirRemisiones = cbIncluirRemisiones.IsChecked.GetValueOrDefault(false),
                    SoloVencidos = cbSoloVencidos.IsChecked.GetValueOrDefault(false),
                    FiltroClientes = CustomerFilter,
                    TipoDeReporte = ReportType,
                    Periodo = iudPeriodo.Value.GetValueOrDefault(0)
                };
            }
        }

        private FiltroClientes CustomerFilter
        {
            get
            {
                if (rbTodosLosClientes.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroClientes.Todos;
                }

                if (rbClientesVendedor.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroClientes.Vendedor;
                }

                if (rbClientes.IsChecked.GetValueOrDefault(false))
                {
                    return FiltroClientes.Cliente;
                }

                return FiltroClientes.Todos;
            }
        }

        private TiposDeReporteAntiguedadDeSaldos ReportType => rbReporteDetallado.IsChecked.GetValueOrDefault(false)
            ? TiposDeReporteAntiguedadDeSaldos.Detallado
            : TiposDeReporteAntiguedadDeSaldos.Totales;

        public void Show(Usuario seller)
        {
            txtVendedor.Text = seller.nombreDeUsuario;
            _seller = seller;
        }

        public void Show(Cliente customer)
        {
            txtCliente.Text = customer.codigo;
            _customer = customer;
        }
    }
}
