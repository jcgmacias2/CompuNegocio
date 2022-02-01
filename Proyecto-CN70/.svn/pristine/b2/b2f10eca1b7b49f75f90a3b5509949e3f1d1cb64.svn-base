using Aprovi.Business.Services;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class CreditNotesListPresenter : BaseListPresenter
    {
        private ICreditNotesListView _view;
        private INotaDeCreditoService _creditNotes;
        private int _idClient;

        public CreditNotesListPresenter(ICreditNotesListView view, INotaDeCreditoService creditNotes, int idClient) : base(view)
        {
            _view = view;
            _creditNotes = creditNotes;
            _idClient = idClient;

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
            List<VMNotaDeCredito> creditNotes;

            try
            {
                if (_view.Parameter.isValid())
                    creditNotes = _creditNotes.WithClientOrFolioLike(_view.Parameter);
                else
                    creditNotes = _creditNotes.List();

                if (_view.OnlyWithoutInvoice)
                {
                    creditNotes = creditNotes.Where(x => !x.idFactura.isValid()).ToList();
                }

                if (_view.OnlyActives)
                {
                    creditNotes = creditNotes.Where(x => !x.CancelacionesDeNotaDeCredito.isValid()).ToList();
                }

                if (_idClient.isValid())
                    creditNotes = creditNotes.Where(c => c.idCliente.Equals(_idClient) && c.TimbresDeNotasDeCredito.isValid()).ToList();

                _view.Show(creditNotes);

                if (creditNotes.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
