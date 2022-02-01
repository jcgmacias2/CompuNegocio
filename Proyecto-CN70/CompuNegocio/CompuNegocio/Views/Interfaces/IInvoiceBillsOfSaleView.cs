using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IInvoiceBillsOfSaleView : IBaseView
    {
        event Action Save;
        event Action Quit;
        event Action Return;
        event Action OpenListCustomers;
        event Action ChangeCurrency;
        event Action FindCustomer;
        event Action FindUser;
        event Action OpenUsersList;

        VMFactura Invoice { get; }

        void Show(List<VMRemision> billsOfSale);
        void Show(VMFactura invoice);
        void Show(Cliente customer);
        void Show(Usuario seller);
        void SetCurrency(Moneda currency);
        void SetExchangeRate(decimal rate);
        void FillCombos(List<Moneda> currencies, List<Regimene> regimenes, List<MetodosPago> paymentMethods, List<UsosCFDI> cfdiUsages);
    }
}
