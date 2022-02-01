using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ItemsView.xaml
    /// </summary>
    public partial class ItemsView : BaseView, IItemsView
    {
        public event Action Quit;
        public event Action New;
        public event Action Find;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action OpenList;
        public event Action OpenEquivalencies;
        public event Action AddClassification;
        public event Action DeleteClassification;
        public event Action AddTax;
        public event Action CalculateByUtility;
        public event Action FindProductService;
        public event Action OpenProductServiceList;
        public event Action DeleteTax;
        public event Action ChangeCurrency;
        public event Action AddSupplierCode;
        public event Action AddCustomerCode;
        public event Action DeleteSupplierCode;
        public event Action DeleteCustomerCode;
        public event Action FindSupplier;
        public event Action OpenSuppliersList;
        public event Action OpenCustomersList;
        public event Action FindCustomer;

        private int _idItem;
        private int _idProductService;
        private VMArticulo _item;
        private Moneda _lastCurrency;
        private Proveedore _supplier;
        private Cliente _customer;

        public ItemsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarArticulos.Click += btnListarArticulos_Click;
            btnEquivalencias.Click += btnEquivalencias_Click;
            btnAgregarClasificacion.Click += btnAgregarClasificacion_Click;
            btnAgregarImpuesto.Click += btnAgregarImpuesto_Click;
            dgClasificaciones.PreviewKeyUp += dgClasificaciones_PreviewKeyUp;
            dgImpuestos.PreviewKeyUp += DgImpuestosOnPreviewKeyUp;
            txtProductoServicio.LostFocus += TxtProductoServicio_LostFocus;
            btnListarProductosServicios.Click += BtnListarProductosServicios_Click;
            cmbMonedas.SelectionChanged += CmbMonedasOnSelectionChanged;

            txtCosto.LostFocus += TxtUtilidad_LostFocus;
            txtUtilidadA.LostFocus += TxtUtilidad_LostFocus;
            txtUtilidadB.LostFocus += TxtUtilidad_LostFocus;
            txtUtilidadC.LostFocus += TxtUtilidad_LostFocus;
            txtUtilidadD.LostFocus += TxtUtilidad_LostFocus;

            btnAgregarCodigoProveedor.Click += BtnAgregarCodigo_Click;
            dgCodigosAlternosProveedor.PreviewKeyUp += DgCodigosAlternos_PreviewKeyUp;
            txtProveedorAlterno.LostFocus += TxtProveedorAlterno_LostFocus;
            btnListarProveedores.Click += BtnListarProveedores_Click;

            btnAgregarCodigoCliente.Click += BtnAgregarCodigoCliente_Click;
            dgCodigosAlternosClientes.PreviewKeyUp += DgCodigosAlternosCliente_PreviewKeyUp;
            txtClienteAlterno.LostFocus += TxtClienteAlterno_LostFocus;
            btnListarClientes.Click += BtnListarClientes_Click;

            _idItem = -1;
            _idProductService = -1;
            _item = new VMArticulo();
            _lastCurrency = new Moneda();
        }

        private void CmbMonedasOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ChangeCurrency.isValid())
            {
                ChangeCurrency();
            }
        }

        private void TxtUtilidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CalculateByUtility.isValid())
                CalculateByUtility();
        }

        private void BtnListarProductosServicios_Click(object sender, RoutedEventArgs e)
        {
            if (OpenProductServiceList.isValid())
                OpenProductServiceList();
        }

        private void TxtProductoServicio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindProductService.isValid())
                FindProductService();
        }

        private void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void btnEquivalencias_Click(object sender, RoutedEventArgs e)
        {
            if (OpenEquivalencies.isValid(AccesoRequerido.Ver))
                OpenEquivalencies();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(IsDirty)
            {
                if (Update.isValid(AccesoRequerido.Total))
                    Update();
            }
            else
            {
                if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                    Save();
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid(AccesoRequerido.Total))
                Delete();
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

        public VMArticulo Item
        {
            get
            {
                return new VMArticulo()
                {
                    idArticulo = _idItem,
                    codigo = txtCodigo.Text,
                    idProductoServicio = _idProductService,
                    ProductosServicio = new ProductosServicio() { idProductoServicio =_idProductService, codigo = txtProductoServicio.Text},
                    descripcion = txtDescripcion.Text,
                    idMoneda = cmbMonedas.SelectedValue == null? -1: cmbMonedas.SelectedValue.ToInt(),
                    Moneda = cmbMonedas.SelectedValue == null? null: (Moneda)cmbMonedas.SelectedItem,
                    idUnidadDeMedida = cmbUnidadesDeMedida.SelectedValue == null? -1: cmbUnidadesDeMedida.SelectedValue.ToInt(),
                    UnidadesDeMedida = cmbUnidadesDeMedida.SelectedValue == null? null: (UnidadesDeMedida)cmbUnidadesDeMedida.SelectedItem,
                    costoUnitario = txtCosto.Text.ToDecimalOrDefault(),
                    Impuestos = _item.Impuestos,
                    Clasificaciones = _item.Clasificaciones,
                    CodigosDeArticuloPorProveedors = _item.CodigosDeArticuloPorProveedors,
                    CodigosDeArticuloPorClientes = _item.CodigosDeArticuloPorClientes,
                    inventariado = chkInventariado.IsChecked.GetValueOrDefault(true),
                    importado = chkImportado.IsChecked.GetValueOrDefault(false),
                    UtilidadA = txtUtilidadA.Text.ToDecimalOrDefault(),
                    UtilidadB = txtUtilidadB.Text.ToDecimalOrDefault(),
                    UtilidadC = txtUtilidadC.Text.ToDecimalOrDefault(),
                    UtilidadD = txtUtilidadD.Text.ToDecimalOrDefault(),
                    TiposDeComision = (TiposDeComision)cbComision.SelectedItem,
                    idTipoDeComision = (int?)(cbComision.SelectedValue),
                    Existencia = txtExistencia.Text.ToDecimalOrDefault()
                };
            }
        }

        public bool IsDirty
        {
            get { return _idItem.isValid(); }
        }

        public Moneda LastCurrency { get { return _lastCurrency; } }

        public Impuesto SelectedTax
        {
            get { return dgImpuestos.SelectedItem.isValid() ? (Impuesto) dgImpuestos.SelectedItem : new Impuesto(); }
        }

        public void UpdatePrices(VMArticulo item)
        {
            txtUtilidadA.Text = item.UtilidadA.ToDecimalString();
            txtPrecioSinImpuestosA.Text = item.PrecioA.ToDecimalString();
            txtPrecioConImpuestosA.Text = item.PrecioConImpuestosA.ToDecimalString();

            txtUtilidadB.Text = item.UtilidadB.ToDecimalString();
            txtPrecioSinImpuestosB.Text = item.PrecioB.ToDecimalString();
            txtPrecioConImpuestosB.Text = item.PrecioConImpuestosB.ToDecimalString();

            txtUtilidadC.Text = item.UtilidadC.ToDecimalString();
            txtPrecioSinImpuestosC.Text = item.PrecioC.ToDecimalString();
            txtPrecioConImpuestosC.Text = item.PrecioConImpuestosC.ToDecimalString();

            txtUtilidadD.Text = item.UtilidadD.ToDecimalString();
            txtPrecioSinImpuestosD.Text = item.PrecioD.ToDecimalString();
            txtPrecioConImpuestosD.Text = item.PrecioConImpuestosD.ToDecimalString();
        }

        public void Show(VMArticulo item)
        {
            txtCodigo.Text = item.codigo;
            txtProductoServicio.Text = item.ProductosServicio.isValid() ? item.ProductosServicio.codigo : string.Empty;
            txtDescripcion.Text = item.descripcion;
            txtCosto.Text = item.costoUnitario.ToDecimalString();
            cmbUnidadesDeMedida.SelectedItem = item.UnidadesDeMedida;
            txtExistencia.Text = item.Existencia.ToDecimalString();
            chkInventariado.IsChecked = item.inventariado;
            chkImportado.IsChecked = item.importado;
            cbComision.SelectedValue = item.idTipoDeComision;

            if (item.Moneda.isValid())
            {
                _lastCurrency = item.Moneda;
                cmbMonedas.SelectedItem = item.Moneda;
            }
            
            Show(item.Impuestos.ToList());
            Show(item.Clasificaciones.ToList());
            Show(item.CodigosDeArticuloPorProveedors.ToList());
            Show(item.CodigosDeArticuloPorClientes.ToList());
            UpdatePrices(item);

            _item = item;
            _idItem = item.idArticulo;
            _idProductService = item.idProductoServicio;
        }

        public void Show(ProductosServicio productService)
        {
            txtProductoServicio.Text = productService.codigo;
            _idProductService = productService.idProductoServicio;
        }

        public void Show(Moneda currency)
        {
            _lastCurrency = currency;
            cmbMonedas.SelectedItem = currency;
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtProductoServicio.Clear();
            txtDescripcion.Clear();
            txtCosto.Clear();
            cmbUnidadesDeMedida.SelectedIndex = -1;
            txtExistencia.Clear();
            cmbMonedas.SelectedIndex = -1;
            txtUtilidadA.Clear();
            txtPrecioSinImpuestosA.Clear();
            txtPrecioConImpuestosA.Clear();
            txtUtilidadB.Clear();
            txtPrecioSinImpuestosB.Clear();
            txtPrecioConImpuestosB.Clear();
            txtUtilidadC.Clear();
            txtPrecioSinImpuestosC.Clear();
            txtPrecioConImpuestosC.Clear();
            txtUtilidadD.Clear();
            txtPrecioSinImpuestosD.Clear();
            txtPrecioConImpuestosD.Clear();
            dgImpuestos.ItemsSource = null;
            dgClasificaciones.ItemsSource = null;
            chkInventariado.IsChecked = true;
            ClearSupplierCode(); //Limpia el codigo en edición
            ClearCustomerCode();
            dgCodigosAlternosProveedor.ItemsSource = null;
            dgCodigosAlternosClientes.ItemsSource = null;

            _idItem = -1;
            _idProductService = -1;

            _lastCurrency = new Moneda();
        }

        public void FillCombos(List<Moneda> currencies, List<UnidadesDeMedida> unitOfMeasure, List<TiposDeComision> comissionTypes)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;
            _lastCurrency = (Moneda)cmbMonedas.SelectedItem;

            cmbUnidadesDeMedida.ItemsSource = unitOfMeasure;
            cmbUnidadesDeMedida.SelectedValuePath = "idUnidadDeMedida";
            cmbUnidadesDeMedida.DisplayMemberPath = "descripcion";

            cbComision.ItemsSource = comissionTypes;
            cbComision.SelectedValuePath = "idTipoDeComision";
            cbComision.DisplayMemberPath = "descripcion";
        }

        #region Impuestos

        private void btnAgregarImpuesto_Click(object sender, RoutedEventArgs e)
        {
            if (AddTax.isValid(AccesoRequerido.Ver_y_Agregar))
                AddTax();
        }

        private void DgImpuestosOnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && DeleteTax.isValid(AccesoRequerido.Total))
            {
                DeleteTax();
            }
        }

        public void Show(List<Impuesto> taxes)
        {
            dgImpuestos.ItemsSource = null;
            dgImpuestos.ItemsSource = taxes;
        }

        #endregion

        #region Clasificaciones

        private void dgClasificaciones_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && DeleteClassification.isValid(AccesoRequerido.Total))
                DeleteClassification();
        }

        private void btnAgregarClasificacion_Click(object sender, RoutedEventArgs e)
        {
            if (AddClassification.isValid(AccesoRequerido.Ver_y_Agregar))
                AddClassification();
        }

        public Clasificacione CurrentClassification
        {
            get { return dgClasificaciones.SelectedIndex >= 0 ? (Clasificacione)dgClasificaciones.SelectedItem : null; }
        }

        public void Show(List<Clasificacione> clasifications)
        {
            dgClasificaciones.ItemsSource = clasifications;
        }

        #endregion

        #region Codigos Alternos Para Proveedores

        private void BtnListarProveedores_Click(object sender, RoutedEventArgs e)
        {
            if (OpenSuppliersList.isValid())
                OpenSuppliersList();
        }

        private void TxtProveedorAlterno_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindSupplier.isValid())
                FindSupplier();
        }

        private void DgCodigosAlternos_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (DeleteSupplierCode.isValid())
                DeleteSupplierCode();
        }

        private void BtnAgregarCodigo_Click(object sender, RoutedEventArgs e)
        {
            if (AddSupplierCode.isValid())
                AddSupplierCode();
        }

        public List<CodigosDeArticuloPorProveedor> SupplierCodes => dgCodigosAlternosProveedor.ItemsSource.isValid() ? dgCodigosAlternosProveedor.ItemsSource.Cast<CodigosDeArticuloPorProveedor>().ToList() : new List<CodigosDeArticuloPorProveedor>();

        public CodigosDeArticuloPorProveedor CurrentSupplierCode
        {
            get
            {
                if (_supplier.isValid())
                    _supplier.codigo = txtProveedorAlterno.Text;
                else
                    _supplier = new Proveedore() { codigo = txtProveedorAlterno.Text };
                return new CodigosDeArticuloPorProveedor() { idArticulo = _idItem, codigo = txtCodigoAlternoProveedor.Text, Proveedore = _supplier };
            }
        }

        public CodigosDeArticuloPorProveedor SelectedSupplierCode => dgCodigosAlternosProveedor.SelectedIndex >= 0 ? (CodigosDeArticuloPorProveedor)dgCodigosAlternosProveedor.SelectedItem : null;

        public void ClearSupplierCode()
        {
            txtCodigoAlternoProveedor.Clear();
            txtProveedorAlterno.Clear();
            _supplier = new Proveedore();
        }

        public void Show(CodigosDeArticuloPorProveedor code)
        {
            txtCodigoAlternoProveedor.Text = code.codigo;
            txtProveedorAlterno.Text = code.Proveedore.isValid() ? code.Proveedore.codigo : string.Empty;
            _supplier = code.Proveedore;
        }

        public void Show(List<CodigosDeArticuloPorProveedor> codes)
        {
            dgCodigosAlternosProveedor.ItemsSource = codes;
        }

        #endregion

        #region Codigos Alternos Para Clientes

        private void BtnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCustomersList.isValid())
                OpenCustomersList();
        }

        private void TxtClienteAlterno_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindCustomer.isValid())
                FindCustomer();
        }

        private void DgCodigosAlternosCliente_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (DeleteCustomerCode.isValid())
                DeleteCustomerCode();
        }

        private void BtnAgregarCodigoCliente_Click(object sender, RoutedEventArgs e)
        {
            if (AddCustomerCode.isValid())
                AddCustomerCode();
        }

        public List<CodigosDeArticuloPorCliente> CustomerCodes => dgCodigosAlternosClientes.ItemsSource.isValid() ? dgCodigosAlternosClientes.ItemsSource.Cast<CodigosDeArticuloPorCliente>().ToList() : new List<CodigosDeArticuloPorCliente>();

        public CodigosDeArticuloPorCliente CurrentCustomerCode
        {
            get
            {
                if (_customer.isValid())
                    _customer.codigo = txtClienteAlterno.Text;
                else
                    _customer = new Cliente() { codigo = txtClienteAlterno.Text };
                return new CodigosDeArticuloPorCliente() { idArticulo = _idItem, codigo = txtCodigoAlternoCliente.Text, Cliente = _customer };
            }
        }

        public CodigosDeArticuloPorCliente SelectedCustomerCode => dgCodigosAlternosClientes.SelectedIndex >= 0 ? (CodigosDeArticuloPorCliente)dgCodigosAlternosClientes.SelectedItem : null;

        public void ClearCustomerCode()
        {
            txtCodigoAlternoCliente.Clear();
            txtClienteAlterno.Clear();
            _customer = new Cliente();
        }

        public void Show(CodigosDeArticuloPorCliente code)
        {
            txtCodigoAlternoCliente.Text = code.codigo;
            txtClienteAlterno.Text = code.Cliente.isValid() ? code.Cliente.codigo : string.Empty;
            _customer = code.Cliente;
        }

        public void Show(List<CodigosDeArticuloPorCliente> codes)
        {
            dgCodigosAlternosClientes.ItemsSource = codes;
        }

        #endregion
    }
}
