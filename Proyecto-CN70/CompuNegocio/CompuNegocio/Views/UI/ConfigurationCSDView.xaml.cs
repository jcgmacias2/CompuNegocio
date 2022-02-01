using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using Microsoft.Win32;
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
    /// Interaction logic for ConfigurationCSDView.xaml
    /// </summary>
    public partial class ConfigurationCSDView : BaseView, IConfigurationCSDView
    {
        public event Action Quit;
        public event Action CreateAndSave;
        public event Action Save;
        public event Action OpenFindCertificate;
        public event Action OpenFindPrivateKey;
        public event Action OpenPfxFolder;
        public event Action OpenFindPfx;

        public ConfigurationCSDView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnBuscarCer.Click += btnBuscarCer_Click;
            btnBuscarKey.Click += btnBuscarKey_Click;
            btnBuscarFolder.Click += btnBuscarFolder_Click;
            btnBuscarPfx.Click += btnBuscarPfx_Click;
        }

        private void btnBuscarPfx_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFindPfx.isValid())
                OpenFindPfx();
        }

        private void btnBuscarFolder_Click(object sender, RoutedEventArgs e)
        {
            if (OpenPfxFolder.isValid())
                OpenPfxFolder();
        }

        private void btnBuscarKey_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFindPrivateKey.isValid())
                OpenFindPrivateKey();
        }

        private void btnBuscarCer_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFindCertificate.isValid())
                OpenFindCertificate();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(txtPfx.Text.isValid())
            {
                if (Save.isValid())
                    Save();
            }
            else
            {
                if (CreateAndSave.isValid())
                    CreateAndSave();
            }

            
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public VMCertificado Certificate
        {
            get { return new VMCertificado(txtCertificado.Text, txtLlavePrivada.Text, txtContraseña.Text, txtFolderPfx.Text, txtPfx.Text); }
        }

        public void Show(VMCertificado certificate)
        {
            txtCertificado.Text = certificate.CertificadoCER;
            txtLlavePrivada.Text = certificate.LlavePrivadaKEY;
            txtContraseña.Text = certificate.ContraseñaLlavePrivada;
            txtFolderPfx.Text = certificate.FolderPfx;
            txtPfx.Text = certificate.CertificadoPFX;
        }
    }
}
