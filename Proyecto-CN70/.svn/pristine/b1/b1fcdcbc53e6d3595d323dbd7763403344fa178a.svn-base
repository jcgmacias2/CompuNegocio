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
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for StockReportView.xaml
    /// </summary>
    public partial class BillsOfSalePerPeriodReportView : BaseView, IBillsOfSalePerPeriodReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;

        public BillsOfSalePerPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
        }

        void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Print.isValid())
                Print();
            Mouse.OverrideCursor = null;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Preview.isValid())
                Preview();
            Mouse.OverrideCursor = null;
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public void FillCombos(List<object> billOfSaleTypes)
        {
            cbStatus.ItemsSource = billOfSaleTypes;
            cbStatus.DisplayMemberPath = "Text";
            cbStatus.SelectedValuePath = "Value";
        }

        public void SetDates(DateTime startDate, DateTime endDate)
        {
            dpFechaInicio.SelectedDate = startDate;
            dpFechaFin.SelectedDate = endDate;
        }

        public DateTime StartDate { get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today); } }
        public DateTime EndDate { get { return dpFechaFin.SelectedDate.GetValueOrDefault(DateTime.Today); } }
        public Tipos_Reporte_Remisiones BillOfSaleType { get { return (Tipos_Reporte_Remisiones)cbStatus.SelectedValue; }  }
    }
}
