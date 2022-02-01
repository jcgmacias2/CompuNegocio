using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;


namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for UnitsOfMeasureListView.xaml
    /// </summary>
    public partial class UnitsOfMeasureListView : BaseListView, IUnitsOfMeasureListView
    {
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        public event Action Select;
        public event Action Search;

        public UnitsOfMeasureListView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += UnitsOfMeasureListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgUnidadesDeMedida.MouseDoubleClick += dgUnidadesDeMedida_MouseDoubleClick;

            base.Grid = dgUnidadesDeMedida;
            base.SearchBox = txtBusqueda;
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

        void UnitsOfMeasureListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        void dgUnidadesDeMedida_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        public UnidadesDeMedida Unit
        {
            get { return CurrentRecord >= 0 ? (UnidadesDeMedida)dgUnidadesDeMedida.SelectedItem : new UnidadesDeMedida(); }
        }

        public void Show(List<UnidadesDeMedida> unitsOfMeasure)
        {
            dgUnidadesDeMedida.ItemsSource = unitsOfMeasure;
        }
    }
}
