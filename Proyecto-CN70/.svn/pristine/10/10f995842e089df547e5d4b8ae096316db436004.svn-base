using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IStockReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action AddClassification;
        event Action OpenClassificationsList;

        bool OnlyWithStock { get; }
        Clasificacione Classification { get; }
        List<Clasificacione> SelectedClassifications { get; }

        void Show(Clasificacione classification);
        void Show(List<Clasificacione> classifications);

        void Clear();
    }
}
