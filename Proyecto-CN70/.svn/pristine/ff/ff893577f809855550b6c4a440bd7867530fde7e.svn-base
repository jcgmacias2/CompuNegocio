using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IClientStatementReportView : IBaseView
    {
        event Action FindClient;
        event Action OpenClientsList;
        event Action Quit;
        event Action Print;
        event Action Preview;

        Cliente Client { get; }
        DateTime Start { get; }
        DateTime End { get; }
        bool OnlyPendingBalance { get; }

        void Show(Cliente client);
    }
}
