using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IClientsView : IBaseView, IGuardianClientView, IClientsItemsCodesView
    {
        event Action Quit;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Find;
        event Action Update;
        event Action OpenList;
        event Action OpenSoldItemsList;
        event Action OpenUsersList;
        event Action FindUser;
        event Action OpenSalesList;

        Cliente Client { get; }
        bool IsDirty { get; }

        void Show(Cliente client);
        void Show(Usuario user);
        void ShowTotals(VwSaldosPorClientePorMoneda totalDollars, VwSaldosPorClientePorMoneda totalPesos);
        void Clear();
        void FillCombo(List<Pais> countries, List<ListasDePrecio> priceLists, List<UsosCFDI> cfdiUsages, List<Regimene> regimenes);
    }
}
