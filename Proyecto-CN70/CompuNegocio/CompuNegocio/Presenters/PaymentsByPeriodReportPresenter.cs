using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;

namespace Aprovi.Presenters
{
    public class PaymentsByPeriodReportPresenter
    {
        private IPaymentsByPeriodReportView _view;
        private IAbonosService _payments;

        public PaymentsByPeriodReportPresenter(IPaymentsByPeriodReportView view, IAbonosService payments, IEmpresaService businessService)
        {
            _view = view;
            _payments = payments;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;

            _view.FillCombo(businessService.List());
        }

        private void Print()
        {
            if (!_view.Business.isValid())
            {
                _view.ShowError("Debe seleccionar la empresa sobre la cual se desean conocer los abonos");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var payments = _payments.ByPeriod(Session.Station.Empresa, _view.Start, _view.End);

                view = new ReportViewerView(Reports.FillReport(payments, _view.Start, _view.End, _view.Business));
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
            if(!_view.Business.isValid())
            {
                _view.ShowError("Debe seleccionar la empresa sobre la cual se desean conocer los abonos");
                return;
            }

            if(_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var payments = _payments.ByPeriod(Session.Station.Empresa, _view.Start, _view.End);

                view = new ReportViewerView(Reports.FillReport(payments, _view.Start, _view.End, _view.Business));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
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

    }
}
