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
    /// Interaction logic for PricesListView.xaml
    /// </summary>
    public partial class PricesListView : BaseListView, IPricesListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        public PricesListView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += PricesListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgListasDePrecios.MouseDoubleClick += dgListasDePrecios_MouseDoubleClick;

            base.Grid = dgListasDePrecios;
            base.SearchBox = txtBusqueda;
        }

        private void dgListasDePrecios_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void PricesListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public ListasDePrecio PricesList
        {
            get { return CurrentRecord >= 0 ? (ListasDePrecio)dgListasDePrecios.SelectedItem : new ListasDePrecio(); }
        }

        public TipoDeBusqueda SearchType
        {
            get { return (TipoDeBusqueda)cmbTipoDeBusqueda.SelectedIndex; }
        }

        public void Show(List<ListasDePrecio> pricesLists)
        {
            dgListasDePrecios.ItemsSource = pricesLists;   
        }

        public void FillCombo(List<TipoDeBusqueda> searchTypes)
        {
            cmbTipoDeBusqueda.ItemsSource = searchTypes;
            cmbTipoDeBusqueda.SelectedIndex = 0;
        }
    }
}
