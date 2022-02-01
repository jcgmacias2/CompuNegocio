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
    public class AuthorizationPresenter
    {
        private IAuthorizationView _view;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private AccesoRequerido _requiredPermission;
        private string _presenter;

        public AuthorizationPresenter(IAuthorizationView view, IUsuarioService usersService, ISeguridadService securityService, AccesoRequerido requiredPermission, string presenter)
        {
            _view = view;
            _users = usersService;
            _security = securityService;
            _requiredPermission = requiredPermission;
            _presenter = presenter;

            _view.Quit += Quit;
            _view.SignIn += SignIn;
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
                    return;
                }

                user.HasAccess(_requiredPermission, _presenter, true);

                //Si la autorizacion fue correcta, se da el aviso
                _view.ShowMessage("Autorización correcta");

                //Se aplica la autorizacion
                _view.SetAuthorization(true);

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
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
