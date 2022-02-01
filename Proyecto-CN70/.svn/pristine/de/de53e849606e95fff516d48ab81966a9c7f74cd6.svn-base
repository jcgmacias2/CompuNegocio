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
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for PaymentPrintView.xaml
    /// </summary>
    public partial class PaymentPrintView : BaseView, IPaymentPrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private Pago _payment;
        private bool _editable;

        public PaymentPrintView()
        {
            InitializeComponent();
            BindComponents();

            _payment = new Pago();
            _editable = true;
        }

        public PaymentPrintView(Pago payment)
        {
            InitializeComponent();
            BindComponents();

            _editable = false;
            Show(payment);
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

        public Pago Payment
        {
            get
            {
                return _payment.idPago.isValid() && !_editable ? _payment : 
                    new Pago() { serie = txtSerie.Text, folio = txtFolio.Text.ToIntOrDefault() };
            }
        }

        public void Show(Pago payment)
        {
            txtSerie.Text = payment.serie;
            txtFolio.Text = payment.folio.ToStringOrDefault();

            txtSerie.IsReadOnly = payment.idPago.isValid() && !_editable;
            txtFolio.IsReadOnly = payment.idPago.isValid() && !_editable;
            btnListarFacturas.IsEnabled = !payment.idPago.isValid() || _editable;

            _payment = payment;
        }
    }
}
