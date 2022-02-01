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
    public interface ISoldItemsListView : IBaseListView, IBaseListPresenter
    {
        event Action Search;

        Cliente Customer { get; }

        void Show(List<VwArticulosVendido> items);
    }
}
