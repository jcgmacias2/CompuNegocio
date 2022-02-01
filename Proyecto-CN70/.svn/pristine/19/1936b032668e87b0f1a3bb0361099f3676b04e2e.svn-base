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
    /// Interaction logic for ItemsHomologationToolView.xaml
    /// </summary>
    public partial class ItemsHomologationToolView : BaseView, IItemsHomologationToolView
    {
        public event Action Quit;
        public event Action ProcessItems;
        public event Action OpenFindXls;

        public ItemsHomologationToolView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += BtnCerrar_Click;
            btnProcesar.Click += BtnProcesar_Click;
            btnBuscarExcel.Click += BtnBuscarExcel_Click;
                
        }

        private void BtnBuscarExcel_Click(object sender, RoutedEventArgs e)
        {
            if (OpenFindXls.isValid())
                OpenFindXls();
        }

        private void BtnProcesar_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessItems.isValid())
                ProcessItems();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public string ExcelFile => txtArchivoExcel.Text;

        public string CNStartCell => string.Format("{0}-{1}", txtColumnaCodigoCN.Text, txtRenglonCodigoCN.Text);

        public string SATStartCell => string.Format("{0}-{1}", txtColumnaCodigoSAT.Text, txtRenglonCodigoSAT.Text);

        public void Fill(string excelFile)
        {
            txtArchivoExcel.Text = excelFile;
        }
    }
}
