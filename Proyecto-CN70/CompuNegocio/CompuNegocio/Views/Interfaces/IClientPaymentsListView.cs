using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IClientPaymentsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        VwListaPago Payment { get; }

        void Show(List<VwListaPago> payments);
    }
}
