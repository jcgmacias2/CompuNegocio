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
    /// Interaction logic for CurrencyExchangeView.xaml
    /// </summary>
    public partial class CurrencyExchangeView : BaseView, ICurrencyExchangeView
    {
        public event Action Change;
        public event Action Quit;

        private AbonosDeFactura _payment;

        public CurrencyExchangeView(AbonosDeFactura payment)
        {
            InitializeComponent();
            BindComponents();
            _payment = payment;
        }

        private void BindComponents()
        {
            btnCambiar.Click += BtnCambiar_Click;
            this.Loaded += CurrencyExchangeView_Loaded;
        }

        private void CurrencyExchangeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Change.isValid())
                Change();
        }

        private void BtnCambiar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public AbonosDeFactura PaymentExchange
        {
            get
            {
                return new AbonosDeFactura()
                {
                    monto = txtAbono.Text.ToDecimalOrDefault(),
                    idMoneda = cmbAbonoMoneda.SelectedValue.ToIntOrDefault(),
                    Moneda = (Moneda)cmbAbonoMoneda.SelectedItem,
                    tipoDeCambio = _payment.tipoDeCambio,
                    FormasPago = _payment.FormasPago,
                    idFormaPago = _payment.idFormaPago,
                    fechaHora = _payment.fechaHora,
                    CuentasBancaria = _payment.CuentasBancaria,
                    idCuentaBancaria = _payment.idCuentaBancaria,
                    CambioDivisa = new CambioDivisa() { idMonedaDivisa = cmbAbonoEquivalenteMoneda.SelectedValue.ToIntOrDefault(), monto = txtAbonoEquivalente.Text.ToDecimalOrDefault()}
                };
            }
        }


        public AbonosDeFactura Payment => _payment;

        public void Show(AbonosDeFactura payment)
        {
            //Abono ya convertido
            txtAbono.Text = _payment.monto.ToDecimalString();
            cmbAbonoMoneda.ItemsSource = null;
            var monedas = new List<Moneda>();
            monedas.Add(_payment.Moneda);
            cmbAbonoMoneda.ItemsSource = monedas;
            cmbAbonoMoneda.SelectedValuePath = "idMoneda";
            cmbAbonoMoneda.DisplayMemberPath = "descripcion";
            cmbAbonoMoneda.SelectedIndex = 0;

            //Abono en divisa original
            txtAbonoEquivalente.Text = _payment.CambioDivisa.monto.ToDecimalString();
            cmbAbonoEquivalenteMoneda.ItemsSource = null;
            var monedasEquivalencia = new List<Moneda>();
            monedasEquivalencia.Add(_payment.CambioDivisa.Moneda);
            cmbAbonoEquivalenteMoneda.ItemsSource = monedasEquivalencia;
            cmbAbonoEquivalenteMoneda.SelectedValuePath = "idMoneda";
            cmbAbonoEquivalenteMoneda.DisplayMemberPath = "descripcion";
            cmbAbonoEquivalenteMoneda.SelectedIndex = 0;

            txtAbonoCambio.Text = (_payment.CambioDivisa.monto - _payment.monto.ToDocumentCurrency(_payment.Moneda, payment.CambioDivisa.Moneda, _payment.tipoDeCambio)).ToDecimalString();
            _payment = payment;
        }
    }
}
