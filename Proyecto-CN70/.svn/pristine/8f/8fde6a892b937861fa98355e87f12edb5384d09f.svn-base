using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Microsoft.Reporting.WinForms;

namespace Aprovi.Presenters
{
    public class CommissionsPerPeriodReportPresenter
    {
        private ICommissionsPerPeriodReportView _view;
        private IRemisionService _billsOfSale;
        private IFacturaService _invoices;
        private IUsuarioService _users;

        public CommissionsPerPeriodReportPresenter(ICommissionsPerPeriodReportView view, IRemisionService billsOfSale, IFacturaService invoices, IUsuarioService users)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _invoices = invoices;
            _users = users;

            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            _view.OpenUsersList += OpenUsersList;

            DateTime todayDate = DateTime.Today;
            DateTime currentMonth = new DateTime(todayDate.Year, todayDate.Month, 1);
            _view.SetDates(currentMonth, todayDate);
        }

        private void OpenUsersList()
        {
            try
            {
                IUsersListView view;
                UsersListPresenter presenter;

                view = new UsersListView();
                presenter = new UsersListPresenter(view, _users);

                view.ShowWindow();

                if (view.User.isValid() && view.User.idUsuario.isValid())
                {
                    _view.Show(view.User);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Print()
        {
            try
            {
                List<VMRDetalleComision> billsOfSaleComissions = _billsOfSale.GetComissionDetailsForPeriodAndUser(_view.StartDate, _view.EndDate, _view.User);
                List<VMRDetalleComision> invoicesComissions = _invoices.GetComissionDetailsForPeriodAndUser(_view.StartDate, _view.EndDate, _view.User);

                List<VMRDetalleComision> transacciones = new List<VMRDetalleComision>();

                transacciones.AddRange(billsOfSaleComissions);
                transacciones.AddRange(invoicesComissions);

                var transaccionesPorUsuario = transacciones.GroupBy(x => x.NombreUsuario);
                List<VMRComisiones> vmTransaccionesUsuario = new List<VMRComisiones>();

                foreach (var transaccionesUsuario in transaccionesPorUsuario)
                {
                    VMRComisiones grupo = new VMRComisiones(transaccionesUsuario.ToList(), transaccionesUsuario.Key);
                    vmTransaccionesUsuario.Add(grupo);
                }

                VMReporte report = Reports.FillReport(vmTransaccionesUsuario,transacciones, _view.StartDate, _view.EndDate);

                IReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Print();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Preview()
        {
            try
            {
                List<VMRDetalleComision> billsOfSaleComissions = _billsOfSale.GetComissionDetailsForPeriodAndUser(_view.StartDate, _view.EndDate, _view.User);
                List<VMRDetalleComision> invoicesComissions = _invoices.GetComissionDetailsForPeriodAndUser(_view.StartDate, _view.EndDate, _view.User);

                List<VMRDetalleComision> transacciones = new List<VMRDetalleComision>();

                transacciones.AddRange(billsOfSaleComissions);
                transacciones.AddRange(invoicesComissions);

                var transaccionesPorUsuario = transacciones.GroupBy(x => x.NombreUsuario);
                List<VMRComisiones> vmTransaccionesUsuario = new List<VMRComisiones>();

                foreach (var transaccionesUsuario in transaccionesPorUsuario)
                {
                    VMRComisiones grupo = new VMRComisiones(transaccionesUsuario.ToList(), transaccionesUsuario.Key);
                    vmTransaccionesUsuario.Add(grupo);
                }

                VMReporte report = Reports.FillReport(vmTransaccionesUsuario, transacciones, _view.StartDate, _view.EndDate);

                IReportViewerView view;
                ReportViewerPresenter presenter;

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Preview();
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
