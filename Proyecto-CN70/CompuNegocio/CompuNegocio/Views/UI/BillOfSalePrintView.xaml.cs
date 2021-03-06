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
    /// Interaction logic for BillOfLadingReportView.xaml
    /// </summary>
    public partial class BillOfSalePrintView : BaseView, IBillOfSalePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private VMRemision _billOfSale;

        public BillOfSalePrintView()
        {
            InitializeComponent();
            BindComponents();

            _billOfSale = new VMRemision();
        }

        public BillOfSalePrintView(VMRemision billOfSale)
        {
            InitializeComponent();
            BindComponents();

            Show(billOfSale);
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarRemisiones.Click+= btnListarRemisiones_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Print.isValid())
                Print();
            Mouse.OverrideCursor = null;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnListarRemisiones_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public VMRemision BillOfSale
        {
            get { return _billOfSale.idRemision.isValid() ? _billOfSale : new VMRemision() { folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public void Show(VMRemision billOfSale)
        {
            txtFolio.Text = billOfSale.folio.ToStringOrDefault();

            txtFolio.IsReadOnly = billOfSale.idRemision.isValid();
            btnListarRemisiones.IsEnabled = !billOfSale.idRemision.isValid();

            _billOfSale = billOfSale;
        }
    }
}
