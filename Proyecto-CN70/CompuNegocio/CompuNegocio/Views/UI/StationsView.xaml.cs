using Aprovi.Application.Helpers;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
    /// Interaction logic for StationsView.xaml
    /// </summary>
    public partial class StationsView : BaseView, IStationsView
    {
        public event Action Find;
        public event Action OpenList;
        public event Action Quit;
        public event Action New;
        public event Action Delete;
        public event Action Save;
        public event Action Update;
        public event Action AssociateStation;
        public event Action DissociateStation;
        public event Action ListPorts;

        private int _idStation;
        private string _computer;

        public StationsView()
        {
            InitializeComponent();
            BindComponents();
        }

        private void BindComponents()
        {
            txtDescripcion.LostFocus += txtDescripcion_LostFocus;
            btnListarEstaciones.Click += btnListarEstaciones_Click;
            btnCerrar.Click += btnCerrar_Click;
            btnNuevo.Click += btnNuevo_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            chkRelacionarEquipo.Checked += chkRelacionarEquipo_Checked;
            chkRelacionarEquipo.Unchecked += chkRelacionarEquipo_Unchecked;
            btnLeerPuertosBascula.Click += btnLeerPuertosBascula_Click;

            _idStation = 0;
            _computer = string.Empty;
        }

        void btnLeerPuertosBascula_Click(object sender, RoutedEventArgs e)
        {
            if (ListPorts.isValid())
                ListPorts();
        }

        void chkRelacionarEquipo_Unchecked(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (DissociateStation.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                DissociateStation();
            Mouse.OverrideCursor = null;
        }

        void chkRelacionarEquipo_Checked(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (AssociateStation.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                AssociateStation();
            Mouse.OverrideCursor = null;
        }

        void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (IsDirty)
            {
                if (Update.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                    Update();
            }
            else
            {
                if (Save.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                    Save();
            }

            this.tabEstacion.Focus();
        }

        void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                Delete();
        }

        void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (New.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                New();
        }

        void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        void btnListarEstaciones_Click(object sender, RoutedEventArgs e)
        {
            if (OpenList.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                OpenList();
        }

        void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Find.isValid() && Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BusinessPresenter", true))
                Find();
        }

        public Estacione Station
        {
            get
            {
                return new Estacione()
                {
                    idEstacion = _idStation,
                    descripcion = txtDescripcion.Text,
                    idEmpresa = cmbEmpresas.SelectedValue.ToIntOrDefault(),
                    Empresa = cmbEmpresas.SelectedValue.isValid() ? (Empresa)cmbEmpresas.SelectedItem : new Empresa(),
                    equipo = _computer,
                    ImpresorasPorEstacions = new List<ImpresorasPorEstacion>()
                    { 
                        new ImpresorasPorEstacion{idTipoDeImpresora = (int)TipoDeImpresora.Recibos, idEstacion = _idStation, impresora = cmbImpresoraDeTickets.SelectedValue.ToStringOrDefault()},
                        new ImpresorasPorEstacion(){idTipoDeImpresora = (int)TipoDeImpresora.Reportes, idEstacion = _idStation, impresora = cmbImpresoraDeReportes.SelectedValue.ToStringOrDefault()}
                    },
                    Bascula = new Bascula()
                    {
                        idBasculaEstacion = _idStation,
                        puerto = cmbPuertoBascula.SelectedValue.ToStringOrDefault(),
                        velocidad = txtVelocidadBascula.Text.ToIntOrDefault(),
                        tiempoDeEscritura = txtTiempoDeEscrituraBascula.Text.ToIntOrDefault(),
                        tiempoDeLectura = txtTiempoDeLecturaBascula.Text.ToIntOrDefault(),
                        bitsDeParada = cmbBitsDeParadaBascula.SelectedItem.isValid() ? (int)(StopBits)cmbBitsDeParadaBascula.SelectedItem : -1,
                        paridad = cmbParidadBascula.SelectedItem.isValid() ? (int)(Parity)cmbParidadBascula.SelectedItem : -1,
                        bitsDeDatos = txtBitsDeDatosBascula.Text.ToIntOrDefault(),
                        finDeLinea = txtFinDeLineaBascula.Text
                    }
                };
            }
        }

        public bool IsDirty
        {
            get { return _idStation.isValid(); }
        }

        public bool IsStationSet
        {
            get { return chkRelacionarEquipo.IsChecked.Value; }
        }

        public Configuracion Configuration
        {
            get { return new Configuracion() { Escaner = chkUtilizarEscaner.IsChecked.Value, CodigoEscaner = txtEscaner.Text, CajonDeEfectivo = chkUtilizarCajon.IsChecked.Value }; }
        }

        public void Show(Estacione station, Configuracion configuration)
        {
            txtDescripcion.Text = station.descripcion;
            cmbEmpresas.SelectedItem = station.Empresa;
            var reports = station.ImpresorasPorEstacions.FirstOrDefault(i => i.idTipoDeImpresora.Equals((int)TipoDeImpresora.Reportes));
            var tickets = station.ImpresorasPorEstacions.FirstOrDefault(i => i.idTipoDeImpresora.Equals((int)TipoDeImpresora.Recibos));
            cmbImpresoraDeReportes.SelectedItem = reports.isValid()? reports.impresora: null;
            cmbImpresoraDeTickets.SelectedItem = tickets.isValid() ? tickets.impresora : null;
            //Báscula
            if(cmbPuertoBascula.HasItems)
                cmbPuertoBascula.Items.Clear();
            cmbPuertoBascula.Items.Add(station.Bascula.puerto);
            cmbPuertoBascula.SelectedItem = station.Bascula.puerto;
            txtTiempoDeEscrituraBascula.Text = station.Bascula.tiempoDeEscritura.ToStringOrDefault();
            cmbBitsDeParadaBascula.SelectedItem = (StopBits)station.Bascula.bitsDeParada;
            txtBitsDeDatosBascula.Text = station.Bascula.bitsDeParada.ToStringOrDefault();
            txtVelocidadBascula.Text = station.Bascula.velocidad.ToStringOrDefault();
            txtTiempoDeLecturaBascula.Text = station.Bascula.tiempoDeLectura.ToStringOrDefault();
            cmbParidadBascula.SelectedItem = (Parity)station.Bascula.paridad;
            txtFinDeLineaBascula.Text = station.Bascula.finDeLinea;

            //Tengo que desasociar el evento para que no se ejecute
            chkRelacionarEquipo.Checked -= chkRelacionarEquipo_Checked;
            chkRelacionarEquipo.Unchecked -= chkRelacionarEquipo_Unchecked;
            chkRelacionarEquipo.IsChecked = Session.Station.isValid() && station.idEstacion.Equals(Session.Station.idEstacion);
            chkRelacionarEquipo.Checked += chkRelacionarEquipo_Checked;
            chkRelacionarEquipo.Unchecked += chkRelacionarEquipo_Unchecked;

            //Si es el equipo relacionado muestro la configuración, de lo contrario no muestro nada
            chkUtilizarEscaner.IsChecked = Session.Station.isValid() && station.idEstacion.Equals(Session.Station.idEstacion) && Session.Configuration.isValid() && Session.Configuration.Escaner;
            txtEscaner.Text = chkUtilizarEscaner.IsChecked.Value ? configuration.CodigoEscaner : string.Empty;
            chkUtilizarCajon.IsChecked = Session.Station.isValid() && station.idEstacion.Equals(Session.Station.idEstacion) && Session.Configuration.isValid() && Session.Configuration.CajonDeEfectivo;

            _idStation = station.idEstacion;
            _computer = station.equipo;
        }

        public void Clear()
        {
            txtDescripcion.Clear();
            cmbEmpresas.SelectedIndex = -1;
            cmbImpresoraDeReportes.SelectedIndex = -1;
            cmbImpresoraDeTickets.SelectedIndex = -1;

            //Báscula
            cmbPuertoBascula.SelectedIndex = -1;
            txtTiempoDeEscrituraBascula.Clear();
            cmbBitsDeParadaBascula.SelectedIndex = -1;
            txtBitsDeDatosBascula.Clear();
            txtVelocidadBascula.Clear();
            txtTiempoDeLecturaBascula.Clear();
            cmbParidadBascula.SelectedIndex = -1;
            txtFinDeLineaBascula.Clear();
            //Tengo que desasociar el evento para que no se ejecute
            chkRelacionarEquipo.Checked -= chkRelacionarEquipo_Checked;
            chkRelacionarEquipo.Unchecked -= chkRelacionarEquipo_Unchecked;
            chkRelacionarEquipo.IsChecked = false;
            chkRelacionarEquipo.Checked += chkRelacionarEquipo_Checked;
            chkRelacionarEquipo.Unchecked += chkRelacionarEquipo_Unchecked;

            //Limpio siempre lo de la configuración
            chkUtilizarEscaner.IsChecked = false;
            txtEscaner.Clear();

            _idStation = -1;
            _computer = string.Empty;
        }

        public void FillCombos(List<string> printers, List<Empresa> businesses, List<StopBits> stopBits, List<Parity> parities)
        {
            cmbImpresoraDeReportes.Items.Clear();
            cmbImpresoraDeReportes.ItemsSource = printers;

            cmbImpresoraDeTickets.Items.Clear();
            cmbImpresoraDeTickets.ItemsSource = printers;

            cmbEmpresas.Items.Clear();
            cmbEmpresas.ItemsSource = businesses;
            cmbEmpresas.DisplayMemberPath = "descripcion";
            cmbEmpresas.SelectedValuePath = "idEmpresa";

            cmbBitsDeParadaBascula.Items.Clear();
            cmbBitsDeParadaBascula.ItemsSource = stopBits;

            cmbParidadBascula.Items.Clear();
            cmbParidadBascula.ItemsSource = parities;
        }

        public void FillPorts(List<string> ports)
        {
            cmbPuertoBascula.Items.Clear();
            cmbPuertoBascula.ItemsSource = ports;
        }
    }
}
