using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for BillsOfLadingView.xaml
    /// </summary>
    public partial class QuotesView : BaseView, IQuotesView
    {

        public event Action FindClient;
        public event Action OpenClientsList;
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
        public event Action Cancel;
        public event Action New;
        public event Action Print;
        public event Action Save;
        public event Action Update;
        public event Action OpenNote;
        public event Action ChangeCurrency;
        public event Action Unlink;
        
        private int _idQuote;
        private Cliente _client;
        private Articulo _currentItem;
        private VMCotizacion _quote;
        private List<DatosExtraPorCotizacion> _extraData = new List<DatosExtraPorCotizacion>();
        private Moneda _currentCurrency;

        public QuotesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += BillOfSaleView_Loaded;
            txtCliente.LostFocus += txtCliente_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarCotizaciones.Click += btnListarCotizaciones_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            txtArticuloCodigo.PreviewKeyDown += TxtArticuloCodigo_PreviewKeyDown;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCantidad.GotFocus += txtArticuloCantidad_GotFocus;
            txtArticuloPrecio.PreviewKeyDown += txtArticuloPrecio_PreviewKeyDown;
            dgDetalle.PreviewKeyUp += dgDetalle_PreviewKeyUp;
            dgDetalle.MouseDoubleClick += dgDetalle_MouseDoubleClick;
            dgDetalle.PreviewMouseRightButtonUp += DgDetalle_PreviewMouseRightButtonUp;
            cmbImpuestos.SelectionChanged += cmbImpuestos_SelectionChanged;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnRegistrar.Click += btnRegistrar_Click;
            btnNota.Click += BtnNotaOnClick;
            cmbMonedas.SelectionChanged += CmbMonedasOnSelectionChanged;
            btnDesvincular.Click += BtnDesvincularOnClick;
        }

        private void BtnDesvincularOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Unlink.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "QuotesPresenter", true))
            {
                Unlink();
            }
        }

        private void CmbMonedasOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ChangeCurrency.isValid())
            {
                ChangeCurrency();
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

        void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDirty)
            {
                if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                    Save();
            }
            else
            {
                if (Update.isValid(AccesoRequerido.Total))
                {
                    Update();
                }
            }
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid(AccesoRequerido.Ver))
                Print();
        }

        void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid(AccesoRequerido.Total))
                Cancel();
        }

        void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void cmbImpuestos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewTaxDetails.isValid())
                ViewTaxDetails();
        }

        void dgDetalle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem.isValid())
                SelectItem();
        }

        void dgDetalle_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveItem.isValid(AccesoRequerido.Ver_y_Agregar))
                RemoveItem();
        }

        void txtArticuloPrecio_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddItem.isValid())
            {
                e.Handled = true;
                AddItem();
                txtArticuloCodigo.Focus();
            }
        }

        void txtArticuloCantidad_GotFocus(object sender, RoutedEventArgs e)
        {
            //Al perder el foco del txtArticuloCodigo llega aquí
            //Si el código del articulo esta en blanco enfocar botón Registrar
            if (!txtArticuloCodigo.Text.isValid())
            {
                btnRegistrar.Focus();
                return;
            }

            //Si el articulo no existe enfoco nuevamente código de articulo
            if (!txtArticuloCantidad.Text.ToDecimalOrDefault().isValid())
            {
                txtArticuloCodigo.Focus();
                return;
            }
        }

        void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid())
                OpenItemsList();
        }

        private void TxtArticuloCodigo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.F2))
                OpenItemsList();
        }

        void txtArticuloCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid())
                FindItem();
        }

        void btnListarCotizaciones_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenClientsList.isValid())
                OpenClientsList();
        }

        void txtCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomerChanged && FindClient.isValid())
                FindClient();
        }

        void BillOfSaleView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        public VMCotizacion Quote
        {
            get
            {
                //_client.codigo = txtCliente.Text;
                return IsDirty
                    ? _quote
                    : new VMCotizacion() //Cuando es una cotizacion ya registrada, regreso el registro completo que se cargo
                    {
                        idCotizacion = _idQuote,
                        Cliente = SelectedCustomerChanged?new Cliente(){codigo = txtCliente.Text}:_client,
                        idCliente = _client.idCliente,
                        folio = txtFolio.Text.ToIntOrDefault(),
                        idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                        Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda) cmbMonedas.SelectedItem : new Moneda(),
                        fechaHora = dpFecha.SelectedDate.Value,
                        tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                        DetalleDeCotizacion = dgDetalle.Items.Cast<VMDetalleDeCotizacion>().ToList(),
                        DatosExtraPorCotizacions = _extraData,
                        idEmpresa = _quote.isValid() && _quote.idCotizacion.isValid() ? _quote.idEmpresa : 0,
                        idEstatusDeCotizacion = _quote.isValid() && _quote.idCotizacion.isValid() ? _quote.idEstatusDeCotizacion : 0,
                    };
            }
        }

        public bool IsDirty
        {
            get { return _quote.isValid() && _quote.idCotizacion.isValid() && _quote.folio == txtFolio.Text.ToIntOrDefault() && _quote.idMoneda == cmbMonedas.SelectedValue.ToIntOrDefault() && !SelectedCustomerChanged; }
        }

        public VMDetalleDeCotizacion CurrentItem
        {
            get
            {
                return new VMDetalleDeCotizacion()
                {
                    idArticulo = _currentItem.idArticulo,
                    Articulo = new Articulo()
                    {
                        idArticulo = _currentItem.isValid()?_currentItem.idArticulo:0,
                        codigo = txtArticuloCodigo.Text
                    },
                    cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                    precioUnitario = txtArticuloPrecio.Text.ToDecimalOrDefault()
                };
            }
        }

        private bool SelectedCustomerChanged => (!_client.isValid() || _client.codigo != txtCliente.Text);

        public VMImpuesto SelectedTax
        {
            get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto) cmbImpuestos.SelectedItem : new VMImpuesto(); }
        }

        public Moneda LastCurrency
        {
            get {
                return _currentCurrency;
            }
        }

        public VMDetalleDeCotizacion SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (VMDetalleDeCotizacion) dgDetalle.SelectedItem : null; }
        }

        public void ShowStock(decimal existencia)
        {
            lblExistencia.Content = existencia.ToDecimalString();
        }

        public void Show(VMCotizacion quote)
        {
            txtCliente.Text = quote.Cliente.codigo;
            lblClienteRazonSocial.Content = quote.Cliente.razonSocial;

            if (quote.Moneda.isValid())
                Show(quote.Moneda);

            txtTipoDeCambio.Text = quote.tipoDeCambio.ToDecimalString();
            txtFolio.Text = quote.folio.ToString();
            dpFecha.SelectedDate = quote.fechaHora;
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloPrecio.Clear();
            _currentItem = new Articulo();

            //No se actualiza al editar si no se resetea el itemssource
            dgDetalle.ItemsSource = null;
            dgDetalle.ItemsSource = quote.DetalleDeCotizacion;

            txtSubtotal.Text = quote.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = quote.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;

            txtTotal.Text = quote.Total.ToDecimalString();

            _idQuote = quote.idCotizacion;
            _client = quote.Cliente;

            //Cuando ya es una existente
            _quote = quote;
            _extraData = quote.DatosExtraPorCotizacions.ToList();

            //Se reinicia la etiqueta del estado
            lblEstadoCotizacion.Content = "";

            if (quote.Factura.isValid() && quote.Factura.idFactura.isValid())
                lblEstadoCotizacion.Content = String.Format("Facturada con folio {0}{1}", quote.Factura.serie, quote.Factura.folio);

            if (quote.Remisione.isValid() && quote.Remisione.idRemision.isValid())
            {
                lblEstadoCotizacion.Content = string.Format("Remisionada con folio {0}", quote.Remisione.folio);
            }

            SetEnvironment((StatusDeCotizacion)quote.idEstatusDeCotizacion);
        }

        public void Show(Moneda currency)
        {
            _currentCurrency = currency;
            cmbMonedas.SelectedItem = currency;
        }

        public void Show(VMDetalleDeCotizacion detail)
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
            lblExistencia.Content = "";
            _currentItem = new Articulo();
        }

        public void FillCombos(List<Moneda> currencies)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;

            //Se establece la moneda por defecto
            _currentCurrency = (Moneda)cmbMonedas.SelectedItem;
        }

        private void SetEnvironment(StatusDeCotizacion status)
        {
            //Si esta en cualquier otro estado que no sea nueva, ya no puedo modificarla
            txtArticuloCodigo.IsEnabled = status.Equals(StatusDeCotizacion.Nueva) || status.Equals(StatusDeCotizacion.Registrada);
            txtArticuloPrecio.IsEnabled = status.Equals(StatusDeCotizacion.Nueva) || status.Equals(StatusDeCotizacion.Registrada);
            dgDetalle.IsReadOnly = !(status.Equals(StatusDeCotizacion.Nueva) || status.Equals(StatusDeCotizacion.Registrada));
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            lblEstadoCotizacion.Visibility = System.Windows.Visibility.Hidden;
            btnCancelar.IsEnabled = true;

            switch (status)
            {
                case StatusDeCotizacion.Nueva:
                    btnRegistrar.IsEnabled = true;
                    btnCancelar.IsEnabled = false;
                    break;
                case StatusDeCotizacion.Registrada:
                    btnRegistrar.IsEnabled = true;
                    break;
                case StatusDeCotizacion.Facturada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    lblEstadoCotizacion.Visibility = System.Windows.Visibility.Visible;
                    break;
                case StatusDeCotizacion.Remisionada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    lblEstadoCotizacion.Visibility = System.Windows.Visibility.Visible;
                    break;
                case StatusDeCotizacion.Cancelada:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    lblCancelada.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
