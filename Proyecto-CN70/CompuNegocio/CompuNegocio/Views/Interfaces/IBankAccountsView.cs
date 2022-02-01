using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IBankAccountsView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Save;
        event Action Update;
        event Action Delete;
        event Action New;

        CuentasBancaria Account { get; }

        void Clear();
        void Show(CuentasBancaria account);
        void FillCombos(List<Banco> bancos, List<Moneda> currencies);
    }
}
