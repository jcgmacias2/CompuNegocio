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

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for SupplierStatementReportView.xaml
    /// </summary>
    public partial class SoldItemsCostReportView : BaseView, ISoldItemsCostReportView
    {
        public event Action Quit;
        public event Action Print;
        public event Action Preview;

        public SoldItemsCostReportView()
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

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public DateTime End
        {
            get { return dpFechaFinal.SelectedDate.GetValueOrDefault(DateTime.Now); }
        }

        public bool IncludeBillsOfSale
        {
            get { return chkIncluirRemisiones.IsChecked.GetValueOrDefault(); }
        }
    }
}
