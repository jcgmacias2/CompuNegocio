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
    /// Interaction logic for TransferPrintView.xaml
    /// </summary>
    public partial class TransferPrintView : BaseView, ITransferPrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;

        private VMTraspaso _transfer;

        public TransferPrintView()
        {
            InitializeComponent();
            BindComponents();

            _transfer = new VMTraspaso();
        }

        public TransferPrintView(VMTraspaso transfer)
        {
            InitializeComponent();
            BindComponents();

            Show(transfer);
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarTraspasos.Click += btnListarPedidos_Click;
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

        void btnListarPedidos_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public VMTraspaso Transfer
        {
            get { return _transfer.idTraspaso.isValid() ? _transfer : new VMTraspaso() { folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public void Show(VMTraspaso transfer)
        {
            txtFolio.Text = transfer.folio.ToStringOrDefault();

            txtFolio.IsReadOnly = transfer.idTraspaso.isValid();
            btnListarTraspasos.IsEnabled = !transfer.idTraspaso.isValid();

            _transfer = transfer;
        }
    }
}