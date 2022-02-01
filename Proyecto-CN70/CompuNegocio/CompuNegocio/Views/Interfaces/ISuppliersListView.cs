using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ISuppliersListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Proveedore Supplier { get; }

        void Show(List<Proveedore> suppliers);
    }
}
