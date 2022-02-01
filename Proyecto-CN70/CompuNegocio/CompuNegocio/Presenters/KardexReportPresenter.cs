using Aprovi.Application.Helpers;
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
    public class KardexReportPresenter
    {
        IKardexReportView _view;
        IArticuloService _items;

        public KardexReportPresenter(IKardexReportView view, IArticuloService items)
        {
            _view = view;
            _items = items;

            _view.Preview += Preview;
            _view.Print += Print;
            _view.Quit += Quit;
            _view.OpenItemsList += OpenItemsList;
            _view.FindItem += FindItem;
        }

        private void Preview()
        {
            if (!_view.Item.idArticulo.isValid())
            {
                _view.ShowError("Debe seleccionar el artículo sobre el cual desea obtener la información");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var kardex = _items.StockFlow(_view.Item, _view.Start, _view.End);

                view = new ReportViewerView(Reports.FillReport(kardex, _view.Start, _view.End, _view.Item));
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Print()
        {
            if (!_view.Item.idArticulo.isValid())
            {
                _view.ShowError("Debe seleccionar el artículo sobre el cual desea obtener la información");
                return;
            }

            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var kardex = _items.StockFlow(_view.Item, _view.Start, _view.End);

                view = new ReportViewerView(Reports.FillReport(kardex, _view.Start, _view.End, _view.Item));
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
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

        private void OpenItemsList()
        {
            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    _view.Show(item);
                }
                else
                    _view.Show(new Articulo());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            try
            {
                var item = new Articulo();

                //Si el código es válido intento buscarlo
                if (_view.Item.codigo.isValid())
                    item = _items.Find(_view.Item.codigo);

                if (item.isValid())
                    _view.Show(item);
                else
                    _view.Show(new Articulo());

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
