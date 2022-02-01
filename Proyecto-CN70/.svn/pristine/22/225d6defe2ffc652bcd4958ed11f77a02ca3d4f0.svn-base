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
    public class PurchasesListPresenter : BaseListPresenter
    {
        private readonly IPurchasesListView _view;
        private ICompraService _purchases;

        public PurchasesListPresenter(IPurchasesListView view, ICompraService purchasesService)
            : base(view)
        {
            _view = view;
            _purchases = purchasesService;

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
            List<Compra> purchases;

            try
            {
                if (_view.Parameter.isValid())
                    purchases = _purchases.WithFolioOrSupplierLike(_view.Parameter);
                else
                    purchases = _purchases.List();

                //Si la ventana fue abierta desde compras, debe filtrarse por el cliente capturado en compras
                if (_view.IdSupplier.isValid())
                    purchases = purchases.Where(p => p.idProveedor.Equals(_view.IdSupplier)).ToList();

                _view.Show(purchases.ToViewModelList());

                if (purchases.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
