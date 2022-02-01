using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class BusinessPresenter
    {
        private IBusinessesView _view;
        private IEmpresaService _businesses;
        private ILicenciaService _licenses;

        public BusinessPresenter(IBusinessesView view, IEmpresaService businessesService, ILicenciaService licenseService)
        {
            _view = view;
            _businesses = businessesService;
            _licenses = licenseService;

            _view.Quit += Quit;
            _view.New += New;
            _view.Save += Save;
            _view.Find += Find;
            _view.Update += Update;
            _view.Delete += Delete;
            _view.OpenList += OpenList;
        }

        private void OpenList()
        {
            try
            {
                IBusinessesListView view;
                BusinessesListPresenter presenter;

                view = new BusinessesListView();
                presenter = new BusinessesListPresenter(view, _businesses);

                view.ShowWindow();

                if (view.Business.idEmpresa.isValid())
                    _view.Show(view.Business);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("No existe una empresa seleccionada para eliminar");
                return;
            }

            try
            {
                if (_businesses.CanDelete(_view.Business))
                {
                    _businesses.Delete(_view.Business);
                }
                else
                {
                    var register = _view.Business;
                    register.activa = false;
                    _businesses.Update(register);
                }

                _view.ShowMessage(string.Format("Empresa {0} removida exitosamente", _view.Business.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            var register = _view.Business;

            if (!register.descripcion.isValid())
            {
                _view.ShowError("Debe capturar una descripción para la empresa");
                return;
            }

            if (!register.licencia.isValid())
            {
                _view.ShowError("Debe capturar una licencia válida para la empresa");
                return;
            }

            try
            {
                // Debo Autenticar, Verificar y Activar esta nueva licencia
                var idUser = _licenses.Authenticate(register.licencia);

                if (!_licenses.Verify(idUser))
                {
                    _view.ShowError("No fué posible verificar la licencia, esto puede ser porque esta registrada a otra empresa y/o usuario");
                    return;
                }

                if (!_licenses.Activate(register.licencia))
                {
                    _view.ShowError("No fué posible activar la licencia, esto puede ser porque ya fue activada anteriormente, o se encuentra vencida");
                    return;
                }

                //Si paso los 3 pasos del licenciamiento, entonces hago el registro en la base de datos de la empresa
                register.activa = true;
                _businesses.Update(register);

                //Mensaje al usuario
                _view.ShowMessage(string.Format("La empresa {0} ha sido actualizada exitosamente", register.descripcion));

                //Limpio la pantalla
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Business.descripcion.isValid())
                return;

            try
            {
                var business = _businesses.Find(_view.Business.descripcion);

                if (business.isValid() && !business.activa)
                    _view.ShowMessage("La empresa ya existe pero esta marcada como inactiva, para reactivarla solo de click en Guardar");

                if (business == null)
                    business = new Empresa() { descripcion = _view.Business.descripcion };

                _view.Show(business);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            var register = _view.Business;
            string idUser;

            if(!register.descripcion.isValid())
            {
                _view.ShowError("Debe capturar una descripción para la empresa");
                return;
            }

            if(!register.licencia.isValid())
            {
                _view.ShowError("Debe capturar una licencia válida para la empresa");
                return;
            }

            try
            {
                // Debo Autenticar, Verificar y Activar la licencia
                idUser = _licenses.Authenticate(register.licencia);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                return;
            }

            try
            {
                if(!_licenses.Verify(idUser))
                {
                    _view.ShowError("No fué posible verificar la licencia, esto puede ser porque esta registrada a otra empresa y/o usuario");
                    return;
                }

                if(!_licenses.Activate(register.licencia))
                {
                    _view.ShowError("No fué posible activar la licencia, esto puede ser porque ya fue activada anteriormente, o se encuentra vencida");
                    return;
                }

                //Si paso los 3 pasos del licenciamiento, entonces hago el registro en la base de datos de la empresa
                _businesses.Add(register);

                //Mensaje al usuario
                _view.ShowMessage(string.Format("La empresa {0} ha sido registrada exitosamente", register.descripcion));

                //Limpio la pantalla
                _view.Clear();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
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
