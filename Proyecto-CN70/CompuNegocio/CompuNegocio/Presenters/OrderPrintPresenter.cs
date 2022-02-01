using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class OrderPrintPresenter
    {
        private IOrderPrintView _view;
        private IPedidoService _orders;

        public OrderPrintPresenter(IOrderPrintView view, IPedidoService orders)
        {
            _view = view;
            _orders = orders;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.Order.idPedido.isValid())
            {
                _view.ShowError("No hay pedido seleccionado para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMRPedido(_view.Order, Session.Configuration)));
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
            if (!_view.Order.idPedido.isValid())
            {
                _view.ShowError("No hay pedido seleccionado para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMRPedido(_view.Order,Session.Configuration)));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {

            if (!_view.Order.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var order = new VMPedido(_orders.FindByFolio(_view.Order.folio));

                _view.Show(order);
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

        private void OpenList()
        {
            try
            {
                IOrdersListView view;
                OrdersListPresenter presenter;

                view = new OrdersListView();
                presenter = new OrdersListPresenter(view,_orders);

                view.ShowWindow();

                if (view.Order.isValid() && view.Order.idPedido.isValid())
                    _view.Show(new VMPedido(view.Order));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {

            try
            {
                var folio = _orders.Last();
                var order = new VMPedido(_orders.FindByFolio(folio));

                _view.Show(order);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
