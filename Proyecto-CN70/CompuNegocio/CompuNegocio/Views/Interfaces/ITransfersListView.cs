using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ITransfersListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Traspaso Transfer { get; }

        void Show(List<Traspaso> transfers);
    }
}
