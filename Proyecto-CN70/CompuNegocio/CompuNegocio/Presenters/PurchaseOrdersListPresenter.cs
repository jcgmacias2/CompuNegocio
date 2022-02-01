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
    public class PurchaseOrdersListPresenter: BaseListPresenter
    {
        private readonly IPurchaseOrdersListView _view;
        private IOrdenDeCompraService _orders;
        private int? _idProveedor;
        private bool _notFulfilledOnly;

        public PurchaseOrdersListPresenter(IPurchaseOrdersListView view, IOrdenDeCompraService orders, int? idProveedor = null, bool notFulfilledOnly = false)
            : base(view)
        {
            _view = view;
            _orders = orders;
            _idProveedor = idProveedor;
            _notFulfilledOnly = notFulfilledOnly;

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
            List<OrdenesDeCompra> orders;

            try
            {
                if (_view.Parameter.isValid())
                    orders = _orders.WithFolioOrProviderLike(_view.Parameter);
                else
                    orders = _orders.List();

                //Se muestran solo las ordenes sin surtir
                if (_notFulfilledOnly)
                {
                    orders = orders.Where(x =>
                        x.idEstatusDeOrdenDeCompra != (int)StatusDeOrdenDeCompra.Cancelado &&
                        x.idEstatusDeOrdenDeCompra != (int)StatusDeOrdenDeCompra.Surtido_Total).ToList();
                }

                //Se muestran solo las ordenes del proveedor
                if (_idProveedor.HasValue && _idProveedor.Value.isValid())
                {
                    orders = orders.Where(x => x.idProveedor == _idProveedor).ToList();
                }

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
