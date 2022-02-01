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
    /// Interaction logic for SupplierStatementReportView.xaml
    /// </summary>
    public partial class SupplierStatementReportView : BaseView, ISupplierStatementReportView
    {
        public event Action FindSupplier;
        public event Action OpenSuppliersList;
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        private Proveedore _supplier;

        public SupplierStatementReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtProveedor.LostFocus += txtProveedor_LostFocus;
            btnListarProveedores.Click += btnListarProveedores_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            _supplier = new Proveedore();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnListarProveedores_Click(object sender, RoutedEventArgs e)
        {
            if (OpenSuppliersList.isValid())
                OpenSuppliersList();
        }

        private void txtProveedor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindSupplier.isValid())
                FindSupplier();
        }

        public Proveedore Supplier
        {
            get { _supplier.codigo = txtProveedor.Text; return _supplier; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public bool OnlyPendingBalance
        {
            get { return chkSoloConDeuda.IsChecked.GetValueOrDefault(); }
        }

        public void Show(Proveedore supplier)
        {
            txtProveedor.Text = supplier.codigo;
            _supplier = supplier;
        }

    }
}
