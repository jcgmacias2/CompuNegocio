using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PrivilegesPresenter
    {
        private IPrivilegesView _view;
        private IPrivilegioService _privileges;

        public PrivilegesPresenter(IPrivilegesView view, IPrivilegioService privilegesService, ICatalogosEstaticosService catalogs)
        {
            _view = view;
            _privileges = privilegesService;

            _view.Quit += Quit;
            _view.Add += Add;
            _view.Delete += Delete;

            _view.Show(_view.User.Privilegios.ToList());
            _view.FillCombos(catalogs.ListPantallas(), catalogs.ListPermisos());
        }

        private void Delete()
        {

            if(!_view.CurrentPrivilege.isValid())
            {
                _view.ShowError("No hay un privilegio seleccionado para eliminar");
                return;
            }

            try
            {
                _privileges.Delete(_view.CurrentPrivilege);
                var privileges = _privileges.List(_view.User.idUsuario);

                _view.ShowMessage("Privilegio removido exitosamente");
                _view.Clear();
                _view.Show(privileges);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Add()
        {
            string error;

            if(!IsPrivilegeValid(_view.Privilege, out error))
            {
                _view.ShowError(error);
                return;
            }

            if(_privileges.Find(_view.Privilege.idUsuario,_view.Privilege.idPantalla) != null)
            {
                _view.ShowError("Ya existe un permiso asignado para esta pantalla");
                return;
            }

            try
            {
                _privileges.Add(_view.Privilege);
                var privileges = _privileges.List(_view.Privilege.idUsuario);

                _view.ShowMessage("Privilegio agregado exitosamente");
                _view.Clear();
                _view.Show(privileges);
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

        private bool IsPrivilegeValid(Privilegio privilege, out string error)
        {
            if(!privilege.idUsuario.isValid())
            {
                error = "Debe seleccionar un usuario al cual aplicarle el permiso";
                return false;
            }

            if(!privilege.idPantalla.isValid())
            {
                error = "Debe seleccionar una pantalla sobre la cual aplicar el permiso";
                return false;
            }

            if(!privilege.idPermiso.isValid())
            {
                error = "Debe seleccionar el permiso que desea aplicar sobre la pantalla";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}
