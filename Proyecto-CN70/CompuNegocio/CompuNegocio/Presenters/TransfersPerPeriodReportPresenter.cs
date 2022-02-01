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
    public class TransfersByPeriodReportPresenter
    {
        private ITransfersByPeriodReportView _view;
        private ITraspasoService _transfers;
        private IEmpresaAsociadaService _associatedCompanies;
        private IArticuloService _items;

        public TransfersByPeriodReportPresenter(ITransfersByPeriodReportView view, ITraspasoService transfers, IEmpresaAsociadaService associatedCompanies, IArticuloService items)
        {
            _view = view;
            _transfers = transfers;
            _associatedCompanies = associatedCompanies;
            _items = items;

            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
            _view.OpenTransfersList += OpenTransfersList;
            _view.OpenDestinationAssociatedCompaniesList += OpenDestinationAssociatedCompaniesList;
            _view.OpenOriginAssociatedCompaniesList += OpenOriginAssociatedCompaniesList;
            _view.FindTransfer += FindTransfer;
            _view.FindOriginAssociatedCompany += FindOriginAssociatedcompany;
            _view.FindDestinationAssociatedCompany += FindDestinationAssociatedCompany;
        }

        private void FindDestinationAssociatedCompany()
        {
            try
            {
                var reportVM = _view.Report;

                if (reportVM.DestinationCompany.isValid() && reportVM.DestinationCompany.nombre.isValid())
                {
                    var associatedCompany = _associatedCompanies.Find(reportVM.DestinationCompany.nombre);

                    if (associatedCompany.isValid())
                    {
                        reportVM.DestinationCompany = associatedCompany;
                        _view.Show(reportVM);
                    }
                    else
                    {
                        //Se limpia la propiedad de empresa destino
                        reportVM.DestinationCompany = new EmpresasAsociada();
                        _view.Show(reportVM);
                        _view.ShowError("No se encontró una empresa asociada con el valor proporcionado");
                    }
                }
                else
                {
                    //Se limpia la propiedad de empresa destino
                    reportVM.DestinationCompany = new EmpresasAsociada();
                    _view.Show(reportVM);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindOriginAssociatedcompany()
        {
            try
            {
                var reportVM = _view.Report;

                if (reportVM.OriginCompany.isValid() && reportVM.OriginCompany.nombre.isValid())
                {
                    var associatedCompany = _associatedCompanies.Find(reportVM.OriginCompany.nombre);

                    if (associatedCompany.isValid())
                    {
                        reportVM.OriginCompany = associatedCompany;
                        _view.Show(reportVM);
                    }
                    else
                    {
                        //Se limpia la propiedad de empresa destino
                        reportVM.OriginCompany = new EmpresasAsociada();
                        _view.Show(reportVM);
                        _view.ShowError("No se encontró una empresa asociada con el valor proporcionado");
                    }
                }
                else
                {
                    //Se limpia la propiedad de empresa destino
                    reportVM.OriginCompany = new EmpresasAsociada();
                    _view.Show(reportVM);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindTransfer()
        {
            try
            {
                var reportVM = _view.Report;

                if (reportVM.Transfer.isValid() && reportVM.Transfer.folio.isValid())
                {
                    var transfer = _transfers.FindByFolio(reportVM.Transfer.folio);

                    if (transfer.isValid())
                    {
                        reportVM.Transfer = transfer;
                        _view.Show(reportVM);
                    }
                    else
                    {
                        //Se limpia la propiedad de empresa destino
                        reportVM.Transfer = new Traspaso();
                        _view.Show(reportVM);
                        _view.ShowError("No se encontró un traspaso con el valor proporcionado");
                    }
                }
                else
                {
                    //Se limpia la propiedad de empresa destino
                    reportVM.Transfer = new Traspaso();
                    _view.Show(reportVM);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenOriginAssociatedCompaniesList()
        {
            try
            {
                IAssociatedCompaniesListView view;
                AssociatedCompaniesListPresenter presenter;

                view = new AssociatedCompaniesListView();
                presenter = new AssociatedCompaniesListPresenter(view, _associatedCompanies);

                view.ShowWindow();

                if (view.AssociatedCompany.isValid() && view.AssociatedCompany.idEmpresaAsociada.isValid())
                {
                    var vm = _view.Report;

                    vm.OriginCompany = view.AssociatedCompany;

                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenDestinationAssociatedCompaniesList()
        {
            try
            {
                IAssociatedCompaniesListView view;
                AssociatedCompaniesListPresenter presenter;

                view = new AssociatedCompaniesListView();
                presenter = new AssociatedCompaniesListPresenter(view, _associatedCompanies);

                view.ShowWindow();

                if (view.AssociatedCompany.isValid() && view.AssociatedCompany.idEmpresaAsociada.isValid())
                {
                    var vm = _view.Report;

                    vm.DestinationCompany = view.AssociatedCompany;

                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenTransfersList()
        {
            try
            {
                ITransfersListView view;
                TransfersListPresenter presenter;

                view = new TransfersListView();
                presenter = new TransfersListPresenter(view, _transfers);

                view.ShowWindow();

                if (view.Transfer.isValid() && view.Transfer.idTraspaso.isValid())
                {
                    var vm = _view.Report;

                    vm.Transfer = view.Transfer;

                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }


        private void Preview()
        {
            var reportVM = _view.Report;
            
            if (reportVM.EndDate < reportVM.StartDate)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                switch (reportVM.ReportType)
                {
                    case TiposDeReporteTraspasos.Traspaso:
                        if (!reportVM.Transfer.isValid() || !reportVM.Transfer.idTraspaso.isValid())
                        {
                            _view.ShowError("No hay traspaso seleccionado para visualizar");
                            return;
                        }

                        report = Reports.FillReport(new VMRTraspaso(new VMTraspaso(_transfers.FindById(reportVM.Transfer.idTraspaso))));
                        break;
                    case TiposDeReporteTraspasos.Todos:
                        //Se verifica que la empresa asociada origen o destino sea la local
                        EmpresasAsociada originCompany = null;
                        EmpresasAsociada destinationCompany = null;

                        if (reportVM.OriginCompany.isValid() && reportVM.OriginCompany.idEmpresaAsociada.isValid())
                        {
                            originCompany = _associatedCompanies.Find(reportVM.OriginCompany.idEmpresaAsociada);
                        }

                        if (reportVM.DestinationCompany.isValid() &&
                            reportVM.DestinationCompany.idEmpresaAsociada.isValid())
                        {
                            destinationCompany =
                                _associatedCompanies.Find(reportVM.DestinationCompany.idEmpresaAsociada);
                        }

                        //Se valida que la empresa asociada actual se seleccione en alguno de los 2 filtros, solo si se proporcionaron
                        if (!(!(originCompany.isValid() || destinationCompany.isValid()) || ((originCompany.isValid() && originCompany.idEmpresaLocal.GetValueOrDefault(0).isValid()) || (destinationCompany.isValid() && destinationCompany.idEmpresaLocal.GetValueOrDefault(0).isValid()))))
                        {
                            _view.ShowError("La empresa asociada actual debe seleccionarse en la empresa asociada orígen o destino");
                            return;
                        }

                        var transactions = _transfers.ListForReport(reportVM.StartDate, reportVM.EndDate, reportVM.OriginCompany, reportVM.DestinationCompany);

                        //Si no existe ningun traspaso, se muestra mensaje
                        if (transactions.Count == 0)
                        {
                            _view.ShowError("No existen traspasos en el período seleccionado");
                            return;
                        }

                        report = Reports.FillReport(transactions, reportVM.StartDate, reportVM.EndDate);

                        break;
                }

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
            var reportVM = _view.Report;

            if (reportVM.EndDate < reportVM.StartDate)
            {
                _view.ShowError("La fecha final no puede ser inferior a la fecha inicial");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                VMReporte report = null;

                switch (reportVM.ReportType)
                {
                    case TiposDeReporteTraspasos.Traspaso:
                        if (!reportVM.Transfer.isValid() || !reportVM.Transfer.idTraspaso.isValid())
                        {
                            _view.ShowError("No hay traspaso seleccionado para visualizar");
                            return;
                        }

                        report = Reports.FillReport(new VMRTraspaso(new VMTraspaso(_transfers.FindById(reportVM.Transfer.idTraspaso))));
                        break;
                    case TiposDeReporteTraspasos.Todos:
                        //Se verifica que la empresa asociada origen o destino sea la local
                        EmpresasAsociada originCompany = null;
                        EmpresasAsociada destinationCompany = null;

                        if (reportVM.OriginCompany.isValid() && reportVM.OriginCompany.idEmpresaAsociada.isValid())
                        {
                            originCompany = _associatedCompanies.Find(reportVM.OriginCompany.idEmpresaAsociada);
                        }

                        if (reportVM.DestinationCompany.isValid() &&
                            reportVM.DestinationCompany.idEmpresaAsociada.isValid())
                        {
                            destinationCompany =
                                _associatedCompanies.Find(reportVM.DestinationCompany.idEmpresaAsociada);
                        }

                        //Se valida que la empresa asociada actual se seleccione en alguno de los 2 filtros, solo si se proporcionaron
                        if (!(!(originCompany.isValid() || destinationCompany.isValid()) || ((originCompany.isValid() && originCompany.idEmpresaLocal.GetValueOrDefault(0).isValid()) || (destinationCompany.isValid() && destinationCompany.idEmpresaLocal.GetValueOrDefault(0).isValid()))))
                        {
                            _view.ShowError("La empresa asociada actual debe seleccionarse en la empresa asociada orígen o destino");
                            return;
                        }

                        var transactions = _transfers.ListForReport(reportVM.StartDate, reportVM.EndDate, reportVM.OriginCompany, reportVM.DestinationCompany);

                        //Si no existe ningun traspaso, se muestra mensaje
                        if (transactions.Count == 0)
                        {
                            _view.ShowError("No existen traspasos en el período seleccionado");
                            return;
                        }

                        report = Reports.FillReport(transactions, reportVM.StartDate, reportVM.EndDate);

                        break;
                }

                view = new ReportViewerView(report);
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
