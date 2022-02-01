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
    /// Interaction logic for MigrationToolsView.xaml
    /// </summary>
    public partial class MigrationToolsView : BaseView, IMigrationToolsView
    {
        public event Action Quit;
        public event Action Process;
        public event Action OpenFindDbc;

        public MigrationToolsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += BtnCerrar_Click;
            btnProcesar.Click += BtnProcesar_Click;
            btnBuscarDbc.Click += BtnBuscarDbc_Click;
        }

        private void BtnProcesar_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (Process.isValid())
                Process();
            this.Cursor = null;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void BtnBuscarDbc_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFindDbc.isValid())
                OpenFindDbc();
        }

        public bool Items
        { get { return chkArticulos.IsChecked.GetValueOrDefault(); } }

        public bool Clients
        { get { return chkClientes.IsChecked.GetValueOrDefault(); } }

        public bool Suppliers
        { get { return chkProveedores.IsChecked.GetValueOrDefault(); } }

        public string DbcPath
        { get { return txtDbcPath.Text; } }

        public void Show(string dbcPath)
        {
            txtDbcPath.Text = dbcPath;
        }
    }
}
