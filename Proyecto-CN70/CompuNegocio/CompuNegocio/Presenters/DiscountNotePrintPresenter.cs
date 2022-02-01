using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class DiscountNotePrintPresenter
    {
        private IDiscountNotePrintView _view;
        private INotaDeDescuentoService _discountNotes;

        public DiscountNotePrintPresenter(IDiscountNotePrintView view, INotaDeDescuentoService discountNote)
        {
            _view = view;
            _discountNotes = discountNote;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.DiscountNote.idNotaDeDescuento.isValid())
            {
                _view.ShowError("No hay nota de crédito seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var discountNote = _discountNotes.FindByFolio(_view.DiscountNote.folio);

                view = new ReportViewerView(Reports.FillReport(new VMRNotaDeDescuento(discountNote, Session.Configuration)));
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            if (!_view.DiscountNote.idNotaDeDescuento.isValid())
            {
                _view.ShowError("No hay nota de crédito seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var discountNote = _discountNotes.FindByFolio(_view.DiscountNote.folio);

                view = new ReportViewerView(Reports.FillReport(new VMRNotaDeDescuento(discountNote, Session.Configuration)));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {

            if (!_view.DiscountNote.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var discountNote = _discountNotes.FindByFolio(_view.DiscountNote.folio);

                _view.Show(discountNote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                IDiscountNotesListView view;
                DiscountNotesListPresenter presenter;

                view = new DiscountNotesListView();
                presenter = new DiscountNotesListPresenter(view,_discountNotes);

                view.ShowWindow();

                if (view.DiscountNote.isValid() && view.DiscountNote.idNotaDeDescuento.isValid())
                    _view.Show(view.DiscountNote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {

            try
            {
                var folio = _discountNotes.Last();
                var discountNote = _discountNotes.FindByFolio(folio);

                _view.Show(discountNote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
