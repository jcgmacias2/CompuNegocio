using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IClientPaymentsView : IBaseView
    {
        event Action Load;
        event Action FindClient;
        event Action OpenClientsList;
        event Action GetFolio;
        event Action Find;
        event Action OpenList;
        event Action SelectInvoice;
        event Action AddPayment;
        event Action Quit;
        event Action New;
        event Action Cancel;
        event Action Print;
        event Action Save;
        event Action Stamp;
        event Action ValidatePayment;

        VMPagoMultiple Payment { get; }
        bool IsDirty { get; }
        VMFacturaConSaldo Selected { get; }
        VMFacturaConSaldo Current { get; }

        void Show(VMPagoMultiple payment);
        void Show(VMFacturaConSaldo selected);
        void Fill(List<Moneda> currencies, List<FormasPago> paymentMethods, List<CuentasBancaria> bankAccounts, List<Regimene> regimenes);
        void ClearPayment();
        void Clear();
    }
}
