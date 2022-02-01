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
    public class ClientPaymentsListPresenter : BaseListPresenter
    {
        private IClientPaymentsListView _view;
        private IPagoService _payments;

        public ClientPaymentsListPresenter(IClientPaymentsListView view, IPagoService paymentsService) : base(view)
        {
            _view = view;
            _payments = paymentsService;

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
            List<VwListaPago> invoices;

            try
            {
                if (_view.Parameter.isValid())
                    invoices = _payments.WithClientOrFolioLike(_view.Parameter);
                else
                    invoices = _payments.List();

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
