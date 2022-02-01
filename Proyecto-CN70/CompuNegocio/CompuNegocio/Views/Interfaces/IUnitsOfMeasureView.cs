using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IUnitsOfMeasureView : IBaseView
    {
        event Action Find;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action OpenList;
        event Action Quit;

        UnidadesDeMedida UnitOfMeasure { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(UnidadesDeMedida unitOfMeasure);
    }
}
