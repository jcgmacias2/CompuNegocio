using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ITaxesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Impuesto Tax { get; }

        void Show(List<Impuesto> taxes);
    }
}
