using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ITransferRequestsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        SolicitudesDeTraspaso TransferRequest { get; }

        void Show(List<SolicitudesDeTraspaso> transferRequests);
    }
}
