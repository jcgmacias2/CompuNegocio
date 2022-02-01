using System;
using System.Windows;
using System.Windows.Input;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for FiscalPaymentReportView.xaml
    /// </summary>
    public partial class FiscalPaymentPrintView : IFiscalPaymentPrintView
    {
        public event Action Find;
        public event Action FindLast;
        public event Action OpenList;
        public event Action Preview;
        public event Action Print;
        public event Action Quit;
        private AbonosDeFactura _payment;
        private bool _editable;
        public FiscalPaymentPrintView()
        {
            InitializeComponent();
            BindComponents();

            _payment = new AbonosDeFactura();
            _editable = true;
        }

        public FiscalPaymentPrintView(AbonosDeFactura payment)
        {
            InitializeComponent();
            BindComponents();

            Show(payment);
            _editable = false;
        }

        private void BindComponents()
        {
            txtSerie.LostFocus += txtSerie_LostFocus;
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarPagos.Click += btnListarPagos_Click;
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

        void btnListarPagos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        void txtSerie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindLast.isValid())
                FindLast();
        }

        public AbonosDeFactura Payment
        {
            get { return _payment.idAbonoDeFactura.isValid() && !_editable ? _payment : new AbonosDeFactura() { TimbresDeAbonosDeFactura = new TimbresDeAbonosDeFactura(){serie = txtSerie.Text, folio = txtFolio.Text.ToIntOrDefault() } }; }
        }

        public void Show(AbonosDeFactura payment)
        {
            //Se determina si es un pago simple o multiple

            if (!payment.Pago.isValid())
            {
                //Es un pago simple
                txtSerie.Text = payment.TimbresDeAbonosDeFactura.serie;
                txtFolio.Text = payment.TimbresDeAbonosDeFactura.folio.ToStringOrDefault();
            }
            else
            {
                //Es un pago multiple
                txtSerie.Text = payment.Pago.serie;
                txtFolio.Text = payment.Pago.folio.ToStringOrDefault();
            }

            txtSerie.IsReadOnly = payment.idAbonoDeFactura.isValid() && !_editable;
            txtFolio.IsReadOnly = payment.idAbonoDeFactura.isValid() && !_editable;
            btnListarPagos.IsEnabled = !payment.idAbonoDeFactura.isValid() || _editable;

            _payment = payment;
        }
    }
}
