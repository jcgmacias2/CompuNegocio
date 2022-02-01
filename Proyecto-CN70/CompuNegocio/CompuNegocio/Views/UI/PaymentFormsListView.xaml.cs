using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for PaymentMethodsListView.xaml
    /// </summary>
    public partial class PaymentFormsListView : BaseListView, IPaymentFormsListView
    {
        public event Action Select;
        public event Action Search;
        public event Action Quit;
        public event Action GoFirst;
        public event Action GoPrevious;
        public event Action GoNext;
        public event Action GoLast;

        public PaymentFormsListView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            this.Loaded += PaymentMethodsListView_Loaded;
            dgFormasPago.MouseDoubleClick += dgFormasPago_MouseDoubleClick;
            btnBuscar.Click += btnBuscar_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnInicio.Click += btnInicio_Click;
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;
            btnFinal.Click += btnFinal_Click;
            btnSeleccionar.Click += btnSeleccionar_Click;

            base.Grid = dgFormasPago;
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

        private void dgFormasPago_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Select.isValid())
                Select();
        }

        private void PaymentMethodsListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Search.isValid())
                Search();
        }

        public FormasPago PaymentForm
        {
            get { return CurrentRecord >= 0 ? (FormasPago)dgFormasPago.SelectedItem : new FormasPago(); }
        }

        public void Show(List<FormasPago> paymentForms)
        {
            dgFormasPago.ItemsSource = paymentForms;
        }
    }
}
