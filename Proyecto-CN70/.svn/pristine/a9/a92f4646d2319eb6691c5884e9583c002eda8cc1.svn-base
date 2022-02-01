using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IPaymentsByPeriodReportView : IBaseView
    {
        event Action Quit;
        event Action Print;
        event Action Preview;

        Empresa Business { get; }
        DateTime Start { get; }
        DateTime End { get; }

        void FillCombo(List<Empresa> registers);
    }
}
