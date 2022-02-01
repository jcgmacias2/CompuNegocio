using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class FiscalPaymentsPresenter
    {
        private IFiscalPaymentsView _view;
        private IAbonoDeFacturaService _payments;

        public FiscalPaymentsPresenter(IFiscalPaymentsView view, IAbonoDeFacturaService payments)
        {
            _view = view;
            _payments = payments;

            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Cancel += Cancel;
        }

        private void Cancel()
        {
            if(!_view.IsDirty)
            {
                _view.ShowError("No hay ninguna parcialidad seleccionada para cancelar");
                return;
            }

            try
            {
                var payment = _payments.Cancel(_view.FiscalPayment.idAbonoDeFactura);

                _view.ShowMessage("Parcialidad cancelada exitosamente");

                //Aquí genero el pdf
                var receipt = new VMAcuse(payment, Session.Configuration);
                var report = Reports.FillReport(receipt);
                report.Export(string.Format("{0}\\{1}{2}-Acuse Cancelación.pdf", Session.Configuration.CarpetaPdf, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio));

                _view.Clear();
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
                IFiscalPaymentsListView view;
                //InvoicesPaymentsListPresenter presenter;
                FiscalPaymentsListPresenter presenter;

                view = new FiscalPaymentsListView();
                //presenter = new InvoicesPaymentsListPresenter(view,_payments);
                presenter = new FiscalPaymentsListPresenter(view,_payments);

                view.ShowWindow();

                if (view.FiscalPayment.isValid() && view.FiscalPayment.idAbonoDeFactura.isValid())
                {
                    var paymentVM = view.FiscalPayment;

                    var payment = _payments.Find(paymentVM.idAbonoDeFactura);

                    _view.Show(payment);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if(!_view.FiscalPayment.TimbresDeAbonosDeFactura.serie.isValid() || !_view.FiscalPayment.TimbresDeAbonosDeFactura.folio.isValid())
            {
                _view.Clear();
                return;
            }

            try
            {
                //Obtengo el abono
                var payment = _payments.Find(_view.FiscalPayment.TimbresDeAbonosDeFactura.serie, _view.FiscalPayment.TimbresDeAbonosDeFactura.folio.ToIntOrDefault());

                if(!payment.isValid())
                {
                    _view.ShowMessage("No existe ninguna parcialidad con el folio {0}{1}", _view.FiscalPayment.TimbresDeAbonosDeFactura.serie, _view.FiscalPayment.TimbresDeAbonosDeFactura.folio);
                    return;
                }

                _view.Show(payment);
                
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
