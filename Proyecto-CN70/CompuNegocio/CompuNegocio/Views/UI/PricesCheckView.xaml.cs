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
    /// Interaction logic for PricesCheckView.xaml
    /// </summary>
    public partial class PricesCheckView : BaseListView, IPricesCheckView
    {
        public event Action Load;
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        private Cliente _client;

        public PricesCheckView()
        {
            InitializeComponent();
            BindComponents();

            _client = null;
        }

        public PricesCheckView(Cliente client)
        {
            InitializeComponent();
            BindComponents();

            _client = client;
        }

        private void BindComponents()
        {
            this.Loaded += PricesCheckView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgPrices.MouseDoubleClick += dgPrices_MouseDoubleClick;

            base.Grid = dgPrices;
            base.SearchBox = txtBusqueda;
        }

        private void dgPrices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void PricesCheckView_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Load.isValid())
                Load();
            Mouse.OverrideCursor = null;
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

        public VMArticulo Item
        {
            get { return CurrentRecord >= 0 ? (VMArticulo)dgPrices.SelectedItem : null; }
        }

        public List<VMArticulo> items
        {
            get { return dgPrices.ItemsSource.Cast<VMArticulo>().ToList(); }
        }

        public Cliente Client
        {
            get { return _client; }
        }

        public void Show(List<VMArticulo> items)
        {
            dgPrices.ItemsSource = items;
        }
    }
}
