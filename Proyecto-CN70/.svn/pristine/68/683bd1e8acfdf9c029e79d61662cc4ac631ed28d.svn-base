using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IKardexReportView : IBaseView
    {
        event Action FindItem;
        event Action OpenItemsList;
        event Action Quit;
        event Action Print;
        event Action Preview;

        Articulo Item { get; }
        DateTime Start { get; }
        DateTime End { get; }

        void Show(Articulo item);
    }
}
