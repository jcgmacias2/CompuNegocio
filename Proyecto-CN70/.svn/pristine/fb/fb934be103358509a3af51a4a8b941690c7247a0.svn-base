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
    public class BillOfSaleToInvoicePresenter
    {
        private IBillOfSaleToInvoiceView _view;
        private IFacturaService _invoices;
        private IAbonoDeFacturaService _payments;
        private ICatalogosEstaticosService _catalogs;
        private IUsosCFDIService _uses;
        private IConfiguracionService _config;
        private IRemisionService _billsOfSale;
        private IEnvioDeCorreoService _mailer;

        public BillOfSaleToInvoicePresenter(IBillOfSaleToInvoiceView view, IFacturaService invoicesService, IAbonoDeFacturaService paymentsService, ICatalogosEstaticosService catalogs, IUsosCFDIService usesCfdi, IConfiguracionService config, IRemisionService billsOfSale, IEnvioDeCorreoService mailer)
        {
            _view = view;
            _invoices = invoicesService;
            _payments = paymentsService;
            _catalogs = catalogs;
            _uses = usesCfdi;
            _config = config;
            _billsOfSale = billsOfSale;
            _mailer = mailer;

            _view.Quit += Quit;
            _view.Save += Save;

            _view.Fill(_catalogs.ListMetodosDePago(), _uses.List().Where(u => u.activo).ToList(), _config.GetDefault().Regimenes.ToList());
        }

        private void Save()
        {
            if(!_view.Invoice.MetodosPago.isValid())
            {
                _view.ShowError("Debe seleccionar un método de pago");
                return;
            }

            if(!Session.Station.isValid())
            {
                _view.ShowError("Este equipo no cuenta con ninguna estación asociada");
                return;
            }

            try
            {
                //Obtengo la factura a registrar
                var invoice = _view.Invoice;
                //Obtengo la remisión que origina la factura
                var billOfSale = _view.BillOfSale;

                //Le actualizo la información requerida para facturar
                invoice.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                invoice.Usuario = Session.LoggedUser;
                invoice.idEmpresa = Session.Station.Empresa.idEmpresa;
                invoice.Empresa = Session.Station.Empresa;

                //Si no tiene vendedor se usa el usuario actual
                if (!invoice.idVendedor.isValid())
                {
                    invoice.idVendedor = Session.LoggedUser.idUsuario;
                    invoice.Usuario1 = Session.LoggedUser;
                }

                //Actualizo el folio
                invoice.serie = Session.SerieFacturas.identificador;
                invoice.folio = _invoices.Next(invoice.serie);

                //Debo asignarle folios a los abonos también
                foreach (var p in invoice.AbonosDeFacturas)
                {
                    //BR: 3.3 Si la forma de pago es bancarizada, debe llevar, sino puede quedarse en blanco
                    if (p.FormasPago.bancarizado && !p.idCuentaBancaria.isValid())
                        throw new Exception(string.Format("La forma de pago {0} requiere una cuenta beneficiaria", p.FormasPago.descripcion));

                    p.idEmpresa = invoice.idEmpresa;
                    p.Empresa = invoice.Empresa;
                    p.folio = _payments.GetNextFolio();
                }

                //Actualizo la cuenta
                invoice.UpdateAccount();

                //Agrego la factura
                invoice = new VMFactura(_invoices.Add(invoice));

                //También debo marcar la remisión como factura
                _billsOfSale.Invoiced(billOfSale.idRemision, invoice.idFactura);

                //Timbro la factura
                invoice = new VMFactura(_invoices.Stamp(invoice, false));

                //Si es en parcialidades timbro abono por abono
                if (invoice.idMetodoPago.Equals((int)MetodoDePago.Pago_en_parcialidades_o_diferido) && invoice.AbonosDeFacturas.Count > 0)
                {
                    _view.ShowMessage("La factura fué registrada exitosamente, ahora se registrarán los comprobantes de pago");
                    foreach (var a in invoice.AbonosDeFacturas)
                    {
                        _payments.Stamp(invoice, a);
                    }
                }

                _view.ShowMessage("Remisión facturada exitosamente");

                //Aquí genero el pdf
                Usuario user = GetSellerForInvoice(invoice);

                var report = Reports.FillReport(new VMRFactura(invoice, Session.Configuration, user));
                report.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, invoice.serie, invoice.folio));

                //Aqui debo enviar el correo si Guardian esta activado
                if (Modulos.Envio_De_Correos.IsActive())
                    _mailer.SendMail(invoice);

                //Ahora el pdf de cada pago
                if (invoice.MetodosPago.idMetodoPago.Equals((int)MetodoDePago.Pago_en_parcialidades_o_diferido))
                {
                    foreach (var a in invoice.AbonosDeFacturas.Where(a => a.TimbresDeAbonosDeFactura.isValid()))
                    {
                        var paymentReporte = Reports.FillReport(new VMPago(a, Session.Configuration));
                        paymentReporte.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, a.TimbresDeAbonosDeFactura.serie, a.TimbresDeAbonosDeFactura.folio));
                        //Aqui debo enviar el correo si Guardian esta activado
                        if (Modulos.Envio_De_Correos.IsActive())
                            _mailer.SendMail(a);
                    }
                }

                IInvoicePrintView view;
                InvoicePrintPresenter presenter;

                view = new InvoicePrintView(invoice);
                presenter = new InvoicePrintPresenter(view, _invoices);

                view.ShowWindow();

                //Al cerrarse la impresión, cierro también esta
                _view.CloseWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
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
