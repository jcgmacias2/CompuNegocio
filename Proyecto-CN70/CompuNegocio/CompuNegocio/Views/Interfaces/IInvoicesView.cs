using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;


namespace Aprovi.Views
{
    public interface IInvoicesView : IBaseView, IInvoicePaymentsView, IRelatedDocuments
    {
        event Action FindClient;
        event Action OpenClientsList;
        event Action GetFolio;
        event Action Find;
        event Action OpenList;
        event Action Load;
        event Action FindItem;
        event Action OpenItemsList;
        event Action AddItem;
        event Action AddItemComment;
        event Action ViewTaxDetails;
        event Action RemoveItem;
        event Action SelectItem;
        event Action Quit;
        event Action New;
        event Action Cancel;
        event Action Print;
        event Action Save;
        event Action Stamp;
        event Action OpenFiscalPaymentReportView;
        event Action OpenNote;
        event Action OpenQuotesList;
        event Action OpenUsersList;
        event Action FindUser;
        event Action ChangeCurrency;
        event Action ToCreditNote;
        event Action AddDisccount;
        event Action RemoveDisccount;

        VMFactura Invoice { get; }
        bool IsDirty { get; }
        VMDetalleDeFactura CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMNotaDeCredito SelectedCreditNote { get; }
        VMDetalleDeFactura SelectedItem { get; }
        Moneda LastCurrency { get; }

        void Show(VMFactura invoice);
        void Show(VMDetalleDeFactura detail);
        void Show(VMImpuesto tax);
        void Show(Usuario seller);
        void Show(Moneda currency);
        void ShowStock(decimal existencia);
        void DisableSaveButton();
        void ClearItem();
        void FillCombos(List<Moneda> currencies, List<MetodosPago> paymentMethods, List<FormasPago> paymentForms, List<UsosCFDI> CFDIUses, List<Regimene> regimes, List<TiposRelacion> relationTypes, List<CuentasBancaria> bankAccounts);
    }
}
