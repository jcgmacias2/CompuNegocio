using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ICreditNotesByPeriodReportView : IBaseView
    {
        event Action FindCustomer;
        event Action OpenCustomersList;
        event Action Quit;
        event Action Print;
        event Action Preview;

        Cliente Customer { get; }
        DateTime Start { get; }
        DateTime End { get; }

        void Show(Cliente customer);
    }
}
