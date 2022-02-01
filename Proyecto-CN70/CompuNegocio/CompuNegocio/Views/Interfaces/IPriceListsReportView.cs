using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IPriceListsReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;
        event Action ReportTypeChanged;
        event Action Load;
        event Action OpenClassificationsList;
        event Action FindClassification;

        VMRListaDePrecios Report { get; }

        void Show(VMRListaDePrecios report);
        void FillCombos(List<Moneda> currencies);
    }
}
