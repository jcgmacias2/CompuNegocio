using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IStockFlowReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;
        event Action FindItem;
        event Action OpenItemsList;
        event Action FindClassification;
        event Action OpenClassificationsList;

        DateTime Start { get; }
        DateTime End { get; }
        ReporteDeFlujoPor Filtro { get; }
        Articulo Item { get; }
        Clasificacione Classification { get; }

        void Show(Articulo item);
        void Show(Clasificacione classification);
        void ClearItem();
        void ClearClassification();
    }
}
