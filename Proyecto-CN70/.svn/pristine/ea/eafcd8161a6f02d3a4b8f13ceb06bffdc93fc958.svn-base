using Aprovi.Data.Models;
using System;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface ISalesPerItemReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;
        event Action FindItem;
        event Action OpenItemsList;
        event Action FindClassification;
        event Action OpenClassificationsList;

        VMVentasPorArticulo Report { get; }

        void Show(VMVentasPorArticulo vm);
    }
}
