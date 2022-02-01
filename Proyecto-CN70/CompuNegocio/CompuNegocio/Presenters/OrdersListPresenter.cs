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
    public class OrdersListPresenter: BaseListPresenter
    {
        private readonly IOrdersListView _view;
        private IPedidoService _orders;

        public OrdersListPresenter(IOrdersListView view, IPedidoService orders)
            : base(view)
        {
            _view = view;
            _orders = orders;

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
            List<Pedido> orders;

            try
            {
                if (_view.Parameter.isValid())
                    orders = _orders.WithFolioOrClientLike(_view.Parameter);
                else
                    orders = _orders.List();

                _view.Show(orders);

                if (orders.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
