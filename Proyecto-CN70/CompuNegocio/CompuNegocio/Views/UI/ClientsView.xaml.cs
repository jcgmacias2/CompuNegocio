using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ClientesView.xaml
    /// </summary>
    public partial class ClientsView : BaseView, IClientsView
    {
        public event Action Quit;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Find;
        public event Action Update;
        public event Action OpenList;
        public event Action OpenSoldItemsList;
        public event Action OpenUsersList;
        public event Action FindUser;
        public event Action OpenSalesList;
        public event Action AddItemCode;
        public event Action DeleteItemCode;
        public event Action FindItem;
        public event Action OpenItemsList;

        private int _idClient;
        private Cliente _customer;
        private Usuario _seller;
        private Articulo _item;

        public ClientsView(bool guardianActive)
        {
            InitializeComponent();
            BindComponents();

            this.tabGuardian.Visibility = guardianActive ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BindComponents()
        {
            txtCodigo.LostFocus += txtCodigo_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnArticulos.Click += BtnArticulosOnClick;
            btnListarUsuarios.Click += BtnListarUsuariosOnClick;
            txtVendedor.LostFocus += TxtVendedorOnLostFocus;
            btnVentas.Click += BtnVentasOnClick;
            btnAgregarCodigoArticulo.Click += BtnAgregarCodigoArticuloOnClick;
            btnListarArticulos.Click += BtnListarArticulosOnClick;
            txtAlternoArticulo.LostFocus += TxtAlternoArticuloOnLostFocus;
            dgCodigosAlternos.PreviewKeyUp += DgCodigosAlternosOnPreviewKeyUp;

            _idClient = -1;
            _seller = new Usuario();
            _customer = new Cliente();

            //Cuentas
            btnAgregarCuenta.Click += BtnAgregarCuenta_Click;
            dgCuentas.PreviewKeyUp += DgCuentas_PreviewKeyUp;

            //Homologados
            _item = new Articulo();
        }

        private void BtnVentasOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenSalesList.isValid())
            {
                OpenSalesList();
            }
        }

        private void TxtVendedorOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindUser.isValid())
            {
                FindUser();
            }
        }

        private void BtnListarUsuariosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenUsersList.isValid())
            {
                OpenUsersList();
            }
        }

        private void BtnArticulosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenSoldItemsList.isValid())
            {
                OpenSoldItemsList();
            }
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

        private void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid(AccesoRequerido.Ver))
                OpenList();
        }

        private void txtCodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid(AccesoRequerido.Ver))
                Find();
        }

        public Cliente Client
        {
            get
            {
                return new Cliente()
                {
                    idCliente = _idClient,
                    codigo = txtCodigo.Text,
                    nombreComercial = txtNombreComercial.Text,
                    razonSocial = txtRazonSocial.Text,
                    rfc = txtRfc.Text,
                    Domicilio = new Domicilio()
                    {
                        calle = txtCalle.Text,
                        numeroExterior = txtNumeroExterior.Text,
                        numeroInterior = txtNumeroInterior.Text,
                        colonia = txtColonia.Text,
                        ciudad = txtCiudad.Text,
                        estado = txtEstado.Text,
                        idPais = cmbPais.SelectedValue.ToIntOrDefault(),
                        codigoPostal = txtCodigoPostal.Text
                    },
                    contacto = txtContacto.Text,
                    correoElectronico = txtCorreo.Text,
                    telefono = txtTelefono.Text,
                    condicionDePago = txtCondicionDePago.Text,
                    idListaDePrecio = cmbListaDePrecio.SelectedValue.ToIntOrDefault(),
                    CuentasDeCorreos = dgCuentas.Items.Count > 0 ? dgCuentas.ItemsSource.Cast<CuentasDeCorreo>().ToList() : null,
                    Usuario = new Usuario(){nombreDeUsuario = txtVendedor.Text},
                    idVendedor = _seller.idUsuario.isValid() ? _seller.idUsuario : (int?)null,
                    limiteCredito = txtLimiteCredito.Text.ToValidatedDecimal(),
                    diasCredito = txtDiasCredito.Text.ToValidatedInt(),
                    idUsoCFDI = cmbUsosCFDI.SelectedValue.ToIntOrDefault(),
                    UsosCFDI = cmbUsosCFDI.SelectedItem as UsosCFDI,
                    idRegimen = cmbRegimenFiscal.SelectedValue.ToIntOrDefault(),
                    CodigosDeArticuloPorClientes = _customer.CodigosDeArticuloPorClientes
                };
            }
        }

        public bool IsDirty
        {
            get { return _idClient.isValid(); }
        }

        public void Show(Cliente client)
        {
            txtCodigo.Text = client.codigo;
            txtNombreComercial.Text = client.nombreComercial;
            txtRazonSocial.Text = client.razonSocial;
            txtRfc.Text = client.rfc;
            txtCalle.Text = client.Domicilio.calle;
            txtNumeroExterior.Text = client.Domicilio.numeroExterior;
            txtNumeroInterior.Text = client.Domicilio.numeroInterior;
            txtColonia.Text = client.Domicilio.colonia;
            txtCiudad.Text = client.Domicilio.ciudad;
            txtEstado.Text = client.Domicilio.estado;
            if (client.Domicilio.idPais.isValid())
                cmbPais.SelectedValue = client.Domicilio.idPais;
            else
                cmbPais.SelectedIndex = 0;
            txtCodigoPostal.Text = client.Domicilio.codigoPostal;
            txtContacto.Text = client.contacto;
            txtCorreo.Text = client.correoElectronico;
            txtTelefono.Text = client.telefono;
            txtCondicionDePago.Text = client.condicionDePago;
            if (client.idListaDePrecio.isValid())
                cmbListaDePrecio.SelectedValue = client.idListaDePrecio;
            else
                cmbListaDePrecio.SelectedIndex = 0;
            txtLimiteCredito.Text = client.limiteCredito.HasValue ? client.limiteCredito.Value.ToDecimalString() : "";
            txtDiasCredito.Text = client.diasCredito.HasValue ? client.diasCredito.Value.ToString() : "";
            cmbUsosCFDI.SelectedValue = client.idUsoCFDI;

            if (client.idRegimen.isValid())
                cmbRegimenFiscal.SelectedValue = client.idRegimen;
            else
                cmbRegimenFiscal.SelectedIndex = 0;

            if (client.Usuario.isValid())
            {
                Show(client.Usuario);
            }
            else
            {
                _seller = new Usuario();
                txtVendedor.Clear();
            }

            _idClient = client.idCliente;
            _customer = client;

            //Cuentas
            txtCuenta.Clear();
            dgCuentas.ItemsSource = client.CuentasDeCorreos;

            //Homologados
            Show(client.CodigosDeArticuloPorClientes.ToList());
        }

        public void Show(Usuario user)
        {
            txtVendedor.Text = user.nombreDeUsuario;
            _seller = user;
        }

        public void ShowTotals(VwSaldosPorClientePorMoneda totalDollars, VwSaldosPorClientePorMoneda totalPesos)
        {
            txtTotalPesos.Text = totalPesos.isValid() ? totalPesos.total.Value.ToCurrencyString() : "$0.00";
            txtSaldoPesos.Text = totalPesos.isValid() ? (totalPesos.total.GetValueOrDefault(0.0m) - totalPesos.abonado.GetValueOrDefault(0.0m)).ToCurrencyString() : "$0.00";
            txtTotalDolares.Text = totalDollars.isValid() ? totalDollars.total.Value.ToCurrencyString() : "$0.00";
            txtSaldoDolares.Text = totalDollars.isValid() ? (totalDollars.total.GetValueOrDefault(0.0m) - totalDollars.abonado.GetValueOrDefault(0.0m)).ToCurrencyString() : "$0.00";
        }

        public void Clear()
        {
            txtCodigo.Clear();
            txtNombreComercial.Clear();
            txtRazonSocial.Clear();
            txtRfc.Clear();
            txtCalle.Clear();
            txtNumeroExterior.Clear();
            txtNumeroInterior.Clear();
            txtColonia.Clear();
            txtCiudad.Clear();
            txtEstado.Clear();
            cmbPais.SelectedIndex = -1;
            txtCodigoPostal.Clear();
            txtContacto.Clear();
            txtCorreo.Clear();
            txtTelefono.Clear();
            txtCondicionDePago.Clear();
            cmbListaDePrecio.SelectedIndex = -1;
            cmbUsosCFDI.SelectedIndex = -1;
            cmbRegimenFiscal.SelectedIndex = -1;
            txtVendedor.Clear();
            txtLimiteCredito.Clear();
            txtDiasCredito.Clear();

            //Cuentas
            txtCuenta.Clear();
            dgCuentas.ItemsSource = null;

            //Saldos
            txtSaldoDolares.Clear();
            txtSaldoPesos.Clear();
            txtTotalDolares.Clear();
            txtTotalPesos.Clear();

            _idClient = -1;
            _customer = new Cliente();
            _seller = new Usuario();
            _item = new Articulo();
        }

        public void FillCombo(List<Pais> countries, List<ListasDePrecio> priceLists, List<UsosCFDI> cfdiUsages, List<Regimene> regimenes)
        {
            cmbPais.ItemsSource = countries;
            cmbPais.DisplayMemberPath = "descripcion";
            cmbPais.SelectedValuePath = "idPais";
            cmbPais.SelectedIndex = 0;

            cmbListaDePrecio.ItemsSource = priceLists;
            cmbListaDePrecio.DisplayMemberPath = "codigo";
            cmbListaDePrecio.SelectedValuePath = "idListaDePrecio";
            cmbListaDePrecio.SelectedIndex = 0;

            cmbUsosCFDI.ItemsSource = cfdiUsages;
            cmbUsosCFDI.DisplayMemberPath = "descripcion";
            cmbUsosCFDI.SelectedValuePath = "idUsoCFDI";
            cmbUsosCFDI.SelectedIndex = 0;

            cmbRegimenFiscal.ItemsSource = regimenes;
            cmbRegimenFiscal.DisplayMemberPath = "descripcion";
            cmbRegimenFiscal.SelectedValuePath = "idRegimen";
            cmbRegimenFiscal.SelectedIndex = 0;
        }

        #region Cuentas

        public event Action AddAccount;
        public event Action RemoveAccount;

        private void DgCuentas_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveAccount.isValid(AccesoRequerido.Total))
                RemoveAccount();
        }

        private void BtnAgregarCuenta_Click(object sender, RoutedEventArgs e)
        {
            if (AddAccount.isValid(AccesoRequerido.Ver_y_Agregar))
                AddAccount();
        }

        public CuentasDeCorreo Account => new CuentasDeCorreo() { idCliente = _idClient, cuenta = txtCuenta.Text };

        public CuentasDeCorreo Selected => dgCuentas.SelectedIndex >= 0 ? (CuentasDeCorreo)dgCuentas.SelectedItem : null;

        public void Fill(List<CuentasDeCorreo> accounts)
        {
            dgCuentas.ItemsSource = accounts;
        }

        #endregion

        #region Homologados
        private void DgCodigosAlternosOnPreviewKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key.Equals(Key.Delete) && DeleteItemCode.isValid())
            {
                DeleteItemCode();
            }
        }

        private void TxtAlternoArticuloOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindItem.isValid())
            {
                FindItem();
            }
        }

        private void BtnListarArticulosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenItemsList.isValid())
            {
                OpenItemsList();
            }
        }

        private void BtnAgregarCodigoArticuloOnClick(object sender, RoutedEventArgs e)
        {
            if (AddItemCode.isValid())
            {
                AddItemCode();
            }
        }

        public List<CodigosDeArticuloPorCliente> ItemCodes => dgCodigosAlternos.ItemsSource.isValid() ? dgCodigosAlternos.ItemsSource.Cast<CodigosDeArticuloPorCliente>().ToList() : new List<CodigosDeArticuloPorCliente>();
        public CodigosDeArticuloPorCliente CurrentItemCode
        {
            get
            {
                if (_item.isValid())
                    _item.codigo = txtAlternoArticulo.Text;
                else
                    _item = new Articulo() { codigo = txtAlternoArticulo.Text };
                return new CodigosDeArticuloPorCliente { codigo = txtCodigoAlterno.Text, Articulo = _item, idCliente = _idClient };
            }
        }
        public CodigosDeArticuloPorCliente SelectedItemCode => dgCodigosAlternos.SelectedIndex >= 0 ? (CodigosDeArticuloPorCliente)dgCodigosAlternos.SelectedItem : null;

        public void ClearItemCode()
        {
            txtAlternoArticulo.Clear();
            txtCodigoAlterno.Clear();

            _item = new Articulo();
        }

        public void Show(CodigosDeArticuloPorCliente code)
        {
            txtCodigoAlterno.Text = code.codigo;
            txtAlternoArticulo.Text = code.Articulo.isValid() ? code.Articulo.codigo : "";

            _item = code.Articulo;
        }

        public void Show(List<CodigosDeArticuloPorCliente> codes)
        {
            dgCodigosAlternos.ItemsSource = null;
            dgCodigosAlternos.ItemsSource = codes;
        }
        #endregion
    }
}
