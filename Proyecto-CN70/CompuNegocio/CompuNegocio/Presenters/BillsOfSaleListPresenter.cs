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
    public class BillsOfSaleListPresenter: BaseListPresenter
    {
        private readonly IBillsOfSaleListView _view;
        private IRemisionService _billsOfSale;

        public BillsOfSaleListPresenter(IBillsOfSaleListView view, IRemisionService billsOfSale)
            : base(view)
        {
            _view = view;
            _billsOfSale = billsOfSale;

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
            List<Remisione> billsOfSale;

            try
            {
                if (_view.Parameter.isValid())
                    billsOfSale = _billsOfSale.WithFolioOrClientLike(_view.Parameter);
                else
                    billsOfSale = _billsOfSale.List();

                _view.Show(billsOfSale);

                if (billsOfSale.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
