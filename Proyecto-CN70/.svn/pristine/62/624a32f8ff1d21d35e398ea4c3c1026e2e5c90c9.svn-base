using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IClientsListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        Cliente Client { get; }

        void Show(List<Cliente> clients);
    }
}
