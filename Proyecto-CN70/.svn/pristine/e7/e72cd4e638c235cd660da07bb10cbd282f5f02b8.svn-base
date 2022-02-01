using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PayableBalancesReportPresenter
    {
        private IPayableBalancesReportView _view;
        private ISaldosPorProveedorService _payableBalances;

        public PayableBalancesReportPresenter(IPayableBalancesReportView view, ISaldosPorProveedorService payableBalances)
        {
            _view = view;
            _payableBalances = payableBalances;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var balances = _payableBalances.List();

                if (balances.Count.Equals(0))
                {
                    _view.ShowMessage("No existen cuentas por pagar");
                    return;
                }

                view = new ReportViewerView(Reports.FillReport(balances));
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
            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var balances = _payableBalances.List();

                if(balances.Count.Equals(0))
                {
                    _view.ShowMessage("No existen cuentas por pagar");
                    return;
                }

                view = new ReportViewerView(Reports.FillReport(balances));
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
