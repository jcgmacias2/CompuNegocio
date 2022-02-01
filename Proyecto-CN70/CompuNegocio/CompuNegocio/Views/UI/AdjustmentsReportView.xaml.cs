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
    /// Interaction logic for AdjustmentsReportView.xaml
    /// </summary>
    public partial class AdjustmentsReportView : BaseView, IAdjustmentsReportView
    {
        public event Action Quit;
        public event Action Preview;

        public AdjustmentsReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
        }

        void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (Preview.isValid())
                Preview();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        public TiposDeAjuste Type
        {
            get { return cmbTipo.SelectedIndex >= 0 ? (TiposDeAjuste)cmbTipo.SelectedItem : null; }
        }

        public DateTime Start
        {
            get { return dpFechaInicio.SelectedDate.GetValueOrDefault(); }
        }

        public DateTime End
        {
            get { return dpFechaFin.SelectedDate.GetValueOrDefault(); }
        }

        public void Fill(List<TiposDeAjuste> types)
        {
            cmbTipo.ItemsSource = types;
            cmbTipo.SelectedValuePath = "idTipoDeAjuste";
            cmbTipo.DisplayMemberPath = "descripcion";
        }
    }
}
