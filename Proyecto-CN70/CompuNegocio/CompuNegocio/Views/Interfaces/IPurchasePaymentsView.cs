using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IPurchasePaymentsView : IBaseView
    {
        event Action OpenPurchasesList;
        event Action AddPayment;
        event Action Quit;
        event Action CancelPayment;

        AbonosDeCompra Payment { get; }
        bool IsPurchaseDirty { get; }
        AbonosDeCompra CurrentPayment { get; }
        VMCompra Purchase { get; }

        void Clear(string folio);
        void Show(VMCompra purchase);
        void FillCombos(List<Moneda> currencies, List<FormasPago> paymentForms);
    }
}
