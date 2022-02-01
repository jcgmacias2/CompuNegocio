using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for ClientSalesListView.xaml
    /// </summary>
    public partial class ClientSalesListView : BaseListView, IClientSalesListView
    {
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        public event Action ShowWithDebtOnly;

        private Cliente _customer;

        public ClientSalesListView(Cliente customer)
        {
            _customer = customer;

            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += ItemsListView_Loaded;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            chkSoloDeuda.Checked += ChkSoloDeudaOnChecked;

            base.Grid = dgArticulos;
            base.SearchBox = txtBusqueda;
        }

        private void ChkSoloDeudaOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Search.isValid())
            {
                Search();
            }
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

        private void ItemsListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public Cliente Customer => _customer;
        public bool WithDebtOnly => chkSoloDeuda.IsChecked.GetValueOrDefault();

        public void Show(List<VwVentasActivasPorCliente> items)
        {
            dgArticulos.ItemsSource = items;
        }
    }
}
