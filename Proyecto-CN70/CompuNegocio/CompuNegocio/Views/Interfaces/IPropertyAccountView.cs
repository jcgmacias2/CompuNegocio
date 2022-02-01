using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IPropertyAccountView : IBaseView
    {
        event Action New;
        event Action Quit;
        event Action Delete;
        event Action Save;
        event Action OpenList;
        event Action Find;

        CuentasPrediale Account { get; }

        void Show(CuentasPrediale account);
        void Clear();
    }
}
