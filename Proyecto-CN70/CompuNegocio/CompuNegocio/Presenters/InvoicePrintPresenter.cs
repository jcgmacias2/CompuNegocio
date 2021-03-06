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
using Aprovi.Data.Models;

namespace Aprovi.Presenters
{
    public class InvoicePrintPresenter
    {
        private IInvoicePrintView _view;
        private IFacturaService _invoices;

        public InvoicePrintPresenter(IInvoicePrintView view, IFacturaService invoices)
        {
            _view = view;
            _invoices = invoices;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
        }

        private void Print()
        {
            var invoice = _view.Invoice;
            //Si no hay factura seleccionada, ni serie o folio lo regreso
            if (!invoice.idFactura.isValid() && (!invoice.serie.isValid() || !invoice.folio.isValid()))
            {
                _view.ShowError("No hay factura seleccionada para visualizar");
                return;
            }

            //Si no tiene factura seleccionada, busco en base a la serie y folio
            if (!invoice.idFactura.isValid())
            {
                var exists = _invoices.Find(invoice.serie, invoice.folio.ToString());

                if (!exists.isValid() || !exists.idFactura.isValid())
                {
                    _view.ShowError("No existe la factura especificada");
                    return;
                }

                invoice = new VMFactura(exists);
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                Usuario seller = GetSellerForInvoice(invoice);
                var reportInvoice = new VMRFactura(invoice, Session.Configuration, seller);
                view = new ReportViewerView(Reports.FillReport(reportInvoice));
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
            var invoice = _view.Invoice;
            //Si no hay factura seleccionada, ni serie o folio lo regreso
            if(!invoice.idFactura.isValid() && (!invoice.serie.isValid() || !invoice.folio.isValid()))
            {
                _view.ShowError("No hay factura seleccionada para visualizar");
                return;
            }

            //Si no tiene factura seleccionada, busco en base a la serie y folio
            if(!invoice.idFactura.isValid())
            {
                var exists = _invoices.Find(invoice.serie, invoice.folio.ToString());

                if(!exists.isValid() || !exists.idFactura.isValid())
                {
                    _view.ShowError("No existe la factura especificada");
                    return;
                }

                invoice = new VMFactura(exists);
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                Usuario user = GetSellerForInvoice(invoice);
                var reportInvoice = new VMRFactura(invoice, Session.Configuration, user);
                view = new ReportViewerView(Reports.FillReport(reportInvoice));
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
            if (!_view.Invoice.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            if(!_view.Invoice.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var invoice = new VMFactura(_invoices.Find(_view.Invoice.serie, _view.Invoice.folio.ToString()));

                _view.Show(invoice);
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
                IInvoicesListView view;
                InvoicesListPresenter presenter;

                view = new InvoicesListView();
                presenter = new InvoicesListPresenter(view, _invoices);

                view.ShowWindow();

                if (view.Invoice.isValid() && view.Invoice.idFactura.isValid())
                {
                    var dbFactura = _invoices.Find(view.Invoice.idFactura);

                    if (dbFactura.isValid() && dbFactura.idFactura.isValid())
                    {
                        _view.Show(new VMFactura(dbFactura));
                    }
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindLast()
        {
            if(!_view.Invoice.serie.isValid())
            {
                _view.ShowError("Debe especificar la serie a buscar");
                return;
            }

            try
            {
                var folio = _invoices.Last(_view.Invoice.serie);
                var invoice = new VMFactura(_invoices.Find(_view.Invoice.serie, folio.ToString()));

                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Show(new VMFactura());
            }
        }

        private Usuario GetSellerForInvoice(VMFactura invoice)
        {
            Usuario user;
            if (invoice.Usuario1.isValid())
            {
                //Se usa el vendedor asignado
                user = invoice.Usuario1;
            }
            else
            {
                if (invoice.Cliente.Usuario.isValid())
                {
                    //Se usa el vendedor del cliente
                    user = invoice.Cliente.Usuario;
                }
                else
                {
                    //Se usa el usuario que registró
                    user = invoice.Usuario;
                }
            }

            return user;
        }
    }
}
