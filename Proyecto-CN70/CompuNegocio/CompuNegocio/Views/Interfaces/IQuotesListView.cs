using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IQuotesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Cotizacione Quote { get; }

        void Show(List<Cotizacione> quote);
    }
}
