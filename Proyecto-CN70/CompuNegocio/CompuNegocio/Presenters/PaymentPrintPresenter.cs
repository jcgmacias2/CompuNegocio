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
    public class PaymentPrintPresenter
    {
        private IPaymentPrintView _view;
        private IPagoService _payments;
        private IAbonoDeFacturaService _invoicePayments;

        public PaymentPrintPresenter(IPaymentPrintView view, IPagoService payments, IAbonoDeFacturaService invoicePayments)
        {
            _view = view;
            _payments = payments;
            _invoicePayments = invoicePayments;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            var payment = _view.Payment;
            //Si no hay pago seleccionado, ni serie o folio lo regreso
            if (!payment.idPago.isValid() && (!payment.serie.isValid() || !payment.folio.isValid()))
            {
                _view.ShowError("No hay pago seleccionado para visualizar");
                return;
            }

            payment = _payments.Find(payment.serie, payment.folio.ToString());

            if (!payment.isValid() || !payment.idPago.isValid())
            {
                _view.ShowError("No existe el pago especificado");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var report = new VMRAbonosFacturas(payment, _invoicePayments.GetPaymentsForReport(payment.AbonosDeFacturas.ToList()), Session.Configuration);
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
            var payment = _view.Payment;
            //Si no hay pago seleccionado, ni serie o folio lo regreso
            if (!payment.idPago.isValid() && (!payment.serie.isValid() || !payment.folio.isValid()))
            {
                _view.ShowError("No hay pago seleccionado para visualizar");
                return;
            }

            payment = _payments.Find(payment.serie, payment.folio.ToString());

            if (!payment.isValid() || !payment.idPago.isValid())
            {
                _view.ShowError("No existe el pago especificado");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var report = new VMRAbonosFacturas(payment, _invoicePayments.GetPaymentsForReport(payment.AbonosDeFacturas.ToList()), Session.Configuration);
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
            if (!_view.Payment.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            if (!_view.Payment.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var payment = _payments.Find(_view.Payment.serie, _view.Payment.folio.ToString());

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
                throw new NotImplementedException();
                //IPaymentsListView view;
                //PaymentsListPresenter presenter;

                //view = new PaymentsListView();
                //presenter = new PaymentsListPresenter(view, _payments);

                //view.ShowWindow();

                //if (view.Payment.isValid() && view.Payment.idPago.isValid())
                //{
                //    var dbPayment = _payments.Find(view.Payment.idPago);

                //    if (dbPayment.isValid() && dbPayment.idPago.isValid())
                //    {
                //        _view.Show(dbPayment);
                //    }
                //}
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {
            if (!_view.Payment.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            try
            {
                var folio = _payments.Last(_view.Payment.serie);
                var payment = _payments.Find(_view.Payment.serie, folio.ToString());

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Show(new Pago());
            }
        }
    }
}
