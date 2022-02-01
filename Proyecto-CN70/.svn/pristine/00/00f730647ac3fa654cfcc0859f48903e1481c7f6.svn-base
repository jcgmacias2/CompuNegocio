using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for DiscountNotesListView.xaml
    /// </summary>
    public partial class DiscountNotesListView : BaseListView, IDiscountNotesListView
    {
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;
        public event Action Select;
        public event Action Search;

        private bool _onlyActives;
        private bool _onlyWithoutAssign;

        public DiscountNotesListView(bool onlyActives = false, bool onlyWithoutAssign = false)
        {
            _onlyActives = onlyActives;
            _onlyWithoutAssign = onlyWithoutAssign;

            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += BanksListView_Loaded;
            btnSeleccionar.Click += btnSeleccionar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnBuscar.Click += btnBuscar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            dgNotasDeDescuento.MouseDoubleClick += dgBancos_MouseDoubleClick;

            base.Grid = dgNotasDeDescuento;
            base.SearchBox = txtBusqueda;
        }

        private void dgBancos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void BanksListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public NotasDeDescuento DiscountNote
        {
            get { return CurrentRecord >= 0 ? (NotasDeDescuento)dgNotasDeDescuento.SelectedItem : new NotasDeDescuento(); }
        }

        public bool OnlyWithoutAssign => _onlyWithoutAssign;
        public bool OnlyActive => _onlyActives;

        public void Show(List<NotasDeDescuento> discountNotes)
        {
            dgNotasDeDescuento.ItemsSource = discountNotes;
        }
    }
}
