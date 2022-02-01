using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IStationsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Estacione Station { get; }

        void Show(List<Estacione> stations);
    }
}
