using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IBaseListPresenter
    {
        event Action Quit;
        event Action GoFirst;
        event Action GoPrevious;
        event Action GoNext;
        event Action GoLast;
    }
}
