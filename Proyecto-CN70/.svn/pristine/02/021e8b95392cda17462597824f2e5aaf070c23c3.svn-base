using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IScaleItemsTransferView : IBaseView
    {
        event Action Quit;
        event Action Transfer;

        Clasificacione Classification { get; }

        void FillCombo(List<Clasificacione> classifications);
    }
}
