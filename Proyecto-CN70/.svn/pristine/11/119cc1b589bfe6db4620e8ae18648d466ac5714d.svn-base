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
    /// Interaction logic for PurchasesListView.xaml
    /// </summary>
    public partial class PurchasesListView : BaseListView, IPurchasesListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        private int _idSupplier;

        public PurchasesListView()
        {
            InitializeComponent();
            BindComponents();

            _idSupplier = -1;
        }

        public PurchasesListView(int idSupplier)
        {
            InitializeComponent();
            BindComponents();

            _idSupplier = idSupplier;
        }

        private void BindComponents()
        {
            this.Loaded += PurchasesListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgCompras.MouseDoubleClick += dgCompras_MouseDoubleClick;
            

            this.Grid = dgCompras;
            this.SearchBox = txtBusqueda;
        }

        private void dgCompras_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void PurchasesListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public VMCompra Purchase
        {
            get { return CurrentRecord >= 0 ? (VMCompra)dgCompras.SelectedItem : new VMCompra(); }
        }

        public int IdSupplier
        {
            get { return _idSupplier; }
        }

        public void Show(List<VMCompra> purchases)
        {
            dgCompras.ItemsSource = purchases;
        }
    }
}
