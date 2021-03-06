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
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class PurchaseOrderPrintPresenter
    {
        private IPurchaseOrderPrintView _view;
        private IOrdenDeCompraService _orders;
        private ICompraService _purchases;

        public PurchaseOrderPrintPresenter(IPurchaseOrderPrintView view, IOrdenDeCompraService orders, ICompraService purchases)
        {
            _view = view;
            _orders = orders;
            _purchases = purchases;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.Order.idOrdenDeCompra.isValid())
            {
                _view.ShowError("No hay orden de compra seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMROrdenDeCompra(_view.Order, Session.Configuration), _view.WithoutPrices));
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
            if (!_view.Order.idOrdenDeCompra.isValid())
            {
                _view.ShowError("No hay orden de compra seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMROrdenDeCompra(_view.Order, Session.Configuration), _view.WithoutPrices));
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
                OrdenesDeCompra order = _view.Order;
                var ordenDeCompra = new VMOrdenDeCompra(_orders.FindByFolio(order.folio),_purchases.Find(order));

                _view.Show(ordenDeCompra);
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
                IPurchaseOrdersListView view;
                PurchaseOrdersListPresenter presenter;

                view = new PurchaseOrdersListView();
                presenter = new PurchaseOrdersListPresenter(view,_orders);

                view.ShowWindow();

                if (view.Order.isValid() && view.Order.idOrdenDeCompra.isValid())
                    _view.Show(new VMOrdenDeCompra(view.Order,_purchases.Find(view.Order)));
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

                var order = _orders.FindByFolio(folio);
                var vmOrder = new VMOrdenDeCompra(order,_purchases.Find(order));

                _view.Show(vmOrder);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
