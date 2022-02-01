using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IBankAccountsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        CuentasBancaria Account { get; }

        void Show(List<CuentasBancaria> accounts);
    }
}
