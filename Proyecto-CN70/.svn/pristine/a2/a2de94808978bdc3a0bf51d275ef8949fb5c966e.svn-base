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
    /// Interaction logic for InvoicesReportView.xaml
    /// </summary>
    public partial class InvoicePrintView : BaseView, IInvoicePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private VMFactura _invoice;
        private bool _editable;

        public InvoicePrintView()
        {
            InitializeComponent();
            BindComponents();

            _invoice = new VMFactura();
            _editable = true;
        }

        public InvoicePrintView(VMFactura invoice)
        {
            InitializeComponent();
            BindComponents();

            _editable = false;
            Show(invoice);
        }

        private void BindComponents()
        {
            txtSerie.LostFocus += txtSerie_LostFocus;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarFacturas.Click += btnListarFacturas_Click;
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

        void btnListarFacturas_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid() && _editable)
                Find();
        }

        void txtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindLast.isValid() && _editable)
                FindLast();
        }

        public VMFactura Invoice
        {
            get { return _invoice.idFactura.isValid() && !_editable ? _invoice : new VMFactura() { serie = txtSerie.Text, folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public void Show(VMFactura invoice)
        {
            txtSerie.Text = invoice.serie;
            txtFolio.Text = invoice.folio.ToStringOrDefault();

            txtSerie.IsReadOnly = invoice.idFactura.isValid() && !_editable;
            txtFolio.IsReadOnly = invoice.idFactura.isValid() && !_editable;
            btnListarFacturas.IsEnabled = !invoice.idFactura.isValid() || _editable;

            _invoice = invoice;
        }
    }
}
