using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using System;
using System.Collections.Generic;

namespace Aprovi.Presenters
{
    public class ProductsServicesListPresenter : BaseListPresenter
    {
        private readonly IProductsServicesListView _view;
        private IProductoServicioService _productsServices;

        public ProductsServicesListPresenter(IProductsServicesListView view, IProductoServicioService productsServices) : base(view)
        {
            _view = view;
            _productsServices = productsServices;

            _view.Search += Search;

            // Estos eventos estan implementados en la clase base BaseListPresenter
            _view.Select += Select;
            _view.Quit += Quit;
            _view.GoFirst += GoFirst;
            _view.GoPrevious += GoPrevious;
            _view.GoNext += GoNext;
            _view.GoLast += GoLast;
        }

        private void Search()
        {
            List<ProductosServicio> productsServices;

            try
            {
                if (_view.Parameter.isValid())
                    productsServices = _productsServices.List(_view.Parameter);
                else
                    productsServices = _productsServices.List();

                _view.Show(productsServices);

                if (productsServices.Count > 0)
                    _view.GoToRecord(0);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
