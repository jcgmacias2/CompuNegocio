using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Views
{
    public interface IFiscalPaymentPrintView : IBaseView
    {
        event Action FindLast;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Preview;
        event Action Print;

        AbonosDeFactura Payment { get; }

        void Show(AbonosDeFactura payment);
    }
}
