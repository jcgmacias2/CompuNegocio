using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
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
    /// Interaction logic for PricesView.xaml
    /// </summary>
    public partial class PricesView : BaseView, IPricesView
    {
        public PricesView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            //Eventos de la vista en gral
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarListasDePrecio.Click += btnListarListasDePrecio_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;

            //Eventos de la pestaña de Artículos
            txtCodigoArticulo.LostFocus += txtCodigoArticulo_LostFocus;
            btnListarArticulos.Click += btnListarArticulos_Click;
            btnAgregarArticulo.Click += btnAgregarArticulo_Click;
            dgArticulos.PreviewKeyUp += dgArticulos_PreviewKeyUp;
            dgArticulos.PreviewMouseDoubleClick += dgArticulos_PreviewMouseDoubleClick;
            txtUtilidad.LostFocus += txtUtilidad_LostFocus;
            txtPrecio.LostFocus += txtPrecio_LostFocus;
            txtPrecioConImpuestos.LostFocus += txtPrecioConImpuestos_LostFocus;

            //Eventos de la pestaña de Clientes
            txtCodigoCliente.LostFocus += txtCodigoCliente_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            btnAgregarCliente.Click += btnAgregarCliente_Click;
            dgClientes.PreviewKeyUp += dgClientes_PreviewKeyUp;

            _idPricesList = -1;
            _idPriceItem = -1;
            _idClient = -1;
            _rfc = string.Empty;
            _razonSocial = string.Empty;
            _prices = new List<VMPrecio>();
            _clients = new List<Cliente>();
            _taxes = new List<Impuesto>();
        }

        #region PricesList

        public event Action Find;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action OpenList;
        public event Action Quit;

        private int _idPricesList;
        private List<VMPrecio> _prices;
        private List<Cliente> _clients;

        public ListasDePrecio PricesList
        {
            get { return new ListasDePrecio() { idListaDePrecio = _idPricesList, codigo = txtCodigo.Text, Clientes = _clients, Precios = _prices.Cast<Precio>().ToList() }; }
        }

        public bool IsDirty
        {
            get { return _idPricesList.isValid(); }
        }

        public void Show(ListasDePrecio pricesList)
        {
            //Limpio posibles registros en edición
            ClearPrice();
            ClearClient();

            //La lista solo tiene código
            txtCodigo.Text = pricesList.codigo;

            //Al mostrar una lista asigno esas listas a las variables locales
            //Convierto la lista de precios a VMPrecios a través de un extension
            _idPricesList = pricesList.idListaDePrecio;
            _prices = pricesList.Precios.ToViewModelList();
            _clients = pricesList.Clientes.ToList();
            dgArticulos.ItemsSource = _prices;
            dgClientes.ItemsSource = _clients;
        }

        public void Clear()
        {
            txtCodigo.Clear();
            ClearPrice();
            ClearClient();

            _prices = new List<VMPrecio>();
            _clients = new List<Cliente>();
            dgArticulos.ItemsSource = _prices;
            dgClientes.ItemsSource = _clients;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                ShowMessage("Solo es necesario guardar las listas nuevas");
                return;
            }

            if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                Save();
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
            if (Quit.isValid(AccesoRequerido.Ver))
                Quit();
        }

        private void btnListarListasDePrecio_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        #endregion

        #region Artículos / Precios

        public event Action FindPriceItem;
        public event Action OpenItemsList;
        public event Action AddOrUpdatePriceItem;
        public event Action DeletePriceItem;
        public event Action SelectPriceItem;
        public event Action CalculateByPrice;
        public event Action CalculateByUtility;
        public event Action CalculateByPriceWithTaxes;

        private int _idPriceItem;
        private List<Impuesto> _taxes;

        public VMPrecio Price
        {
            get
            {
                return new VMPrecio()
                {
                    Articulo = new Articulo() { codigo = txtCodigoArticulo.Text, costoUnitario = txtCosto.Text.ToDecimalOrDefault(), Moneda = new Moneda() { descripcion = lblMoneda.Content.ToString() }, Impuestos = _taxes },
                    idArticulo = _idPriceItem,
                    idListaDePrecio = _idPricesList,
                    utilidad = txtUtilidad.Text.ToDecimalOrDefault(),
                    Precio = txtPrecio.Text.ToDecimalOrDefault(),
                    PrecioConImpuestos = txtPrecioConImpuestos.Text.ToDecimalOrDefault(),
                };
            }
        }

        public VMPrecio CurrentPrice
        {
            get { return dgArticulos.SelectedIndex >= 0 ? (VMPrecio)dgArticulos.SelectedItem : null; }
        }

        public void Show(VMPrecio priceItem)
        {
            txtCodigoArticulo.Text = priceItem.Articulo.codigo;
            txtCosto.Text = priceItem.Articulo.costoUnitario.ToDecimalString();
            lblMoneda.Content = priceItem.Articulo.Moneda.descripcion;
            txtUtilidad.Text = priceItem.utilidad.ToDecimalString();
            txtPrecio.Text = priceItem.Precio.ToDecimalString();
            txtPrecioConImpuestos.Text = priceItem.PrecioConImpuestos.ToDecimalString();

            _idPriceItem = priceItem.idArticulo;
            _taxes = priceItem.Articulo.Impuestos.ToList();
        }

        public void ClearPrice()
        {
            txtCodigoArticulo.Clear();
            txtCosto.Clear();
            lblMoneda.Content = string.Empty;
            txtUtilidad.Clear();
            txtPrecio.Clear();
            txtPrecioConImpuestos.Clear();

            _idPriceItem = -1;
        }

        private void txtPrecioConImpuestos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CalculateByPriceWithTaxes.isValid(AccesoRequerido.Ver_y_Agregar))
                CalculateByPriceWithTaxes();
        }

        private void txtPrecio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CalculateByPrice.isValid(AccesoRequerido.Ver_y_Agregar))
                CalculateByPrice();
        }

        private void txtUtilidad_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CalculateByUtility.isValid(AccesoRequerido.Ver_y_Agregar))
                CalculateByUtility();
        }

        private void dgArticulos_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && DeletePriceItem.isValid(AccesoRequerido.Total))
            {
                if (PricesList.Precios.Count.Equals(1))
                {
                    if (MessageBoxResult.Yes.Equals(ShowMessageWithOptions("Al eliminar este artículo se eliminará toda la lista, ¿desea continuar?")))
                    {
                        Delete();
                    }
                }
                else
                {
                    DeletePriceItem();
                }
            }
        }

        private void dgArticulos_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectPriceItem.isValid(AccesoRequerido.Total))
                SelectPriceItem();
        }

        private void btnAgregarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (AddOrUpdatePriceItem.isValid(AccesoRequerido.Ver_y_Agregar))
                AddOrUpdatePriceItem();
        }

        private void btnListarArticulos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenItemsList.isValid(AccesoRequerido.Ver_y_Agregar))
                OpenItemsList();
        }

        private void txtCodigoArticulo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindPriceItem.isValid(AccesoRequerido.Ver_y_Agregar))
                FindPriceItem();
        }

        #endregion

        #region Clientes

        public event Action FindClient;
        public event Action OpenClientsList;
        public event Action AddClient;
        public event Action DeleteClient;

        private int _idClient;
        private string _rfc;
        private string _razonSocial;

        public Cliente Client
        {
            get { return new Cliente() { idCliente = _idClient, codigo = txtCodigoCliente.Text, idListaDePrecio = _idPricesList, nombreComercial = txtNombreComercial.Text, rfc = _rfc, razonSocial = _razonSocial }; }
        }

        public Cliente CurrentClient
        {
            get { return dgClientes.SelectedIndex >= 0 ? (Cliente)dgClientes.SelectedItem : null; }
        }

        public void Show(Cliente client)
        {
            txtCodigoCliente.Text = client.codigo;
            txtNombreComercial.Text = client.nombreComercial;
            _idClient = client.idCliente;
            _rfc = client.rfc;
            _razonSocial = client.razonSocial;
        }

        public void ClearClient()
        {
            txtCodigoCliente.Clear();
            txtNombreComercial.Clear();
            _idClient = -1;
            _rfc = string.Empty;
            _razonSocial = string.Empty;
        }

        private void dgClientes_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && DeleteClient.isValid(AccesoRequerido.Total))
                DeleteClient();
        }

        private void btnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (AddClient.isValid(AccesoRequerido.Ver_y_Agregar))
                AddClient();
        }

        private void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenClientsList.isValid(AccesoRequerido.Ver_y_Agregar))
                OpenClientsList();
        }

        private void txtCodigoCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindClient.isValid(AccesoRequerido.Ver_y_Agregar))
                FindClient();
        }

        #endregion
    }
}
