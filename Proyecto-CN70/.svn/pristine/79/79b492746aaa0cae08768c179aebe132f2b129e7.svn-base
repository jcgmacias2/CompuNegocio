using System;
using System.Collections.Generic;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IInvoicesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        VwListaFactura Invoice { get; }

        void Show(List<VwListaFactura> invoices);
    }
}
