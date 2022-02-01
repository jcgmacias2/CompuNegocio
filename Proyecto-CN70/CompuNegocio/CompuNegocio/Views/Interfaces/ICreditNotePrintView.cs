using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface ICreditNotePrintView : IBaseView
    {
        event Action FindLast;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Preview;
        event Action Print;

        NotasDeCredito CreditNote { get; }

        void Show(NotasDeCredito creditNote);
    }
}
