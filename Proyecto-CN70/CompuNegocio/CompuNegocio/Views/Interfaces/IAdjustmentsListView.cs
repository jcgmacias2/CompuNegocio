using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IAdjustmentsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Ajuste Adjustment { get; }

        void Show(List<Ajuste> adjustments);
    }
}
