using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aprovi.Presenters
{
    public class TransfersListPresenter : BaseListPresenter
    {
        private ITransfersListView _view;
        private ITraspasoService _transfers;

        public TransfersListPresenter(ITransfersListView view, ITraspasoService transfers)
            : base(view)
        {
            _view = view;
            _transfers = transfers;

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
            List<Traspaso> transfers;

            try
            {
                if (_view.Parameter.isValid())
                    transfers = _transfers.WithFolioOrCompanyLike(_view.Parameter);
                else
                    transfers = _transfers.List();

                _view.Show(transfers);

                if (transfers.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

    }
}
