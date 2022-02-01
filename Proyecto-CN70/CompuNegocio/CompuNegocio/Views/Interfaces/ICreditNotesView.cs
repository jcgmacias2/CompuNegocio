using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;


namespace Aprovi.Views
{
    public interface ICreditNotesView : IBaseView
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
        event Action ViewTaxDetails;
        event Action RemoveItem;
        event Action SelectItem;
        event Action Quit;
        event Action New;
        event Action Cancel;
        event Action Print;
        event Action Save;
        event Action Stamp;
        event Action OpenNote;

        VMNotaDeCredito CreditNote { get; }
        bool IsDirty { get; }
        VMDetalleDeNotaDeCredito CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMDetalleDeNotaDeCredito SelectedItem { get; }

        void Show(VMNotaDeCredito creditNote);
        void Show(VMDetalleDeNotaDeCredito detail);
        void Show(VMImpuesto tax);
        void ClearItem();
        void FillCombos(List<Moneda> currencies, List<FormasPago> paymentForms, List<Regimene> regimes, List<CuentasBancaria> bankAccounts);
    }
}
