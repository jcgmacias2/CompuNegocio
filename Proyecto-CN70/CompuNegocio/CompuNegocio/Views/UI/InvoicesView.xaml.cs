using Aprovi.Application.ViewModels;
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
    /// Interaction logic for InvoicesView.xaml
    /// </summary>
    public partial class InvoicesView : BaseView, IInvoicesView
    {
        public event Action FindClient;
        public event Action OpenClientsList;
        public event Action GetFolio;
        public event Action Find;
        public event Action OpenList;
        public event Action Load;
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action AddItem;
        public event Action AddItemComment;
        public event Action ViewTaxDetails;
        public event Action RemoveItem;
        public event Action SelectItem;
        public event Action Quit;
        public event Action New;
        public event Action Cancel;
        public event Action Print;
        public event Action Save;
        public event Action Stamp;
        public event Action OpenFiscalPaymentReportView;
        public event Action OpenNote;
        public event Action OpenQuotesList;
        public event Action OpenUsersList;
        public event Action FindUser;
        public event Action ChangeCurrency;
        public event Action ToCreditNote;
        public event Action AddDisccount;
        public event Action RemoveDisccount;

        private int _idInvoice;
        private Cliente _client;
        private Usuario _seller;
        private Articulo _currentItem;
        private Moneda _lastCurrency;
        private VMFactura _invoice;
        private List<DatosExtraPorFactura> _extraData = new List<DatosExtraPorFactura>();
        private Cotizacione _quote;
        private List<Remisione> _billsOfSale;
        private int? _idPedido = null;
        private string _lastFolio = null;
        private CambioDivisa _cambioDivisa;
        private List<NotasDeDescuento> _discountNotes = new List<NotasDeDescuento>();

        public InvoicesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += InvoicesView_Loaded;
            this.KeyDown += OpenQuotesListView;
            txtCliente.LostFocus += txtCliente_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            txtSerie.LostFocus += txtSerie_LostFocus;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarFacturas.Click += btnListarFacturas_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            txtArticuloCodigo.PreviewKeyDown += TxtArticuloCodigo_PreviewKeyDown;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCantidad.GotFocus += txtArticuloCantidad_GotFocus;
            txtArticuloPrecio.PreviewKeyDown += txtArticuloPrecio_PreviewKeyDown;
            dgDetalle.PreviewKeyUp += dgDetalle_PreviewKeyUp;
            dgDetalle.MouseDoubleClick += dgDetalle_MouseDoubleClick;
            txtVendedor.LostFocus += TxtVendedorOnLostFocus;
            btnListarVendedores.Click += BtnListarVendedoresOnClick;
            cmbMonedas.SelectionChanged += CmbMonedasOnSelectionChanged;
            btnDescuentoAgregar.Click += BtnDescuentoAgregarOnClick;
            dgDescuentos.PreviewKeyUp += DgDescuentosOnPreviewKeyUp;

            dgDetalle.PreviewMouseRightButtonUp += DgDetalle_PreviewMouseRightButtonUp;
            cmbImpuestos.SelectionChanged += cmbImpuestos_SelectionChanged;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;
            btnNota.Click += BtnNotaOnClick;
            btnNotaCredito.Click += BtnNotaCreditoOnClick;

            _seller = new Usuario();

            //Abonos
            btnAbonoAgregar.Click += BtnAbonoAgregar_Click;
            btnAbonoCancelar.Click += BtnAbonoCancelar_Click;
            btnAbonoTimbrar.Click += BtnAbonoTimbrar_Click;
            btnAbonoReporte.Click += BtnAbonoReporte_Click;
            cmbAbonoMonedas.SelectionChanged += CmbAbonoMonedas_SelectionChanged;
            txtAbonoCantidad.LostFocus += TxtAbonoCantidad_LostFocus;

            //Sustitucion
            txtFolioDR.LostFocus += TxtFolioDR_LostFocus;
            btnListarFacturasDR.Click += BtnListarFacturasDR_Click;
            _related = new Factura();

            //Facturacion masiva
            _billsOfSale = null;

            //Cotizacion
            _quote = null;

            //Descuentos
            _discountNotes = new List<NotasDeDescuento>();
        }

        private void DgDescuentosOnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveDisccount.isValid(AccesoRequerido.Ver_y_Agregar))
                RemoveDisccount();
        }

        private void BtnDescuentoAgregarOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AddDisccount.isValid())
            {
                AddDisccount();
            }
        }

        private void BtnNotaCreditoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (ToCreditNote.isValid())
            {
                ToCreditNote();
            }
        }

        private void OpenQuotesListView(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.Z))
            {
                if (OpenQuotesList.isValid())
                {
                    OpenQuotesList();
                }
            }
        }

        private void CmbMonedasOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ChangeCurrency.isValid())
            {
                ChangeCurrency();
            }
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

        private void BtnNotaOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenNote.isValid())
            {
                OpenNote();
            }
        }

        private void DgDetalle_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var row = dgDetalle.SelectedIndex;

            if (row >= 0 && AddItemComment.isValid())
                AddItemComment();
        }

        private void BtnAbonoReporte_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFiscalPaymentReportView.isValid())
                OpenFiscalPaymentReportView();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Stamp.isValid())
                    Stamp();
            }
            else
            {
                if (Save.isValid())
                    Save();
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid())
                Cancel();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void cmbImpuestos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewTaxDetails.isValid())
                ViewTaxDetails();
        }

        private void dgDetalle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem.isValid())
                SelectItem();
        }

        private void dgDetalle_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveItem.isValid(AccesoRequerido.Ver_y_Agregar))
                RemoveItem();
        }

        private void txtArticuloPrecio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Tab) && AddItem.isValid())
            {
                e.Handled = true;
                AddItem();
                txtArticuloCodigo.Focus();
            }
        }

        private void txtArticuloCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            //Al perder el foco del txtArticuloCodigo llega aquí
            //Si el código del articulo esta en blanco enfocar botón Pagar
            if (!txtArticuloCodigo.Text.isValid())
            {
                //cmbAbonoMetodosDePago.Focus();
                //btnRegistrar.Focus();
                return;
            }

            //Si el articulo no existe enfoco nuevamente código de articulo
            if (!txtArticuloCantidad.Text.ToDecimalOrDefault().isValid())
            {
                txtArticuloCodigo.Focus();
                return;
            }
        }

        private void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        private void TxtArticuloCodigo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F2))
                OpenItemsList();
        }

        private void txtArticuloCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        private void btnListarFacturas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(_quote.isValid() && _quote.idCotizacion.isValid() && _lastFolio == txtFolio.Text) && Find.isValid())
                Find();
        }

        private void txtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(_quote.isValid() && _quote.idCotizacion.isValid() && _lastFolio == txtFolio.Text) && GetFolio.isValid())
                GetFolio();
        }

        private void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenClientsList.isValid())
                OpenClientsList();
        }

        private void txtCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            //Si no tengo ningún cliente seleccionado entonces busco uno
            //Si ya hay uno seleccionado no busco, ya que al ir y venir de pestañas se activaría este evento.
            if (SelectedCustomerChanged && FindClient.isValid())
                FindClient();
        }

        private void InvoicesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        public VMFactura Invoice
        {
            get
            {
                _related.serie = txtSerieDR.Text;
                _related.folio = txtFolioDR.Text.ToIntOrDefault();

                return IsDirty ? _invoice : new VMFactura() //Cuando es una factura ya registrada, regreso el registro completo que se cargo
                {
                    idFactura = _idInvoice,
                    Cliente = SelectedCustomerChanged ? new Cliente() { codigo = txtCliente.Text } : _client,
                    idCliente = _client.idCliente,
                    serie = txtSerie.Text,
                    folio = txtFolio.Text.ToIntOrDefault(),
                    idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                    Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda)cmbMonedas.SelectedItem : new Moneda(),
                    fechaHora = dpFecha.SelectedDate.Value,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    DetalleDeFactura = dgDetalle.Items.Cast<VMDetalleDeFactura>().ToList(),
                    idMetodoPago = cmbMetodosPago.SelectedValue.ToIntOrDefault(),
                    MetodosPago = cmbMetodosPago.SelectedIndex >= 0 ? (MetodosPago)cmbMetodosPago.SelectedItem : new MetodosPago(),
                    idUsoCFDI = cmbUsos.SelectedValue.ToIntOrDefault(),
                    UsosCFDI = cmbUsos.SelectedIndex >= 0 ? (UsosCFDI)cmbUsos.SelectedItem : new UsosCFDI(),
                    idRegimen = cmbRegimenes.SelectedValue.ToIntOrDefault(),
                    Regimene = cmbRegimenes.SelectedIndex >= 0 ? (Regimene)cmbRegimenes.SelectedItem : new Regimene(),
                    AbonosDeFacturas = dgAbonos.ItemsSource.isValid() ? dgAbonos.ItemsSource.Cast<AbonosDeFactura>().ToList() : new List<AbonosDeFactura>(),
                    idTipoRelacion = cmbTiposRelacion.SelectedValue.ToIntOrDefault(),
                    TiposRelacion = cmbTiposRelacion.SelectedIndex >= 0 ? (TiposRelacion)cmbTiposRelacion.SelectedItem : null,
                    idComprobanteOriginal = _related.idFactura,
                    Factura1 = _related,
                    ordenDeCompra = txtOrdenDeCompra.Text,
                    DatosExtraPorFacturas = _extraData,
                    Cotizaciones = _quote.isValid() && _quote.idCotizacion.isValid() ? new List<Cotizacione>() { _quote } : null,
                    idPedido = _idPedido,
                    idVendedor = _seller.idUsuario.isValid() ? _seller.idUsuario : (int?)null,
                    Usuario1 = new Usuario() { nombreDeUsuario = txtVendedor.Text, idUsuario = _seller.idUsuario },
                    Remisiones = _billsOfSale,
                    NotasDeDescuentoes = _discountNotes,
                    Total = txtTotalDocumento.Text.ToDecimalOrDefault(),
                    Abonado = txtTotalAbonado.Text.ToDecimalOrDefault(),
                    Acreditado = txtTotalNotas.Text.ToDecimalOrDefault()
                };
            }
        }

        public Moneda LastCurrency
        {
            get { return _lastCurrency; }
        }

        public bool IsDirty
        {
            get { return _invoice.isValid() && _invoice.idFactura.isValid() && _invoice.serie == txtSerie.Text && _invoice.folio.ToString() == txtFolio.Text && !SelectedCustomerChanged; }
        }

        public VMDetalleDeFactura CurrentItem
        {
            get
            {
                //_currentItem.codigo = txtArticuloCodigo.Text;
                return new VMDetalleDeFactura()
                {
                    idArticulo = _currentItem.idArticulo,
                    Articulo = new Articulo()
                    {
                        idArticulo = _currentItem.idArticulo,
                        codigo = txtArticuloCodigo.Text
                    },
                    cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                    precioUnitario = txtArticuloPrecio.Text.ToDecimalOrDefault()
                };
            }
        }

        public VMNotaDeCredito SelectedCreditNote => dgDescuentos.SelectedItem as VMNotaDeCredito;

        public VMImpuesto SelectedTax { get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto)cmbImpuestos.SelectedItem : new VMImpuesto(); } }

        public VMDetalleDeFactura SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (VMDetalleDeFactura)dgDetalle.SelectedItem : null; }
        }

        private bool SelectedCustomerChanged => (!_client.isValid() || txtCliente.Text.isValid() && _client.codigo != txtCliente.Text);

        public void Show(VMFactura invoice)
        {
            //Generales
            txtCliente.Text = invoice.Cliente.codigo;
            lblClienteRazonSocial.Content = invoice.Cliente.razonSocial;

            if (invoice.Moneda.isValid())
            {
                _lastCurrency = invoice.Moneda;
                cmbMonedas.SelectedItem = invoice.Moneda;
            }

            txtTipoDeCambio.Text = invoice.tipoDeCambio.ToDecimalString();
            txtSerie.Text = invoice.serie;
            txtFolio.Text = invoice.folio.ToString();
            _lastFolio = invoice.folio.ToString();

            dpFecha.SelectedDate = invoice.fechaHora;
            txtOrdenDeCompra.Text = invoice.ordenDeCompra;
            if (invoice.UsosCFDI.isValid())
                cmbUsos.SelectedItem = invoice.UsosCFDI;
            else
                cmbUsos.SelectedIndex = 0;

            if (invoice.Regimene.isValid())
                cmbRegimenes.SelectedItem = invoice.Regimene;
            else
                cmbRegimenes.SelectedIndex = 0;

            //Vendedor
            if (invoice.Usuario1.isValid())
            {
                Show(invoice.Usuario1);
            }
            else
            {
                //Si es una factura nueva, se debe poner el vendedor del cliente, y en su defecto el usuario actual
                if (!IsDirty)
                {
                    if (_client.isValid() && _client.idCliente.isValid() && _client.idVendedor.isValid())
                    {
                        //Si el cliente tiene vendedor, se asigna este
                        Show(_client.Usuario);
                    }
                    else
                    {
                        Show(Session.LoggedUser);
                    }
                }
                else
                {
                    Show(new Usuario());
                }
            }

            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloPrecio.Clear();
            _currentItem = new Articulo();

            dgDetalle.ItemsSource = invoice.DetalleDeFactura;
            txtSubtotal.Text = invoice.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = invoice.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;

            txtTotal.Text = invoice.Total.ToDecimalString();

            //Por default escondido
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            if (invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Anulada) || invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Cancelada))
                lblCancelada.Visibility = System.Windows.Visibility.Visible;

            //Abonos
            dgAbonos.ItemsSource = invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));

            if (invoice.idMetodoPago.isValid())
                cmbMetodosPago.SelectedItem = invoice.MetodosPago;
            else
                cmbMetodosPago.SelectedIndex = ((int)MetodoDePago.Pago_en_parcialidades_o_diferido) - 1;

            if (invoice.idMoneda.isValid())
                cmbAbonoMonedas.SelectedValue = invoice.idMoneda;
            txtAbonoTipoDeCambio.Text = Session.Configuration.tipoDeCambio.ToDecimalString();
            txtTotalDocumento.Text = invoice.Total.ToDecimalString();
            txtTotalAbonado.Text = invoice.Abonado.ToDecimalString(); 
            txtSaldo.Text = invoice.Saldo.ToDecimalString();

            //Sustitución
            txtSerieDR.Clear();
            txtFolioDR.Clear();
            txtClienteDR.Clear();
            lblClienteRazonSocialDR.Content = string.Empty;
            txtFechaDR.Clear();
            txtMonedaDR.Clear();
            txtTipoCambioDR.Clear();
            txtMetodoPagoDR.Clear();
            cmbTiposRelacion.SelectedIndex = 0;

            if (invoice.idComprobanteOriginal.HasValue && invoice.idComprobanteOriginal.Value.isValid())
                Show(invoice.Factura1);

            _idInvoice = invoice.idFactura;
            _client = invoice.Cliente;

            //Descuentos
            if (invoice.NotasDeDescuentoes.isValid() && !invoice.NotasDeDescuentoes.IsEmpty())
            {
                _discountNotes = invoice.NotasDeDescuentoes.ToList();
            }
            else
            {
                _discountNotes = new List<NotasDeDescuento>();
            }
            dgDescuentos.ItemsSource = null;
            dgDescuentos.ItemsSource = _discountNotes;
            txtTotalNotas.Text = invoice.Acreditado.ToDecimalString();

            //Cuando ya es una existente
            _invoice = invoice;
            _extraData = invoice.DatosExtraPorFacturas.ToList();

            //Cuando viene de un pedido
            _idPedido = invoice.idPedido;

            //Cuando viene de una cotizacion
            if (invoice.Cotizaciones.isValid() && !invoice.Cotizaciones.IsEmpty())
            {
                _quote = invoice.Cotizaciones.FirstOrDefault();
            }
            else
            {
                _quote = new Cotizacione();
            }

            //Cuando viene de una facturacion de remisiones masiva
            if (invoice.Remisiones.isValid() && !invoice.Remisiones.IsEmpty())
            {
                _billsOfSale = invoice.Remisiones.ToList();
            }

            //Si ya esta timbrada deshabilito el boton de registrar
            SetEnvironment((StatusDeFactura)invoice.idEstatusDeFactura);
        }

        public void Show(Moneda currency)
        {
            cmbMonedas.SelectedItem = currency;
            _lastCurrency = currency;
        }

        public void Show(VMDetalleDeFactura detail)
        {
            txtArticuloCodigo.Text = detail.Articulo.codigo;
            txtArticuloDescripcion.Text = detail.Articulo.descripcion;
            txtArticuloCantidad.Text = detail.cantidad.ToDecimalString();
            txtArticuloUnidad.Text = detail.Articulo.UnidadesDeMedida.descripcion;
            txtArticuloPrecio.Text = detail.precioUnitario.ToDecimalString();
            _currentItem = detail.Articulo;
            txtArticuloCantidad.Focus();
        }

        public void Show(VMImpuesto tax)
        {
            txtImpuestos.Text = tax.Importe.ToDecimalString();
        }

        public void Show(Usuario seller)
        {
            txtVendedor.Text = seller.nombreDeUsuario;
            _seller = seller;
        }

        public void ShowStock(decimal stock)
        {
            lblExistencia.Content = stock.ToDecimalString();
        }

        public void DisableSaveButton()
        {
            btnRegistrar.IsEnabled = false;
        }

        public void ClearItem()
        {
            txtArticuloPrecio.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloDescripcion.Clear();
            lblExistencia.Content = "";
            _currentItem = new Articulo();
        }

        public void FillCombos(List<Moneda> currencies, List<MetodosPago> paymentMethods, List<FormasPago> paymentForms, List<UsosCFDI> CFDIUses, List<Regimene> regimes, List<TiposRelacion> relationTypes, List<CuentasBancaria> bankAccounts)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
            _lastCurrency = (Moneda)cmbMonedas.SelectedItem;

            cmbMetodosPago.ItemsSource = paymentMethods;
            cmbMetodosPago.SelectedValuePath = "idMetodoPago";
            cmbMetodosPago.DisplayMemberPath = "descripcion";
            cmbMetodosPago.SelectedIndex = 0;

            cmbAbonoMonedas.ItemsSource = currencies;
            cmbAbonoMonedas.SelectedValuePath = "idMoneda";
            cmbAbonoMonedas.DisplayMemberPath = "descripcion";
            cmbAbonoMonedas.SelectedIndex = 0;

            cmbAbonoFormasPago.ItemsSource = paymentForms;
            cmbAbonoFormasPago.SelectedValuePath = "idFormaPago";
            cmbAbonoFormasPago.DisplayMemberPath = "descripcion";
            cmbAbonoFormasPago.SelectedIndex = 0;

            cmbUsos.ItemsSource = CFDIUses;
            cmbUsos.SelectedValuePath = "idUsoCFDI";
            cmbUsos.DisplayMemberPath = "descripcion";
            cmbUsos.SelectedIndex = 0;

            cmbRegimenes.ItemsSource = regimes;
            cmbRegimenes.SelectedValuePath = "idRegimen";
            cmbRegimenes.DisplayMemberPath = "descripcion";
            cmbRegimenes.SelectedIndex = 0;

            cmbTiposRelacion.ItemsSource = relationTypes;
            cmbTiposRelacion.SelectedValuePath = "idTipoRelacion";
            cmbTiposRelacion.DisplayMemberPath = "descripcion";
            cmbTiposRelacion.SelectedIndex = 0;

            cmbAbonoCuentas.ItemsSource = bankAccounts;
            cmbAbonoCuentas.SelectedValuePath = "idCuentaBancaria";
            cmbAbonoCuentas.DisplayMemberPath = "numeroDeCuenta";
            cmbAbonoCuentas.SelectedIndex = -1;
        }

        private void SetEnvironment(StatusDeFactura status)
        {
            //Si esta en cualquier otro estado que no sea nueva, ya no puedo modificarla
            txtArticuloCodigo.IsEnabled = status.Equals(StatusDeFactura.Nueva);
            txtArticuloPrecio.IsEnabled = status.Equals(StatusDeFactura.Nueva);
            dgDetalle.IsReadOnly = !status.Equals(StatusDeFactura.Nueva);

            switch (status)
            {
                case StatusDeFactura.Nueva:
                    btnRegistrar.IsEnabled = true;
                    break;
                case StatusDeFactura.Pendiente_de_timbrado:
                    btnImprimir.IsEnabled = false;
                    break;
                case StatusDeFactura.Timbrada:
                    btnRegistrar.IsEnabled = false;
                    break;
                case StatusDeFactura.Anulada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    btnImprimir.IsEnabled = false;
                    break;
                case StatusDeFactura.Cancelada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        #region Abonos

        public event Action AddPayment;
        public event Action RemovePayment;
        public event Action StampPayment;
        public event Action PaymentCurrencyChanged;

        public AbonosDeFactura Payment
        {
            get
            {
                return new AbonosDeFactura()
                {
                    idFactura = _idInvoice,
                    idEstatusDeAbono = (int)StatusDeAbono.Registrado,
                    idFormaPago = cmbAbonoFormasPago.SelectedValue.ToIntOrDefault(),
                    FormasPago = cmbAbonoMonedas.SelectedIndex >= 0 ? (FormasPago)cmbAbonoFormasPago.SelectedItem : null,
                    idMoneda = cmbAbonoMonedas.SelectedValue.ToIntOrDefault(),
                    Moneda = cmbMonedas.SelectedIndex>=0? (Moneda)cmbAbonoMonedas.SelectedItem:null,
                    monto = txtAbonoCantidad.Text.ToDecimalOrDefault(),
                    idCuentaBancaria = cmbAbonoCuentas.SelectedValue.ToIntOrDefault(),
                    CuentasBancaria = cmbAbonoCuentas.SelectedIndex>=0? (CuentasBancaria) cmbAbonoCuentas.SelectedItem: null,
                    fechaHora = dpAbonoFecha.SelectedDate.GetValueOrDefault(DateTime.Now),
                    tipoDeCambio = txtAbonoTipoDeCambio.Text.ToDecimalOrDefault(),
                    CambioDivisa = _cambioDivisa
                };
            }
        }

        public AbonosDeFactura Selected
        {
            get { return dgAbonos.SelectedIndex >= 0 ? (AbonosDeFactura)dgAbonos.SelectedItem : null; }
        }

        public void Show(List<AbonosDeFactura> payments)
        {
            cmbAbonoFormasPago.SelectedIndex = 0;
            cmbAbonoMonedas.SelectedIndex = 0;
            txtAbonoCantidad.Clear();
            cmbAbonoCuentas.SelectedIndex = -1;
            dpAbonoFecha.SelectedDate = DateTime.Now;
            dgAbonos.ItemsSource = null;
            dgAbonos.ItemsSource = payments.Where(p => p.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));

            //Calculo la cuenta nuevamente para poder definir el flujo del foco de los controles
            _invoice.AbonosDeFacturas = payments;
            _invoice.UpdateAccount();

            //Actualizo la cuenta de abonos
            txtTotalDocumento.Text = _invoice.Total.ToDecimalString();
            txtTotalAbonado.Text = _invoice.Abonado.ToDecimalString();
            txtSaldo.Text = _invoice.Saldo.ToDecimalString();

            //Si ya esta registrada no cambio el método de pago
            if(!_invoice.idFactura.isValid())
            {
                if(_invoice.Saldo.Equals(0.0m))
                    cmbMetodosPago.SelectedIndex = cmbMetodosPago.SelectedIndex = ((int)MetodoDePago.Pago_en_una_sola_exhibicion) - 1;

                if (_invoice.Saldo.Equals(_invoice.Total))
                    cmbMetodosPago.SelectedIndex = cmbMetodosPago.SelectedIndex = ((int)MetodoDePago.Pago_en_parcialidades_o_diferido) - 1;
            }
        }

        public void Show(AbonosDeFactura payment)
        {
            cmbAbonoFormasPago.SelectedValue = payment.idFormaPago;
            cmbAbonoMonedas.SelectedValue = payment.idMoneda;
            txtAbonoCantidad.Text = payment.monto.ToDecimalString();
            if (payment.idCuentaBancaria.HasValue && payment.idCuentaBancaria.Value.isValid())
                cmbAbonoCuentas.SelectedValue = payment.idCuentaBancaria.Value;
            else
                cmbAbonoCuentas.SelectedIndex = -1;
            dpAbonoFecha.SelectedDate = payment.fechaHora;

            if(payment.CambioDivisa.isValid())
                _cambioDivisa = payment.CambioDivisa;
        }

        private void BtnAbonoAgregar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //Agrego el abono
            if (AddPayment.isValid())
                AddPayment();
            Mouse.OverrideCursor = null;
        }

        private void BtnAbonoTimbrar_Click(object sender, RoutedEventArgs e)
        {
            if (StampPayment.isValid())
                StampPayment();
        }

        private void BtnAbonoCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (RemovePayment.isValid())
                RemovePayment();
        }

        private void CmbAbonoMonedas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaymentCurrencyChanged.isValid())
                PaymentCurrencyChanged();
        }

        private void TxtAbonoCantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidatePayment.isValid())
                ValidatePayment();
        }

        #endregion

        #region Sustitución

        public event Action FindInvoice;
        public event Action OpenInvoicesList;
        public event Action ValidatePayment;

        private Factura _related;

        public void Show(Factura related)
        {
            txtSerieDR.Text = related.serie;
            txtFolioDR.Text = related.folio.ToString();
            txtClienteDR.Text = related.Cliente.codigo;
            lblClienteRazonSocialDR.Content = related.Cliente.razonSocial;
            txtFechaDR.Text = related.fechaHora.ToShortDateString();
            txtMonedaDR.Text = related.Moneda.descripcion;
            txtTipoCambioDR.Text = related.tipoDeCambio.ToDecimalString();
            txtMetodoPagoDR.Text = related.MetodosPago.descripcion;

            _related = related;
        }

        public Factura RelatedInvoice
        {
            get { return _related; }
        }

        private void BtnListarFacturasDR_Click(object sender, RoutedEventArgs e)
        {
            if (OpenInvoicesList.isValid())
                OpenInvoicesList();
        }

        private void TxtFolioDR_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindInvoice.isValid())
                FindInvoice();
        }

        #endregion
    }
}
