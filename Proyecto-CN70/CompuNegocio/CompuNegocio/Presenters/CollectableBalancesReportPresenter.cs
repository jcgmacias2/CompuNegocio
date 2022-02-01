using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class CollectableBalancesReportPresenter
    {
        private ICollectableBalancesReportView _view;
        private ISaldosPorClienteService _collectableBalances;
        private IFacturaService _invoices;
        private IRemisionService _billsOfSale;
        private IUsuarioService _users;
        private IClienteService _customers;

        public CollectableBalancesReportPresenter(ICollectableBalancesReportView view, ISaldosPorClienteService collectableBalances, IUsuarioService users, IClienteService customers, IFacturaService invoices, IRemisionService billsOfSale)
        {
            _view = view;
            _collectableBalances = collectableBalances;
            _users = users;
            _customers = customers;
            _invoices = invoices;
            _billsOfSale = billsOfSale;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;

            _view.FindCustomer += FindCustomer;
            _view.FindSeller += FindSeller;
            _view.OpenCustomersList += OpenCustomersList;
            _view.OpenSellersList += OpenSellersList;
        }

        private void OpenSellersList()
        {
            try
            {
                IUsersListView view;
                UsersListPresenter presenter;

                view = new UsersListView();
                presenter = new UsersListPresenter(view, _users);

                view.ShowWindow();

                if (view.User.isValid() && view.User.idUsuario.isValid())
                {
                    _view.Show(view.User);
                }
                else
                {
                    _view.Show(new Usuario());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenCustomersList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _customers);

                view.ShowWindow();

                if (view.Client.isValid() && view.Client.idCliente.isValid())
                {
                    _view.Show(view.Client);
                }
                else
                {
                    _view.Show(new Cliente());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindSeller()
        {
            try
            {
                if (_view.Report.Vendedor.nombreDeUsuario.isValid())
                {
                    var seller = _users.Find(_view.Report.Vendedor.nombreDeUsuario);

                    if (seller.isValid())
                    {
                        _view.Show(seller);
                    }
                    else
                    {
                        _view.ShowError("No se encontró un usuario con el nombre de usuario proporcionado");
                        _view.Show(new Usuario());
                    }
                }
                else
                {
                    _view.Show(new Usuario());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindCustomer()
        {
            try
            {
                if (_view.Report.Cliente.codigo.isValid())
                {
                    var customer = _customers.Find(_view.Report.Cliente.codigo);

                    if (customer.isValid())
                    {
                        _view.Show(customer);
                    }
                    else
                    {
                        _view.ShowError("No se encontró un cliente con el código proporcionado");
                        _view.Show(new Usuario());
                    }
                }
                else
                {
                    _view.Show(new Usuario());
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var report = _view.Report;
                VMReporte r;

                //Se debe verificar cual de los 2 reportes es
                if (report.TipoDeReporte == TiposDeReporteAntiguedadDeSaldos.Totales)
                {
                    //Es el detallado
                    var details = new List<VMRTotalAntiguedadSaldos>();

                    details.AddRange(_invoices.ListForTotalCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha));

                    if (report.IncluirRemisiones)
                    {
                        var d = _billsOfSale.ListForTotalCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha);
                        details.AddRange(d);
                    }

                    details = details.OrderBy(x => x.Fecha).ToList();

                    if (details.IsEmpty())
                    {
                        _view.ShowError("No existen cuentas por cobrar");
                        return;
                    }

                    r = Reports.FillReport(details, report.Fecha);
                }
                else
                {
                    //Es el detallado
                    var details = new List<VMRDetalleAntiguedadSaldos>();

                    details.AddRange(_invoices.ListForDetailedCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha));

                    if (report.IncluirRemisiones)
                    {
                        var d = _billsOfSale.ListForDetailedCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha);
                        details.AddRange(d);
                    }

                    details = details.OrderBy(x => x.Fecha).ToList();

                    if (details.IsEmpty())
                    {
                        _view.ShowError("No existen cuentas por cobrar");
                        return;
                    }

                    r = Reports.FillReport(details, report.Periodo, report.Fecha);
                }

                view = new ReportViewerView(r);
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
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var report = _view.Report;
                VMReporte r;

                //Se debe verificar cual de los 2 reportes es
                if (report.TipoDeReporte == TiposDeReporteAntiguedadDeSaldos.Totales)
                {
                    //Es el detallado
                    var details = new List<VMRTotalAntiguedadSaldos>();

                    details.AddRange(_invoices.ListForTotalCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha));

                    if (report.IncluirRemisiones)
                    {
                        var d = _billsOfSale.ListForTotalCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha);
                        details.AddRange(d);
                    }

                    details = details.OrderBy(x => x.Fecha).ToList();

                    if (details.IsEmpty())
                    {
                        _view.ShowError("No existen cuentas por cobrar");
                        return;
                    }

                    r = Reports.FillReport(details, report.Fecha);
                }
                else
                {
                    //Es el detallado
                    var details = new List<VMRDetalleAntiguedadSaldos>();

                    details.AddRange(_invoices.ListForDetailedCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha));

                    if (report.IncluirRemisiones)
                    {
                        var d = _billsOfSale.ListForDetailedCollectableBalancesReport(report.Cliente, report.Vendedor, report.SoloVencidos, report.Fecha);
                        details.AddRange(d);
                    }

                    details = details.OrderBy(x => x.Fecha).ToList();

                    if (details.IsEmpty())
                    {
                        _view.ShowError("No existen cuentas por cobrar");
                        return;
                    }

                    r = Reports.FillReport(details,report.Periodo,report.Fecha);
                }

                view = new ReportViewerView(r);
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
