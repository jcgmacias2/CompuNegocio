using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class UsersPresenter
    {
        private readonly IUsersView _view;
        private IUsuarioService _users;
        private IPrivilegioService _privileges;
        private ICatalogosEstaticosService _catalogs;

        public UsersPresenter(IUsersView view, IUsuarioService usersService, IPrivilegioService privilegesService, ICatalogosEstaticosService catalogsService)
        {
            _view = view;
            _users = usersService;
            _privileges = privilegesService;
            _catalogs = catalogsService;

            _view.Quit += Quit;
            _view.New += New;
            _view.Find += Find;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.OpenPrivileges += OpenPrivileges;
            _view.AddCommission += ViewOnAddCommission;
            _view.RemoveCommission += ViewOnRemoveCommission;

            _view.FillCombos(catalogsService.ListComisiones());
        }

        private void ViewOnRemoveCommission()
        {
            ComisionesPorUsuario selectedCommission = _view.SelectedComission;

            if (!selectedCommission.isValid())
            {
                return;
            }

            try
            {
                List<ComisionesPorUsuario> commissions = _view.Comissions;
                commissions.Remove(selectedCommission);
                _view.Show(commissions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ViewOnAddCommission()
        {
            if (!_view.CurrentCommission.valor.isValid())
            {
                _view.ShowMessage("El porcentaje de la comisión no es válido");

                return;
            }

            if (!_view.CurrentCommission.idTipoDeComision.isValid())
            {
                _view.ShowMessage("El tipo de comisión no es válido");

                return;
            }

            try
            {
                List<ComisionesPorUsuario> commissions = _view.Comissions;
                commissions.Add(_view.CurrentCommission);
                _view.Show(commissions);
                _view.ClearCommission();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenPrivileges()
        {
            if(!_view.IsDirty)
            {
                _view.ShowMessage("Los privilegios solo se pueden editar para usuarios registrados");
                return;
            }

            try
            {
                IPrivilegesView view;
                PrivilegesPresenter presenter;

                var user = _view.User;
                user.Privilegios = _privileges.List(user.idUsuario);

                view = new PrivilegesView(user);
                presenter = new PrivilegesPresenter(view, _privileges, _catalogs);

                view.ShowWindow();

                //Si el usuario que envie al constructor de la vista es el mismo que esta loggeado, actualizar los privilegios de la sesion
                if (_view.User.idUsuario.Equals(Session.LoggedUser.idUsuario))
                    Session.LoggedUser.Privilegios = _privileges.List(_view.User.idUsuario);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                IUsersListView view;
                UsersListPresenter presenter;

                view = new UsersListView();
                presenter = new UsersListPresenter(view, _users);

                view.ShowWindow();

                //Si seleccionó algún usuario lo muestro
                if (view.User.idUsuario.isValid())
                    _view.Show(view.User);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            string error;

            if (!IsUserValid(_view.User, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var user = _view.User;
                user.activo = true;
                _users.Update(user);

                _view.ShowMessage(string.Format("Usuario {0} modificado exitosamente", _view.User.nombreDeUsuario));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            if(!IsUserValid(_view.User, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var user = _view.User;
                user.activo = true;
                _users.Add(user);

                _view.ShowMessage(string.Format("Usuario {0} agregado exitosamente", _view.User.nombreDeUsuario));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if (_view.User.isValid())
            {
                _view.ShowError("No hay usuario para eliminar");
                return;
            }

            try
            {
                if(_users.CanDelete(_view.User))
                {
                    _users.Delete(_view.User);
                }
                else
                {
                    var local = _view.User;
                    local.activo = false;
                    _users.Update(local);
                }

                _view.ShowMessage(string.Format("Usuario {0} removido exitosamente", _view.User.nombreDeUsuario));
                _view.Clear();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.User.isValid() || !_view.User.nombreDeUsuario.isValid())
                return;

            try
            {

                var user = _users.Find(_view.User.nombreDeUsuario);

                if (user.isValid() && !user.activo)
                    _view.ShowMessage("El usuario ya existe pero esta marcado como inactivo, para reactivarlo solo de click en Guardar");

                if (user == null)
                    user = new Usuario() { nombreDeUsuario = _view.User.nombreDeUsuario };

                _view.Show(user);
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

        private bool IsUserValid(Usuario user, out string error)
        {
            if(!user.nombreDeUsuario.isValid())
            {
                error = "El nombre de usuario no es válido";
                return false;
            }

            if(!user.nombreCompleto.isValid())
            {
                error = "El nombre completo del usuario no es válido";
                return false;
            }

            if (user.descuento < 0.0m || user.descuento > 100.0m)
            {
                error = "El rango de descuento máximo es entre 0 y 100";
                return false;
            }


            error = string.Empty;
            return true;
        }
    }
}
