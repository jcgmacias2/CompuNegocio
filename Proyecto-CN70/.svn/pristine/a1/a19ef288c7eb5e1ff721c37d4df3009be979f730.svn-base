using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Application.Helpers;
using System;
using System.Linq;
using Aprovi.Data.Models;
using Aprovi.Views.UI;
using System.Windows;
using System.Text.RegularExpressions;

namespace Aprovi.Presenters
{
    public class ConfigurationPresenter
    {
        private readonly IConfigurationView _view;
        private IConfiguracionService _configurations;
        private IRegimenService _regimes;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private ICertificadoService _certificates;
        private ISerieService _series;
        private ICatalogosEstaticosService _catalogs;
        private IClienteService _clients;
        private IComprobantFiscaleService _fiscalReceipts;
        private ILicenciaService _licenses;
        private ICuentaGuardianService _accounts;

        public ConfigurationPresenter(IConfigurationView view, IConfiguracionService configurationsService, IRegimenService regimesService,
            IUsuarioService userService, ISeguridadService securityService, ICertificadoService certificateService, ISerieService seriesService,
            ICatalogosEstaticosService catalogsService, IClienteService clientsService, IComprobantFiscaleService fiscalReceiptsService, ILicenciaService licensesService, ICuentaGuardianService accounts)
        {
            _view = view;
            _configurations = configurationsService;
            _regimes = regimesService;
            _users = userService;
            _security = securityService;
            _certificates = certificateService;
            _series = seriesService;
            _catalogs = catalogsService;
            _clients = clientsService;
            _fiscalReceipts = fiscalReceiptsService;
            _licenses = licensesService;
            _accounts = accounts;

            _view.Load += Load;
            _view.Quit += Quit;
            _view.Save += Save;

            _view.AddRegime += AddRegime;
            _view.DeleteRegime += DeleteRegime;

            _view.OpenConfigurationPAC += OpenConfigurationPAC;
            _view.OpenConfigurationCSD += OpenConfigurationCSD;

            _view.AddSerie += AddSerie;
            _view.SelectSerie += SelectSerie;
            _view.UpdateSerie += UpdateSerie;

            _view.OpenXmlDirectoryBrowse += OpenXmlDirectoryBrowse;
            _view.OpenPdfDirectoryBrowse += OpenPdfDirectoryBrowse;
            _view.OpenCbbDirectoryBrowse += OpenDbbDirectoryBrowse;

            _view.AddAccount += AddAccount;
            _view.RemoveAccount += RemoveAccount;

            _view.SelectLogoFile += SelectLogoFile;

            _view.FilterFormats += FilterFormats;
            _view.AddOrUpdateFormat += AddOrUpdateFormat;

            _view.FillCombo(_catalogs.ListPaises(), _catalogs.ListOpcionesCostos(), _catalogs.ListReportes());
        }

