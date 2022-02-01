using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ISoldItemsCostReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;

        DateTime Start { get; }
        DateTime End { get; }
        bool IncludeBillsOfSale { get; }
    }
}
