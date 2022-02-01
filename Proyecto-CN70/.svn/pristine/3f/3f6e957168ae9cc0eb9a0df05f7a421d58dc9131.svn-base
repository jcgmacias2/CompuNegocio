using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface ICreditNotesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        VMNotaDeCredito CreditNote { get; }
        bool OnlyWithoutInvoice { get; }
        bool OnlyActives { get; }

        void Show(List<VMNotaDeCredito> creditNotes);
    }
}
