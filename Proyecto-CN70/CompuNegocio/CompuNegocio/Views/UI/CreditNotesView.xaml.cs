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
    /// Interaction logic for CreditNotesView.xaml
    /// </summary>
    public partial class CreditNotesView : BaseView, ICreditNotesView
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
        public event Action ViewTaxDetails;
        public event Action RemoveItem;
        public event Action SelectItem;
        public event Action Quit;
        public event Action New;
        public event Action Cancel;
        public event Action Print;
        public event Action Save;
        public event Action Stamp;
        public event Action OpenNote;

        private int _idCreditNote;
        private Cliente _client;
        private Articulo _currentItem;
        private VMNotaDeCredito _creditNote;
        private List<DatosExtraPorNotaDeCredito> _extraData = new List<DatosExtraPorNotaDeCredito>();
        private Factura _invoice;
        private string _lastFolio = null;

        public CreditNotesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += CreditNotesView_Loaded;
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

            cmbImpuestos.SelectionChanged += cmbImpuestos_SelectionChanged;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;
            btnNota.Click += BtnNotaOnClick;

            //Factura
            _invoice = null;
            _client = new Cliente();
        }

        private void BtnNotaOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenNote.isValid())
            {
                OpenNote();
            }
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
            if (!(_invoice.isValid() && _invoice.idFactura.isValid() && _lastFolio == txtFolio.Text) && Find.isValid())
                Find();
        }

        private void txtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(_invoice.isValid() && _invoice.idFactura.isValid() && _lastFolio == txtFolio.Text) && GetFolio.isValid())
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

        private void CreditNotesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        public VMNotaDeCredito CreditNote
        {
            get
            {
                return IsDirty ? _creditNote : new VMNotaDeCredito() //Cuando es una nota de credito ya registrada, regreso el registro completo que se cargo
                {
                    idNotaDeCredito = _idCreditNote,
                    Cliente = SelectedCustomerChanged ? new Cliente() { codigo = txtCliente.Text } : _client,
                    idCliente = _client.idCliente,
                    serie = txtSerie.Text,
                    folio = txtFolio.Text.ToIntOrDefault(),
                    idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                    Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda)cmbMonedas.SelectedItem : new Moneda(),
                    fechaHora = dpFecha.SelectedDate.Value,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    DetalleDeNotaDeCredito = dgDetalle.Items.Cast<VMDetalleDeNotaDeCredito>().ToList(),
                    idRegimen = cmbRegimenes.SelectedValue.ToIntOrDefault(),
                    Regimene = cmbRegimenes.SelectedIndex >= 0 ? (Regimene)cmbRegimenes.SelectedItem : new Regimene(),
                    Factura = _invoice,
                    DatosExtraPorNotaDeCreditoes = _extraData,
                    idFormaDePago = cmbFormasDePago.SelectedValue.ToIntOrDefault(),
                    idCuentaBancaria = (int?)cmbCuentasBancarias.SelectedValue,
                    FormasPago = cmbFormasDePago.SelectedItem as FormasPago,
                    CuentasBancaria = cmbCuentasBancarias.SelectedItem as CuentasBancaria,
                    idFactura = _invoice.isValid() && _invoice.idFactura.isValid() ? _invoice.idFactura : (int?)null,
                    descripcion = txtDescripcionNotaCredito.Text
                };
            }
        }

        public bool IsDirty
        {
            get { return _creditNote.isValid() && _creditNote.idNotaDeCredito.isValid() && _creditNote.serie == txtSerie.Text && _creditNote.folio.ToString() == txtFolio.Text && !SelectedCustomerChanged; }
        }

        public VMDetalleDeNotaDeCredito CurrentItem
        {
            get
            {
                return new VMDetalleDeNotaDeCredito()
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

        public VMImpuesto SelectedTax { get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto)cmbImpuestos.SelectedItem : new VMImpuesto(); } }

        public VMDetalleDeNotaDeCredito SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (VMDetalleDeNotaDeCredito)dgDetalle.SelectedItem : null; }
        }

        private bool SelectedCustomerChanged => (!_client.isValid() || txtCliente.Text.isValid() && _client.codigo != txtCliente.Text);

        public void Show(VMNotaDeCredito creditNote)
        {
            //Generales
            _creditNote = creditNote;
            _idCreditNote = creditNote.idNotaDeCredito;
            txtCliente.Text = creditNote.Cliente.codigo;
            lblClienteRazonSocial.Content = creditNote.Cliente.razonSocial;

            if (creditNote.Moneda.isValid())
            {
                cmbMonedas.SelectedItem = creditNote.Moneda;
            }

            txtTipoDeCambio.Text = creditNote.tipoDeCambio.ToDecimalString();
            txtSerie.Text = creditNote.serie;
            txtFolio.Text = creditNote.folio.ToString();
            _lastFolio = creditNote.folio.ToString();

            dpFecha.SelectedDate = creditNote.fechaHora;

            if (creditNote.Regimene.isValid())
                cmbRegimenes.SelectedItem = creditNote.Regimene;
            else
                cmbRegimenes.SelectedIndex = 0;

            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloPrecio.Clear();
            _currentItem = new Articulo();

            dgDetalle.ItemsSource = creditNote.DetalleDeNotaDeCredito;
            txtSubtotal.Text = creditNote.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = creditNote.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;

            txtTotal.Text = creditNote.Total.ToDecimalString();
            txtDescripcionNotaCredito.Text = creditNote.descripcion;

            //Por default escondido
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            if (creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Anulada) || creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Cancelada))
                lblCancelada.Visibility = System.Windows.Visibility.Visible;

            if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid() && creditNote.idNotaDeCredito.isValid())
            {
                lblFacturaAsociada.Visibility = Visibility.Visible;
                lblFacturaAsociada.Content = string.Format("Asociada a la factura {0}{1}", creditNote.Factura.serie, creditNote.Factura.folio);
            }
            else
            {
                lblFacturaAsociada.Visibility = Visibility.Hidden;
                lblFacturaAsociada.Content = "";
            }

            if (creditNote.idFormaDePago.isValid())
                cmbFormasDePago.SelectedItem = creditNote.FormasPago;
            else
                cmbFormasDePago.SelectedIndex = 0;

            if (creditNote.CuentasBancaria.isValid())
            {
                cmbCuentasBancarias.SelectedItem = creditNote.CuentasBancaria;
            }
            else
            {
                cmbCuentasBancarias.SelectedIndex = -1;
            }

            _client = creditNote.Cliente;

            //Cuando viene de una factura
            _invoice = creditNote.Factura;
            _extraData = creditNote.DatosExtraPorNotaDeCreditoes.ToList();

            //Si ya esta timbrada deshabilito el boton de registrar
            SetEnvironment((StatusDeNotaDeCredito)creditNote.idEstatusDeNotaDeCredito, creditNote.Factura.isValid());
        }

        public void Show(VMDetalleDeNotaDeCredito detail)
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

        public void ClearItem()
        {
            txtArticuloPrecio.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloDescripcion.Clear();
            _currentItem = new Articulo();
        }

        public void FillCombos(List<Moneda> currencies, List<FormasPago> paymentForms, List<Regimene> regimes, List<CuentasBancaria> bankAccounts)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;

            cmbRegimenes.ItemsSource = regimes;
            cmbRegimenes.SelectedValuePath = "idRegimen";
            cmbRegimenes.DisplayMemberPath = "descripcion";
            cmbRegimenes.SelectedIndex = 0;

            cmbFormasDePago.ItemsSource = paymentForms;
            cmbFormasDePago.SelectedValuePath = "idFormaPago";
            cmbFormasDePago.DisplayMemberPath = "descripcion";
            cmbFormasDePago.SelectedIndex = -1;

            cmbCuentasBancarias.ItemsSource = bankAccounts;
            cmbCuentasBancarias.SelectedValuePath = "idCuentaBancaria";
            cmbCuentasBancarias.DisplayMemberPath = "numeroDeCuenta";
            cmbCuentasBancarias.SelectedIndex = -1;
        }

        private void SetEnvironment(StatusDeNotaDeCredito status, bool withRelatedDocument)
        {
            //Si esta en cualquier otro estado que no sea nueva, ya no puedo modificarla
            txtArticuloCodigo.IsEnabled = status.Equals(StatusDeNotaDeCredito.Nueva);
            txtArticuloPrecio.IsEnabled = status.Equals(StatusDeNotaDeCredito.Nueva);
            dgDetalle.IsReadOnly = !status.Equals(StatusDeNotaDeCredito.Nueva);

            //Las notas de credito para descuentos no tienen devolucion
            dgDetalle.IsEnabled = withRelatedDocument;
            txtArticuloCantidad.IsEnabled = withRelatedDocument;
            btnListarArticulos.IsEnabled = withRelatedDocument;
            txtArticuloCodigo.IsEnabled = withRelatedDocument;
            txtArticuloDescripcion.IsEnabled = withRelatedDocument;
            txtArticuloCantidad.IsEnabled = withRelatedDocument;
            txtArticuloPrecio.IsEnabled = withRelatedDocument;
            txtArticuloUnidad.IsEnabled = withRelatedDocument;
            btnImprimir.IsEnabled = status.Equals(StatusDeNotaDeCredito.Timbrada);

            switch (status)
            {
                case StatusDeNotaDeCredito.Nueva:
                    btnRegistrar.IsEnabled = true;
                    break;
                case StatusDeNotaDeCredito.Pendiente_De_Timbrado:
                    btnRegistrar.IsEnabled = true;
                    break;
                case StatusDeNotaDeCredito.Timbrada:
                    btnRegistrar.IsEnabled = false;
                    break;
                case StatusDeNotaDeCredito.Anulada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    break;
                case StatusDeNotaDeCredito.Cancelada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }
    }
}
