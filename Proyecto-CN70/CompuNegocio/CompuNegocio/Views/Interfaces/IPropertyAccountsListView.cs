using Aprovi.Data.Models;
using System;
using System.Collections.Generic;


namespace Aprovi.Views
{
    public interface IPropertyAccountsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        CuentasPrediale Account { get; }

        void Show(List<CuentasPrediale> accounts);
    }
}
