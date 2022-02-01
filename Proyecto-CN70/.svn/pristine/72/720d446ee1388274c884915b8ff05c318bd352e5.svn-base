using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Helpers;

namespace Aprovi.Presenters
{
    public class AuthenticationPresenter
    {
        private readonly IAuthenticationView _view;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private IComprobantFiscaleService _fiscalReceipts;
        private ILicenciaService _licenses;

        public AuthenticationPresenter(IAuthenticationView view, IUsuarioService usersService, ISeguridadService securityService, IComprobantFiscaleService fiscalReceiptsService, ILicenciaService licensesService)
        {
            _view = view;
            _users = usersService;
            _security = securityService;
            _fiscalReceipts = fiscalReceiptsService;
            _licenses = licensesService;

            _view.Quit += Quit;
            _view.SignIn += SignIn;
            _view.AuthorizeOnAPI += AuthorizeOnAPI;
        }

        private void AuthorizeOnAPI()
        {
            if (!_view.Credentials.Usuario.isValid())
            {
                _view.ShowError("El usuario no es válido");
                return;
            }

            try
            {
                //Autentico al usuario
                var valid = _security.AuthenticateWithAPI(_view.Credentials.Usuario, _view.Credentials.Contraseña, Session.Configuration.Mode.Equals(Ambiente.Production));
                Logger.Log(valid.ToString());
                if (!valid)
                {
                    _view.ShowMessage("Las credenciales proporcionadas no son válidas");
                    return;
                }

                //Si fue autenticado correctamente le doy la bienvenida
                _view.ShowMessage(string.Format("Acceso a configuración concedido {0}", _view.Credentials.Usuario));

                //Hago Show de las credentials con ApiAuthorized en TRUE
                _view.Show(new VMCredencial(_view.Credentials.Usuario, _view.Credentials.Contraseña, true));

                //Al autenticarse con un usuario en Web API el usuario que se quedara en sesión sera el usuario administrativo "Aprovi", y este sera el sujeto a la autorización del sistema.
                //Cargo el usuario
                Session.LoggedUser = _users.GetApiDefault();

                //Cierro la ventana de autenticación
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex);
            }
        }

        private void SignIn()
        {
            if (!_view.Credentials.Usuario.isValid())
            {
                _view.ShowError("El usuario no es válido");
                return;
            }

            try
            {
                //Autentico al usuario
                var user = _security.Authenticate(_view.Credentials.Usuario, _view.Credentials.Contraseña);

                if (!user.isValid())
                {
                    _view.ShowError("Las credenciales proporcionadas no son válidas");
                    Session.LoggedUser = null;
                    return;
                }

                if (!Session.Station.isValid())
                {
                    _view.ShowError("No tiene una estación configurada");
                    return;
                }

                //Si esta en Production Mode o en Development
                if (Session.Configuration.Mode.Equals(Ambiente.Production) || Session.Configuration.Mode.Equals(Ambiente.Development))
                {
                    var license = Session.Station.Empresa.licencia;
                    var stamps = _fiscalReceipts.GetTotalTimbresUtilizados();

                    //Si no pasa la validación
                    if (!_licenses.Validate(license, stamps))
                    {
                        _view.ShowError("La licencia del sistema no es válida");
                        Session.LoggedUser = null;
                        return;
                    }
                }

                //Si fue autenticado correctamente le doy la bienvenida
                _view.ShowMessage(string.Format("Bienvenido {0}", user.nombreCompleto));

                //Realizo el inicio de sesión cargando lo necesario en Session
                //Cargo el usuario
                Session.LoggedUser = user;

                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Quit()
        {
            try
            {
                Session.LoggedUser = null;
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
