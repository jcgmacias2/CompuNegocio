using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.ObjectBuilder2;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : BaseView, IOrdersView
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
        public event Action OpenNote;
        public event Action Update;
        public event Action ChangeCurrency;
        public event Action OpenUsersList;
        public event Action FindUser;

        private int _idOrder;
        private Cliente _client;
        private int _idArticulo;
        private VMPedido _order;
        private Moneda _lastCurrency;
        private Usuario _seller;
        private List<DatosExtraPorPedido> _extraData = new List<DatosExtraPorPedido>();
        private VMDetalleDePedido _currentItem;

        public OrdersView()
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
            btnListarPedidos.Click += btnListarPedidos_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            txtArticuloCodigo.PreviewKeyDown += TxtArticuloCodigo_PreviewKeyDown;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCantidad.GotFocus += txtArticuloCantidad_GotFocus;
            txtArticuloPrecio.PreviewKeyDown += txtArticuloPrecio_PreviewKeyDown;
            txtArticuloSurtido.LostFocus += TxtArticuloSurtidoOnLostFocus;
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
            txtVendedor.LostFocus += TxtVendedorOnLostFocus;
            btnListarVendedores.Click += BtnListarVendedoresOnClick;

            //Vendedor
            _seller = new Usuario();
            _order = new VMPedido();
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

        private void CmbMonedasOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ChangeCurrency.isValid())
            {
                ChangeCurrency();
            }
        }

        private void TxtArticuloSurtidoOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            VMDetalleDePedido currentItem = CurrentItem;

            //Se valida que la cantidad de orden y surtido sean validas
            if (currentItem.Cantidad.isValid() && currentItem.Surtido.isValid())
            {
                txtArticuloPendiente.Text = (currentItem.Cantidad - currentItem.surtidoEnOtros - currentItem.Surtido).ToDecimalString();
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
            if (SelectItem.isValid() && Order.idEstatusDePedido != (int)StatusDePedido.Surtido_Total)
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
            //Si el código del articulo esta en blanco enfocar botón Pagar
            if (!txtArticuloCodigo.Text.isValid())
            {
                //cmbAbonoFormasPago.Focus();
                //btnRegistrar.Focus();
                return;
            }

            //Si el articulo no existe enfoco nuevamente código de articulo
            if (!txtArticuloCantidad.Text.isValid())
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
            if (FindItem.isValid() && IsEditable)
                FindItem();
        }

        void btnListarPedidos_Click(object sender, RoutedEventArgs e)
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
            //Se activa al cargar una remision si no se usa la condicion del idCliente
            if (SelectedCustomerChanged && FindClient.isValid())
                FindClient();
        }

        void BillOfSaleView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        public VMPedido Order
        {
            get
            {
                return new VMPedido()
                {
                    idPedido = _idOrder,
                    //Si se cambia el codigo, se regresa el objeto con el codigo ingresado en lugar del seleccionado
                    Cliente = SelectedCustomerChanged?new Cliente(){codigo = txtCliente.Text}:_client,
                    idCliente = _client.idCliente,
                    folio = txtFolio.Text.ToIntOrDefault(),
                    idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                    Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda) cmbMonedas.SelectedItem : new Moneda(),
                    fechaHora = dpFecha.SelectedDate.Value,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    Detalles = dgDetalle.Items.Cast<VMDetalleDePedido>().ToList(),
                    DatosExtraPorPedidoes = _extraData,
                    Vendedor = new Usuario() { nombreDeUsuario = txtVendedor.Text, idUsuario = _seller.idUsuario },
                    idEstatusDePedido = _order.isValid() && _order.idPedido.isValid() ? _order.idEstatusDePedido : (int)StatusDePedido.Nuevo,
                    ordenDeCompra = txtOrdenDeCompra.Text
                };
            }
        }

        public bool IsDirty
        {
            get { return _order.isValid() && _order.idPedido.isValid() && _order.folio == txtFolio.Text.ToIntOrDefault() && !SelectedCustomerChanged; }
        }

        public bool IsEditable
        {
            get
            {
                return (_idOrder.isValid() && _order.isValid() &&
                       _order.idEstatusDePedido != (int) StatusDePedido.Cancelado)||(!_idOrder.isValid());
            }
        }

        public VMDetalleDePedido CurrentItem
        {
            get
            {
                //_currentItem.codigo = txtArticuloCodigo.Text;
                return new VMDetalleDePedido()
                {
                    CodigoArticulo = txtArticuloCodigo.Text,
                    idArticulo = _idArticulo,
                    Cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                    PrecioUnitario = txtArticuloPrecio.Text.ToDecimalOrDefault(),
                    Surtido = txtArticuloSurtido.Text.ToDecimalOrDefault(),
                    Comentario = _currentItem.isValid()?_currentItem.Comentario:"",
                    DescripcionArticulo = txtArticuloDescripcion.Text,
                    DescripcionUnidad = txtArticuloUnidad.Text,
                    surtidoEnOtros = _currentItem.isValid()?_currentItem.surtidoEnOtros:0m,
                };
            }
        }

        private bool SelectedCustomerChanged => (!_client.isValid() || txtCliente.Text.isValid() && _client.codigo != txtCliente.Text);

        public VMImpuesto SelectedTax
        {
            get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto) cmbImpuestos.SelectedItem : new VMImpuesto(); }
        }

        public VMDetalleDePedido SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (VMDetalleDePedido) dgDetalle.SelectedItem : null; }
        }

        public Moneda LastCurrency
        {
            get { return _lastCurrency; }
        }

        public Opciones_Pedido SelectedOption
        {
            get { return cmbOperaciones.SelectedIndex.isValid()?(Opciones_Pedido)cmbOperaciones.SelectedItem:default(Opciones_Pedido); }
        }

        public void ShowStock(decimal existencia)
        {
            lblExistencia.Content = existencia.ToDecimalString();
        }

        public void Show(VMPedido order)
        {
            txtCliente.Text = order.Cliente.codigo;
            lblClienteRazonSocial.Content = order.Cliente.razonSocial;

            if (order.Moneda.isValid())
                Show(order.Moneda);

            if (_order.isValid() && order.Vendedor.isValid())
            {
                //Se muestra el vendedor de la orden
                Show(order.Vendedor);
            }
            else
            {
                //Si el cliente tiene un vendedor asignado
                if (order.Cliente.isValid() && order.Cliente.Usuario.isValid())
                {
                    Show(order.Cliente.Usuario);
                }
            }

            txtTipoDeCambio.Text = order.tipoDeCambio.ToDecimalString();
            txtFolio.Text = order.folio.ToString();
            dpFecha.SelectedDate = order.fechaHora;
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidad.Clear();
            txtArticuloUnidad.Clear();
            txtArticuloPrecio.Clear();
            txtArticuloPendiente.Clear();
            txtArticuloSurtido.Clear();
            _currentItem = null;
            txtOrdenDeCompra.Text = order.ordenDeCompra;

            dgDetalle.ItemsSource = null;
            dgDetalle.ItemsSource = order.Detalles;
            txtSubtotal.Text = order.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = order.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;

            txtTotal.Text = order.Total.ToDecimalString();

            _idOrder = order.idPedido;
            _client = order.Cliente;

            //Llena la tabla de operaciones relacionadas
            List<VMOperacion> operations = new List<VMOperacion>();

            if (order.isValid() && order.Facturas.isValid())
            {
                order.Facturas.ForEach(x => operations.Add(new VMOperacion(x)));
            }

            if (order.isValid() && order.Remisiones.isValid())
            {
                order.Remisiones.ForEach(x => operations.Add(new VMOperacion(x)));
            }

            dgOperaciones.ItemsSource = null;
            dgOperaciones.ItemsSource = operations.OrderBy(x=>x.FechaHora).ToList();

            //Cuando ya es una existente
            _order = order;
            _extraData = order.DatosExtraPorPedidoes.ToList();

            //Si ya esta surtida deshabilito el boton de registrar

            SetEnvironment((StatusDePedido)order.idEstatusDePedido);
        }

        public void Show(Usuario user)
        {
            txtVendedor.Text = user.nombreDeUsuario;
            _seller = user;
        }

        public void Show(Moneda currency)
        {
            cmbMonedas.SelectedItem = currency;
            _lastCurrency = currency;
        }

        public void Show(VMDetalleDePedido detail)
        {
            txtArticuloCodigo.Text = detail.CodigoArticulo;
            txtArticuloDescripcion.Text = detail.DescripcionArticulo;
            txtArticuloCantidad.Text = detail.Cantidad.ToDecimalString();
            txtArticuloUnidad.Text = detail.DescripcionUnidad;
            txtArticuloPrecio.Text = detail.PrecioUnitario.ToDecimalString();
            txtArticuloSurtido.Text = detail.Surtido.ToDecimalString();
            txtArticuloPendiente.Text = detail.Pendiente.ToDecimalString();
            _idArticulo = detail.idArticulo;
            _currentItem = detail;

            //Si se esta surtiendo el pedido, se hace focus al textbox de surtido
            if (_order.isValid() && _order.isValid() && (_order.idEstatusDePedido == (int) StatusDePedido.Registrado ||
                                                         _order.idEstatusDePedido ==
                                                         (int) StatusDePedido.Surtido_Parcial))
            {
                txtArticuloSurtido.Focus();
            }
            else
            {
                txtArticuloCantidad.Focus();
            }
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
            txtArticuloSurtido.Clear();
            txtArticuloPendiente.Clear();
            lblExistencia.Content = "";
            _idArticulo = 0;
        }

        public void FillCombos(List<Moneda> currencies, List<Opciones_Pedido> transactions)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
            _lastCurrency = (Moneda)cmbMonedas.SelectedItem;
            cmbOperaciones.ItemsSource = transactions;
        }

        private void SetEnvironment(StatusDePedido status)
        {
            txtArticuloCodigo.IsEnabled = !status.Equals(StatusDePedido.Cancelado);
            txtArticuloPrecio.IsEnabled = !status.Equals(StatusDePedido.Cancelado);
            txtArticuloPendiente.IsReadOnly = true;
            dgDetalle.IsReadOnly = status.Equals(StatusDePedido.Cancelado);
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            lblFacturada.Visibility = System.Windows.Visibility.Hidden;
            btnCancelar.IsEnabled = true;

            switch (status)
            {
                case StatusDePedido.Nuevo:
                    btnRegistrar.IsEnabled = true;
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDePedido.Registrado:
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDePedido.Surtido_Parcial:
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDePedido.Surtido_Total:
                    btnRegistrar.IsEnabled = true;
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = true;
                    break;
                case StatusDePedido.Cancelado:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    lblCancelada.Visibility = System.Windows.Visibility.Visible;
                    txtArticuloCantidad.IsReadOnly = true;
                    txtArticuloSurtido.IsReadOnly = true;
                    break;
                default:
                    break;
            }
        }
    }
}
