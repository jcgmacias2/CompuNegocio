using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Presenters
{
    public class QuotePrintPresenter
    {
        private IQuotePrintView _view;
        private ICotizacionService _quotes;
        private IEnvioDeCorreoService _mailer;

        public QuotePrintPresenter(IQuotePrintView view, ICotizacionService quotes, IEnvioDeCorreoService mailer)
        {
            _view = view;
            _quotes = quotes;
            _mailer = mailer;

            _view.FindLast += FindLast;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.Quit += Quit;
            _view.Preview += Preview;
            _view.Print += Print;
            _view.SendEmail += SendEmail;
        }

        private void SendEmail()
        {
            //Envia correo si el modulo esta activado
            try
            {
                //La ruta de la carpeta de cotizaciones cargala en Session.Cotizaciones
                //Obtiene la ruta de la carpeta de cotizaciones
                string dir = Session.Configuration.CarpetaReportes;
                string path = Path.Combine(dir, "Cotizaciones");

                var quote = _view.Quote;

                //Si no hay cotizacion seleccionada, ni folio lo regreso
                if (!quote.idCotizacion.isValid() && !quote.folio.isValid())
                {
                    _view.ShowError("No hay cotización seleccionada para enviar");
                    return;
                }

                //Si no tiene cotizacion seleccionada, busco en base el folio
                if (!quote.idCotizacion.isValid())
                {
                    var exists = _quotes.FindByFolio(quote.folio);

                    if (!exists.isValid() || !exists.idCotizacion.isValid())
                    {
                        _view.ShowError("No existe la cotizacion especificada");
                        return;
                    }

                    quote = new VMCotizacion(exists);
                }

                //Se genera el pdf
                //BR: Si el cliente tiene vendedor, se debe mostrar ese usuario en el area de la firma, si no, se muestra el que registro
                var user = quote.Cliente.idVendedor.isValid() ? quote.Cliente.Usuario : quote.Usuario;
                var report = Reports.FillReport(new VMRCotizacion(quote, Session.Configuration, user));
                report.Export(string.Format("{0}\\{1}.pdf", path, quote.folio));

                //Se envia el correo
                _mailer.SendMail(quote, _view.EmailOption, _view.GivenEmail);

                _view.ShowMessage("Cotización enviada exitosamente");
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Print()
        {
            var quote = _view.Quote;

            //Si no hay cotizacion seleccionada, ni folio lo regreso
            if (!quote.idCotizacion.isValid() && !quote.folio.isValid())
            {
                _view.ShowError("No hay cotización seleccionada para visualizar");
                return;
            }

            //Si no tiene cotizacion seleccionada, busco en base el folio
            if (!quote.idCotizacion.isValid())
            {
                var exists = _quotes.FindByFolio(quote.folio);

                if (!exists.isValid() || !exists.idCotizacion.isValid())
                {
                    _view.ShowError("No existe la cotizacion especificada");
                    return;
                }

                quote = new VMCotizacion(exists);
            }

            try
            {
                ReportViewerView view;
                ReportViewerPresenter presenter;

                //BR: Si el cliente tiene vendedor, se debe mostrar ese usuario en el area de la firma, si no, se muestra el que registro
                var user = quote.Cliente.idVendedor.isValid() ? quote.Cliente.Usuario : quote.Usuario;

                var reportInvoice = new VMRCotizacion(quote, Session.Configuration, user);
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
            var quote = _view.Quote;

            //Si no hay cotizacion seleccionada, ni folio lo regreso
            if (!quote.idCotizacion.isValid() && !quote.folio.isValid())
            {
                _view.ShowError("No hay cotización seleccionada para visualizar");
                return;
            }

            //Si no tiene cotizacion seleccionada, busco en base el folio
            if (!quote.idCotizacion.isValid())
            {
                var exists = _quotes.FindByFolio(quote.folio);

                if (!exists.isValid() || !exists.idCotizacion.isValid())
                {
                    _view.ShowError("No existe la cotizacion especificada");
                    return;
                }

                quote = new VMCotizacion(exists);
            }

            try
            { 
                ReportViewerView view;
                ReportViewerPresenter presenter;

                //BR: Si el cliente tiene vendedor, se debe mostrar ese usuario en el area de la firma, si no, se muestra el que registro
                var user = quote.Cliente.idVendedor.isValid() ? quote.Cliente.Usuario : quote.Usuario;

                var reportInvoice = new VMRCotizacion(quote, Session.Configuration, user);
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
            if(!_view.Quote.folio.isValid())
            {
                _view.ShowError("Debe especificar el folio a buscar");
                return;
            }

            try
            {
                var invoice = new VMCotizacion(_quotes.FindByFolio(_view.Quote.folio));

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
                IQuotesListView view;
                QuotesListPresenter presenter;

                view = new QuotesListView();
                presenter = new QuotesListPresenter(view, _quotes);

                view.ShowWindow();

                if (view.Quote.isValid())
                    _view.Show(new VMCotizacion(view.Quote));
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
                var folio = _quotes.Last();
                var quote = new VMCotizacion(_quotes.FindByFolio(folio));

                _view.Show(quote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                _view.Show(new VMCotizacion());
            }
        }
    }
}
