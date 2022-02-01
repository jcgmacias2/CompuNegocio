using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IEquivalenciesView : IBaseView
    {
        event Action Quit;
        event Action Add;
        event Action Delete;

        Articulo Item { get; }
        Equivalencia Equivalency { get; }

        int CurrentRecord { get; }
        Equivalencia CurrentEquivalency { get; }

        void Show(List<Equivalencia> equivalencies);
        void Clear();
        void FillCombos(List<UnidadesDeMedida> unitsOfMeasure);
    }
}
