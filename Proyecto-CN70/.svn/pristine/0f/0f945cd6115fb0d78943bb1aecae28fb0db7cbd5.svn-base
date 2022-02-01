using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PaymentFormsListPresenter : BaseListPresenter
    {
        private readonly IPaymentFormsListView _view;
        private IFormaPagoService _paymentForms;
        private bool _showOnlyActive;

        public PaymentFormsListPresenter(IPaymentFormsListView view, IFormaPagoService payments, bool onlyShowActive)
            : base(view)
        {
            _view = view;
            _paymentForms = payments;

            _showOnlyActive = onlyShowActive;
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
            List<FormasPago> paymentMethods;

            try
            {
                if (_view.Parameter.isValid())
                    paymentMethods = _paymentForms.WithDescriptionLike(_view.Parameter);
                else
                    paymentMethods = _paymentForms.List();

                if (_showOnlyActive)
                    paymentMethods = paymentMethods.Where(p => p.activa).ToList();

                _view.Show(paymentMethods);

                if (paymentMethods.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
