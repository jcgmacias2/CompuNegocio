using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Presenters
{
    public class TransferRequestsListPresenter : BaseListPresenter
    {
        private ITransferRequestsListView _view;
        private ISolicitudDeTraspasoService _transferRequests;

        public TransferRequestsListPresenter(ITransferRequestsListView view, ISolicitudDeTraspasoService transferRequests)
            : base(view)
        {
            _view = view;
            _transferRequests = transferRequests;

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
            List<SolicitudesDeTraspaso> transferRequests;

            try
            {
                if (_view.Parameter.isValid())
                    transferRequests = _transferRequests.WithFolioOrCompanyLike(_view.Parameter);
                else
                    transferRequests = _transferRequests.List();

                _view.Show(transferRequests);

                if (transferRequests.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
