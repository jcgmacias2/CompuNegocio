using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IPaymentFormsView : IBaseView
    {
        event Action Find;
        event Action New;
        event Action Deactivate;
        event Action Update;
        event Action OpenList;
        event Action Quit;

        FormasPago PaymentForm { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(FormasPago paymentForm);
    }
}
