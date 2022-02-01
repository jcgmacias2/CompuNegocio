using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IDiscountNotesView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action FindCustomer;
        event Action OpenCustomersList;
        event Action Quit;
        event Action Save;
        event Action Update;
        event Action Cancel;
        event Action New;
        event Action Load;
        event Action AmountChanged;
        event Action Print;

        NotasDeDescuento DiscountNote { get; }

        void Clear();
        void Show(NotasDeDescuento discountNote);
        void FillCombos(List<Moneda> currencies);
    }
}
