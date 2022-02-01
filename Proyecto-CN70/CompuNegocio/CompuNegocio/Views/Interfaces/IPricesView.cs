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
    public interface IPricesView : IBaseView
    {
        #region View

        event Action Find;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action OpenList;
        event Action Quit;

        ListasDePrecio PricesList { get; }
        bool IsDirty { get; }
        void Show(ListasDePrecio pricesList);
        void Clear();

        #endregion

        #region Pestaña Artículos

        event Action FindPriceItem;
        event Action OpenItemsList;
        event Action AddOrUpdatePriceItem;
        event Action DeletePriceItem;
        event Action SelectPriceItem;
        event Action CalculateByPrice;
        event Action CalculateByUtility;
        event Action CalculateByPriceWithTaxes;

        VMPrecio Price { get; }
        VMPrecio CurrentPrice { get; }

        void Show(VMPrecio priceItem);
        void ClearPrice();

        #endregion

        #region Pestaña Clientes

        event Action FindClient;
        event Action OpenClientsList;
        event Action AddClient;
        event Action DeleteClient;

        Cliente Client { get; }
        Cliente CurrentClient { get; }

        void Show(Cliente client);
        void ClearClient();

        #endregion
    }
}
