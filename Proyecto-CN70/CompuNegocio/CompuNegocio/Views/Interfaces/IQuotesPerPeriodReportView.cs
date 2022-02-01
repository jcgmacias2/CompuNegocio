using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IQuotesPerPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;
        event Action OpenCustomersList;
        event Action FindCustomer;

        DateTime StartDate { get; }
        DateTime EndDate { get; }
        Cliente Customer { get; }
        bool OnlySalePending { get; }

        void SetDates(DateTime startDate, DateTime endDate);
        void Show(Cliente customer);
    }
}
