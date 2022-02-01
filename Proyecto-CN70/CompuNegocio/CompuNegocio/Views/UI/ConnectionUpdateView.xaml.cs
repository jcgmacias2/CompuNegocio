using System;
using System.Windows;


namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ConnectionUpdateView.xaml
    /// </summary>
    public partial class ConnectionUpdateView : BaseView, IConnectionUpdateView
    {
        public event Action Quit;
        public event Action Save;

        public ConnectionUpdateView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;
        }

        void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public string Server
        {
            get { return txtServidor.Text; }
        }

        public string User
        {
            get { return txtUsuario.Text; }
        }

        public string Password
        {
            get { return txtContraseña.Text; }
        }

        public string Database
        {
            get { return txtBaseDeDatos.Text; }
        }
    }
}
