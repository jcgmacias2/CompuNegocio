using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface ICurrencyExchangeView : IBaseView
    {
        event Action Change;
        event Action Quit;

        AbonosDeFactura Payment { get; }
        AbonosDeFactura PaymentExchange { get; }

        void Show(AbonosDeFactura payment);
    }
}
