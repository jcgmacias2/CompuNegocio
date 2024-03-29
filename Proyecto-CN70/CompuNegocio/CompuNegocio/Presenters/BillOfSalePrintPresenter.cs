﻿using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Application.ViewModels;
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class BillOfSalePrintPresenter
    {
        private IBillOfSalePrintView _view;
        private IRemisionService _billsOfSale;
        private IConfiguracionService _configs;

        public BillOfSalePrintPresenter(IBillOfSalePrintView view, IRemisionService billsOfSale, IConfiguracionService configs)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _configs = configs;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.BillOfSale.idRemision.isValid())
            {
                _view.ShowError("No hay remisión seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var bos = _view.BillOfSale;
                var seller = GetSellerForBillOfSale(bos);
                var format = Session.Configuration.FormatosPorConfiguracions.FirstOrDefault(f => f.Reporte.nombre.Equals("Remisión"));
                var report = Reports.FillReport(new VMRRemision(_view.BillOfSale, Session.Configuration, seller, format.Formato.archivo));

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
            if (!_view.BillOfSale.idRemision.isValid())
            {
                _view.ShowError("No hay remisión seleccionada para visualizar");
                return;
            }

            try
            {
                IReportViewerView view;
                ReportViewerPresenter presenter;

                var bos = _view.BillOfSale;
                var seller = GetSellerForBillOfSale(bos);
                var format = Session.Configuration.FormatosPorConfiguracions.FirstOrDefault(f => f.Reporte.nombre.Equals("Remisión"));
                var report = Reports.FillReport(new VMRRemision(_view.BillOfSale, Session.Configuration, seller, format.Formato.archivo));

                view = new ReportViewerView(report);
                presenter = new ReportViewerPresenter(view);

                view.Preview();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {

            if (!_view.BillOfSale.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var billOfSale = new VMRemision(_billsOfSale.FindByFolio(_view.BillOfSale.folio));

                _view.Show(billOfSale);
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

        private void OpenList()
        {
            try
            {
                IBillsOfSaleListView view;
                BillsOfSaleListPresenter presenter;

                view = new BillsOfSaleListView();
                presenter = new BillsOfSaleListPresenter(view,_billsOfSale);

                view.ShowWindow();

                if (view.BillOfSale.isValid())
                    _view.Show(new VMRemision(view.BillOfSale));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {

            try
            {
                var folio = _billsOfSale.Last();
                var billOfSale = new VMRemision(_billsOfSale.FindByFolio(folio));

                _view.Show(billOfSale);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private Usuario GetSellerForBillOfSale(VMRemision billOfSale)
        {
            Usuario user;
            if (billOfSale.Usuario1.isValid())
            {
                //Se usa el vendedor asignado
                user = billOfSale.Usuario1;
            }
            else
            {
                if (billOfSale.Cliente.Usuario.isValid())
                {
                    //Se usa el vendedor del cliente
                    user = billOfSale.Cliente.Usuario;
                }
                else
                {
                    //Se usa el usuario que registró
                    user = billOfSale.Usuario;
                }
            }

            return user;
        }
    }
}
