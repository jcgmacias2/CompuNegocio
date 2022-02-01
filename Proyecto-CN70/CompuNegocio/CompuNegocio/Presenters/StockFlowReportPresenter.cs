using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class StockFlowReportPresenter
    {
        private IStockFlowReportView _view;
        private IArticuloService _items;
        private IClasificacionService _classifications;

        private static BackgroundWorker bworker;

        public StockFlowReportPresenter(IStockFlowReportView view, IArticuloService items, IClasificacionService classifications)
        {
            _view = view;
            _items = items;
            _classifications = classifications;

            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
            _view.FindItem += FindItem;
            _view.OpenItemsList += OpenItemsList;
            _view.FindClassification += FindClassification;
            _view.OpenClassificationsList += OpenClassificationsList;
        }

        private void OpenClassificationsList()
        {
            try
            {
                IClassificationsListView view;
                ClassificationsListPresenter presenter;

                view = new ClassificationsListView();
                presenter = new ClassificationsListPresenter(view, _classifications);

                view.ShowWindow();

                if (view.Classification.isValid() && view.Classification.idClasificacion.isValid())
                    _view.Show(view.Classification);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClassification()
        {
            if (!_view.Classification.isValid())
                return;

            if (!_view.Classification.descripcion.isValid())
                return;

            try
            {
                var classification = _classifications.Find(_view.Classification.descripcion);

                if (!classification.isValid())
                    return;

                _view.Show(classification);

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

                //Si seleccionó uno lo muestro
                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    _view.Show(item);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.Item.isValid() || !_view.Item.codigo.isValid())
                return;

            try
            {
                var item = _items.Find(_view.Item.codigo);

                if (item.isValid() && !item.activo)
                    return;

                _view.Show(item);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var itemsStockFlow = new List<VMFlujoPorArticulo>();

                //bworker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                //bworker.DoWork += DoWork;
                //bworker.ProgressChanged += ProgressChanged;
                //bworker.RunWorkerCompleted += Completed;
                //bworker.RunWorkerAsync();

                switch (_view.Filtro)
                {
                    case ReporteDeFlujoPor.TodosLosArticulos:
                        itemsStockFlow = _items.StockFlow(_view.Start, _view.End);
                        break;
                    case ReporteDeFlujoPor.PorUnArticulo:
                        if (!_view.Item.idArticulo.isValid())
                            throw new Exception("Debe seleccionar el artículo a reportear");
                        itemsStockFlow = new List<VMFlujoPorArticulo>();
                        itemsStockFlow.Add(_items.StockFlow(_view.Start, _view.End, _view.Item));
                        break;
                    case ReporteDeFlujoPor.PorClasificacion:
                        if (!_view.Classification.idClasificacion.isValid())
                            throw new Exception("Debe seleccionar la clasificación a reportear");
                        itemsStockFlow = _items.StockFlow(_view.Start, _view.End, _view.Classification);
                        break;
                }

                view = new ReportViewerView(Reports.FillReport(itemsStockFlow, _view.Start, _view.End));
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
            if (_view.End < _view.Start)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var itemsStockFlow = new List<VMFlujoPorArticulo>();

                switch (_view.Filtro)
                {
                    case ReporteDeFlujoPor.TodosLosArticulos:
                        itemsStockFlow = _items.StockFlow(_view.Start, _view.End);
                        break;
                    case ReporteDeFlujoPor.PorUnArticulo:
                        if (!_view.Item.idArticulo.isValid())
                            throw new Exception("Debe seleccionar el artículo a reportear");
                        itemsStockFlow = new List<VMFlujoPorArticulo>();
                        itemsStockFlow.Add(_items.StockFlow(_view.Start, _view.End, _view.Item));
                        break;
                    case ReporteDeFlujoPor.PorClasificacion:
                        if (!_view.Classification.idClasificacion.isValid())
                            throw new Exception("Debe seleccionar la clasificación a reportear");
                        itemsStockFlow = _items.StockFlow(_view.Start, _view.End, _view.Classification);
                        break;
                }

                view = new ReportViewerView(Reports.FillReport(itemsStockFlow, _view.Start, _view.End));
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
    }
}
