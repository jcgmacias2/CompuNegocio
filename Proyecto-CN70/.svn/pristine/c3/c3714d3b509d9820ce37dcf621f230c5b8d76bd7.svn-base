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
    public class ClientsListPresenter : BaseListPresenter
    {
        private readonly IClientsListView _view;
        private IClienteService _clients;

        public ClientsListPresenter(IClientsListView view, IClienteService clientsService) : base(view)
        {
            _view = view;
            _clients = clientsService;

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
            List<Cliente> clients;

            try
            {
                if (_view.Parameter.isValid())
                    clients = _clients.WithNameLike(_view.Parameter);
                else
                    clients = _clients.List();

                _view.Show(clients);

                if (clients.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
