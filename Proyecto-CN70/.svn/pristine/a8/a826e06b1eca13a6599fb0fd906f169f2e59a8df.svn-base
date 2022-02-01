using Aprovi.Business.ViewModels;
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
    /// Interaction logic for PurchasePrintView.xaml
    /// </summary>
    public partial class PurchasePrintView : BaseView, IPurchasePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;

        private int _idCompra;
        private Proveedore _proveedor;

        public PurchasePrintView()
        {
            InitializeComponent();
            BindComponents();

            _idCompra = -1;
            _proveedor = new Proveedore() { idProveedor = -1 };
        }

        public PurchasePrintView(VMCompra purchase)
        {
            InitializeComponent();
            BindComponents();

            Show(purchase);
        }

        private void BindComponents()
        {
            txtProveedor.LostFocus += txtProveedor_LostFocus;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarCompras.Click += btnListarCompras_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnListarCompras_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        void txtProveedor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindLast.isValid())
                FindLast();
        }

        public VMCompra Purchase
        {
            get
            {
                _proveedor.codigo = txtProveedor.Text;
                return new VMCompra() { idCompra = _idCompra, idProveedor = _proveedor.idProveedor, Proveedore = _proveedor, folio = txtFolio.Text };
            }
        }

        public void Show(VMCompra purchase)
        {
            txtProveedor.Text = purchase.Proveedore.codigo;
            txtFolio.Text = purchase.folio;

            _proveedor = purchase.Proveedore;
            _idCompra = purchase.idCompra;
        }
    }
}
