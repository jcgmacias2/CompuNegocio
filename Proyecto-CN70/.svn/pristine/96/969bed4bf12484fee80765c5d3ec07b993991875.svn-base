using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IPurchaseOrdersView : IBaseView
    {
        event Action FindProvider;
        event Action OpenProvidersList;
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
        event Action Print;
        event Action Save;
        event Action OpenNote;
        event Action Update;

        VMOrdenDeCompra Order { get; }
        bool IsDirty { get; }
        VMDetalleDeOrdenDeCompra CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMDetalleDeOrdenDeCompra SelectedItem { get; }

        void Show(VMOrdenDeCompra order);
        void Show(VMDetalleDeOrdenDeCompra  detail, List<UnidadesDeMedida> unitOfMeasure);
        void Show(VMImpuesto tax);
        void ShowStock(decimal stock);
        void ClearItem();
        void FillCombos(List<Moneda> currencies);
    }
}
