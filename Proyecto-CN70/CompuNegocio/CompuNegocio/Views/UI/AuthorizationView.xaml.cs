using Aprovi.Application.ViewModels;
using Aprovi.Business.ViewModels;
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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthorizationView : BaseView, IAuthorizationView
    {
        public event Action SignIn;
        public event Action Quit;
        private bool _authorized;

        public AuthorizationView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCancelar.Click += btnCancelar_Click;
            btnAutorizar.Click += btnAutorizar_Click;
        }

        private void btnAutorizar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SignIn.isValid())
                SignIn();
            Mouse.OverrideCursor = null;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMCredencial Credentials
        {
            get { return new VMCredencial(txtUsuario.Text, txtContraseña.Password, false); }
        }

        public bool Authorized
        {
            get { return _authorized; }
        }

        public void Show(VMCredencial credentials)
        {
            txtUsuario.Text = credentials.Usuario;
            txtContraseña.Password = credentials.Contraseña;
        }

        public void SetAuthorization(bool authorized)
        {
            _authorized = authorized;
        }
    }
}
