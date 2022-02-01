using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Presenters
{
    public class DiscountNotesListPresenter : BaseListPresenter
    {
        private IDiscountNotesListView _view;
        private INotaDeDescuentoService _discountService;

        private int _idCliente;

        public DiscountNotesListPresenter(IDiscountNotesListView view, INotaDeDescuentoService discountService, int idCliente = 0)
            : base(view)
        {
            _view = view;
            _discountService = discountService;
            _idCliente = idCliente;

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
            List<NotasDeDescuento> discountNotes;

            try
            {
                if (_view.Parameter.isValid())
                    discountNotes = _discountService.WithClientOrFolioLike(_view.Parameter);
                else
                    discountNotes = _discountService.List();

                if (_view.OnlyActive)
                {
                    discountNotes = discountNotes.Where(x=>x.idEstatusDeNotaDeDescuento != (int)StatusDeNotaDeDescuento.Cancelada).ToList();
                }

                if (_view.OnlyWithoutAssign)
                {
                    discountNotes = discountNotes.Where(x => x.idFactura == null && x.idNotaDeCredito == null).ToList();
                }

                if (_idCliente.isValid())
                {
                    discountNotes = discountNotes.Where(x => x.idCliente == _idCliente).ToList();
                }

                _view.Show(discountNotes);

                if (discountNotes.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
