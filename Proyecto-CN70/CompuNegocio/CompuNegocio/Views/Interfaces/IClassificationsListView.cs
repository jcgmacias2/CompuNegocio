using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IClassificationsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Clasificacione Classification { get; }

        void Show(List<Clasificacione> classifications);
    }
}
