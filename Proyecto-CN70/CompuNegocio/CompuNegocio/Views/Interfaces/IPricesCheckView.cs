using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPricesCheckView : IBaseListView, IBaseListPresenter
    {
        event Action Load;
        event Action Select;
        event Action Search;

        VMArticulo Item { get; }
        List<VMArticulo> items { get; }
        Cliente Client { get; }

        void Show(List<VMArticulo> items);
    }
}
