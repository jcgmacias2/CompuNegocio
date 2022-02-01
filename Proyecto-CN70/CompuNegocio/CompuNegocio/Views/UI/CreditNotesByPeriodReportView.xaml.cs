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
    /// Interaction logic for CreditNotesByPeriodReportView.xaml
    /// </summary>
    public partial class CreditNotesByPeriodReportView : BaseView, ICreditNotesByPeriodReportView
    {
        public event Action FindCustomer;
        public event Action OpenCustomersList;
        public event Action Quit;
        public event Action Print;
        public event Action Preview;
        private Cliente _customer;

        public CreditNotesByPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtCliente.LostFocus += txtCliente_LostFocus;
            btnListarClientes.Click += btnListarClientes_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            _customer = new Cliente();
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (Print.isValid())
                Print();
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void btnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCustomersList.isValid())
                OpenCustomersList();
        }

        private void txtCliente_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindCustomer.isValid())
                FindCustomer();
        }

        public Cliente Customer
        {
            get { _customer.codigo = txtCliente.Text; return _customer; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public void Show(Cliente customer)
        {
            txtCliente.Text = customer.codigo;
            _customer = customer;
        }
    }
}
