using Aprovi.Data.Models;
using System;


namespace Aprovi.Views
{
    public interface ICFDIUsesView : IBaseView
    {
        event Action Find;
        event Action New;
        event Action Deactivate;
        event Action Update;
        event Action OpenList;
        event Action Quit;

        UsosCFDI Use { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(UsosCFDI use);
    }
}
