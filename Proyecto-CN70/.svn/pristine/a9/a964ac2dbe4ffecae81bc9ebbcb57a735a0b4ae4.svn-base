using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPricesListView: IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        ListasDePrecio PricesList { get; }
        TipoDeBusqueda SearchType { get; }

        void Show(List<ListasDePrecio> pricesLists);
        void FillCombo(List<TipoDeBusqueda> searchTypes);
    }
}
