using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IFiscalPaymentsView : IBaseView
    {
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action Cancel;

        AbonosDeFactura FiscalPayment { get; }
        bool IsDirty { get; }

        void Show(AbonosDeFactura payment);
        void Clear();
    }
}
