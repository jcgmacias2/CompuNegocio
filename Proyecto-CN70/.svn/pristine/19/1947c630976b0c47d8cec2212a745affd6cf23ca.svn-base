using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface ITransfersByPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenOriginAssociatedCompaniesList;
        event Action OpenDestinationAssociatedCompaniesList;
        event Action OpenTransfersList;
        event Action FindOriginAssociatedCompany;
        event Action FindDestinationAssociatedCompany;
        event Action FindTransfer;

        VMReporteTraspasos Report { get; }

        void Show(VMReporteTraspasos vm);
    }
}
