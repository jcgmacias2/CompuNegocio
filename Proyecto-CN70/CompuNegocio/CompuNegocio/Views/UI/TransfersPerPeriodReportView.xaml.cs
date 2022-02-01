using Aprovi.Business.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Aprovi.Data.Models;

namespace Aprovi.Views.UI
{
    /// <summary>
    /// Interaction logic for TransfersPerPeriodReportView.xaml
    /// </summary>
    public partial class TransfersByPeriodReportView : BaseView, ITransfersByPeriodReportView
    {
        public event Action Quit;
        public event Action Preview;
        public event Action Print;
        
        public event Action OpenOriginAssociatedCompaniesList;
        public event Action OpenDestinationAssociatedCompaniesList;
        public event Action OpenTransfersList;
        public event Action FindOriginAssociatedCompany;
        public event Action FindDestinationAssociatedCompany;
        public event Action FindTransfer;

        private int _idTransfer;
        private int _idOriginAssociatedCompany;
        private int _idDestinationAssociatedCompany;


        public TransfersByPeriodReportView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            btnCerrar.Click += btnCerrar_Click;
            btnVistaPrevia.Click += btnVistaPrevia_Click;
            btnImprimir.Click += btnImprimir_Click;
            btnListarEmpresasOrigen.Click += BtnListarEmpresasOrigenOnClick;
            btnListarEmpresasDestino.Click += BtnListarEmpresasDestinoOnClick;
            btnListarTraspasos.Click += BtnListarTraspasosOnClick;
            txtEmpresaOrigen.LostFocus += TxtEmpresaOrigenOnLostFocus;
            txtEmpresaDestino.LostFocus += TxtEmpresaDestinoOnLostFocus;
            txtTraspaso.LostFocus += TxtTraspasoOnLostFocus;
            rbSoloTraspaso.Click += UpdateTransferType;
            rbTodosTraspasos.Click += UpdateTransferType;
        }

        private void UpdateTransferType(object sender, RoutedEventArgs routedEventArgs)
        {
            SetEnvironment(rbTodosTraspasos.IsChecked.GetValueOrDefault(false) ? TiposDeReporteTraspasos.Todos : TiposDeReporteTraspasos.Traspaso);
        }

        private void TxtTraspasoOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindTransfer.isValid())
            {
                FindTransfer();
            }
        }

        private void TxtEmpresaDestinoOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindDestinationAssociatedCompany.isValid())
            {
                FindDestinationAssociatedCompany();
            }
        }

        private void TxtEmpresaOrigenOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (FindOriginAssociatedCompany.isValid())
            {
                FindOriginAssociatedCompany();
            }
        }

        private void BtnListarTraspasosOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenTransfersList.isValid())
            {
                OpenTransfersList();
            }
        }

        private void BtnListarEmpresasDestinoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenDestinationAssociatedCompaniesList.isValid())
            {
                OpenDestinationAssociatedCompaniesList();
            }
        }

        private void BtnListarEmpresasOrigenOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (OpenOriginAssociatedCompaniesList.isValid())
            {
                OpenOriginAssociatedCompaniesList();
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

        public void Show(VMReporteTraspasos vm)
        {
            if (vm.Transfer.isValid())
            {
                txtTraspaso.Text = vm.Transfer.folio.isValid() ? vm.Transfer.folio.ToString() : "";
                _idTransfer = vm.Transfer.idTraspaso;
            }

            if (vm.OriginCompany.isValid())
            {
                txtEmpresaOrigen.Text = vm.OriginCompany.nombre;
                _idOriginAssociatedCompany = vm.OriginCompany.idEmpresaAsociada;
            }

            if (vm.DestinationCompany.isValid())
            {
                txtEmpresaDestino.Text = vm.DestinationCompany.nombre;
                _idDestinationAssociatedCompany = vm.DestinationCompany.idEmpresaAsociada;
            }

            dpFechaInicio.SelectedDate = vm.StartDate;
            dpFechaFin.SelectedDate = vm.EndDate;
        }

        public VMReporteTraspasos Report => new VMReporteTraspasos()
        {
            Transfer = new Traspaso() { folio = txtTraspaso.Text.ToIntOrDefault(), idTraspaso = _idTransfer },
            OriginCompany = new EmpresasAsociada() { nombre = txtEmpresaOrigen.Text, idEmpresaAsociada = _idOriginAssociatedCompany},
            DestinationCompany = new EmpresasAsociada() { nombre = txtEmpresaDestino.Text, idEmpresaAsociada = _idDestinationAssociatedCompany},
            ReportType = rbTodosTraspasos.IsChecked.GetValueOrDefault(false) ? TiposDeReporteTraspasos.Todos : TiposDeReporteTraspasos.Traspaso,
            StartDate = dpFechaInicio.SelectedDate.GetValueOrDefault(DateTime.Today),
            EndDate = dpFechaFin.SelectedDate.GetValueOrDefault(DateTime.Today)
        };

        private void SetEnvironment(TiposDeReporteTraspasos reportType)
        {
            txtTraspaso.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Traspaso);
            btnListarTraspasos.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Traspaso);

            txtEmpresaOrigen.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);
            txtEmpresaDestino.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);
            btnListarEmpresasDestino.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);
            btnListarEmpresasOrigen.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);
            dpFechaInicio.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);
            dpFechaFin.IsEnabled = reportType.Equals(TiposDeReporteTraspasos.Todos);

            txtTraspaso.Clear();
            txtEmpresaDestino.Clear();
            txtEmpresaOrigen.Clear();

            _idTransfer = 0;
            _idDestinationAssociatedCompany = 0;
            _idOriginAssociatedCompany = 0;
        }
    }
}