        private void FilterFormats()
        {
            try
            {
                if (!_view.SelectedReport.isValid())
                    return;

                _view.FillCombo(_view.SelectedReport.Formatos.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddOrUpdateFormat()
        {
            try
            {
                var format = _view.Format;

                if (!format.idFormato.isValid() || !format.idReporte.isValid())
                    return;

                var formatos = _view.Configuration.FormatosPorConfiguracions;

                formatos.Remove(formatos.FirstOrDefault(f => f.idReporte.Equals(format.idReporte)));
                formatos.Add(format);

                _view.Show(formatos.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectLogoFile()
        {
            try
            {
                var path = _view.OpenFileFinder("Imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png");

                //Si no se selecciona ninguna, se muestra la actual
                _view.ShowImage(path.isValid()?new Uri(path):_view.Configuration.Logo);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenDbbDirectoryBrowse()
        {
            try
            {
                _view.ShowCbbDirectory(_view.OpenFolderFinder("Ruta de archivos Cbb"));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenPdfDirectoryBrowse()
        {
            try
            {
                _view.ShowPdfDirectory(_view.OpenFolderFinder("Ruta de archivos Pdf"));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenXmlDirectoryBrowse()
        {
            try
            {
                _view.ShowXmlDirectory(_view.OpenFolderFinder("Ruta de archivos Xml"));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void UpdateSerie()
        {
            TiposDeComprobante receiptTypeSerie = null;
            var change = _view.CambioSerie;
            var serie = _view.Serie;

            serie.folioFinal = change < 0 ? serie.folioFinal - Math.Abs(change) : serie.folioFinal + Math.Abs(change);
            //El folio final resultante no puede ser inferior al folio inicial
            if(change <0 && serie.folioFinal < serie.folioInicial)
            {
                _view.ShowError("La disminución en el tiraje de folios crea un folio final inferior al folio inicial");
                return;
            }

            // Debo saber cual es el ultimo folio utilizado, para asegurar que el folio final no es inferior al ultimo folio utlizado
            if(serie.folioFinal < _series.Last(serie.identificador))
            {
                _view.ShowError("La modificación de los folios produce un folio final menor al último folio utilizado en esa serie");
                return;
            }

            try
            {
                //Le pregunto al usuario si desea relacionar la serie con un tipo de comprobante
                if(_view.ShowMessageWithOptions("Desea relacionar esta serie con un tipo de comprobante?").Equals(MessageBoxResult.Yes))
                {
                    IReceiptTypeView view;
                    ReceiptTypePresenter presenter;

                    view = new ReceiptTypeView(serie);
                    presenter = new ReceiptTypePresenter(view, _series, _catalogs);

                    view.ShowWindow();

                    //Cuando cierre verfico si selecciono algo o no
                    if(view.Selected)
                        receiptTypeSerie = view.ReceiptType;
                }


                //Hago el cambio de la serie
                serie = _series.Update(serie);

                //Si relaciono algun tipo de comprobante tambien lo cambio
                if (receiptTypeSerie.isValid())
                    _series.Update(serie, receiptTypeSerie);

                //Notifico el cambio
                _view.ShowMessage(string.Format("Serie {0} actualizada exitosamente", serie.identificador));

                //Deshabilito la edición
                _view.SetEdition(false);

                //Limpio el espacio de la serie
                _view.ClearSerie();

                //Obtengo la configuración actualizada
                var newConfiguration = _configurations.GetDefault();

                //Muestro los nuevos datos configurados
                _view.Show(newConfiguration);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectSerie()
        {
            try
            {
                //Para permitir editar una serie debo validar primero las credenciales con el API
                if (!HasAPIAuthorization())
                    return;

                //Si llego aquí entonces habilito la edición, mostrando la serie y el espacio de cambio
                _view.Show(_view.CurrentSerie);
                _view.SetEdition(true);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddSerie()
        {
            if(!_view.Serie.identificador.isValid())
            {
                _view.ShowError("La serie no es válida");
                return;
            }

            if(_view.Serie.identificador.Length > 1)
            {
                _view.ShowError("La serie solo puede ser un caracter");
                return;
            }

            if(_series.Find(_view.Serie.identificador[0])!= null)
            {
                _view.ShowError("La serie que intenta agregar ya existe");
                return;
            }

            if(!_view.Serie.folioInicial.isValid())
            {
                _view.ShowError("El folio inicial no es válido");
                return;
            }

            if(!_view.Serie.folioFinal.isValid())
            {
                _view.ShowError("El folio final no es válido");
                return;
            }

            if(_view.Serie.folioFinal <= _view.Serie.folioInicial)
            {
                _view.ShowError("El folio final no puede ser inferior al folio inicial");
                return;
            }

            try
            {
                //Para permitir agregar una serie debo validar primero las credenciales con el API
                if (!HasAPIAuthorization())
                    return;

                //Si llega hasta aquí entonces lo permito
                var serie = _view.Serie;
                serie.idConfiguracion = Session.Configuration.idConfiguracion;
                serie = _series.Add(serie);

                //Mando mensaje al usuario
                _view.ShowMessage(string.Format("Serie {0} agregada exitosamente", serie.identificador));

                //Limpio el espacio
                _view.ClearSerie();

                //Obtengo la configuración actualizada
                var newConfiguration = _configurations.GetDefault();

                //Muestro los nuevos datos configurados
                _view.Show(newConfiguration);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenConfigurationCSD()
        {
            try
            {
                //Para abrir la configuración del CSD debo validar primero las credenciales con el API
                if (!HasAPIAuthorization())
                    return;

                //Si llega hasta aquí entonces le muestro la configuración del CSD
                IConfigurationCSDView viewCSDConfiguration;
                ConfigurationCSDPresenter presenterCSDConfiguration;

                viewCSDConfiguration = new ConfigurationCSDView();
                presenterCSDConfiguration = new ConfigurationCSDPresenter(viewCSDConfiguration, _certificates);

                //Muestro la pantalla para que haga los cambios
                viewCSDConfiguration.ShowWindow();

                //Obtengo la configuración actualizada
                var config = _configurations.GetDefault();

                //Muestro los nuevos datos configurados
                _view.Show(config);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenConfigurationPAC()
        {
            try
            {
                //Para abrir la configuración del PAC debo validar primero las credenciales con el API
                if (!HasAPIAuthorization())
                    return;

                //Si llega hasta aquí entonces le muestro la configuración del PAC
                IConfigurationPACView viewPACConfiguration;
                ConfigurationPACPresenter presenterPACConfiguration;

                viewPACConfiguration = new ConfigurationPACView(_view.Configuration);
                presenterPACConfiguration = new ConfigurationPACPresenter(viewPACConfiguration, _configurations);

                viewPACConfiguration.ShowWindow();

                //Después de mostrar el inicio de sesión obtengo la configuración actualizada
                var config = _configurations.GetDefault();

                //La asigno a la vista actual, así como a la sesión
                _view.Show(config);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DeleteRegime()
        {
            if(!_view.CurrentRegime.idRegimen.isValid())
            {
                _view.ShowError("No hay ningún régimen seleccionado para eliminar");
                return;
            }

            try
            {
                //Lo elimino
                _regimes.Delete(_view.CurrentRegime);

                _view.ShowMessage("Régimen removido exitosamente");

                var config = _configurations.GetDefault();

                _view.Show(config.Regimenes.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddRegime()
        {
            if(!_view.Regime.isValid())
            {
                _view.ShowError("El régimen no es válido");
                return;
            }

            try
            {
                var regimes = _view.Configuration.Regimenes;

                //Si el régimen ya esta registrado me regreso
                if (regimes.FirstOrDefault(r => r.activo && r.codigo.Equals(_view.Regime.codigo, StringComparison.InvariantCultureIgnoreCase)) != null)
                {
                    _view.ShowError(string.Format("El régimen {0} ya se encuentra registrado", _view.Regime.descripcion));
                    return;
                }

                //Si llega aquí entonces lo agrego
                _regimes.Add(_view.Regime);

                _view.ShowMessage(string.Format("Régimen {0} agregado exitosamente", _view.Regime.descripcion));

                _view.ClearRegime();

                var config = _configurations.GetDefault();

                _view.Show(config.Regimenes.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            var config = _view.Configuration;

            if (!IsConfigurationValid(config, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le paso los datos respecto el escanner y el cajon
                config.CajonDeEfectivo = Session.Configuration.CajonDeEfectivo;
                config.Escaner = Session.Configuration.Escaner;
                config.CodigoEscaner = Session.Configuration.CodigoEscaner;

                //Realizo la actualización de los datos
                _configurations.Update(_view.Configuration);

                //Asigno a la configuración local los cambios
                Session.Configuration = _configurations.GetDefault();

                //Mando mensaje de exito al usuario
                _view.ShowMessage("Propiedades generales actualizadas exitosamente");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex);
            }
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Load()
        {
            try
            {
                _view.Show(Session.Configuration);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsConfigurationValid(Configuracion config, out string error)
        {
            if(!config.idConfiguracion.isValid())
            {
                error = "Configuración sin registro";
                return false;
            }

            if(!config.razonSocial.isValid())
            {
                error = "La razón social no es válida";
                return false;
            }

            //Valido el regex de la razón social
            var rgx = new Regex(@"([A-Z]|[a-z]|[0-9]| |Ñ|ñ|!|&quot;|%|&amp;|&apos;|´|-|:|;|&gt;|=|&lt;|@|_|,|\{|\}|`|~|á|é|í|ó|ú|Á|É|Í|Ó|Ú|ü|Ü){1,254}");
            if(!rgx.IsMatch(config.razonSocial))
            {
                error = "La razón social no cumple con el patrón de validación del SAT";
                return false;
            }

            if(!config.rfc.isValid())
            {
                error = "El registro federal de contribuyentes no es válido";
                return false;
            }

            if(!config.tipoDeCambio.isValid())
            {
                error = "Tipo de cambio no es válido";
                return false;
            }

            if(!config.Domicilio.isValid())
            {
                error = "Domicilio inválido";
                return false;
            }

            if(!config.Domicilio.calle.isValid())
            {
                error = "La calle no es válida";
            }

            if(!config.Domicilio.numeroExterior.isValid())
            {
                error = "El número exterior no es válido";
                return false;
            }

            if(!config.Domicilio.colonia.isValid())
            {
                error = "La colonia no es válida";
                return false;
            }

            if(!config.Domicilio.ciudad.isValid())
            {
                error = "La ciudad no es válida";
                return false;
            }

            if(!config.Domicilio.estado.isValid())
            {
                error = "El estado no es válido";
                return false;
            }

            if(!config.Domicilio.idPais.isValid())
            {
                error = "El país no es válido";
                return false;
            }

            if(!config.Domicilio.codigoPostal.isValid())
            {
                error = "El código postal no es válido";
                return false;
            }

            if(!config.telefono.isValid())
            {
                error = "El teléfono no es válido";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private bool HasAPIAuthorization()
        {
            try
            {
                IAuthenticationView viewAuthentication;
                AuthenticationPresenter presenterAuthentication;

                viewAuthentication = new AuthenticationView(true);
                presenterAuthentication = new AuthenticationPresenter(viewAuthentication, _users, _security, _fiscalReceipts, _licenses);

                viewAuthentication.ShowWindow();

                //Cuando la ventana de autorización valida las credenciales con la API llena la propiedad APIAuthorized
                return viewAuthentication.Credentials.APIAuthorized;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Guardián

        private void RemoveAccount()
        {
            try
            {
                var account = _view.Selected;

                if (!account.isValid())
                    throw new Exception("Debe seleccionar una cuenta para eliminar");

                if (!account.idConfiguracion.isValid())
                    throw new Exception("La cuenta no tiene referencia de configuración");

                if (!account.idCuentaGuardian.isValid())
                    throw new Exception("La cuenta no tiene identificador");

                _accounts.Remove(account);

                _view.ShowMessage("Cuenta Guardián removida con éxito");

                _view.Fill(_accounts.List(account.idConfiguracion));

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddAccount()
        {
            try
            {
                var account = _view.Account;

                if (!account.direccion.isValid())
                    throw new Exception("La dirección de la cuenta de correo no es válida");

                if (!account.servidor.isValid())
                    throw new Exception("El servidor de la cuenta de correo no es válido");

                if (!account.contrasena.isValid())
                    throw new Exception("La contraseña de la cuenta de correo no es válida");

                if (!account.puerto.isValid())
                    throw new Exception("El puerto de la cuenta de correo no es válido");

                _accounts.Add(account);

                _view.ShowMessage("Cuenta Guardián registrada con éxito");

                _view.Fill(_accounts.List(account.idConfiguracion));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion
    }
}
