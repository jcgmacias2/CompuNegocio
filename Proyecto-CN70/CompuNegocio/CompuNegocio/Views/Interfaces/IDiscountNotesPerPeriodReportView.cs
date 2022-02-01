using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IDiscountNotesPerPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenCustomersList;
        event Action FindCustomer;

        VMReporteNotasDeDescuento Report { get; }

        void Show(VMReporteNotasDeDescuento vm);
    }
}
