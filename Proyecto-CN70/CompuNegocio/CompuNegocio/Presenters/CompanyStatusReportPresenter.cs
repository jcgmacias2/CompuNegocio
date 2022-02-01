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
using Aprovi.Business.ViewModels;

namespace Aprovi.Presenters
{
    public class CompanyStatusReportPresenter
    {
        ICompanyStatusReportView _view;
        IArticuloService _items;
        private IEstadoDeLaEmpresaService _companyStatus;

        public CompanyStatusReportPresenter(ICompanyStatusReportView view, IEstadoDeLaEmpresaService companyStatus)
        {
            _view = view;
            _companyStatus = companyStatus;

            _view.Preview += Preview;
            _view.Print += Print;
            _view.Quit += Quit;
            _view.Load += Load;
            _view.FilterChanged += FilterChanged;
        }

        private void FilterChanged()
        {
            try
            {
                if (_view.Report.FechaFin < _view.Report.FechaInicio)
                {
                    _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                    return;
                }

                //Se debe volver a llenar el reporte
                var vm = _view.Report;

                //Se genera uno nuevo
                vm = new VMEstadoDeLaEmpresa()
                {
                    FechaFin = vm.FechaFin,
                    FechaInicio = vm.FechaInicio,
                    IncluirRemisiones = vm.IncluirRemisiones,
                    TipoDeCambio = vm.TipoDeCambio
                };

                //Se obtienen los detalles del reporte
                _companyStatus.List(vm, vm.FechaInicio, vm.FechaFin, vm.IncluirRemisiones);

                //Se muestra el reporte
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
                //Se debe llenar la vista con la fecha de inicio del mes y la fecha final
                DateTime fechaActual = DateTime.Today;

                var defaultReport = new VMEstadoDeLaEmpresa()
                {
                    FechaInicio = new DateTime(fechaActual.Year, fechaActual.Month, 1),
                    FechaFin = fechaActual,
                    IncluirRemisiones = false,
                    TipoDeCambio = Session.Configuration.tipoDeCambio
                };

                _view.Show(defaultReport);

                //Se debe llenar el reporte por defecto
                FilterChanged();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Preview()
        {
            if (_view.Report.FechaFin < _view.Report.FechaInicio)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;


                view = new ReportViewerView(Reports.FillReport(_view.Report));
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
            if (_view.Report.FechaFin < _view.Report.FechaInicio)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(Reports.FillReport(_view.Report));
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
