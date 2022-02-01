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
    public partial class PurchaseOrdersView : BaseView, IPurchaseOrdersView
    {

        public event Action FindProvider;
        public event Action OpenProvidersList;
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

        private int _idOrder;
        private Proveedore _provider;
        private int _idArticulo;
        private VMOrdenDeCompra _order;
        private List<DatosExtraPorOrdenDeCompra> _extraData = new List<DatosExtraPorOrdenDeCompra>();
        private VMDetalleDeOrdenDeCompra _currentItem;

        public PurchaseOrdersView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += PurchaseOrdersView_Loaded;
            txtProveedor.LostFocus += txtProveedor_LostFocus;
            btnListarProveedores.Click += btnListarProveedores_Click;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarPedidos.Click += btnListarPedidos_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
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
        }

        private void TxtArticuloSurtidoOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            VMDetalleDeOrdenDeCompra currentItem = CurrentItem;

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
            if (SelectItem.isValid() && Order.idEstatusDeOrdenDeCompra != (int)StatusDeOrdenDeCompra.Surtido_Total)
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

        void btnListarProveedores_Click(object sender, RoutedEventArgs e)
        {
            if (OpenProvidersList.isValid())
                OpenProvidersList();
        }

        void txtProveedor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SelectedProviderChanged && FindProvider.isValid())
                FindProvider();
        }

        void PurchaseOrdersView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid())
                Load();
        }

        public VMOrdenDeCompra Order
        {
            get
            {
                return IsDirty
                    ? _order
                    : new VMOrdenDeCompra() //Cuando es un pedido ya registrado, regreso el registro completo que se cargo
                    {
                        idOrdenDeCompra = _idOrder,
                        Proveedore = SelectedProviderChanged?new Proveedore(){codigo = txtProveedor.Text} : _provider,
                        idProveedor = _provider.idProveedor,
                        folio = txtFolio.Text.ToIntOrDefault(),
                        idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                        Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda) cmbMonedas.SelectedItem : new Moneda(),
                        fechaHora = dpFecha.SelectedDate.Value,
                        tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                        Detalles = dgDetalle.Items.Cast<VMDetalleDeOrdenDeCompra>().ToList(),
                        DatosExtraPorOrdenDeCompras = _extraData,
                    };
            }
        }

        public bool IsDirty
        {
            get { return _order.isValid() && _order.idOrdenDeCompra.isValid() && _order.folio == txtFolio.Text.ToIntOrDefault() && !SelectedProviderChanged; }
        }

        public bool IsEditable
        {
            get
            {
                return (_idOrder.isValid() && _order.isValid() &&
                       _order.idEstatusDeOrdenDeCompra == (int) StatusDeOrdenDeCompra.Registrado)||(!_idOrder.isValid());
            }
        }

        public VMDetalleDeOrdenDeCompra CurrentItem
        {
            get
            {
                //_currentItem.codigo = txtArticuloCodigo.Text;
                return new VMDetalleDeOrdenDeCompra()
                {
                    CodigoArticulo = txtArticuloCodigo.Text,
                    idArticulo = _idArticulo,
                    Cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                    CostoUnitario = txtArticuloPrecio.Text.ToDecimalOrDefault(),
                    idUnidadArticulo = cbArticuloUnidad.SelectedValue.ToIntOrDefault(),
                    Surtido = txtArticuloSurtido.Text.ToDecimalOrDefault(),
                    Comentario = _currentItem.isValid()?_currentItem.Comentario:"",
                    DescripcionArticulo = txtArticuloDescripcion.Text,
                    surtidoEnOtros = _currentItem.isValid()?_currentItem.surtidoEnOtros:0m,
                    UnidadDeMedida = cbArticuloUnidad.SelectedItem.isValid()?((UnidadesDeMedida)cbArticuloUnidad.SelectedItem):null
                };
            }
        }

        private bool SelectedProviderChanged => (!_order.isValid() || txtProveedor.Text.isValid() && _provider.codigo != txtProveedor.Text);

        public VMImpuesto SelectedTax
        {
            get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto) cmbImpuestos.SelectedItem : new VMImpuesto(); }
        }

        public VMDetalleDeOrdenDeCompra SelectedItem
        {
            get { return dgDetalle.SelectedIndex >= 0 ? (VMDetalleDeOrdenDeCompra) dgDetalle.SelectedItem : null; }
        }

        public void ShowStock(decimal existencia)
        {
            lblExistencia.Content = existencia.ToDecimalString();
        }

        public void Show(VMOrdenDeCompra order)
        {
            txtProveedor.Text = order.Proveedore.codigo;
            lblProveedorRazonSocial.Content = order.Proveedore.razonSocial;

            if (order.Moneda.isValid())
                cmbMonedas.SelectedItem = order.Moneda;

            txtTipoDeCambio.Text = order.tipoDeCambio.ToDecimalString();
            txtFolio.Text = order.folio.ToString();
            dpFecha.SelectedDate = order.fechaHora;
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloCantidad.Clear();
            cbArticuloUnidad.SelectedIndex = -1;
            txtArticuloPrecio.Clear();
            txtArticuloPendiente.Clear();
            txtArticuloSurtido.Clear();
            _currentItem = null;

            dgDetalle.ItemsSource = null;
            dgDetalle.ItemsSource = order.Detalles;
            txtSubtotal.Text = order.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = order.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;

            txtTotal.Text = order.Total.ToDecimalString();

            _idOrder = order.idOrdenDeCompra;
            _provider = order.Proveedore;

            //Llena la tabla de operaciones relacionadas
            List<VMOperacion> operations = new List<VMOperacion>();

            if (order.isValid() && order.Compras.isValid())
            {
                order.Compras.ForEach(x => operations.Add(new VMOperacion(x)));
            }

            dgOperaciones.ItemsSource = null;
            dgOperaciones.ItemsSource = operations.OrderBy(x=>x.FechaHora).ToList();

            //Cuando ya es una existente
            _order = order;
            _extraData = order.DatosExtraPorOrdenDeCompras.ToList();

            //Si ya esta surtida deshabilito el boton de registrar

            SetEnvironment((StatusDeOrdenDeCompra)order.idEstatusDeOrdenDeCompra);
        }

        public void Show(VMDetalleDeOrdenDeCompra detail, List<UnidadesDeMedida> measureUnits)
        {
            txtArticuloCodigo.Text = detail.CodigoArticulo;
            txtArticuloDescripcion.Text = detail.DescripcionArticulo;
            txtArticuloCantidad.Text = detail.Cantidad.ToDecimalString();
            txtArticuloPrecio.Text = detail.CostoUnitario.ToDecimalString();
            txtArticuloSurtido.Text = detail.Surtido.ToDecimalString();
            txtArticuloPendiente.Text = detail.Pendiente.ToDecimalString();
            _idArticulo = detail.idArticulo;
            _currentItem = detail;

            cbArticuloUnidad.ItemsSource = measureUnits;
            cbArticuloUnidad.SelectedValuePath = "idUnidadDeMedida";
            cbArticuloUnidad.DisplayMemberPath = "descripcion";
            cbArticuloUnidad.SelectedValue = detail.idUnidadArticulo;

            //Si se esta surtiendo el pedido, se hace focus al textbox de surtido
            if (_order.isValid() && _order.isValid() && (_order.idEstatusDeOrdenDeCompra == (int) StatusDePedido.Registrado ||
                                                         _order.idEstatusDeOrdenDeCompra ==
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
            cbArticuloUnidad.SelectedIndex = -1;
            txtArticuloCantidad.Clear();
            txtArticuloDescripcion.Clear();
            txtArticuloSurtido.Clear();
            txtArticuloPendiente.Clear();
            lblExistencia.Content = "";
            _idArticulo = 0;
        }

        public void FillCombos(List<Moneda> currencies)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
        }

        private void SetEnvironment(StatusDeOrdenDeCompra status)
        {
            txtArticuloCodigo.IsEnabled = !status.Equals(StatusDeOrdenDeCompra.Surtido_Total);
            txtArticuloPrecio.IsEnabled = !status.Equals(StatusDeOrdenDeCompra.Surtido_Total);
            cbArticuloUnidad.IsTabStop = status.Equals(StatusDeOrdenDeCompra.Nuevo);
            cbArticuloUnidad.IsEnabled = status.Equals(StatusDeOrdenDeCompra.Nuevo);
            txtArticuloPendiente.IsReadOnly = true;
            dgDetalle.IsReadOnly = status.Equals(StatusDeOrdenDeCompra.Surtido_Total);
            lblCancelada.Visibility = System.Windows.Visibility.Hidden;
            lblFacturada.Visibility = System.Windows.Visibility.Hidden;
            btnCancelar.IsEnabled = true;

            switch (status)
            {
                case StatusDeOrdenDeCompra.Nuevo:
                    btnRegistrar.IsEnabled = true;
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDeOrdenDeCompra.Registrado:
                    txtArticuloCantidad.IsReadOnly = false;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDeOrdenDeCompra.Surtido_Parcial:
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = true;
                    txtArticuloSurtido.IsReadOnly = false;
                    break;
                case StatusDeOrdenDeCompra.Surtido_Total:
                    btnRegistrar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;
                    txtArticuloCantidad.IsReadOnly = true;
                    txtArticuloSurtido.IsReadOnly = true;
                    break;
                case StatusDeOrdenDeCompra.Cancelado:
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
