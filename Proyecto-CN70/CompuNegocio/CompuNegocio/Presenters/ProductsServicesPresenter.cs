using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class ProductsServicesPresenter
    {
        private IProductsServicesView _view;
        private IProductoServicioService _productsServices;

        public ProductsServicesPresenter(IProductsServicesView view, IProductoServicioService productServices)
        {
            _view = view;
            _productsServices = productServices;
            _view.Find += Find;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
        }

        private void Quit()
        {
            try
            {
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenList()
        {
            try
            {
                IProductsServicesListView view;
                ProductsServicesListPresenter presenter;

                view = new ProductsServicesListView();
                presenter = new ProductsServicesListPresenter(view, _productsServices);

                view.ShowWindow();

                if (view.ProductService.isValid() && view.ProductService.idProductoServicio.isValid())
                    _view.Show(view.ProductService);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            if (!_view.ProductService.codigo.isValid())
            {
                _view.ShowMessage("Debe capturar el código del producto o servicio");
                return;
            }

            if (!_view.ProductService.descripcion.isValid())
            {
                _view.ShowMessage("Debe capturar la descripción del producto o servicio");
                return;
            }

            try
            {
                _productsServices.Update(_view.ProductService);
                _view.ShowMessage(string.Format("Producto o servicio {0} actualizado exitosamente", _view.ProductService.descripcion));
                _view.Clear();

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            if (!_view.ProductService.codigo.isValid())
            {
                _view.ShowMessage("Debe capturar el código del producto o servicio");
                return;
            }

            if (!_view.ProductService.descripcion.isValid())
            {
                _view.ShowMessage("Debe capturar la descripción del producto o servicio");
                return;
            }

            try
            {
                _productsServices.Add(_view.ProductService);
                _view.ShowMessage(string.Format("Producto o servicio {0} agregada exitosamente", _view.ProductService.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if (!_view.IsDirty)
            {
                _view.ShowError("No existe producto o servicio seleccionado para eliminar");
                return;
            }

            try
            {

                    _productsServices.Remove(_view.ProductService.idProductoServicio);


                _view.ShowMessage(string.Format("Producto o servicio {0} removida exitosamente", _view.ProductService.descripcion));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void New()
        {
            try
            {
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.ProductService.isValid())
                return;

            if (!_view.ProductService.codigo.isValid())
                return;

            try
            {
                var unit = _productsServices.Find(_view.ProductService.codigo);

                if (unit == null)
                    unit = new ProductosServicio() { codigo = _view.ProductService.codigo };

                _view.Show(unit);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
