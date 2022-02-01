using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface ITaxesView : IBaseView
    {
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action OpenList;
        event Action Quit;

        Impuesto Tax { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(Impuesto tax);
        void FillCombos(List<TiposDeImpuesto> taxTypes, List<Impuestos> taxes, List<TiposFactor> factors);
    }
}
