using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ConfigurationPACPresenter
    {
        private IConfigurationPACView _view;
        private IConfiguracionService _configurations;

        public ConfigurationPACPresenter(IConfigurationPACView view, IConfiguracionService configService)
        {
            _view = view;
            _configurations = configService;

            _view.Quit += Quit;
            _view.Save += Save;
        }

        private void Save()
        {
            if(!_view.Config.idConfiguracion.isValid())
            {
                _view.ShowMessage("La configuración a editar no es válida");
                return;
            }

            try
            {
                _configurations.UpdatePAC(_view.Config);
                _view.ShowMessage("Credenciales del PAC actualizadas exitosamente");
            }
            catch (Exception)
            {
                throw;
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
