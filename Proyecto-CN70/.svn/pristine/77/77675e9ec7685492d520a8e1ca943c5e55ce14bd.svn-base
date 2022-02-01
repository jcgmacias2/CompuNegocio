using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IBillsOfSalePerPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Preview;
        event Action Print;

        DateTime StartDate { get; }
        DateTime EndDate { get; }
        Tipos_Reporte_Remisiones BillOfSaleType { get; }

        void FillCombos(List<object> billOfSaleTypes);

        void SetDates(DateTime startDate, DateTime endDate);
    }
}
