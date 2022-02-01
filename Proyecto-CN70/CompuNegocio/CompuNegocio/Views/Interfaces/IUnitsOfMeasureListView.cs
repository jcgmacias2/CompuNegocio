using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IUnitsOfMeasureListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        UnidadesDeMedida Unit { get; }

        void Show(List<UnidadesDeMedida> unitsOfMeasure);
    }
}
