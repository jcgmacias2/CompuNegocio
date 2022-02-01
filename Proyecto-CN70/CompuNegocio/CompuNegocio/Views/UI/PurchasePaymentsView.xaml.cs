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
    /// Interaction logic for PurchasePaymentsView.xaml
    /// </summary>
    public partial class PurchasePaymentsView : BaseView, IPurchasePaymentsView
    {
        public event Action OpenPurchasesList;
        public event Action AddPayment;
        public event Action Quit;
        public event Action CancelPayment;

        private VMCompra _purchase;

        /// <summary>
        /// Para cuando se abre a partir de la ventana de compras
        /// </summary>
        /// <param name="purchase">Compra a la que estarán asociados los abonos</param>
        public PurchasePaymentsView(VMCompra purchase)
        {
            InitializeComponent();
            BindComponents();

            _purchase = purchase;
        }

        /// <summary>
        /// Para cuando se abre directo del menú
        /// </summary>
        public PurchasePaymentsView()
        {
            InitializeComponent();
            BindComponents();

            _purchase = new VMCompra();
        }

        private void BindComponents()
        {
            this.Loaded += PurchasePaymentsView_Loaded;
            btnListarCompras.Click += btnListarCompras_Click;
            btnAgregarAbono.Click += btnAgregarAbono_Click;
            dgAbonos.PreviewKeyUp += dgAbonos_PreviewKeyUp;
            btnCerrar.Click += btnCerrar_Click;
        }

        private void dgAbonos_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "PurchasesPresenter", true) && CancelPayment.isValid())
                CancelPayment();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnAgregarAbono_Click(object sender, RoutedEventArgs e)
        {
            if (Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "PurchasesPresenter", true) && AddPayment.isValid())
                AddPayment();

        }

        private void btnListarCompras_Click(object sender, RoutedEventArgs e)
        {
            if (Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "PurchasesPresenter", true) && OpenPurchasesList.isValid())
                OpenPurchasesList();
        }

        private void PurchasePaymentsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_purchase.folio.isValid()) //Aqui es donde piensa que no es dirty porque tiene un 0, pero en realidad ya viene cargada
            {
                txtCompraFolio.IsReadOnly = true;
                btnListarCompras.IsEnabled = false;
                Show(_purchase);
            }
            else
            { 
                if (Session.IsUserLogged && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "PurchasesPresenter", true) && OpenPurchasesList.isValid())
                    OpenPurchasesList();
            }
        }

        public AbonosDeCompra Payment
        {
            get
            {
                return new AbonosDeCompra()
                {
                    idCompra = _purchase.idCompra,
                    fechaHora = DateTime.Now,
                    folio = txtFolio.Text,
                    monto = txtCantidad.Text.ToDecimalOrDefault(),
                    idMoneda = cmbMonedas.SelectedValue.ToInt(),
                    Moneda = (Moneda)cmbMonedas.SelectedItem,
                    idFormaPago = cmbFormaPago.SelectedValue.ToInt(),
                    referencia = txtReferencia.Text,
                    tipoDeCambio = Session.Configuration.tipoDeCambio
                };
            }
        }

        public AbonosDeCompra CurrentPayment
        {
            get { return dgAbonos.SelectedIndex >= 0 ? (AbonosDeCompra)dgAbonos.SelectedItem : new AbonosDeCompra(); }
        }

        public bool IsPurchaseDirty
        {
            get { return _purchase.idCompra.isValid(); }
        }

        public VMCompra Purchase
        {
            get { return _purchase; }
        }

        public void Clear(string folio)
        {
            txtFolio.Text = folio ?? string.Empty;
            txtCantidad.Clear();
            cmbMonedas.SelectedItem = _purchase.Moneda;
            cmbFormaPago.SelectedIndex = 0;
            txtReferencia.Clear();
        }

        public void Show(VMCompra purchase)
        {
            txtCompraFolio.Text = purchase.folio;
            lblProveedor.Content = purchase.Proveedore.codigo;
            lblTipoDeCambio.Content = Session.Configuration.tipoDeCambio.ToDecimalString();// purchase.tipoDeCambio.ToDecimalString();
            cmbMonedas.SelectedItem = purchase.Moneda;
            dgAbonos.ItemsSource = purchase.AbonosDeCompras.Where(p => !p.cancelado);

            _purchase = purchase;
        }

        public void FillCombos(List<Moneda> currencies, List<FormasPago> paymentForms)
        {
            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.SelectedValuePath = "idMoneda";
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedIndex = 0;

            cmbFormaPago.ItemsSource = paymentForms;
            cmbFormaPago.SelectedValuePath = "idFormaPago";
            cmbFormaPago.DisplayMemberPath = "descripcion";
            cmbFormaPago.SelectedIndex = 0;
        }
    }
}
