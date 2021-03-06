using Aprovi.Application.Helpers;
using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
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
    /// Interaction logic for PurchasesView.xaml
    /// </summary>
    public partial class PurchasesView : BaseView, IPurchasesView
    {
        public event Action FindSupplier;
        public event Action OpenSuppliersList;
        public event Action FindItem;
        public event Action OpenItemsList;
        public event Action AddItem;
        public event Action SelectItem;
        public event Action RemoveItem;
        public event Action ViewTaxDetails;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action New;
        public event Action Cancel;
        public event Action Save;
        public event Action OpenPayments;
        public event Action Print;
        public event Action OpenPurchaseOrdersList;
        public event Action FindPurchaseOrder;
        public event Action UpdateCharges;
        public event Action ImportCFDI;

        private Compra _purchase;
        private Proveedore _provider;
        private Articulo _currentItem;
        private ICollection<AbonosDeCompra> _purchasePayments;
        private VMOrdenDeCompra _purchaseOrder;

        public PurchasesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtProveedor.LostFocus += txtProveedor_LostFocus;
            txtFolio.GotFocus += txtFolio_GotFocus;
            btnListarProveedores.Click += btnListarProveedores_Click;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarCompras.Click += btnListarCompras_Click;
            txtArticuloCodigo.LostFocus += txtArticuloCodigo_LostFocus;
            btnListarArticulos.Click += btnListarArticulos_Click;
            txtArticuloCosto.PreviewKeyDown += txtArticuloCosto_PreviewKeyDown;
            dgDetalle.MouseDoubleClick += dgDetalle_MouseDoubleClick;
            dgDetalle.PreviewKeyUp += dgDetalle_PreviewKeyUp;
            cmbImpuestos.SelectionChanged += cmbImpuestos_SelectionChanged;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnAbonar.Click += btnAbonar_Click;
            btnRegistrar.Click += btnRegistrar_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnImportar.Click += BtnImportarOnClick;
            btnListarOrdenesDeCompra.Click += BtnListarOrdenesDeCompraOnClick; 
            txtOrdenDeCompra.LostFocus += TxtOrdenDeCompraOnLostFocus;
            txtCargos.LostFocus += OnUpdateCharges;
            txtDescuentos.LostFocus += OnUpdateCharges;

            _purchase = new VMCompra();;
            _provider = new Proveedore();
            _currentItem = new Articulo();
            _purchaseOrder = null;
            lblCancelada.Visibility = Visibility.Hidden;
        }

        private void BtnImportarOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (ImportCFDI.isValid())
            {
                ImportCFDI();
            }
        }

        private void OnUpdateCharges(object sender, RoutedEventArgs e)
        {
            if (UpdateCharges.isValid())
            {
                UpdateCharges();
            }
        }

        private void TxtOrdenDeCompraOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindPurchaseOrder.isValid())
            {
                FindPurchaseOrder();
            }
        }

        private void BtnListarOrdenesDeCompraOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenPurchaseOrdersList.isValid())
            {
                OpenPurchaseOrdersList();
            }
        }

        void txtArticuloCosto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (AddItem.isValid(AccesoRequerido.Ver_y_Agregar))
                    AddItem();

                txtArticuloCodigo.Focus();
            }
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnAbonar_Click(object sender, RoutedEventArgs e)
        {
            if (OpenPayments.isValid(AccesoRequerido.Ver_y_Agregar))
                OpenPayments();
        }

        private void txtFolio_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!txtProveedor.Text.isValid())
                OpenSuppliersList();
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                Save();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid(AccesoRequerido.Total))
                Cancel();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid(AccesoRequerido.Ver_y_Agregar))
                New();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void cmbImpuestos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewTaxDetails.isValid(AccesoRequerido.Ver))
                ViewTaxDetails();
        }

        private void dgDetalle_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveItem.isValid(AccesoRequerido.Ver_y_Agregar))
                RemoveItem();
        }

        private void dgDetalle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem.isValid(AccesoRequerido.Ver_y_Agregar))
                SelectItem();
        }

        private void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid(AccesoRequerido.Ver_y_Agregar))
                OpenItemsList();
        }

        private void txtArticuloCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindItem.isValid(AccesoRequerido.Ver_y_Agregar))
                FindItem();
        }

        private void btnListarCompras_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!_purchase.idOrdenDeCompra.isValid() && Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        private void btnListarProveedores_Click(object sender, RoutedEventArgs e)
        {
            if (OpenSuppliersList.isValid(AccesoRequerido.Ver))
                OpenSuppliersList();
        }

        private void txtProveedor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SelectedProviderChanged && FindSupplier.isValid(AccesoRequerido.Ver))
                FindSupplier();
        }

        public VMCompra Purchase
        {
            get
            {
                return new VMCompra()
                {
                    idCompra = _purchase.idCompra,
                    idProveedor = _provider.idProveedor,
                    Proveedore = SelectedProviderChanged?new Proveedore(){codigo = txtProveedor.Text} : _provider,
                    folio = txtFolio.Text,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    fechaHora = dpFecha.SelectedDate.Value,
                    OrdenesDeCompra = new OrdenesDeCompra() { folio = txtOrdenDeCompra.Text.ToIntOrDefault(), idOrdenDeCompra = _purchase.idOrdenDeCompra.GetValueOrDefault(0)},
                    idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                    Moneda = (Moneda)cmbMonedas.SelectedItem,
                    DetallesDeCompras = dgDetalle.Items.Cast<DetallesDeCompra>().ToList(),
                    Impuestos = cmbImpuestos.Items.Cast<VMImpuesto>().ToList(),
                    Abonado = txtAbonado.Text.ToDecimalOrDefault(),
                    Subtotal = txtSubtotal.Text.ToDecimalOrDefault(),
                    Total = txtTotal.Text.ToDecimalOrDefault(),
                    AbonosDeCompras = _purchasePayments,
                    idOrdenDeCompra = _purchase.idOrdenDeCompra,
                    cargos =  txtCargos.Text.ToDecimalOrDefault(),
                    descuentos = txtDescuentos.Text.ToDecimalOrDefault()
                };
            }
        }

        public DetallesDeCompra CurrentItem
        {
            get
            {
                //_currentItem.codigo = txtArticuloCodigo.Text;
                return new DetallesDeCompra()
                {
                    idArticulo = _currentItem.idArticulo,
                    Articulo = new Articulo(){idArticulo = _currentItem.idArticulo, codigo = txtArticuloCodigo.Text},
                    idUnidadDeMedida = cmbArticuloUnidadesDeMedida.SelectedValue == null ? -1 : cmbArticuloUnidadesDeMedida.SelectedValue.ToInt(),
                    UnidadesDeMedida = (UnidadesDeMedida)cmbArticuloUnidadesDeMedida.SelectedItem,
                    cantidad = txtArticuloCantidad.Text.ToDecimalOrDefault(),
                    costoUnitario = txtArticuloCosto.Text.ToDecimalOrDefault()
                };
            }
        }

        public bool IsDirty { get { return _purchase.isValid() && _purchase.idCompra.isValid() && _purchase.folio == txtFolio.Text && !SelectedProviderChanged; } }

        public DetallesDeCompra SelectedItem { get { return dgDetalle.SelectedIndex >= 0 ? (DetallesDeCompra)dgDetalle.SelectedItem : new DetallesDeCompra(); } }

        public VMImpuesto SelectedTax { get { return cmbImpuestos.SelectedIndex >= 0 ? (VMImpuesto)cmbImpuestos.SelectedItem : new VMImpuesto(); } }

        private bool SelectedProviderChanged => (!_provider.isValid() || txtProveedor.Text.isValid() && _provider.codigo != txtProveedor.Text);

        public VMOrdenDeCompra PurchaseOrder { get { return _purchaseOrder;} }

        public void Show(VMCompra purchase)
        {
            txtProveedor.Text = purchase.Proveedore.codigo;
            lblProveedorRazonSocial.Content = purchase.Proveedore.razonSocial;
            txtFolio.Text = purchase.folio;
            txtTipoDeCambio.Text = purchase.tipoDeCambio.ToDecimalString();
            dpFecha.SelectedDate = purchase.fechaHora;
            txtOrdenDeCompra.Text = purchase.OrdenesDeCompra.isValid() && purchase.OrdenesDeCompra.folio.isValid()?purchase.OrdenesDeCompra.folio.ToString():"";
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            cmbArticuloUnidadesDeMedida.ItemsSource = null;
            txtArticuloCantidad.Clear();
            txtArticuloCosto.Clear();
            _currentItem = new Articulo();
            dgDetalle.ItemsSource = purchase.DetallesDeCompras.ToList();
            txtAbonado.Text = purchase.Abonado.ToDecimalString();
            txtSaldo.Text = purchase.Saldo.ToDecimalString();
            txtSubtotal.Text = purchase.Subtotal.ToDecimalString();
            cmbImpuestos.ItemsSource = purchase.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;
            cmbMonedas.SelectedValue = purchase.idMoneda;
            txtTotal.Text = purchase.Total.ToDecimalString();
            txtCargos.Text = purchase.cargos.ToDecimalString();
            txtDescuentos.Text = purchase.descuentos.ToDecimalString();
            _purchase = purchase;
            _provider = purchase.Proveedore;
            _purchasePayments = purchase.AbonosDeCompras;

            //No se utilizan las validaciones de orden de compra cuando no viene de una orden de compra
            if (!purchase.idOrdenDeCompra.isValid())
            {
                _purchaseOrder = null;
            }

            lblCancelada.Visibility = purchase.idEstatusDeCompra.Equals((int)StatusDeCompra.Cancelada) ? Visibility.Visible : Visibility.Hidden;
        }

        public void Show(VMOrdenDeCompra purchaseOrder, List<DetallesDeCompra> detail)
        {
            _purchaseOrder = purchaseOrder;

            txtProveedor.Text = purchaseOrder.Proveedore.codigo;
            lblProveedorRazonSocial.Content = purchaseOrder.Proveedore.razonSocial;
            txtTipoDeCambio.Text = purchaseOrder.tipoDeCambio.ToDecimalString();
            txtOrdenDeCompra.Text = purchaseOrder.folio.ToString();
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            cmbArticuloUnidadesDeMedida.ItemsSource = null;
            txtArticuloCantidad.Clear();
            txtArticuloCosto.Clear();
            _currentItem = new Articulo();
            txtAbonado.Text = purchaseOrder.Abonado.ToDecimalString();
            txtSaldo.Text = purchaseOrder.Saldo.ToDecimalString();
            txtSubtotal.Text = purchaseOrder.Subtotal.ToDecimalString();
            dgDetalle.ItemsSource = detail;
            cmbImpuestos.ItemsSource = purchaseOrder.Impuestos;
            cmbImpuestos.SelectedValuePath = "idImpuesto";
            cmbImpuestos.DisplayMemberPath = "Descripcion";
            cmbImpuestos.SelectedValue = 0;
            cmbMonedas.SelectedValue = purchaseOrder.idMoneda;
            txtTotal.Text = purchaseOrder.Total.ToDecimalString();
            _provider = purchaseOrder.Proveedore;

            _purchasePayments = new List<AbonosDeCompra>();

            //Se debe guardar la relacion a la orden de compra
            _purchase = new Compra(){idOrdenDeCompra = purchaseOrder.idOrdenDeCompra};
        }

        public void Clear()
        {
            txtProveedor.Clear();
            lblProveedorRazonSocial.Content = string.Empty;
            txtFolio.Clear();
            txtTipoDeCambio.Clear();
            dpFecha.SelectedDate = DateTime.Now;
            txtOrdenDeCompra.Clear();
            cmbMonedas.SelectedIndex = 0;
            ClearItem();
            dgDetalle.ItemsSource = null;
            txtSubtotal.Clear();
            cmbImpuestos.ItemsSource = null;
            txtImpuestos.Clear();
            txtTotal.Clear();
            txtAbonado.Clear();
            txtSaldo.Clear();
            txtCargos.Clear();
            txtDescuentos.Clear();

            _purchase = new VMCompra();
            _provider = new Proveedore();
            lblCancelada.Visibility = Visibility.Hidden;
            txtProveedor.Focus();
            _purchaseOrder = null;
        }

        public void Show(DetallesDeCompra currentItem, List<UnidadesDeMedida> unitsOfMeasure)
        {
            txtArticuloCodigo.Text = currentItem.Articulo.codigo;
            txtArticuloDescripcion.Text = currentItem.Articulo.descripcion;
            cmbArticuloUnidadesDeMedida.ItemsSource = unitsOfMeasure;
            cmbArticuloUnidadesDeMedida.SelectedValuePath = "idUnidadDeMedida";
            cmbArticuloUnidadesDeMedida.DisplayMemberPath = "descripcion";
            cmbArticuloUnidadesDeMedida.SelectedItem = currentItem.UnidadesDeMedida;
            txtArticuloCantidad.Text = "1.0";
            txtArticuloCosto.Text = currentItem.costoUnitario.ToDecimalString();
            _currentItem = currentItem.Articulo;
        }

        public void ClearItem()
        {
            txtArticuloCodigo.Clear();
            txtArticuloDescripcion.Clear();
            cmbArticuloUnidadesDeMedida.ItemsSource = null;
            txtArticuloCantidad.Clear();
            txtArticuloCosto.Clear();
            _currentItem = new Articulo();

            txtArticuloCodigo.Focus();
        }

        public void Show(VMImpuesto tax)
        {
            txtImpuestos.Text = tax.Importe.ToDecimalString();
        }

        public void FillMonedas(List<Moneda> currencies)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
        }

    }
}
