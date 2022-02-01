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

namespace Aprovi.Presenters
{
    public class TransferPrintPresenter
    {
        private ITransferPrintView _view;
        private ITraspasoService _transfers;

        public TransferPrintPresenter(ITransferPrintView view, ITraspasoService transfer)
        {
            _view = view;
            _transfers = transfer;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.Transfer.idTraspaso.isValid())
            {
                _view.ShowError("No hay traspaso seleccionado para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMRTraspaso(_view.Transfer)));
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
            if (!_view.Transfer.idTraspaso.isValid())
            {
                _view.ShowError("No hay pedido seleccionado para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(new VMRTraspaso(_view.Transfer)));
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

            if (!_view.Transfer.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var transfer = new VMTraspaso(_transfers.FindByFolio(_view.Transfer.folio));

                _view.Show(transfer);
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
                ITransfersListView view;
                TransfersListPresenter presenter;

                view = new TransfersListView();
                presenter = new TransfersListPresenter(view,_transfers);

                view.ShowWindow();

                if (view.Transfer.isValid() && view.Transfer.idTraspaso.isValid())
                    _view.Show(new VMTraspaso(view.Transfer));
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
                var folio = _transfers.Last();
                var transfer = new VMTraspaso(_transfers.FindByFolio(folio));

                _view.Show(transfer);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
