using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class CreditNotePrintPresenter
    {
        private ICreditNotePrintView _view;
        private INotaDeCreditoService _creditNotes;

        public CreditNotePrintPresenter(ICreditNotePrintView view, INotaDeCreditoService creditNotes)
        {
            _view = view;
            _creditNotes = creditNotes;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            var creditNote = _view.CreditNote;
            //Si no hay nota de credito seleccionada, ni serie o folio lo regreso
            if (!creditNote.idNotaDeCredito.isValid() && (!creditNote.serie.isValid() || !creditNote.folio.isValid()))
            {
                _view.ShowError("No hay nota de crédito seleccionada para visualizar");
                return;
            }

            creditNote = _creditNotes.Find(creditNote.serie, creditNote.folio.ToString());

            if (!creditNote.isValid() || !creditNote.idNotaDeCredito.isValid())
            {
                _view.ShowError("No existe la nota de crédito especificada");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var report = new VMRNotaDeCredito(new VMNotaDeCredito(creditNote), Session.Configuration);
                view = new ReportViewerView(Reports.FillReport(report));
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
            var creditNote = _view.CreditNote;
            //Si no hay nota de credito seleccionada, ni serie o folio lo regreso
            if (!creditNote.idNotaDeCredito.isValid() && (!creditNote.serie.isValid() || !creditNote.folio.isValid()))
            {
                _view.ShowError("No hay nota de crédito seleccionada para visualizar");
                return;
            }

            creditNote = _creditNotes.Find(creditNote.serie, creditNote.folio.ToString());

            if (!creditNote.isValid() || !creditNote.idNotaDeCredito.isValid())
            {
                _view.ShowError("No existe la nota de crédito especificada");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var report = new VMRNotaDeCredito(new VMNotaDeCredito(creditNote), Session.Configuration);
                view = new ReportViewerView(Reports.FillReport(report));
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
            if (!_view.CreditNote.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            if (!_view.CreditNote.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var payment = _creditNotes.Find(_view.CreditNote.serie, _view.CreditNote.folio.ToString());

                _view.Show(payment);
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
                ICreditNotesListView view;
                CreditNotesListPresenter presenter;

                view = new CreditNotesListView();
                presenter = new CreditNotesListPresenter(view, _creditNotes, -1);

                view.ShowWindow();

                if (view.CreditNote.isValid() && view.CreditNote.idNotaDeCredito.isValid())
                {
                    var dbCreditNote = _creditNotes.Find(view.CreditNote.idNotaDeCredito);

                    if (dbCreditNote.isValid() && dbCreditNote.idNotaDeCredito.isValid())
                    {
                        _view.Show(dbCreditNote);
                    }
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {
            if (!_view.CreditNote.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            try
            {
                var folio = _creditNotes.Last(_view.CreditNote.serie);
                var payment = _creditNotes.Find(_view.CreditNote.serie, folio.ToString());

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Show(new NotasDeCredito());
            }
        }
    }
}
