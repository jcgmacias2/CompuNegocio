using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IItemSelectionView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        VMArticulo Item { get; }

        void Show(List<VMArticulo> items);
    }
}
