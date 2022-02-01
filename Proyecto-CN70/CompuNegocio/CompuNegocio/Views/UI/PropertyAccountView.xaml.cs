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
    /// Interaction logic for PropertyAccountView.xaml
    /// </summary>
    public partial class PropertyAccountView : BaseView, IPropertyAccountView
    {
        public event Action New;
        public event Action Quit;
        public event Action Delete;
        public event Action Save;
        public event Action OpenList;
        public event Action Find;
        private int _idAccount;

        public PropertyAccountView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCuenta.LostFocus += TxtCuenta_LostFocus;
            btnListarCuentas.Click += BtnListarCuentas_Click;
            btnCerrar.Click += BtnCerrar_Click;
            btnNuevo.Click += BtnNuevo_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnGuardar.Click += BtnGuardar_Click;
            _idAccount = -1;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Ver_y_Agregar, "ItemsPresenter", true))
                Save();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "ItemsPresenter", true))
                Delete();
                    
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid())
                New();
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

        public CuentasPrediale Account
        {
            get { return new CuentasPrediale() { idCuentaPredial = _idAccount, cuenta = txtCuenta.Text }; }
        }

        public void Clear()
        {
            txtCuenta.Clear();
            _idAccount = -1;
        }

        public void Show(CuentasPrediale account)
        {
            txtCuenta.Text = account.cuenta;
            _idAccount = account.idCuentaPredial;
        }
    }
}
