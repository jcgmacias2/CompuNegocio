using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
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
    /// Interaction logic for TaxesView.xaml
    /// </summary>
    public partial class InvoiceBillsOfSaleView : BaseView, IInvoiceBillsOfSaleView
    {
        public event Action Save;
        public event Action Return;
        public event Action Quit;
        public event Action OpenListCustomers;
        public event Action ChangeCurrency;
        public event Action FindCustomer;
        public event Action FindUser;
        public event Action OpenUsersList;

        private Cliente _customer;
        private VMFactura _invoice;
        private Usuario _seller;

        public InvoiceBillsOfSaleView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnRegresar.Click += btnRegresar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnListarClientes.Click += BtnListarClientesOnClick;
            cmbMoneda.SelectionChanged += CmbMonedaOnSelectionChanged;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
            txtVendedor.LostFocus += TxtVendedorOnLostFocus;
            btnListarVendedores.Click += BtnListarVendedoresOnClick;

            //Vendedor
            _seller = new Usuario();
        }

        private void BtnListarVendedoresOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenUsersList.isValid())
            {
                OpenUsersList();
            }
        }

        private void TxtVendedorOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindUser.isValid())
            {
                FindUser();
            }
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (SelectedCustomerChanged && FindCustomer.isValid())
            {
                FindCustomer();
            }
        }

        private void CmbMonedaOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ChangeCurrency.isValid())
            {
                ChangeCurrency();
            }
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenListCustomers.isValid())
            {
                OpenListCustomers();
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            if (Return.isValid())
                Return();
        }

        public VMFactura Invoice
        {
            
            get
            {
                return new VMFactura()
                {
                    idMoneda = cmbMoneda.SelectedValue.ToIntOrDefault(),
                    Moneda = cmbMoneda.SelectedItem.isValid() ? (Moneda)cmbMoneda.SelectedItem : new Moneda(),
                    idCliente = _customer.isValid()?_customer.idCliente:0,
                    Cliente = SelectedCustomerChanged ? new Cliente() { codigo = txtCliente.Text } : _customer,
                    idMetodoPago = cmbMetodoDePago.SelectedValue.ToIntOrDefault(),
                    MetodosPago = cmbMetodoDePago.SelectedItem.isValid() ? (MetodosPago)cmbMetodoDePago.SelectedItem:new MetodosPago(),
                    fechaHora = DateTime.Now,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    DetalleDeFactura = _invoice.isValid()?_invoice.DetalleDeFactura: new List<VMDetalleDeFactura>(),
                    idRegimen = cmbRegimen.SelectedValue.ToIntOrDefault(),
                    Regimene = cmbRegimen.SelectedItem.isValid() ? (Regimene)cmbRegimen.SelectedItem : new Regimene(),
                    idUsoCFDI = cmbUsoCFDI.SelectedValue.ToIntOrDefault(),
                    UsosCFDI = cmbUsoCFDI.SelectedItem.isValid() ? (UsosCFDI)cmbUsoCFDI.SelectedItem : new UsosCFDI(),
                    serie = txtSerie.Text,
                    folio = txtFolio.Text.ToIntOrDefault(),
                    idUsuarioRegistro = _invoice.isValid()?_invoice.idUsuarioRegistro:0,
                    Usuario = _invoice.isValid() ? _invoice.Usuario : new Usuario(),
                    DatosExtraPorFacturas = _invoice.isValid() ? _invoice.DatosExtraPorFacturas : new List<DatosExtraPorFactura>(),
                    AbonosDeFacturas = _invoice.isValid() ? _invoice.AbonosDeFacturas : new List<AbonosDeFactura>(),
                    Usuario1 = new Usuario() {nombreDeUsuario = txtVendedor.Text,idUsuario = _seller.idUsuario}
                };
            }
        }

        public void Show(VMFactura invoice)
        {
            txtSerie.Text = invoice.serie;
            txtFolio.Text = invoice.folio.ToString();
            txtCliente.Text = invoice.Cliente.codigo;
            txtTipoDeCambio.Text = invoice.tipoDeCambio.ToDecimalString();
            lblRazonSocialCliente.Content = invoice.Cliente.razonSocial;
            _customer = invoice.Cliente;
            cmbMoneda.SelectedValue = invoice.idMoneda;
            cmbRegimen.SelectedValue = invoice.Cliente.idRegimen;
            cmbMetodoDePago.SelectedValue = invoice.idMetodoPago;
            cmbUsoCFDI.SelectedValue = invoice.idUsoCFDI;

            _seller = invoice.Usuario1;

            txtArticulos.Text = invoice.DetalleDeFactura.Count.ToString();
            txtUnidades.Text = invoice.DetalleDeFactura.Sum(x => x.cantidad).ToDecimalString();
            txtImporte.Text = invoice.Subtotal.ToCurrencyString();
            txtTotal.Text = invoice.Total.ToCurrencyString();
            txtAbonado.Text = invoice.Abonado.ToCurrencyString();
            txtImpuestos.Text = invoice.Impuestos[0].Importe.ToCurrencyString();

            _invoice = invoice;
        }

        private bool SelectedCustomerChanged => (!_customer.isValid() || txtCliente.Text.isValid() && _customer.codigo != txtCliente.Text);

        public void Show(List<VMRemision> billsOfSale)
        {
            txtRemisiones.Text = billsOfSale.Count.ToString();
        }

        public void Show(Usuario seller)
        {
            txtVendedor.Text = seller.nombreDeUsuario;
            _seller = seller;
        }

        public void Show(Cliente customer)
        {
            txtCliente.Text = customer.codigo;
            lblRazonSocialCliente.Content = customer.razonSocial;

            if (customer.Usuario.isValid())
            {
                Show(customer.Usuario);
            }

            if (customer.UsosCFDI.isValid())
            {
                cmbUsoCFDI.SelectedItem = customer.UsosCFDI;
            }
            else
            {
                cmbUsoCFDI.SelectedIndex = -1;
            }

            if (customer.idRegimen.isValid())
            {
                cmbRegimen.SelectedValue = customer.idRegimen;
            }
            else
            {
                cmbRegimen.SelectedIndex = -1;
            }

            _customer = customer;
        }

        public void SetCurrency(Moneda currency)
        {
            cmbMoneda.SelectedValue = currency.idMoneda;
        }

        public void SetExchangeRate(decimal rate)
        {
            txtTipoDeCambio.Text = rate.ToDecimalString();
        }

        public void FillCombos(List<Moneda> currencies, List<Regimene> regimens, List<MetodosPago> paymentMethods, List<UsosCFDI> cfdiUsages)
        {
            cmbMoneda.ItemsSource = currencies;
            cmbMoneda.DisplayMemberPath = "descripcion";
            cmbMoneda.SelectedValuePath = "idMoneda";

            cmbRegimen.ItemsSource = regimens;
            cmbRegimen.DisplayMemberPath = "descripcion";
            cmbRegimen.SelectedValuePath = "idRegimen";

            cmbMetodoDePago.ItemsSource = paymentMethods;
            cmbMetodoDePago.DisplayMemberPath = "descripcion";
            cmbMetodoDePago.SelectedValuePath = "idMetodoPago";

            cmbUsoCFDI.ItemsSource = cfdiUsages;
            cmbUsoCFDI.DisplayMemberPath = "descripcion";
            cmbUsoCFDI.SelectedValuePath = "idUsoCFDI";
        }
    }
}
