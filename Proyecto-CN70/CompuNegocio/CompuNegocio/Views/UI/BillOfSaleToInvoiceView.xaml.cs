using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
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
    /// Interaction logic for BillOfSaleToInvoiceView.xaml
    /// </summary>
    public partial class BillOfSaleToInvoiceView : BaseView, IBillOfSaleToInvoiceView
    {
        public event Action Quit;
        public event Action Save;

        private VMRemision _billOfSale;
        private IAbonoDeFacturaService _invoicePayments;

        public BillOfSaleToInvoiceView(VMRemision billOfSale, ICuentaBancariaService accounts, IAbonoDeFacturaService invoicePayments)
        {
            InitializeComponent();
            BindComponents();

            _invoicePayments = invoicePayments;
            Show(billOfSale, accounts.List());
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnRegistrar.Click += btnRegistrar_Click;
        }

        void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMFactura Invoice
        {
            get
            {
                return new VMFactura()
                {
                    idFactura = 0,
                    Cliente = _billOfSale.Cliente,
                    idCliente = _billOfSale.idCliente,
                    idMoneda = _billOfSale.idMoneda,
                    Moneda = _billOfSale.Moneda,
                    fechaHora = DateTime.Now,
                    tipoDeCambio = _billOfSale.tipoDeCambio,
                    DetalleDeFactura = _billOfSale.DetalleDeRemision.ToDetalleDeFactura(),
                    idMetodoPago = cmbMetodosDePago.SelectedValue.ToIntOrDefault(),
                    MetodosPago = (MetodosPago)cmbMetodosDePago.SelectedItem,
                    idUsoCFDI = cmbUsos.SelectedValue.ToIntOrDefault(),
                    UsosCFDI = (UsosCFDI)cmbUsos.SelectedItem,
                    idRegimen = cmbRegimenes.SelectedValue.ToIntOrDefault(),
                    Regimene = (Regimene)cmbRegimenes.SelectedItem,
                    AbonosDeFacturas = dgAbonos.ItemsSource.isValid() ? dgAbonos.ItemsSource.Cast<AbonosDeFactura>().ToList() : new List<AbonosDeFactura>(),
                    idVendedor = _billOfSale.idVendedor,
                    ordenDeCompra = _billOfSale.ordenDeCompra
                };
            }
        }

        public VMRemision BillOfSale { get { return _billOfSale; } }

        public void Show(VMRemision billOfSale, List<CuentasBancaria> accounts)
        {
            txtFolio.Text = billOfSale.folio.ToString();
            txtTotal.Text = billOfSale.Total.ToDecimalString();
            lblMoneda.Content = billOfSale.Moneda.descripcion;
            txtClienteCodigo.Text = billOfSale.Cliente.codigo;
            lblClienteRazonSocial.Content = billOfSale.Cliente.razonSocial;
            cmbUsos.SelectedItem = billOfSale.Cliente.UsosCFDI;

            var payments = billOfSale.AbonosDeRemisions.ToInvoicePayments();

            //Si no tiene abonos es una sola exhibición
            if (payments.Count().Equals(0))
                cmbMetodosDePago.SelectedValue = (int)MetodoDePago.Pago_en_una_sola_exhibicion;

            //Si los abonos saldan la operación es una sola exhibición
            if(billOfSale.Saldo.Equals(0.0m))
                cmbMetodosDePago.SelectedValue = (int)MetodoDePago.Pago_en_una_sola_exhibicion;

            //Si los abonos no saldan la operación es en parcialidades
            if (billOfSale.Saldo > 0.0m)
                cmbMetodosDePago.SelectedValue = (int)MetodoDePago.Pago_en_parcialidades_o_diferido;

            //Así lleno las cuentas
            dgAbonos.ItemsSource = payments;
            ((DataGridComboBoxColumn)dgAbonos.Columns[4]).ItemsSource = accounts;

            _billOfSale = billOfSale;
        }

        public void Fill(List<MetodosPago> paymentMethod, List<UsosCFDI> uses, List<Regimene> regimens)
        {
            cmbMetodosDePago.ItemsSource = paymentMethod;
            cmbMetodosDePago.SelectedValuePath = "idMetodoPago";
            cmbMetodosDePago.DisplayMemberPath = "descripcion";
            cmbMetodosDePago.SelectedIndex = 0;

            cmbUsos.ItemsSource = uses;
            cmbUsos.SelectedValuePath = "idUsoCFDI";
            cmbUsos.DisplayMemberPath = "descripcion";
            if (_billOfSale.Cliente.isValid() && _billOfSale.Cliente.UsosCFDI.isValid())
            {
                cmbUsos.SelectedItem = _billOfSale.Cliente.UsosCFDI;
            }
            else
            {
                cmbUsos.SelectedIndex = 0;
            }

            cmbRegimenes.ItemsSource = regimens;
            cmbRegimenes.SelectedValuePath = "idRegimen";
            cmbRegimenes.DisplayMemberPath = "descripcion";
            cmbRegimenes.SelectedIndex = 0;
        }
    }
}
