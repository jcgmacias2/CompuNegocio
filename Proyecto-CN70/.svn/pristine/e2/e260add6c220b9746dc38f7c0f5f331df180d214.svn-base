using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for CFDIUsesListView.xaml
    /// </summary>
    public partial class CFDIUsesListView : BaseListView, ICFDIUsesListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        public CFDIUsesListView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += CFDIUsesListView_Loaded;
            dgUsos.MouseDoubleClick += dgUsos_MouseDoubleClick;
            btnBuscar.Click += btnBuscar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            btnSeleccionar.Click += btnSeleccionar_Click;

            base.Grid = dgUsos;
            base.SearchBox = txtBusqueda;
        }

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
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

        private void dgUsos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void CFDIUsesListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public UsosCFDI Use
        {
            get { return CurrentRecord >= 0 ? (UsosCFDI)dgUsos.SelectedItem : new UsosCFDI(); }
        }

        public void Show(List<UsosCFDI> uses)
        {
            dgUsos.ItemsSource = uses;
        }
    }
}
