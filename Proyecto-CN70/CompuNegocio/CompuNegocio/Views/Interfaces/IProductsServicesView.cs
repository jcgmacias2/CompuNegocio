using Aprovi.Data.Models;
using System;

namespace Aprovi.Views
{
    public interface IProductsServicesView : IBaseView
    {
        event Action Find;
        event Action New;
        event Action Delete;
        event Action Save;
        event Action Update;
        event Action OpenList;
        event Action Quit;

        ProductosServicio ProductService { get; }
        bool IsDirty { get; }

        void Clear();
        void Show(ProductosServicio productService);
    }
}
