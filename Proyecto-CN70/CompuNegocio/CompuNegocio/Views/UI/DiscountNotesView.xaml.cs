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
using Aprovi.Data.Models;
using Aprovi.Application.Helpers;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for DiscountNotesView.xaml
    /// </summary>
    public partial class DiscountNotesView : BaseView, IDiscountNotesView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action FindCustomer;
        public event Action OpenCustomersList;
        public event Action Quit;
        public event Action Save;
        public event Action Update;
        public event Action Cancel;
        public event Action New;
        public event Action Load;
        public event Action AmountChanged;
        public event Action Print;

        private int _idDiscountNote;
        private Cliente _customer;

        public DiscountNotesView()
        {
            InitializeComponent();
            BindComponents();

            _idDiscountNote = -1;
            _customer = new Cliente();
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += TxtFolio_LostFocus;
            txtCliente.LostFocus += TxtClienteOnLostFocus;
            btnListarNotasDeDescuento.Click += BtnListarNotasDeDescuento_Click;
            btnListarClientes.Click += BtnListarClientesOnClick;
            btnCerrar.Click += BtnCerrar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnImprimir.Click += BtnImprimirOnClick;
            this.Loaded += OnLoaded;
            txtMonto.LostFocus += TxtMontoOnLostFocus;
        }

        private void BtnImprimirOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Print.isValid())
            {
                Print();
            }
        }

        private void TxtMontoOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (AmountChanged.isValid())
            {
                AmountChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Load.isValid())
            {
                Load();
            }
        }

        private void BtnListarClientesOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenCustomersList.isValid())
            {
                OpenCustomersList();
            }
        }

        private void TxtClienteOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (SelectedCustomerChanged && FindCustomer.isValid())
            {
                FindCustomer();
            }
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_idDiscountNote.isValid())
            {
                if (Update.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "DiscountNotesPresenter", true))
                    Update();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "DiscountNotesPresenter", true))
                    Save();
            }    
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "DiscountNotesPresenter", true))
                Cancel();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnListarNotasDeDescuento_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void TxtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public NotasDeDescuento DiscountNote
        {
            get
            {
                return new NotasDeDescuento()
                {
                    idNotaDeDescuento = _idDiscountNote,
                    folio = txtFolio.Text.ToIntOrDefault(),
                    fechaHora = dpFecha.SelectedDate,
                    monto = txtMonto.Text.ToDecimalOrDefault(),
                    descripcion = txtDescripcion.Text,
                    idMoneda = cmbMonedas.SelectedValue.ToIntOrDefault(),
                    idCliente = _customer.idCliente,
                    Moneda = cmbMonedas.SelectedItem as Moneda,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    Cliente = new Cliente()
                    {
                        idCliente = _customer.idCliente,
                        codigo = txtCliente.Text
                    },
                };
            }
        }

        private bool SelectedCustomerChanged => (!_customer.isValid() || txtCliente.Text.isValid() && _customer.codigo != txtCliente.Text);

        public void Clear()
        {
            txtFolio.Clear();
            dpFecha.SelectedDate = DateTime.Now;
            txtCliente.Clear();
            cmbMonedas.SelectedIndex = -1;
            txtMonto.Clear();
            txtDescripcion.Clear();

            _idDiscountNote = -1;
            _customer = new Cliente();
        }

        public void FillCombos(List<Moneda> currencies)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedValuePath = "idMoneda";
        }

        public void Show(NotasDeDescuento discountNote)
        {
            txtFolio.Text = discountNote.folio.ToString();
            dpFecha.SelectedDate = discountNote.fechaHora;
            txtCliente.Text = discountNote.Cliente.codigo;
            cmbMonedas.SelectedItem = discountNote.Moneda;
            txtMonto.Text = discountNote.monto.ToDecimalString();
            txtDescripcion.Text = discountNote.descripcion;
            txtTipoDeCambio.Text = discountNote.tipoDeCambio.ToDecimalString();

            _idDiscountNote = discountNote.idNotaDeDescuento;
            _customer = discountNote.Cliente;
        }
    }
}
