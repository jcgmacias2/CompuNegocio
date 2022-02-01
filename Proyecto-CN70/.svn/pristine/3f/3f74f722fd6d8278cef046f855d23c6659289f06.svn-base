using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IBusinessesView : IBaseView
    {
        event Action Quit;
        event Action New;
        event Action Save;
        event Action Find;
        event Action Update;
        event Action Delete;
        event Action OpenList;

        Empresa Business { get; }
        bool IsDirty { get; }

        void Show(Empresa business);
        void Clear();
    }
}
