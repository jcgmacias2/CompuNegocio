using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface ISalesPerPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;

        DateTime StartDate { get; }
        DateTime EndDate { get; }
        bool IncludeBillsOfSale { get; }

        void SetDates(DateTime startDate, DateTime endDate);
    }
}
