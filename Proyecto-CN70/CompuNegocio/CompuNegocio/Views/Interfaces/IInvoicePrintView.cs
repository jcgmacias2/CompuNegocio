using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IInvoicePrintView : IBaseView
    {
        event Action FindLast;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Preview;
        event Action Print;

        VMFactura Invoice { get; }

        void Show(VMFactura invoice);
    }
}
