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
    public partial class CommissionsPerPeriodReportView : BaseView, ICommissionsPerPeriodReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        public event Action OpenUsersList;

        private Usuario _user;

        public CommissionsPerPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnListarUsuarios.Click += BtnListarUsuariosOnClick;
        }

        private void BtnListarUsuariosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenUsersList.isValid())
            {
                OpenUsersList();
            }
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

        public void SetDates(DateTime startDate, DateTime endDate)
        {
            dpFechaInicio.SelectedDate = startDate;
            dpFechaFin.SelectedDate = endDate;
        }

        public void Show(Usuario user)
        {
            txtUsuario.Text = user.nombreDeUsuario;
            _user = user;
        }

        public DateTime StartDate { get { return dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today); } }
        public DateTime EndDate { get { return dpFechaFin.SelectedDate.GetValueOrDefault(DateTime.Today); } }
        public Usuario User { get { return _user ?? new Usuario(); }  }
    }
}
