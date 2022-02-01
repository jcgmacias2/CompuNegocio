using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class CustomsApplicationPresenter
    {
        private ICustomsApplicationView _view;

        public CustomsApplicationPresenter(ICustomsApplicationView view)
        {
            _view = view;

            _view.Save += Save;
        }

        private void Save()
        {
            try
            {
                var customsApplication = _view.CustomsApplication;

                if (!customsApplication.añoOperacion.isValid())
                    throw new Exception("Debe capturar los 2 dígitos del año de operación");

                if (!customsApplication.aduana.isValid())
                    throw new Exception("Debe capturar los 2 dígitos de la aduana");

                if (!customsApplication.patente.isValid())
                    throw new Exception("Debe capturar los 4 dígitos de la patente");

                if (!customsApplication.añoEnCurso.isValid())
                    throw new Exception("Debe capturar 1 dígito del año en curso");

                if (!customsApplication.progresivo.isValid())
                    throw new Exception("Debe capturar los 6 dígitos progresivos");

                if (customsApplication.fecha.Date > DateTime.Now.Date)
                    throw new Exception("La fecha no puede ser mayor al dia de hoy");

                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
