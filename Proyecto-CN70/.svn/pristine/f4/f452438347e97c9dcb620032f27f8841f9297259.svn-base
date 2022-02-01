using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Views
{
    public interface IQuotesView : IBaseView
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
        event Action Update;
        event Action OpenNote;
        event Action ChangeCurrency;
        event Action Unlink;

        VMCotizacion Quote { get; }
        bool IsDirty { get; }
        VMDetalleDeCotizacion CurrentItem { get; }
        VMImpuesto SelectedTax { get; }
        VMDetalleDeCotizacion SelectedItem { get; }
        Moneda LastCurrency { get; }

        void Show(VMCotizacion cotizacion);
        void Show(VMDetalleDeCotizacion detail);
        void Show(VMImpuesto tax);
        void Show(Moneda currency);
        void ShowStock(decimal stock);
        void ClearItem();
        void FillCombos(List<Moneda> currencies);
    }
}
