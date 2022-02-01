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
    public class ClientStatementReportPresenter
    {
        private IClientStatementReportView _view;
        private ISaldosPorClienteService _balances;
        private IClienteService _clients;

        public ClientStatementReportPresenter(IClientStatementReportView view, IClienteService clients, ISaldosPorClienteService balances)
        {
            _view = view;
            _clients = clients;
            _balances = balances;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            _view.FindClient += FindClient;
            _view.OpenClientsList += OpenClientsList;
        }

        private void OpenClientsList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _clients);

                view.ShowWindow();

                if (view.Client.idCliente.isValid())
                    _view.Show(view.Client);
                else
                    _view.Show(new Cliente());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            try
            {
                var client = new Cliente();

                //Si el código es válido intento buscarlo
                if (_view.Client.codigo.isValid())
                    client = _clients.Find(_view.Client.codigo);

                if (client.isValid())
                    _view.Show(client);
                else
                    _view.Show(new Cliente());

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Print()
        {
            if (!_view.Client.idCliente.isValid())
            {
                _view.ShowError("Debe seleccionar el cliente sobre el cual desea obtener la información");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var purchases = _balances.List(_view.Client, _view.Start, _view.End, _view.OnlyPendingBalance);

                view = new ReportViewerView(Reports.FillReport(purchases, _view.Start, _view.End, _view.Client));
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            if (!_view.Client.idCliente.isValid())
            {
                _view.ShowError("Debe seleccionar el cliente sobre el cual desea obtener la información");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var purchases = _balances.List(_view.Client, _view.Start, _view.End, _view.OnlyPendingBalance);

                view = new ReportViewerView(Reports.FillReport(purchases, _view.Start, _view.End, _view.Client));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
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
