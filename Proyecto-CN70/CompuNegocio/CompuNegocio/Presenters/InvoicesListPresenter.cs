using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class InvoicesListPresenter: BaseListPresenter
    {
        private readonly IInvoicesListView _view;
        private IFacturaService _invoices;

        public InvoicesListPresenter(IInvoicesListView view, IFacturaService invoiceService) : base(view)
        {
            _view = view;
            _invoices = invoiceService;

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
            List<VwListaFactura> invoices;

            try
            {
                if (_view.Parameter.isValid())
                    invoices = _invoices.WithClientOrFolioLike(_view.Parameter);
                else
                    invoices = _invoices.List();

                _view.Show(invoices);

                if (invoices.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
