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
    /// Interaction logic for ItemsListView.xaml
    /// </summary>
    public partial class ItemsListView : BaseListView, IItemsListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        private bool _onlyActives;

        public ItemsListView(bool onlyActives)
        {
            InitializeComponent();
            _onlyActives = onlyActives;
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += ItemsListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgArticulos.MouseDoubleClick += dgArticulos_MouseDoubleClick;
            dgArticulos.PreviewKeyUp += dgArticulos_PreviewKeyUp;

            base.Grid = dgArticulos;
            base.SearchBox = txtBusqueda;
        }

        private void dgArticulos_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void dgArticulos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void ItemsListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public VMArticulo Item
        {
            get { return CurrentRecord >= 0 ? (VMArticulo)dgArticulos.SelectedItem : new VMArticulo(); }
        }

        public bool OnlyActives => _onlyActives;

        public void Show(List<VMArticulo> items)
        {
            dgArticulos.ItemsSource = items;
        }
    }
}
