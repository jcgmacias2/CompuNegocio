using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IPurchasesView : IBaseView
    {
        event Action FindSupplier;
        event Action OpenSuppliersList;
        event Action FindItem;
        event Action OpenItemsList;
        event Action AddItem;
        event Action SelectItem;
        event Action RemoveItem;
        event Action ViewTaxDetails;
        event Action OpenPayments;
        event Action Find;
        event Action OpenList;
        event Action Quit;
        event Action New;
        event Action Cancel;
        event Action Save;
        event Action Print;
        event Action OpenPurchaseOrdersList;
        event Action FindPurchaseOrder;
        event Action UpdateCharges;
        event Action ImportCFDI;

        VMCompra Purchase { get; }
        DetallesDeCompra CurrentItem { get; }
        bool IsDirty { get; }

        DetallesDeCompra SelectedItem { get; }
        VMImpuesto SelectedTax { get; }
        VMOrdenDeCompra PurchaseOrder { get; }

        void Show(VMCompra purchase);
        void Show(VMOrdenDeCompra purchaseOrder, List<DetallesDeCompra> details);
        void Clear();
        void Show(DetallesDeCompra currentItem, List<UnidadesDeMedida> unitsOfMeasure);
        void ClearItem();
        void Show(VMImpuesto tax);
        void FillMonedas(List<Moneda> currencies);
    }
}
