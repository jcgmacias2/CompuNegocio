using Aprovi.Data.Models;
using Aprovi.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : BaseView, IConfigurationView
    {
        public ConfigurationView(bool guardianActive)
        {
            InitializeComponent();
            BindComponents();

            this.tabGuardian.Visibility = guardianActive ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BindComponents()
        {
            this.Loaded += ConfigurationView_Loaded;
            btnCerrar.Click += btnCerrar_Click;
            btnGuardar.Click += btnGuardar_Click;

            btnAgregarRegimen.Click += btnAgregarRegimen_Click;
            dgRegimenes.PreviewKeyUp += dgRegimenes_PreviewKeyUp;
            txtUsuarioPAC.MouseDoubleClick += txtUsuarioPAC_MouseDoubleClick;
            txtNumeroDeCertificado.MouseDoubleClick += txtNumeroDeCertificado_MouseDoubleClick;

            btnAgregarSerie.Click += btnAgregarSerie_Click;
            dgSeries.MouseDoubleClick += dgSeries_MouseDoubleClick;

            btnBuscarRutaXml.Click += btnBuscarRutaXml_Click;
            btnBuscarRutaPdf.Click += btnBuscarRutaPdf_Click;
            btnBuscarRutaCbb.Click += btnBuscarRutaCbb_Click;

            //Guardián
            btnGuardianAgregar.Click += BtnGuardianAgregar_Click;
            dgGuardianCuentas.PreviewKeyUp += DgGuardianCuentas_PreviewKeyUp;

            //Formatos
            btnBuscarRutaLogo.Click += BtnBuscarRutaLogoOnClick;
            cmbReportes.SelectionChanged += CmbReportes_SelectionChanged;
            btnAgregarFormato.Click += BtnAgregarFormato_Click;

            SetEdition(false);
        }

        #region General

        public event Action Quit;
        public event Action Save;
        public event Action Load;
        public event Action OpenConfigurationPAC;
        public event Action OpenConfigurationCSD;

        private int _idConfiguration;

        public void FillCombo(List<Pais> countries, List<object> opcionesCosto, List<Reporte> reportes, List<Periodicidad> periodicidads)
        {
            cmbPais.ItemsSource = countries;
            cmbPais.DisplayMemberPath = "descripcion";
            cmbPais.SelectedValuePath = "idPais";

            cmbOpcionCostoAumenta.ItemsSource = opcionesCosto;
            cmbOpcionCostoAumenta.DisplayMemberPath = "Text";
            cmbOpcionCostoAumenta.SelectedValuePath = "Value";

            cmbOpcionCostoDisminuye.ItemsSource = opcionesCosto;
            cmbOpcionCostoDisminuye.DisplayMemberPath = "Text";
            cmbOpcionCostoDisminuye.SelectedValuePath = "Value";

            cmbReportes.ItemsSource = reportes;
            cmbReportes.DisplayMemberPath = "nombre";
            cmbReportes.SelectedValuePath = "idReporte";
            
            cmbPeriodicidad.ItemsSource = periodicidads;
            cmbPeriodicidad.DisplayMemberPath = "descripcion";
            cmbPeriodicidad.SelectedValuePath = "idPeriodicidad";
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (Save.isValid(AccesoRequerido.Ver_y_Agregar))
                Save();

            Mouse.OverrideCursor = null;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            if (Quit.isValid())
                Quit();
        }

        private void ConfigurationView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Load.isValid(AccesoRequerido.Ver_y_Agregar))
                Load();
        }

        private void txtUsuarioPAC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OpenConfigurationPAC.isValid())
                OpenConfigurationPAC();
        }

        private void txtNumeroDeCertificado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OpenConfigurationCSD.isValid())
                OpenConfigurationCSD();
        }

        public Configuracion Configuration
        {
            get
            {
                return new Configuracion()
                {
                    idConfiguracion = _idConfiguration,
                    razonSocial = txtRazonSocial.Text,
                    rfc = txtRFC.Text,
                    telefono = txtTelefono.Text,
                    tipoDeCambio = txtTipoDeCambio.Text.ToDecimalOrDefault(),
                    CarpetaCbb = txtRutaCbb.Text,
                    CarpetaPdf = txtRutaPdf.Text,
                    CarpetaXml = txtRutaXml.Text,
                    Regimenes = _regimes,
                    Domicilio = new Domicilio()
                    {
                        calle = txtCalle.Text,
                        numeroExterior = txtNumeroExterior.Text,
                        numeroInterior = txtNumeroInterior.Text,
                        colonia = txtColonia.Text,
                        ciudad = txtCiudad.Text,
                        estado = txtEstado.Text,
                        idPais = cmbPais.SelectedValue.ToIntOrDefault(),
                        codigoPostal = txtCodigoPostal.Text,
                    },
                    usuarioPAC = txtUsuarioPAC.Text,
                    CuentasGuardians = dgGuardianCuentas.ItemsSource.Cast<CuentasGuardian>().ToList(),
                    idOpcionCostoDisminuye = cmbOpcionCostoDisminuye.SelectedValue.ToIntOrDefault(),
                    idOpcionCostoAumenta = cmbOpcionCostoAumenta.SelectedValue.ToIntOrDefault(),
                    FormatosPorConfiguracions = dgFormatos.ItemsSource.Cast<FormatosPorConfiguracion>().ToList(),
                    Logo = new Uri(txtLogo.Text),
                    idPeriodicidad = cmbPeriodicidad.SelectedValue.ToIntOrDefault(),
                };
            }
        }

        public void Show(Configuracion configuration)
        {
            txtRazonSocial.Text = configuration.razonSocial;
            txtRFC.Text = configuration.rfc;
            txtTipoDeCambio.Text = configuration.tipoDeCambio.ToDecimalString();
            txtCalle.Text = configuration.Domicilio.calle;
            txtNumeroExterior.Text = configuration.Domicilio.numeroExterior;
            txtNumeroInterior.Text = configuration.Domicilio.numeroInterior;
            txtColonia.Text = configuration.Domicilio.colonia;
            txtCiudad.Text = configuration.Domicilio.ciudad;
            txtEstado.Text = configuration.Domicilio.estado;
            cmbPais.SelectedValue = configuration.Domicilio.idPais;

            if (configuration.idPeriodicidad.isValid())
                cmbPeriodicidad.SelectedValue = configuration.idPeriodicidad;
            else
                cmbPeriodicidad.SelectedIndex = 0;

            cmbOpcionCostoAumenta.SelectedValue = configuration.idOpcionCostoAumenta;
            cmbOpcionCostoDisminuye.SelectedValue = configuration.idOpcionCostoDisminuye;
            txtCodigoPostal.Text = configuration.Domicilio.codigoPostal;
            txtTelefono.Text = configuration.telefono;
            txtUsuarioPAC.Text = configuration.usuarioPAC;
            var certificado = configuration.Certificados.FirstOrDefault(c => c.activo);
            txtNumeroDeCertificado.Clear();
            txtExpedicionDeCertificado.Clear();
            txtVencimientoDeCertificado.Clear();
            txtCertificadoBase64.Clear();
            if(certificado.isValid())
            { 
                txtNumeroDeCertificado.Text = certificado.numero;
                txtExpedicionDeCertificado.Text = certificado.expedicion.ToShortDateString();
                txtVencimientoDeCertificado.Text = certificado.vencimiento.ToShortDateString();
                txtCertificadoBase64.Text = certificado.certificadoBase64;
            }

            Show(configuration.Regimenes.ToList());
            Show(configuration.Series.ToList());
            Show(configuration.FormatosPorConfiguracions.ToList());

            ShowXmlDirectory(configuration.CarpetaXml);
            ShowPdfDirectory(configuration.CarpetaPdf);
            ShowCbbDirectory(configuration.CarpetaCbb);

            //Guardián
            Fill(configuration.CuentasGuardians.ToList());

            //Logo
            ShowImage(configuration.Logo);

            _idConfiguration = configuration.idConfiguracion;
        }

        #endregion

        #region Regimen

        public event Action AddRegime;
        public event Action DeleteRegime;

        private ICollection<Regimene> _regimes;

        private void dgRegimenes_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && DeleteRegime.isValid(AccesoRequerido.Total))
                DeleteRegime();
        }

        private void btnAgregarRegimen_Click(object sender, RoutedEventArgs e)
        {
            if (AddRegime.isValid(AccesoRequerido.Total))
                AddRegime();
        }

        public Regimene Regime { get { return new Regimene() { codigo = txtCodigoRegimen.Text, descripcion = txtDescripcionRegimen.Text, idConfiguracion = _idConfiguration }; } }

        public Regimene CurrentRegime { get { return dgRegimenes.SelectedIndex >= 0 ? (Regimene)dgRegimenes.SelectedItem : new Regimene(); } }

        public void Show(List<Regimene> regimes)
        {
            dgRegimenes.ItemsSource = null;
            dgRegimenes.ItemsSource = regimes.Where(r => r.activo);
            _regimes = regimes;
        }

        public void ClearRegime()
        {
            txtCodigoRegimen.Clear();
            txtDescripcionRegimen.Clear();
        }

        #endregion

        #region Serie

        public event Action AddSerie;
        public event Action SelectSerie;
        public event Action UpdateSerie;
        private int _idSerie;

        private void btnAgregarSerie_Click(object sender, RoutedEventArgs e)
        {
            //La autorización se hace con la API dentro del metodo
            if (IsDirtySerie)
            {
                //Es actualizacion
                if (UpdateSerie.isValid())
                    UpdateSerie();
            }
            else
            {
                //Es nueva
                if (AddSerie.isValid())
                    AddSerie();
            }
        }

        private void dgSeries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectSerie.isValid())
                SelectSerie();
        }

        public Series Serie
        {
            get { return new Series() { idSerie = _idSerie, identificador = txtSerie.Text, folioInicial = txtFolioInicial.Text.ToIntOrDefault(), folioFinal = txtFolioFinal.Text.ToIntOrDefault() }; }
        }

        public Series CurrentSerie
        {
            get { return dgSeries.SelectedIndex >= 0 ? (Series)dgSeries.SelectedItem : new Series(); }
        }

        public int CambioSerie
        {
            get { return txtCambioSerie.Text.ToIntOrDefault(); }
        }

        public bool IsDirtySerie
        {
            get { return _idSerie.isValid(); }
        }

        public void Show(List<Series> series)
        {
            dgSeries.ItemsSource = series;
            _idSerie = -1;
        }

        public void ClearSerie()
        {
            txtSerie.Clear();
            txtFolioInicial.Clear();
            txtFolioFinal.Clear();
        }

        public void SetEdition(bool visible)
        {
            txtCambioSerie.Visibility = visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lblCambioSerie.Visibility = visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            txtCambioSerie.Text = "0";
            txtSerie.IsReadOnly = visible;
            txtFolioInicial.IsReadOnly = visible;
            txtFolioFinal.IsReadOnly = visible;

        }

        public void Show(Series serie)
        {
            txtSerie.Text = serie.identificador;
            txtFolioInicial.Text = serie.folioInicial.ToString();
            txtFolioFinal.Text = serie.folioFinal.ToString();
            _idSerie = serie.idSerie;
        }

        #endregion

        #region Files

        public event Action OpenXmlDirectoryBrowse;
        public event Action OpenPdfDirectoryBrowse;
        public event Action OpenCbbDirectoryBrowse;

        public void ShowXmlDirectory(string directory)
        {
            txtRutaXml.Text = directory;
        }

        public void ShowPdfDirectory(string directory)
        {
            txtRutaPdf.Text = directory;
        }

        public void ShowCbbDirectory(string directory)
        {
            txtRutaCbb.Text = directory;
        }

        void btnBuscarRutaCbb_Click(object sender, RoutedEventArgs e)
        {
            if (OpenCbbDirectoryBrowse.isValid(AccesoRequerido.Total))
                OpenCbbDirectoryBrowse();
        }

        void btnBuscarRutaPdf_Click(object sender, RoutedEventArgs e)
        {
            if (OpenPdfDirectoryBrowse.isValid(AccesoRequerido.Total))
                OpenPdfDirectoryBrowse();
        }

        void btnBuscarRutaXml_Click(object sender, RoutedEventArgs e)
        {
            if (OpenXmlDirectoryBrowse.isValid(AccesoRequerido.Total))
                OpenXmlDirectoryBrowse();
        }

        #endregion

        #region Guardián

        public event Action AddAccount;
        public event Action RemoveAccount;

        public CuentasGuardian Account => new CuentasGuardian() { idConfiguracion = _idConfiguration, direccion = txtGuardianDireccion.Text, servidor = txtGuardianServidor.Text, ssl = chkGuardianSSL.IsChecked.Value, contrasena = txtGuardianContraseña.Text, puerto = txtGuardianPuerto.Text.ToIntOrDefault() };

        public CuentasGuardian Selected => dgGuardianCuentas.SelectedIndex >= 0 ? (CuentasGuardian)dgGuardianCuentas.SelectedItem : null;

        public void Fill(List<CuentasGuardian> accounts)
        {
            dgGuardianCuentas.ItemsSource = accounts;
        }

        private void DgGuardianCuentas_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete) && RemoveAccount.isValid(AccesoRequerido.Total))
                RemoveAccount();
        }

        private void BtnGuardianAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (AddAccount.isValid(AccesoRequerido.Total))
                AddAccount();
        }

        #endregion

        #region Formatos

        public event Action SelectLogoFile;
        public event Action FilterFormats;
        public event Action AddOrUpdateFormat;

        public Reporte SelectedReport => cmbReportes.SelectedValue.isValid() ? (Reporte)cmbReportes.SelectedItem : null;

        public FormatosPorConfiguracion Format => new FormatosPorConfiguracion() { idConfiguracion = _idConfiguration, idReporte = cmbReportes.SelectedValue.ToIntOrDefault(), Reporte = SelectedReport, idFormato = cmbFormatos.SelectedValue.ToIntOrDefault(), Formato = cmbFormatos.SelectedValue.isValid()? (Formato)cmbFormatos.SelectedItem : null };

        public void ShowImage(Uri path)
        {
            txtLogo.Text = path.OriginalString;
            imgLogoPreview.Source = new BitmapImage(path);
        }

        private void BtnBuscarRutaLogoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (SelectLogoFile.isValid())
            {
                SelectLogoFile();
            }
        }

        private void CmbReportes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterFormats.isValid())
                FilterFormats();
        }

        private void BtnAgregarFormato_Click(object sender, RoutedEventArgs e)
        {
            if (AddOrUpdateFormat.isValid())
                AddOrUpdateFormat();
        }

        public void FillCombo(List<Formato> formats)
        {
            cmbFormatos.ItemsSource = null;
            cmbFormatos.ItemsSource = formats;
            cmbFormatos.DisplayMemberPath = "descripcion";
            cmbFormatos.SelectedValuePath = "idFormato";
        }

        public void Show(List<FormatosPorConfiguracion> formats)
        {
            dgFormatos.ItemsSource = null;
            dgFormatos.ItemsSource = formats;
        }

        #endregion

    }
}
