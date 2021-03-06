using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;

namespace Aprovi.Presenters
{
    public class SalesPerItemReportPresenter
    {
        private ISalesPerItemReportView _view;
        private IVentasPorArticuloService _salesPerItem;
        private IArticuloService _items;
        private IClasificacionService _classifications;

        public SalesPerItemReportPresenter(ISalesPerItemReportView view,IArticuloService items, IClasificacionService classifications, IVentasPorArticuloService salesPerItem)
        {
            _view = view;
            _salesPerItem = salesPerItem;
            _items = items;
            _classifications = classifications;
            _salesPerItem = salesPerItem;

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
                {
                    var vm = _view.Report;

                    vm.Classification = view.Classification;

                    _view.Show(vm);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClassification()
        {
            var vm = _view.Report;

            if (!vm.Classification.isValid() && !vm.Classification.descripcion.isValid())
            {
                vm.Classification = new Clasificacione();
            }

            try
            {
                var classification = _classifications.Find(vm.Classification.descripcion);

                if (!classification.isValid())
                {
                    vm.Classification = new Clasificacione();
                    _view.ShowError("No se encontró una clasificación con el valor proporcionado");
                }
                else
                {
                    vm.Classification = classification;
                }

                _view.Show(vm);
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
                    var vm = _view.Report;

                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    vm.Item = item;

                    _view.Show(vm);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            var vm = _view.Report;

            if (!vm.Item.isValid() || !vm.Item.codigo.isValid())
            {
                vm.Item = new Articulo();
            }

            try
            {
                var item = _items.Find(vm.Item.codigo);

                if (item.isValid() && !item.activo)
                {
                    vm.Item = item;
                }
                else
                {
                    vm.Item = new Articulo();
                    _view.ShowError("No se encontró un artículo con el valor proporcionado");
                }

                _view.Show(vm);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            var vm = _view.Report;

            if (vm.EndDate < vm.StartDate)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                List<VMRDetalleVentasPorArticulo> items;

                if (vm.ReportType == TiposDeReporteVentasPorArticulo.Detallado || vm.ReportType ==
                    TiposDeReporteVentasPorArticulo.Detallado_Con_Datos_Del_Cliente || vm.IncludePorcentages)
                {
                    items = _salesPerItem.ListDetailed(vm.Item, vm.Classification, vm.StartDate, vm.EndDate, vm.IncludeInvoices, vm.IncludeBillsOfSale, vm.IncludeCancellations);
                }
                else
                {
                    items = _salesPerItem.ListTotals(vm.Item, vm.Classification, vm.StartDate, vm.EndDate, vm.IncludeInvoices, vm.IncludeBillsOfSale, vm.IncludeCancellations);
                }

                VMReporte report = Reports.FillReport(items, vm.StartDate, vm.EndDate, vm.ReportType, vm.IncludePorcentages);

                view = new ReportViewerView(report);
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
            try
            { 

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
