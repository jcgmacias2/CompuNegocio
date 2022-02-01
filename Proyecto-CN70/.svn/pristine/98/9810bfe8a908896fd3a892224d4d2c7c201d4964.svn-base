using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IAdjustmentsView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action FindItem;
        event Action OpenItemsList;
        event Action AddItem;
        event Action RemoveItem;
        event Action SelectItem;

        event Action New;
        event Action Quit;
        event Action Print;
        event Action Save;

        Ajuste Adjustment { get; }
        bool IsDirty { get; }
        DetallesDeAjuste CurrentItem { get; }
        DetallesDeAjuste SelectedItem { get; }

        void Show(Ajuste adjustment);
        void Clear();
        void FillCombo(List<TiposDeAjuste> adjustmentTypes);
        void Show(DetallesDeAjuste detail);
        void ShowStock(decimal stock);
        void ClearItem();
    }
}
