using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
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
            chkGlobalInvoice.Click += chkGlobalInvoice_Click;
            dpFechaIni.SelectedDateChanged += dpFechaIni_dateChange;

            lbl_periodicidad_cfg.Content = Session.Configuration.Periodicidad.descripcion;

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

        private void chkGlobalInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (chkGlobalInvoice.IsChecked.GetValueOrDefault(false)){
                //Diario, Semanal, Quincenal, Mensual, Bimestras
                //if (Session.Configuration.idPeriodicidad == 1 || Session.Configuration.idPeriodicidad == 2 || Session.Configuration.idPeriodicidad == 3 || Session.Configuration.idPeriodicidad == 4 || Session.Configuration.idPeriodicidad == 5) {
                    dpFechaIni.IsEnabled = true;
                    dpFechaFin_show.Opacity = Session.Configuration.idPeriodicidad == 1 ? 0 : 50;
                    lbl_endDate.Opacity = Session.Configuration.idPeriodicidad == 1 ? 0 : 50;
                    btnBuscarDate.IsEnabled = Session.Configuration.idPeriodicidad == 1 ? true : false;
                //}
            }
            else{
                dpFechaIni.IsEnabled = false;
                dpFechaFin.IsEnabled = false;
                dpFechaFin_show.IsEnabled = false;
                btnBuscarDate.IsEnabled = false;
            };
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

        private void dpFechaIni_dateChange(object sender, SelectionChangedEventArgs e)
        {
            if (dpFechaIni.IsEnabled)
            {
                var fecha = dpFechaIni.SelectedDate.Value.Date;

                if (Session.Configuration.idPeriodicidad == 1)
                {
                    dpFechaFin.SelectedDate = fecha.AddHours(24);
                    dpFechaFin_show.SelectedDate = fecha;
                    btnBuscarDate.IsEnabled = true;
                }

                if (Session.Configuration.idPeriodicidad == 2)
                {
                    if (fecha.DayOfWeek != DayOfWeek.Monday)
                    {
                        MessageBox.Show("Debe seleccionar un lunes");
                    }
                    else
                    {
                        dpFechaFin.SelectedDate = fecha.AddDays(7);
                        dpFechaFin_show.SelectedDate = fecha.AddDays(6);
                        btnBuscarDate.IsEnabled = true;
                    }
                }

                if (Session.Configuration.idPeriodicidad == 3)
                {
                    if (fecha.Day != 1 && fecha.Day != 15) 
                    { 
                        MessageBox.Show("Debe seleccionar el día 1 o 15 del mes.");
                    }
                    else
                    {
                        dpFechaFin.SelectedDate = fecha.Day == 1 ? fecha.AddDays(15) : fecha.AddMonths(1).AddDays(-14);
                        dpFechaFin_show.SelectedDate = fecha.Day == 1 ? fecha.AddDays(14) : fecha.AddMonths(1).AddDays(-15);
                        btnBuscarDate.IsEnabled = true;
                    }
                }

                if (Session.Configuration.idPeriodicidad == 4 || Session.Configuration.idPeriodicidad == 5)
                {
                    if (fecha.Day != 1)
                    {
                        MessageBox.Show("Debe seleccionar el día 1 del mes.");
                    }
                    else
                    {
                        dpFechaFin.SelectedDate = Session.Configuration.idPeriodicidad == 4 ? fecha.AddMonths(1) : fecha.AddMonths(2);
                        dpFechaFin_show.SelectedDate = Session.Configuration.idPeriodicidad == 4 ? fecha.AddMonths(1).AddDays(-1) : fecha.AddMonths(2).AddDays(-1);
                        btnBuscarDate.IsEnabled = true;
                    }
                }
            }
        }

    }
}
