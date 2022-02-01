using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using Aprovi.Business.ViewModels;

namespace Aprovi.Views
{
    public interface IItemsView : IBaseView, IItemsCodesForSuppliersView, IItemsCodesForCustomersView, IItemsTaxesView, IItemsClassificationsView
    {
        event Action Quit;
        event Action New;
        event Action Find;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action OpenList;
        event Action OpenEquivalencies;
        event Action CalculateByUtility;
        event Action FindProductService;
        event Action OpenProductServiceList;
        event Action ChangeCurrency;

        VMArticulo Item { get; }
        bool IsDirty { get; }
        Moneda LastCurrency { get; }

        void UpdatePrices(VMArticulo item);
        void Show(Moneda currency);
        void Show(VMArticulo item);
        void Show(ProductosServicio productService);
        void Clear();
        void FillCombos(List<Moneda> currencies, List<UnidadesDeMedida> unitOfMeasure, List<TiposDeComision> comissionTypes);
    }
}
