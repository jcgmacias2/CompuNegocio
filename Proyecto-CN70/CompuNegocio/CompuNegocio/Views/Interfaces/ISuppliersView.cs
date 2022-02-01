using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ISuppliersView : IBaseView
    {
        event Action Quit;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Find;
        event Action Update;
        event Action OpenList;

        Proveedore Supplier { get; }
        bool IsDirty { get; }

        void Show(Proveedore supplier);
        void Clear();
        void FillCombo(List<Pais> countries);
    }
}
