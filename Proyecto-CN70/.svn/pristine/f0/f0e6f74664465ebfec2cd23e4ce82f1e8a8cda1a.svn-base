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
    /// Interaction logic for QuotePrintView.xaml
    /// </summary>
    public partial class QuotePrintView : BaseView, IQuotePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action SendEmail;

        private VMCotizacion _quote;

        private bool _editable;
        private bool _useGuardian;

        public QuotePrintView(bool useGuardian)
        {
            _useGuardian = useGuardian;
            InitializeComponent();
            BindComponents();

            _quote = new VMCotizacion();
            _editable = true;
        }

        public QuotePrintView(VMCotizacion quote, bool useGuardian)
        {
            InitializeComponent();
            BindComponents();

            _editable = false;
            _useGuardian = useGuardian;

            Show(quote);
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarFacturas.Click += btnListarFacturas_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnEnviar.Click += BtnEnviarOnClick;
        }

        private void BtnEnviarOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (SendEmail.isValid())
            {
                SendEmail();
            }
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
            if (Find.isValid())
                Find();
        }

        public VMCotizacion Quote
        {
            get { return _quote.idCotizacion.isValid() && !_editable ? _quote : new VMCotizacion() { folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public string GivenEmail
        {
            get { return txtCorreoElectronico.Text; }
        }

        public Opciones_Envio_Correo EmailOption => rdCorreoProporcionado.IsChecked.GetValueOrDefault(false) ? Opciones_Envio_Correo.Proporcionada : Opciones_Envio_Correo.Configuradas;

        public void Show(VMCotizacion quote)
        {
            txtFolio.Text = quote.folio.ToStringOrDefault();

            txtFolio.IsReadOnly = quote.idCotizacion.isValid() && !_editable;
            btnListarFacturas.IsEnabled = !quote.idCotizacion.isValid() || _editable;
            tabGuardian.Visibility = _useGuardian ? Visibility.Visible : Visibility.Hidden;

            _quote = quote;
        }
    }
}
