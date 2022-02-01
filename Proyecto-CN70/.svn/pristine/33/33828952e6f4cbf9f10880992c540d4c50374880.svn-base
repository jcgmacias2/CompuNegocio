using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ICFDIUsesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        UsosCFDI Use { get; }

        void Show(List<UsosCFDI> uses);

    }
}
