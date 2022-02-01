using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class OrdersReportPresenter
    {
        private IOrdersReportView _view;
        private IPedidoService _orders;
        private IClienteService _customers;
        private IConfiguracionService _configs;

        public OrdersReportPresenter(IOrdersReportView view, IPedidoService orders, IClienteService customers, IConfiguracionService configs)
        {
            _view = view;
            _orders = orders;
            _customers = customers;
            _configs = configs;

            _view.FindOrder += FindOrder;
            _view.FindCustomer += FindCustomer;
            _view.OpenCustomersList += OpenCustomersList;
            _view.OpenOrdersList += OpenOrdersList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void OpenOrdersList()
        {
            try
            {
                IOrdersListView view;
                OrdersListPresenter presenter;

                view = new OrdersListView();
                presenter = new OrdersListPresenter(view, _orders);

                view.ShowWindow();

                if (view.Order.isValid() && view.Order.idPedido.isValid())
                    _view.Show(new VMPedido(view.Order));
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
                List<DetallesDePedido> ordersDetails;
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                switch (_view.ReportType)
                {
                    case Reportes_Pedidos.Pendientes_De_Surtir:
                        ordersDetails = _orders.PendingOrdersReport();
                        report = Reports.FillReport(ordersDetails.Select(x => new VMDetalleDePedido(x)).ToList());
                        break;
                    case Reportes_Pedidos.Del_Cliente:
                        if (!_view.Customer.isValid() || !_view.Customer.idCliente.isValid())
                        {
                            _view.ShowError("El cliente seleccionado no es válido");
                            return;
                        }

                        ordersDetails = _orders.CustomerOrders(_view.Customer);
                        report = Reports.FillReport(_view.Customer, ordersDetails.Select(x => new VMRDetalleDePedido(x)).ToList());
                        break;
                    case Reportes_Pedidos.Pedido:
                        if (!_view.Order.isValid() || !_view.Order.idPedido.isValid())
                        {
                            _view.ShowError("El pedido seleccionado no es válido");
                            return;
                        }

                        report = Reports.FillReport(new VMRPedido(_view.Order, _configs.GetDefault()));
                        break;
                }

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Preview()
        {
            try
            {
                List<DetallesDePedido> ordersDetails;
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                switch (_view.ReportType)
                {
                    case Reportes_Pedidos.Pendientes_De_Surtir:
                        ordersDetails = _orders.PendingOrdersReport();

                        //Se totalizan los detalles
                        var orders = ordersDetails.Select(x => new VMDetalleDePedido(x)).ToList().Totalize();

                        report = Reports.FillReport(orders);
                        break;
                    case Reportes_Pedidos.Del_Cliente:
                        if (!_view.Customer.isValid() || !_view.Customer.idCliente.isValid())
                        {
                            _view.ShowError("El cliente seleccionado no es válido");
                            return;
                        }

                        ordersDetails = _orders.CustomerOrders(_view.Customer);
                        report = Reports.FillReport(_view.Customer, ordersDetails.Select(x => new VMRDetalleDePedido(x)).ToList());
                        break;
                    case Reportes_Pedidos.Pedido:
                        if (!_view.Order.isValid() || !_view.Order.idPedido.isValid())
                        {
                            _view.ShowError("El pedido seleccionado no es válido");
                            return;
                        }

                        report = Reports.FillReport(new VMRPedido(_view.Order,Session.Configuration));
                        break;
                }

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindCustomer()
        {
            if (!_view.Customer.codigo.isValid())
            {
                _view.ShowError("Debe especificar el codigo a buscar");
                return;
            }

            try
            {
                var customer = _customers.Find(_view.Customer.codigo);

                _view.Show(customer);
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

        private void OpenCustomersList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view,_customers);

                view.ShowWindow();

                if (view.Client.isValid() && view.Client.idCliente.isValid())
                    _view.Show(view.Client);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindOrder()
        {
            if (!_view.Order.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var billOfSale = new VMPedido(_orders.FindByFolio(_view.Order.folio));

                _view.Show(billOfSale);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
