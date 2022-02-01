using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class PurchasePrintPresenter
    {
        private IPurchasePrintView _view;
        private ICompraService _purchases;
        private IProveedorService _suppliers;

        public PurchasePrintPresenter(IPurchasePrintView view, ICompraService purchases, IProveedorService suppliers)
        {
            _view = view;
            _purchases = purchases;
            _suppliers = suppliers;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            if (!_view.Purchase.idCompra.isValid())
            {
                _view.ShowError("No hay compra seleccionada para imprimir");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var purchase = new VMCompra(_purchases.Find(_view.Purchase.idCompra));

                view = new ReportViewerView(Reports.FillReport(purchase));
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
            if (!_view.Purchase.idCompra.isValid())
            {
                _view.ShowError("No hay compra seleccionada para visualizar");
                return;
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                var purchase = new VMCompra(_purchases.Find(_view.Purchase.idCompra));

                view = new ReportViewerView(Reports.FillReport(purchase));
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

        private void OpenList()
        {
            try
            {
                IPurchasesListView view;
                PurchasesListPresenter presenter;

                if (_view.Purchase.idProveedor.isValid())
                    view = new PurchasesListView(_view.Purchase.idProveedor);
                else
                    view = new PurchasesListView();
                presenter = new PurchasesListPresenter(view, _purchases);

                view.ShowWindow();

                if (view.Purchase.idCompra.isValid())
                    _view.Show(new VMCompra(view.Purchase));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Purchase.Proveedore.codigo.isValid())
            {
                _view.ShowError("El código del proveedor no es válido");
                return;
            }

            if(!_view.Purchase.folio.isValid())
            {
                _view.ShowError("El folio no es válido");
                return;
            }

            try
            {
                var supplier = _suppliers.Find(_view.Purchase.Proveedore.codigo);

                if (!supplier.isValid())
                {
                    _view.ShowError("El código del proveedor no corresponde a ninguno registrado");
                    return;
                }

                var purchase = _purchases.Find(_view.Purchase.idProveedor, _view.Purchase.folio);

                if (!purchase.isValid())
                {
                    _view.ShowError("El folio de compra no corresponde a ninguna compra realizada al proveedor especificado");
                    return;
                }

                _view.Show(new VMCompra(purchase));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {
            //Si ya hay una establecida no busco la ultima
            if (_view.Purchase.folio.isValid() && _view.Purchase.idCompra.isValid())
                return;

            if(!_view.Purchase.Proveedore.codigo.isValid())
            {
                _view.ShowError("El código del proveedor no es válido");
                return;
            }

            try
            {
                var supplier = _suppliers.Find(_view.Purchase.Proveedore.codigo);

                if (!supplier.isValid())
                {
                    _view.ShowError("El código del proveedor no corresponde a ninguno registrado");
                    return;
                }

                var purchase = _purchases.Last(supplier);

                if(!purchase.isValid())
                {
                    _view.ShowError("No existen compras registradas a este proveedor");
                    return;
                }

                _view.Show(new VMCompra(purchase));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
