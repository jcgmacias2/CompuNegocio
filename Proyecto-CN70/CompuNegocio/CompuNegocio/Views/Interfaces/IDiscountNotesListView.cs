using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IDiscountNotesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        NotasDeDescuento DiscountNote { get; }
        bool OnlyWithoutAssign { get; }
        bool OnlyActive { get; }

        void Show(List<NotasDeDescuento> discountNote);
    }
}
