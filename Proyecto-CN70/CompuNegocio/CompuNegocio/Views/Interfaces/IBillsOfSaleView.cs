using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IBillsOfSaleView : IBaseView, IBillsOfSalePaymentsView
    {
        event Action FindClient;
        event Action OpenClientsList;
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
        event Action Cancel;
        event Action New;
        event Action OpenBillOfSaleToInvoice;
        event Action Print;
        event Action Save;
        event Action OpenNote;
        event Action OpenQuotesList;
        event Action ChangeCurrency;
        event Action OpenUsersList;
        event Action FindUser;
        event Action Update;

        VMRemision BillOfSale { get; }
        bool IsDirty { get; }
        VMDetalleDeRemision CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMDetalleDeRemision SelectedItem { get; }
        Moneda LastCurrency { get; }

        void Show(VMRemision billOfSale);
        void Show(VMDetalleDeRemision detail);
        void Show(VMImpuesto tax);
        void Show(Moneda currency);
        void Show(Usuario seller);
        void ShowStock(decimal stock);
        void DisableSaveButton();
        void ClearItem();
        void FillCombos(List<Moneda> currencies, List<FormasPago> paymentForms);
    }
}
