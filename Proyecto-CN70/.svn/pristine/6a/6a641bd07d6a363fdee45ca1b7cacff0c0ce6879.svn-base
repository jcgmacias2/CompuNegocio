using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IBillOfSaleToInvoiceView : IBaseView
    {
        event Action Quit;
        event Action Save;

        VMFactura Invoice { get; }
        VMRemision BillOfSale { get; }

        void Show(VMRemision billOfSale, List<CuentasBancaria> accounts);
        void Fill(List<MetodosPago> paymentMethod, List<UsosCFDI> uses, List<Regimene> regimens);
    }
}
