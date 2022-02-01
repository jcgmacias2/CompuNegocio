using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IBanksView : IBaseView
    {
        event Action Find;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action OpenList;
        event Action Quit;

        Banco Bank { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(Banco bank);
    }
}
