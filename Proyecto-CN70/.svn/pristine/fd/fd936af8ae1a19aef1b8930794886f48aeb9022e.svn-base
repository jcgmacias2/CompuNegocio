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
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PriceListsReportPresenter
    {
        private IPriceListsReportView _view;
        private IListasDePreciosService _pricelists;
        private ICatalogosEstaticosService _catalogs;
        private IClasificacionService _classifications;

        public PriceListsReportPresenter(IPriceListsReportView view, IListasDePreciosService priceLists, ICatalogosEstaticosService catalogs, IClasificacionService classifications)
        {
            _view = view;
            _pricelists = priceLists;
            _catalogs = catalogs;
            _classifications = classifications;

            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
            _view.ReportTypeChanged += ReportTypeChanged;
            _view.Load += Load;
            _view.OpenClassificationsList += OpenClassificationsList;
            _view.FindClassification += FindClassification;

            _view.FillCombos(_catalogs.ListMonedas());
        }

        private void FindClassification()
        {
            try
            {
                var vm = _view.Report;

                    vm.Clasificacion = _classifications.Find(vm.Clasificacion.descripcion);

                    if (!vm.Clasificacion.isValid())
                    {
                        vm.Clasificacion = new Clasificacione();
                        _view.Show(vm);
                        _view.ShowError("No se encontró una clasificación con el código proporcionado");
                        return;
                    }

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenClassificationsList()
        {
            try
            {
                var vm = _view.Report;

                IClassificationsListView view;
                ClassificationsListPresenter presenter;

                view = new ClassificationsListView();
                presenter = new ClassificationsListPresenter(view, _classifications);

                view.ShowWindow();

                var classification = view.Classification;

                if (!classification.isValid() || !classification.idClasificacion.isValid())
                {
                    return;
                }

                vm.Clasificacion = classification;

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Load()
        {
            try
            {
                var vm = new VMRListaDePrecios()
                {
                    TipoDeCambio = Session.Configuration.tipoDeCambio,
                    Clasificacion = new Clasificacione()
                };

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ReportTypeChanged()
        {
            try
            {
                var vm = _view.Report;

                if (!vm.FilterType.Equals(TiposFiltroReporteListaDePrecios.Clasificacion))
                {
                    vm.Clasificacion = new Clasificacione();
                }

                _view.Show(vm);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Preview()
        {
            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var vm = _view.Report;

                var report = _pricelists.List(new ListasDePrecio(){idListaDePrecio = (int)vm.ReportType}, vm.Clasificacion, vm.SoloConInventario, vm.IncluirNoInventariados, vm.IncluirImpuestos, vm.Moneda, vm.TipoDeCambio);

                report.ReportType = vm.ReportType;

                view = new ReportViewerView(Reports.FillReport(report));
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
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var vm = _view.Report;

                var report = _pricelists.List(new ListasDePrecio() { idListaDePrecio = (int)vm.ReportType }, vm.Clasificacion, vm.SoloConInventario, vm.IncluirNoInventariados, vm.IncluirImpuestos, vm.Moneda, vm.TipoDeCambio);

                view = new ReportViewerView(Reports.FillReport(report));
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
