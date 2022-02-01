using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IPaymentFormsListView: IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        FormasPago PaymentForm { get; }

        void Show(List<FormasPago> paymentForms);
    }
}
