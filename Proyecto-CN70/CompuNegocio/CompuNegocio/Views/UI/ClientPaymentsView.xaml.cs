using Aprovi.Application.Helpers;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ClientPaymentsView.xaml
    /// </summary>
    public partial class ClientPaymentsView : BaseView, IClientPaymentsView
    {
        public event Action Load;
        public event Action FindClient;
        public event Action OpenClientsList;
        public event Action GetFolio;
        public event Action Find;
        public event Action OpenList;
        public event Action SelectInvoice;
        public event Action AddPayment;
        public event Action Quit;
        public event Action New;
        public event Action Cancel;
        public event Action Save;
        public event Action Print;
        public event Action Stamp;
        public event Action ValidatePayment;

        private VMFacturaConSaldo _selectedInvoice;
        private int _idCliente;
        private int _idPago;
        private TimbresDePago _timbre;

        public ClientPaymentsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += ClientPaymentsView_Loaded;
            txtCliente.LostFocus += TxtCliente_LostFocus;
            btnListarClientes.Click += BtnListarClientesOnClick;
            txtSerie.LostFocus += TxtSerie_LostFocus;
            txtFolio.LostFocus += TxtFolio_LostFocus;
            btnListarPagos.Click += BtnListarPagos_Click;
            txtMonto.LostFocus += TxtMonto_LostFocus;

            dgFacturas.MouseDoubleClick += DgFacturasOnMouseDoubleClick;
            dpFecha.PreviewKeyDown += CmbFechaOnPreviewKeyDown;
            btnAddPayment.Click += BtnAddPaymentOnClick;

            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnCancelar.Click += BtnCancelar_Click;
            btnImprimir.Click += BtnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;
        }


        #region Eventos

        private void ClientPaymentsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        private void TxtCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindClient.isValid())
            {
                FindClient();
            }
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenClientsList.isValid())
            {
                OpenClientsList();
            }
        }

        private void TxtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (GetFolio.isValid())
                GetFolio();
        }

        private void TxtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        private void BtnListarPagos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void TxtMonto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidatePayment.isValid())
                ValidatePayment();
        }

        private void DgFacturasOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (SelectInvoice.isValid())
            {
                txtMonto.Focus();
                SelectInvoice();
            }
        }

        private void CmbFechaOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Tab) && AddPayment.isValid())
            {
                e.Handled = true;
                AddPayment();
                dgFacturas.Focus();
            }
        }

        private void BtnAddPaymentOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AddPayment.isValid())
            {
                AddPayment();
            }
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "InvoicesPresenter", true))
                Cancel();
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Stamp.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "InvoicesPresenter", true))
                    Stamp();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "InvoicesPresenter", true))
                    Save();
            }

        }

        #endregion

        #region Propiedades

        public VMPagoMultiple Payment => new VMPagoMultiple()
        {
            IdPago = _idPago,
            FechaHora = DateTime.Now,
            Serie = txtSerie.Text,
            Folio = txtFolio.Text.ToIntOrDefault(),
            TipoDeCambio = txtTipoCambio.Text.ToDecimalOrDefault(),
            IdCliente = _idCliente,
            Cliente = new Cliente() { idCliente = _idCliente, codigo = txtCliente.Text, razonSocial = lblClienteRazonSocial.Content.ToString() },
            IdUsuario = Session.LoggedUser.idUsuario,
            IdMetodoDePago = (int)MetodoDePago.Pago_en_parcialidades_o_diferido,
            IdEstatusDePago = (int)StatusDePago.Nuevo,
            IdRegimen = cmbRegimenes.SelectedValue.ToIntOrDefault(),
            Regimene = cmbRegimenes.SelectedIndex >=0? (Regimene)cmbRegimenes.SelectedItem : null,
            IdUsoCFDI = (int)UsoCFDI.Por_Definir,
            TimbresDePago = _timbre,
            FacturasConSaldo = dgFacturas.ItemsSource.isValid()? dgFacturas.ItemsSource.Cast<VMFacturaConSaldo>().ToList() : null
        };

        public bool IsDirty => _idPago.isValid();

        public VMFacturaConSaldo Selected => dgFacturas.SelectedItem.isValid() ? (VMFacturaConSaldo)dgFacturas.SelectedItem : null;

        public VMFacturaConSaldo Current
        {
            get
            {
                //A la factura en edición le agrego la información del abono
                if (_selectedInvoice.isValid())
                {
                    _selectedInvoice.Abono = new AbonosDeFactura()
                    {
                        idFactura = _selectedInvoice.IdFactura,
                        fechaHora = dpFechaAbono.SelectedDate.GetValueOrDefault(DateTime.Now),
                        monto = txtMonto.Text.ToDecimalOrDefault(),
                        idMoneda = cmbMoneda.SelectedValue.ToIntOrDefault(),
                        Moneda = cmbMoneda.SelectedIndex >= 0 ? (Moneda)cmbMoneda.SelectedItem : null,
                        idFormaPago = cmbFormaDePago.SelectedValue.ToIntOrDefault(),
                        FormasPago = cmbFormaDePago.SelectedIndex >= 0 ? (FormasPago)cmbFormaDePago.SelectedItem : null,
                        tipoDeCambio = txtTipoCambio.Text.ToDecimalOrDefault(),
                        idEstatusDeAbono = (int)StatusDeAbono.Registrado,
                        idEmpresa = Session.Configuration.Estacion.idEmpresa,
                        idCuentaBancaria = cmbCuentaBancaria.SelectedValue as int?,
                        idPago = _idPago
                    };
                }

                return _selectedInvoice;
            }
        }

        #endregion

        #region Funciones

        public void Show(VMPagoMultiple payment)
        {
            txtCliente.Text = payment.Cliente.isValid() ? payment.Cliente.codigo : string.Empty;
            lblClienteRazonSocial.Content = payment.Cliente.isValid() ? payment.Cliente.razonSocial : string.Empty;
            
            txtSerie.Text = payment.Serie;
            txtFolio.Text = payment.Folio.ToString();
            dpFecha.SelectedDate = payment.FechaHora;
            txtTipoCambio.Text = payment.TipoDeCambio.ToDecimalString();
            cmbRegimenes.SelectedIndex = 0;
            if(payment.Regimene.isValid())
                cmbRegimenes.SelectedItem = payment.Regimene;

            dgFacturas.ItemsSource = null;
            dgFacturas.ItemsSource = payment.FacturasConSaldo;
            _idCliente = payment.Cliente.isValid() ? payment.Cliente.idCliente : -1;
            _idPago = payment.IdPago;
            _timbre = payment.TimbresDePago;

            txtAbonoTotalPesos.Text = payment.TotalAbonadoPesos.ToDecimalString();
            txtAbonoTotalDolares.Text = payment.TotalAbonadoDolares.ToDecimalString();
            //Por default escondido
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            if (payment.IdEstatusDePago.Equals((int)StatusDePago.Anulado) || payment.IdEstatusDePago.Equals((int)StatusDePago.Cancelado))
            { 
                lblCancelada.Visibility = System.Windows.Visibility.Visible;
            }

            btnCancelar.IsEnabled = _idPago.isValid() && !(payment.IdEstatusDePago.Equals((int)StatusDePago.Cancelado) || payment.IdEstatusDePago.Equals((int)StatusDePago.Anulado));
            btnRegistrar.IsEnabled = payment.IdEstatusDePago.Equals((int)StatusDePago.Nuevo) || payment.IdEstatusDePago.Equals((int)StatusDePago.Pendiente_de_timbrado);
        }

        public void Show(VMFacturaConSaldo selected)
        {
            //Defaults
            txtFolioAbono.Text = selected.Folio;
            cmbMoneda.SelectedValue = selected.IdMoneda; 
            txtMonto.Text = string.Empty;
            cmbFormaDePago.SelectedIndex = 0;
            dpFechaAbono.SelectedDate = DateTime.Now;

            //Si tiene abono sobreescribo los defaults
            if (selected.Abono.isValid())
            {
                cmbMoneda.SelectedValue = selected.Abono.idMoneda;
                txtMonto.Text = selected.Abono.monto.ToDecimalString();
                cmbFormaDePago.SelectedValue = selected.Abono.idFormaPago;
                cmbCuentaBancaria.SelectedValue = selected.Abono.idCuentaBancaria;
                dpFechaAbono.SelectedDate = selected.Abono.fechaHora;
            }
            
            _selectedInvoice = selected;
        }

        public void Fill(List<Moneda> currencies, List<FormasPago> paymentMethods, List<CuentasBancaria> bankAccounts, List<Regimene> regimens)
        {
            cmbMoneda.ItemsSource = currencies;
            cmbMoneda.SelectedValuePath = "idMoneda";
            cmbMoneda.DisplayMemberPath = "descripcion";
            cmbMoneda.SelectedIndex = 0;

            cmbFormaDePago.ItemsSource = paymentMethods;
            cmbFormaDePago.SelectedValuePath = "idFormaPago";
            cmbFormaDePago.DisplayMemberPath = "descripcion";
            cmbFormaDePago.SelectedIndex = 0;

            cmbCuentaBancaria.ItemsSource = bankAccounts;
            cmbCuentaBancaria.SelectedValuePath = "idCuentaBancaria";
            cmbCuentaBancaria.DisplayMemberPath = "numeroDeCuenta";

            cmbRegimenes.ItemsSource = regimens;
            cmbRegimenes.SelectedValuePath = "idRegimen";
            cmbRegimenes.DisplayMemberPath = "descripcion";
            cmbRegimenes.SelectedIndex = 0;
        }

        public void ClearPayment()
        {
            txtFolioAbono.Clear();
            cmbMoneda.SelectedIndex = 0;
            txtMonto.Clear();
            cmbFormaDePago.SelectedIndex = 0;
            cmbCuentaBancaria.SelectedIndex = -1;
            dpFecha.SelectedDate = DateTime.Now;
            _selectedInvoice = null;
        }

        public void Clear()
        {
            txtCliente.Clear();
            txtSerie.Clear();
            txtFolio.Clear();
            dpFecha.SelectedDate = DateTime.Now;
            txtTipoCambio.Clear();
            ClearPayment();
            dgFacturas.ItemsSource = null;
            _idPago = -1;
            _timbre = null;
        }

        #endregion
    }
}
