using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IClassificationsView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Update;

        bool IsDirty { get; }
        Clasificacione Classification { get; }

        void Clear();
        void Show(Clasificacione classification);
    }
}
