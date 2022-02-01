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
    public class UsersListPresenter : BaseListPresenter
    {
        private readonly IUsersListView _view;
        private IUsuarioService _users;

        public UsersListPresenter(IUsersListView view, IUsuarioService usersService)
            : base(view)
        {
            _view = view;
            _users = usersService;

            _view.Search += Search;

            //Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<Usuario> users;

            try
            {
                if (_view.Parameter.isValid())
                    users = _users.WithNameLike(_view.Parameter);
                else
                    users = _users.List();

                _view.Show(users);

                if (users.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
