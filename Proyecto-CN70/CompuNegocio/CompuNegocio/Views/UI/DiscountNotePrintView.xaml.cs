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
    /// Interaction logic for DiscountNotePrintView.xaml
    /// </summary>
    public partial class DiscountNotePrintView : BaseView, IDiscountNotePrintView
    {
        public event Action FindLast;
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        private NotasDeDescuento _discountNote;

        public DiscountNotePrintView()
        {
            InitializeComponent();
            BindComponents();

            _discountNote = new NotasDeDescuento();
        }

        public DiscountNotePrintView(NotasDeDescuento discountNote)
        {
            InitializeComponent();
            BindComponents();

            Show(discountNote);
        }

        private void BindComponents()
        {
            txtFolio.LostFocus += txtFolio_LostFocus;
            btnListarOrdenesDeCompra.Click += btnListarOrdenesDeCompra_Click;
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

        void btnListarOrdenesDeCompra_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid())
                OpenList();
        }

        void txtFolio_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid())
                Find();
        }

        public NotasDeDescuento DiscountNote
        {
            get { return _discountNote.idNotaDeDescuento.isValid() ? _discountNote : new NotasDeDescuento() { folio = txtFolio.Text.ToIntOrDefault() }; }
        }

        public void Show(NotasDeDescuento discountNote)
        {
            txtFolio.Text = discountNote.folio.ToStringOrDefault();

            txtFolio.IsReadOnly = discountNote.idNotaDeDescuento.isValid();
            btnListarOrdenesDeCompra.IsEnabled = !discountNote.idNotaDeDescuento.isValid();

            _discountNote = discountNote;
        }
    }
}
