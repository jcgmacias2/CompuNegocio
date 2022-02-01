using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IAdjustmentsReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;

        TiposDeAjuste Type { get; }
        DateTime Start { get; }
        DateTime End { get; }

        void Fill(List<TiposDeAjuste> types);
    }
}
