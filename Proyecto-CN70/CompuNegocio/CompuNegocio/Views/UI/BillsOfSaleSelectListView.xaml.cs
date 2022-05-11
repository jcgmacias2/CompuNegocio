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
using Aprovi.Business.ViewModels;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for BillsOfSaleSelectListView.xaml
    /// </summary>
    public partial class BillsOfSaleSelectListView : BaseListView, IBillsOfSaleSelectListView
    {
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        public event Action Select;
        public event Action Search;
        public event Action SelectAll;
        public event Action DeselectAll;
        public event Action SearchDate;

        public BillsOfSaleSelectListView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += BillsOfSaleListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            btnSeleccionarTodo.Click += BtnSeleccionarTodoOnClick;
            btnDeseleccionarTodo.Click += BtnDeseleccionarTodoOnClick;
            btnBuscarDate.Click += btnBuscarDate_Click;

            base.Grid = dgRemisiones;
            base.SearchBox = txtBusqueda;
        }

        private void BtnDeseleccionarTodoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (DeselectAll.isValid())
            {
                DeselectAll();
            }
        }

        private void BtnSeleccionarTodoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (SelectAll.isValid())
            {
                SelectAll();
            }
        }

        void BillsOfSaleListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
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

        private void btnBuscarDate_Click(object sender, RoutedEventArgs e)
        {
            if (SearchDate.isValid())
                SearchDate();
        }

        public List<VMRemision> SelectedBillsOfSale
        {
            get
            {
                List<VMRemision> billsOfSale = dgRemisiones.Items.Cast<VMRemision>().Where(x => x.Selected).ToList();

                return billsOfSale;
            }
        }

        public List<VMRemision> BillsOfSale
        {
            get
            {
                List<VMRemision> billsOfSale = dgRemisiones.Items.Cast<VMRemision>().ToList();

                return billsOfSale;
            }
        }

        public void Show(List<VMRemision> billsOfSale)
        {
            dgRemisiones.ItemsSource = billsOfSale;
        }

        public DateTime Start
        {
            get { return dpFechaIni.SelectedDate.GetValueOrDefault(); }
        }

        public DateTime End
        {
            get { return dpFechaFin.SelectedDate.GetValueOrDefault(); }
        }
    }
}
