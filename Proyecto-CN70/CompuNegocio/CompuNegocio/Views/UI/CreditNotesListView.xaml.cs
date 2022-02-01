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
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for CreditNotesListView.xaml
    /// </summary>
    public partial class CreditNotesListView : BaseListView, ICreditNotesListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        private bool _onlyWithoutInvoice;
        private bool _onlyActives;

        public CreditNotesListView()
        {
            InitializeComponent();
            BindComponents();
        }

        public CreditNotesListView(bool onlyWithoutInvoice, bool onlyActives)
        {
            InitializeComponent();
            BindComponents();
            _onlyWithoutInvoice = onlyWithoutInvoice;
            _onlyActives = onlyActives;
        }

        private void BindComponents()
        {
            this.Loaded += ClientPaymentsListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgNotasDeCredito.MouseDoubleClick += dgNotasDeCredito_MouseDoubleClick;

            base.Grid = dgNotasDeCredito;
            base.SearchBox = txtBusqueda;
        }

        private void dgNotasDeCredito_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void btnFinal_Click(object sender, RoutedEventArgs e)
        {
            if (GoLast.isValid())
                GoLast();
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (GoNext.isValid())
                GoNext();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (GoPrevious.isValid())
                GoPrevious();
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            if (GoFirst.isValid())
                GoFirst();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void ClientPaymentsListView_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Search.isValid())
                Search();
            Mouse.OverrideCursor = null;
        }

        public VMNotaDeCredito CreditNote
        {
            get { return CurrentRecord >= 0 ? (VMNotaDeCredito)dgNotasDeCredito.SelectedItem : null; }
        }

        public bool OnlyWithoutInvoice => _onlyWithoutInvoice;
        public bool OnlyActives => _onlyActives;

        public void Show(List<VMNotaDeCredito> creditNotes)
        {
            dgNotasDeCredito.ItemsSource = creditNotes.OrderByDescending(p => p.idNotaDeCredito);
        }
    }
}
