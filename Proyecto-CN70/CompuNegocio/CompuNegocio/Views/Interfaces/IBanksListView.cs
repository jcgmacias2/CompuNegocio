using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IBanksListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Banco Bank { get; }

        void Show(List<Banco> banks);
    }
}
