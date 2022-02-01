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
    /// Interaction logic for InvoicePayments.xaml
    /// </summary>
    public partial class FiscalPaymentsView : BaseView, IFiscalPaymentsView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Cancel;
        private int _idFiscalPayment;

        public FiscalPaymentsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarParcialidades.Click += btnListarParcialidades_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnCerrar.Click += btnCerrar_Click;

            _idFiscalPayment = -1;
        }

        void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel.isValid())
                Cancel();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnListarParcialidades_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public AbonosDeFactura FiscalPayment
        {
            get
            {
                return new AbonosDeFactura()
                {
                    idAbonoDeFactura = _idFiscalPayment,
                    TimbresDeAbonosDeFactura = new TimbresDeAbonosDeFactura()
                    {
                        serie = txtSerie.Text,
                        folio = txtFolio.Text.ToIntOrDefault()
                    }
                };
            }
        }


        public bool IsDirty
        {
            get { return _idFiscalPayment.isValid(); }
        }

        public void Show(AbonosDeFactura payment)
        {
            txtSerie.Text = payment.TimbresDeAbonosDeFactura.serie;
            txtFolio.Text = payment.TimbresDeAbonosDeFactura.folio.ToString();
            txtImporte.Text = payment.monto.ToDecimalString();
            txtMoneda.Text = payment.Moneda.descripcion;
            txtCliente.Text = payment.Factura.Cliente.razonSocial;
            txtFecha.Text = payment.fechaHora.ToShortDateString();

            _idFiscalPayment = payment.idAbonoDeFactura;
        }

        public void Clear()
        {
            txtSerie.Clear();
            txtFolio.Clear();
            txtImporte.Clear();
            txtMoneda.Clear();
            txtCliente.Clear();
            txtFecha.Clear();

            _idFiscalPayment = -1;
        }
    }
}
