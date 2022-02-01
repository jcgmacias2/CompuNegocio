using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPurchaseOrdersListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        OrdenesDeCompra Order { get; }

        void Show(List<OrdenesDeCompra> orders);
    }
}
