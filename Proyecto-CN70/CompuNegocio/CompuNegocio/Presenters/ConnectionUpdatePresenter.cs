using Aprovi.Business.Services;
using Aprovi.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ConnectionUpdatePresenter
    {
        IConnectionUpdateView _view;
        IConfiguracionService _config;

        public ConnectionUpdatePresenter(IConnectionUpdateView view, IConfiguracionService config)
        {
            _view = view;
            _config = config;

            _view.Quit += Quit;
            _view.Save += Save;
        }

        private void Save()
        {
            try
            {
                if(!_view.Server.isValid())
                {
                    _view.ShowError("Debe especificar el servidor al que desea conectarse");
                    return;
                }

                if (!_view.User.isValid())
                {
                    _view.ShowError("Debe especificar el usuario para conectarse al servidor");
                    return;
                }

                if (!_view.Password.isValid())
                {
                    _view.ShowError("Debe especificar la contraseña para conectarse al servidor");
                    return;
                }


                _config.UpdateConnection(_view.Server, _view.User, _view.Password, _view.Database);

                _view.ShowMessage("Conexión actualizada exitosamente");

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
