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
using Aprovi.Application.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class FiscalPaymentPrintPresenter
    {
        private IFiscalPaymentPrintView _view;
        private IAbonoDeFacturaService _invoicePayments;
        private IPagoService _payments;

        public FiscalPaymentPrintPresenter(IFiscalPaymentPrintView view, IAbonoDeFacturaService invoicePayments, IPagoService payments)
        {
            _view = view;
            _invoicePayments = invoicePayments;
            _payments = payments;

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
            VMReporte report = null;
            //Si no hay pago seleccionado, ni serie o folio lo regreso
            if (!payment.idAbonoDeFactura.isValid() && payment.TimbresDeAbonosDeFactura.isValid() && (!payment.TimbresDeAbonosDeFactura.serie.isValid() || !payment.TimbresDeAbonosDeFactura.folio.isValid()))
            {
                _view.ShowError("No hay pago seleccionado para visualizar");
                return;
            }

            try
            {
                //var exists = _payments.Find(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio);
                var exists = _invoicePayments.FindVMBySerieAndFolio(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio);

                if (!exists.isValid() || !exists.idAbonoDeFactura.isValid())
                {
                    _view.ShowError("No existe el pago especificado");
                    return;
                }

                //Se determina si el pago es sencillo o multiple
                if (exists.TipoDeAbono == (int)TiposParcialidad.Simple)
                {
                    var invoicePayment = _invoicePayments.Find(exists.idAbonoDeFactura);
                    report = Reports.FillReport(new VMPago(invoicePayment, Session.Configuration));
                }
                else
                {
                    var pago = _payments.Find(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio.ToString());
                    var vm = new VMRAbonosFacturas(pago, _invoicePayments.GetPaymentsForReport(pago.AbonosDeFacturas.ToList()), Session.Configuration);
                    report = Reports.FillReport(vm);
                }

                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(report);
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
            VMReporte report = null;
            //Si no hay pago seleccionado, ni serie o folio lo regreso
            if (!payment.idAbonoDeFactura.isValid() && payment.TimbresDeAbonosDeFactura.isValid() && (!payment.TimbresDeAbonosDeFactura.serie.isValid() || !payment.TimbresDeAbonosDeFactura.folio.isValid()))
            {
                _view.ShowError("No hay pago seleccionado para visualizar");
                return;
            }

            try
            {
                //var exists = _payments.Find(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio);
                var exists = _invoicePayments.FindVMBySerieAndFolio(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio);

                if (!exists.isValid() || !exists.idAbonoDeFactura.isValid())
                {
                    _view.ShowError("No existe el pago especificado");
                    return;
                }

                //Se determina si el pago es sencillo o multiple
                if (exists.TipoDeAbono == (int)TiposParcialidad.Simple)
                {
                    var invoicePayment = _invoicePayments.Find(exists.idAbonoDeFactura);
                    report = Reports.FillReport(new VMPago(invoicePayment, Session.Configuration));
                }
                else
                {
                    var pago = _payments.Find(payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio.ToString());
                    var vm = new VMRAbonosFacturas(pago, _invoicePayments.GetPaymentsForReport(pago.AbonosDeFacturas.ToList()), Session.Configuration);
                    report = Reports.FillReport(vm);
                }

                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(report);
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
            try
            {
                if (!_view.Payment.TimbresDeAbonosDeFactura.serie.isValid())
                    return;

                if (!_view.Payment.TimbresDeAbonosDeFactura.folio.isValid())
                    return;

                var exists = _invoicePayments.FindVMBySerieAndFolio(_view.Payment.TimbresDeAbonosDeFactura.serie, _view.Payment.TimbresDeAbonosDeFactura.folio);

                if (!exists.isValid())
                    throw new Exception("No se encontró ninguna pago con el folio especificado");

                var abono = _invoicePayments.Find(exists.idAbonoDeFactura);

                _view.Show(abono);
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
                FiscalPaymentsListPresenter presenter;

                view = new FiscalPaymentsListView();
                presenter = new FiscalPaymentsListPresenter(view, _invoicePayments);

                view.ShowWindow();

                if (view.FiscalPayment.isValid())
                {
                    var abono = _invoicePayments.Find(view.FiscalPayment.idAbonoDeFactura);
                    _view.Show(abono);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {
            if (!_view.Payment.TimbresDeAbonosDeFactura.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            try
            {
                var payment = _invoicePayments.LastParcialidad(_view.Payment.TimbresDeAbonosDeFactura.serie);

                if(payment.isValid())
                    _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Show(new AbonosDeFactura());
            }
        }
    }
}
