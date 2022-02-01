using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IOrdersView : IBaseView
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
        event Action Print;
        event Action Save;
        event Action OpenNote;
        event Action Update;
        event Action ChangeCurrency;
        event Action OpenUsersList;
        event Action FindUser;

        VMPedido Order { get; }
        bool IsDirty { get; }
        VMDetalleDePedido CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMDetalleDePedido SelectedItem { get; }
        Opciones_Pedido SelectedOption { get; }
        Moneda LastCurrency { get; }

        void Show(VMPedido order);
        void Show(VMDetalleDePedido detail);
        void Show(VMImpuesto tax);
        void Show(Moneda currency);
        void Show(Usuario seller);
        void ShowStock(decimal stock);
        void ClearItem();
        void FillCombos(List<Moneda> currencies, List<Opciones_Pedido> transactions);
    }
}
