﻿using Aprovi.Data.Models;
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
    /// Interaction logic for BillsOfSaleListView.xaml
    /// </summary>
    public partial class QuotesListView : BaseListView, IQuotesListView
    {
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        public event Action Select;
        public event Action Search;

        public QuotesListView()
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
            dgCotizaciones.MouseDoubleClick += dgRemisiones_MouseDoubleClick;

            base.Grid = dgCotizaciones;
            base.SearchBox = txtBusqueda;
        }

        void dgRemisiones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
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

        public Cotizacione Quote
        {
            get { return CurrentRecord >= 0 ? (Cotizacione)dgCotizaciones.SelectedItem : null; }
        }

        public void Show(List<Cotizacione> quotes)
        {
            dgCotizaciones.ItemsSource = quotes;
        }
    }
}
