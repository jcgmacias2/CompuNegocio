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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : BaseView, IAuthenticationView
    {
        public event Action SignIn;
        public event Action Quit;
        public event Action AuthorizeOnAPI;

        private bool _apiAuthentication;
        private bool _apiAuthorized;

        public AuthenticationView(bool apiAuthentication)
        {
            InitializeComponent();
            BindComponents();

            _apiAuthentication = apiAuthentication;
            _apiAuthorized = false;
        }

        private void BindComponents()
        {
            btnCancelar.Click += btnCancelar_Click;
            btnEntrar.Click += btnEntrar_Click;
        }

        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if(ApiAuthentication)
            {
                if (AuthorizeOnAPI.isValid())
                    AuthorizeOnAPI();
            }
            else
            { 
                if (SignIn.isValid())
                    SignIn();
            }
            Mouse.OverrideCursor = null;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMCredencial Credentials
        {
            get { return new VMCredencial(txtUsuario.Text, txtContraseña.Password, _apiAuthorized); }
        }

        public bool ApiAuthentication
        {
            get { return _apiAuthentication; }
        }

        public void Show(VMCredencial credentials)
        {
            txtUsuario.Text = credentials.Usuario;
            txtContraseña.Password = credentials.Contraseña;
            _apiAuthorized = credentials.APIAuthorized;
        }
    }
}
