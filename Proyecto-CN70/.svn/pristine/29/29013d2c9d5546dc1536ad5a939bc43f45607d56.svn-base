using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface ICommissionsPerPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenUsersList;

        DateTime StartDate { get; }
        DateTime EndDate { get; }
        Usuario User { get; }

        void SetDates(DateTime startDate, DateTime endDate);
        void Show(Usuario user);
    }
}
