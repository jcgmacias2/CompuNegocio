using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPurchasesByPeriodReportView : IBaseView
    {
        event Action FindSupplier;
        event Action OpenSuppliersList;
        event Action Quit;
        event Action Print;
        event Action Preview;

        Proveedore Supplier { get; }
        DateTime Start { get; }
        DateTime End { get; }
        bool Detailed { get; }

        void Show(Proveedore supplier);
    }
}
