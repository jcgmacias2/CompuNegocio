using Aprovi.Data.Models;
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
    /// Interaction logic for AdminConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationPACView : BaseView, IConfigurationPACView
    {
        public event Action Quit;
        public event Action Save;

        private Configuracion _config;

        public ConfigurationPACView(Configuracion config)
        {
            InitializeComponent();
            BindComponents();

            _config = config;
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Save.isValid())
                Save();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public Configuracion Config
        {
            get
            {
                _config.usuarioPAC = txtUsuario.Text;
                _config.contraseñaPAC = txtContraseña.Text;
                return _config;
            }
        }
    }
}
