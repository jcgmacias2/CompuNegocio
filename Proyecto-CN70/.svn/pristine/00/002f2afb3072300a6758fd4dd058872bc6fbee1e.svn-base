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
    /// Interaction logic for BankAccountsView.xaml
    /// </summary>
    public partial class BankAccountsView : BaseView, IBankAccountsView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Save;
        public event Action Update;
        public event Action Delete;
        public event Action New;

        private int _idAccount;

        public BankAccountsView()
        {
            InitializeComponent();
            BindComponents();

            _idAccount = -1;
        }

        private void BindComponents()
        {
            txtCuenta.LostFocus += TxtCuenta_LostFocus;
            btnListarCuentas.Click += BtnListarCuentas_Click;
            btnCerrar.Click += BtnCerrar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            btnNuevo.Click += BtnNuevo_Click;
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_idAccount.isValid())
            {
                if (Update.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BanksPresenter", true))
                    Update();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "BanksPresenter", true))
                    Save();
            }    
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BanksPresenter", true))
                Delete();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnListarCuentas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        private void TxtCuenta_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public CuentasBancaria Account
        {
            get { return new CuentasBancaria() { idCuentaBancaria = _idAccount, Banco = cmbBancos.SelectedIndex >= 0 ? (Banco)cmbBancos.SelectedItem : new Banco(), Moneda = cmbMonedas.SelectedIndex >= 0 ? (Moneda)cmbMonedas.SelectedItem : new Moneda(), numeroDeCuenta = txtCuenta.Text }; }
        }

        public void Clear()
        {
            txtCuenta.Clear();
            cmbBancos.SelectedIndex = -1;
            cmbMonedas.SelectedIndex = -1;
            _idAccount = -1;
        }

        public void FillCombos(List<Banco> bancos, List<Moneda> currencies)
        {
            cmbBancos.ItemsSource = bancos;
            cmbBancos.DisplayMemberPath = "nombre";
            cmbBancos.SelectedValuePath = "idBanco";

            cmbMonedas.ItemsSource = currencies;
            cmbMonedas.DisplayMemberPath = "descripcion";
            cmbMonedas.SelectedValuePath = "idMoneda";
        }

        public void Show(CuentasBancaria account)
        {
            txtCuenta.Text = account.numeroDeCuenta;
            cmbBancos.SelectedItem = account.Banco;
            cmbMonedas.SelectedItem = account.Moneda;
            _idAccount = account.idCuentaBancaria;
        }
    }
}
