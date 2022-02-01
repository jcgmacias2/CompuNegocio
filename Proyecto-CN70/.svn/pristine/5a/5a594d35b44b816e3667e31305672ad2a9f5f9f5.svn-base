using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Views
{
    public interface IProductsServicesListView : IBaseListView, IBaseListPresenter
    {
        event Action Select;
        event Action Search;

        ProductosServicio ProductService { get; }

        void Show(List<ProductosServicio> productService);
    }
}
