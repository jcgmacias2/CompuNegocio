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
    public class SupplierStatementReportPresenter
    {
        private ISupplierStatementReportView _view;
        private IProveedorService _suppliers;
        private ISaldosPorProveedorService _balances;

        public SupplierStatementReportPresenter(ISupplierStatementReportView view, IProveedorService suppliers, ISaldosPorProveedorService balances)
        {
            _view = view;
            _suppliers = suppliers;
            _balances = balances;

            _view.FindSupplier += FindSupplier;
            _view.OpenSuppliersList += OpenSuppliersList;
            _view.Quit += Quit;
            _view.Print += Print;
            _view.Preview += Preview;
        }

        private void Preview()
        {
            if (!_view.Supplier.idProveedor.isValid())
            {
                _view.ShowError("Debe seleccionar el proveedor sobre el cual desea obtener la información");
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

                var purchases = _balances.List(_view.Supplier, _view.Start, _view.End, _view.OnlyPendingBalance);

                view = new ReportViewerView(Reports.FillReport(purchases, _view.Start, _view.End, _view.Supplier));
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
            if (!_view.Supplier.idProveedor.isValid())
            {
                _view.ShowError("Debe seleccionar el proveedor sobre el cual desea obtener la información");
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

                var purchases = _balances.List(_view.Supplier, _view.Start, _view.End, _view.OnlyPendingBalance);

                view = new ReportViewerView(Reports.FillReport(purchases, _view.Start, _view.End, _view.Supplier));
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

        private void OpenSuppliersList()
        {
            try
            {
                ISuppliersListView view;
                SuppliersListPresenter presenter;

                view = new SuppliersListView();
                presenter = new SuppliersListPresenter(view, _suppliers);

                view.ShowWindow();

                if (view.Supplier.idProveedor.isValid())
                    _view.Show(view.Supplier);
                else
                    _view.Show(new Proveedore());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindSupplier()
        {
            try
            {
                var supplier = new Proveedore();

                //Si el código es válido intento buscarlo
                if (_view.Supplier.codigo.isValid())
                    supplier = _suppliers.Find(_view.Supplier.codigo);

                if (supplier.isValid())
                    _view.Show(supplier);
                else
                    _view.Show(new Proveedore());

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
