using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Aprovi.Views
{
    public interface IInvoicePaymentsView
    {
        event Action AddPayment;
        event Action RemovePayment;
        event Action StampPayment;
        event Action ValidatePayment;

        AbonosDeFactura Payment { get; }
        AbonosDeFactura Selected { get; }

        void Show(List<AbonosDeFactura> payments);
        void Show(AbonosDeFactura payment);
    }
}
