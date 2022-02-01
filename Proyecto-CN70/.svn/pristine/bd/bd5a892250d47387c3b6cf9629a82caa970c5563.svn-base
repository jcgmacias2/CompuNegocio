using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IItemsAppraisalReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenClassificationsList;
        event Action FindClassification;
        event Action SelectedFilterChanged;
        event Action Load;

        VMRAvaluo Report { get; }

        void Show(Clasificacione classification);
        void Show(VMRAvaluo appraisal);
    }
}
