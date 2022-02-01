using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class FiscalPaymentsListPresenter : BaseListPresenter
    {
        private IFiscalPaymentsListView _view;
        private IAbonoDeFacturaService _payments;

        public FiscalPaymentsListPresenter(IFiscalPaymentsListView view, IAbonoDeFacturaService payments)
            : base(view)
        {
            _view = view;
            _payments = payments;

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
            List<VwListaParcialidade> payments;

            try
            {
                if (_view.Parameter.isValid())
                    payments = _payments.ListParcialidadesLike(_view.Parameter);
                else
                    payments = _payments.ListParcialidades();

                _view.Show(payments);

                if (payments.Count > 0)
                    _view.GoToRecord(0);

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
