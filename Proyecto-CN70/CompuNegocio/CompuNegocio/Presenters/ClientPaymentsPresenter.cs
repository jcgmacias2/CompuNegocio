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
    public class ClientPaymentsPresenter
    {
        private IClientPaymentsView _view;
        private IClienteService _clients;
        private ICatalogosEstaticosService _catalogs;
        private ICuentaBancariaService _bankAccounts;
        private IFacturaService _invoices;
        private IAbonoDeFacturaService _invoicePayments;
        private IPagoService _payments;
        private readonly IConfiguracionService _configs;
        private IEnvioDeCorreoService _mailer;

        public ClientPaymentsPresenter(IClientPaymentsView view, IClienteService clients, ICatalogosEstaticosService catalogs, ICuentaBancariaService bankAccounts, IFacturaService invoices, IAbonoDeFacturaService invoicePayments, IEnvioDeCorreoService mailer, IPagoService payments, IConfiguracionService configs)
        {
            _view = view;
            _clients = clients;
            _catalogs = catalogs;
            _bankAccounts = bankAccounts;
            _invoices = invoices;
            _invoicePayments = invoicePayments;
            _payments = payments;
            _configs = configs;
            _mailer = mailer;

            _view.Load += Load;
            _view.FindClient += FindClient;
            _view.OpenClientsList += OpenClientsList;
            _view.GetFolio += GetFolio;
            _view.Find += Find;
            _view.OpenList += OpenList;
            _view.SelectInvoice += SelectInvoice;
            _view.AddPayment += AddPayment;
            _view.Quit += Quit;
            _view.New += New;
            _view.Cancel += Cancel;
            _view.Print += Print;
            _view.Save += Save;
            _view.Stamp += Stamp;
            _view.ValidatePayment += ValidatePayment;

            _view.Fill(_catalogs.ListMonedas(), _catalogs.ListFormasDePago().Where(f => f.activa).ToList(), _bankAccounts.List(), _configs.GetDefault().Regimenes.ToList());
        }

        private void Load()
        {
            if (Session.Configuration.Series.Count <= 0)
            {
                _view.ShowError("Antes de registrar pagos debe configurar series");
                return;
            }

            if (!Session.SerieParcialidades.isValid() || !Session.SerieParcialidades.idSerie.isValid())
            {
                _view.ShowError("Debe configurar una serie default para parcialidades y/o pagos antes de realizar registros de las mismas");
                return;
            }

            if (!Session.Configuration.CarpetaXml.isValid())
            {
                _view.ShowError("Antes de registrar pagos debe configurar la carpeta para depositar los comprobantes xml");
                return;
            }
            if (!Session.Configuration.CarpetaCbb.isValid())
            {
                _view.ShowError("Antes de registrar pagos debe configurar la carpeta para depositar los codigos bidimensionales");
                return;
            }
            if (!Session.Configuration.CarpetaPdf.isValid())
            {
                _view.ShowError("Antes de registrar pagos debe configurar la carpeta para depositar los comprobantes pdf");
                return;
            }

            var cert = Session.Configuration.Certificados.FirstOrDefault(c => c.activo);
            if (!cert.isValid())
            {
                _view.ShowError("Antes de registrar pagos debe configurar un certificado de sello digital");
                return;
            }

            if (cert.expedicion > DateTime.Now || cert.vencimiento < DateTime.Now)
            {
                _view.ShowError("El certificado de sello digital configurado no esta vigente");
                return;
            }

            if (Session.Configuration.Regimenes.Where(r => r.activo).Count() <= 0)
            {
                _view.ShowError("Debe tener al menos un régimen dado de alta activamente");
                return;
            }

            try
            {
                //Establezco los defaults
                var payment = new VMPagoMultiple();
                payment.FechaHora = DateTime.Now;
                payment.TipoDeCambio = Session.Configuration.tipoDeCambio;
                payment.IdEstatusDePago = (int)StatusDePago.Nuevo;

                //Si existe una serie default la cargo
                if (Session.SerieParcialidades.isValid())
                {
                    payment.Serie = Session.SerieParcialidades.identificador;
                    payment.Folio = _invoices.Next(payment.Serie);
                }

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            try
            {
                if (!_view.Payment.Cliente.codigo.isValid())
                    return;

                var client = _clients.Find(_view.Payment.Cliente.codigo);

                //Si el cliente no es válido envío mensaje de error y recargo la ventana
                if (!client.isValid())
                {
                    _view.ShowMessage("No se encontró ningún cliente el código {0}", _view.Payment.Cliente.codigo);
                    Load();
                    return;
                }

                //Si encuentro al cliente cargo un nuevo pago con todos los saldos del cliente
                if (client.isValid() && client.idCliente.isValid())
                    _view.Show(new VMPagoMultiple(client, _invoices.List(client), _view.Payment.Serie, _payments.Next(_view.Payment.Serie), Session.Configuration.tipoDeCambio));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenClientsList()
        {
            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _clients);

                view.ShowWindow();

                //Si encuentro al cliente cargo un nuevo pago con todos los saldos del cliente
                if (view.Client.isValid() && view.Client.idCliente.isValid())
                    _view.Show(new VMPagoMultiple(view.Client, _invoices.List(view.Client), _view.Payment.Serie, _payments.Next(_view.Payment.Serie), Session.Configuration.tipoDeCambio));
                else
                    Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void GetFolio()
        {
            if (!_view.Payment.Serie.isValid())
                return;

            try
            {
                VMPagoMultiple payment;

                var folio = _payments.Next(_view.Payment.Serie);
                //Si es el mismo ya esta en operación, no es necesario poner uno nuevo
                if (folio.Equals(_view.Payment.Folio))
                    return;

                if (_view.Payment.Cliente.idCliente.isValid())
                    payment = new VMPagoMultiple() { Cliente = _view.Payment.Cliente, Serie = _view.Payment.Serie, Folio = folio, TipoDeCambio = Session.Configuration.tipoDeCambio, FechaHora = DateTime.Now };
                else
                    payment = new VMPagoMultiple() { Serie = _view.Payment.Serie, Folio = folio, TipoDeCambio = Session.Configuration.tipoDeCambio, FechaHora = DateTime.Now };

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                Load();
            }
        }

        private void Find()
        {
            if (!_view.Payment.Serie.isValid() || !_view.Payment.Folio.isValid())
                return;

            try
            {

                var payment = _payments.Find(_view.Payment.Serie, _view.Payment.Folio.ToString());

                //Si el id del pago encontrado es igual al del pago que se esta mostrando no hago nada
                if (payment.isValid() && _view.IsDirty && _view.Payment.IdPago.Equals(payment.idPago))
                    return;

                //Si llego aqui y el pago es válido muestro el pago existente
                if (payment.isValid())
                {
                    _view.Show(new VMPagoMultiple(payment, _invoices.List(payment)));
                    return;
                }

                //Si el pago no es válido pero el cliente si lo es
                if (!payment.isValid())
                    _view.Show(new VMPagoMultiple(_view.Payment.Cliente, _invoices.List(_view.Payment.Cliente), _view.Payment.Serie, _invoices.Next(_view.Payment.Serie), Session.Configuration.tipoDeCambio));

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenList()
        {

            try
            {
                IClientPaymentsListView view;
                ClientPaymentsListPresenter presenter;

                view = new ClientPaymentsListView();
                presenter = new ClientPaymentsListPresenter(view, _payments);

                view.ShowWindow();

                if (view.Payment.isValid() && view.Payment.idPago.isValid())
                {
                    //Obtiene el pago de la base de datos
                    var payment = _payments.Find(view.Payment.idPago);

                    if (payment.isValid() && payment.idPago.isValid())
                        _view.Show(new VMPagoMultiple(payment, _invoices.List(payment)));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectInvoice()
        {
            try
            {
                if (_view.IsDirty)
                    throw new Exception("No puede editar abonos registrados");

                if (_view.Selected.isValid() && _view.Selected.IdFactura.isValid())
                    _view.Show(_view.Selected);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void AddPayment()
        {
            try
            {
                var currentPayment = _view.Current; //Factura con abono
                var invoices = _view.Payment.FacturasConSaldo; //Lista de facturas

                if(!currentPayment.isValid() || !currentPayment.IdFactura.isValid())
                {
                    _view.ShowError("Debe seleccionar una factura con adeudo para agregar un abono");
                    currentPayment.Abono = null;
                    return;
                }

                if (!currentPayment.Abono.idFormaPago.isValid())
                {
                    _view.ShowError("Debe seleccionar la forma de pago del abono");
                    currentPayment.Abono = null;
                    return;
                }

                if (!currentPayment.Abono.idMoneda.isValid())
                {
                    _view.ShowError("Debe seleccionar la moneda del abono");
                    return;
                }

                if (!currentPayment.Abono.monto.isValid())
                {
                    _view.ShowError("La cantidad a abonar no es válida");
                    currentPayment.Abono = null;
                    return;
                }

                if (currentPayment.Abono.FormasPago.bancarizado && (!currentPayment.Abono.idCuentaBancaria.HasValue || !currentPayment.Abono.idCuentaBancaria.Value.isValid()))
                {
                    _view.ShowError(string.Format("La forma de pago {0} requiere una cuenta beneficiaria", currentPayment.Abono.FormasPago.descripcion));
                    currentPayment.Abono = null;
                    return;
                }

                //La fecha de registro del abono no puede ser una fecha futura
                if (currentPayment.Abono.fechaHora.Date > DateTime.Now.ToNextMidnight())
                {
                    _view.ShowError("El abono no puede ser de una fecha futura");
                    currentPayment.Abono = null;
                    return;
                }

                //Ni anterior al registro de la factura
                if (currentPayment.Abono.fechaHora.Date < currentPayment.FechaHora.Date)
                {
                    _view.ShowError("El abono no puede ser anterior al registro de la factura");
                    currentPayment.Abono = null;
                    return;
                }

                //Ni anterior al abono previo
                var invoice = _invoices.Find(currentPayment.IdFactura);
                var prev = invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado).OrderByDescending(a => a.fechaHora).FirstOrDefault();
                if (prev.isValid() && currentPayment.Abono.fechaHora.Date < prev.fechaHora.Date)
                {
                    _view.ShowError("El abono no puede ser anterior al abono previo registrado a la factura");
                    currentPayment.Abono = null;
                    return;
                }

                //Si el abono anterior (no cancelado) no ha sido timbrado y debería timbrarse.
                //No es valido si no tiene un timbre y tampoco forma parte de un pago
                if (prev.isValid() && ((!prev.TimbresDeAbonosDeFactura.isValid() || !prev.TimbresDeAbonosDeFactura.idTimbreDeAbonoDeFactura.isValid()) && !prev.idPago.HasValue))
                {
                    _view.ShowError("No es posible agregar un nuevo abono sin antes timbrar el anterior");
                    currentPayment.Abono = null;
                    return;
                }

                //Si el monto (convertido a la moneda de la factura) que voy a abonar es mayor al saldo total reducir el monto al saldo
                if (currentPayment.Abono.monto.ToDocumentCurrency(new Moneda() { idMoneda = currentPayment.Abono.idMoneda }, new Moneda() { idMoneda = currentPayment.IdMoneda }, currentPayment.Abono.tipoDeCambio) > currentPayment.Saldo)
                {
                    if (currentPayment.IdMoneda.Equals(currentPayment.Abono.idMoneda))
                        currentPayment.Abono.monto = currentPayment.Saldo;
                    else
                        currentPayment.Abono.monto = currentPayment.Saldo.ToDocumentCurrency(new Moneda() { idMoneda = currentPayment.IdMoneda }, new Moneda() { idMoneda = currentPayment.Abono.idMoneda }, currentPayment.Abono.tipoDeCambio);
                }

                //Si ya esta saldada la cuenta ya no puede agregar mas abonos
                if (currentPayment.Abono.monto <= 0.0m)
                {
                    _view.ShowMessage("La cuenta ya esta saldada, no es posible realizar más abonos");
                    currentPayment.Abono = null;
                    return;
                }

                //Agrego el abono
                _view.Show(_view.Payment.UpdateAccount());
                _view.ClearPayment();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ValidatePayment()
        {
            try
            {
                //Si tiene una moneda distinta realizar el cambio de divisas y mostrarlo
                var payment = _view.Current;
                if (payment.IdMoneda.Equals(payment.Abono.idMoneda))
                    return;

                ICurrencyExchangeView view;
                CurrencyExchangePresenter presenter;

                view = new CurrencyExchangeView(payment.Abono);
                presenter = new CurrencyExchangePresenter(view, _catalogs);

                view.ShowWindow();

                payment.Abono = view.PaymentExchange;
                _view.Show(payment);
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

        private void New()
        {
            try
            {
                _view.Clear();
                //Establezco los defaults
                var payment = new VMPagoMultiple();
                payment.FechaHora = DateTime.Now;
                payment.TipoDeCambio = Session.Configuration.tipoDeCambio;
                payment.IdEstatusDePago = (int)StatusDePago.Nuevo;

                //Si existe una serie default la cargo
                if (Session.SerieParcialidades.isValid())
                {
                    payment.Serie = Session.SerieParcialidades.identificador;
                    payment.Folio = _invoices.Next(payment.Serie);
                }

                _view.Show(payment);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Cancel()
        {
            if (!_view.IsDirty)
            {
                _view.ShowError("No hay ninguna factura seleccionada para cancelación");
                return;
            }

            try
            {
                //Se solicita el motivo de la cancelacion
                ICancellationView view;
                CancellationPresenter presenter;

                view = new CancellationView();
                presenter = new CancellationPresenter(view);

                view.ShowWindow();

                //Aqui realizo la cancelacion
                var payment = _payments.Cancel(_view.Payment.IdPago, view.Reason);

                _view.ShowMessage("Pago cancelado exitosamente");

                //Aquí genero el pdf si fue una cancelación fiscal
                if (payment.TimbresDePago.isValid())
                {
                    var receipt = new VMAcuse(payment, Session.Configuration);
                    var report = Reports.FillReport(receipt);
                    report.Export(string.Format("{0}\\{1}{2}-Acuse Cancelación.pdf", Session.Configuration.CarpetaPdf, payment.serie, payment.folio));
                }

                //Inicializo nuevamente
                Load();
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
                //Se debe generar el reporte de los abonos timbrados
                IPaymentPrintView view;
                PaymentPrintPresenter presenter;

                var payment = _view.Payment.ToPago();

                if (!payment.TimbresDePago.isValid() || !payment.TimbresDePago.idTimbreDePago.isValid())
                    throw new Exception("No es posible imprimir un pago que no ha sido timbrado");

                view = new PaymentPrintView(payment);
                presenter = new PaymentPrintPresenter(view, _payments, _invoicePayments);

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            try
            {
                var payment = _view.Payment;

                if (!payment.IdCliente.isValid())
                {
                    _view.ShowError("El cliente no es válido");
                    return;
                }

                if (!payment.TipoDeCambio.isValid())
                {
                    _view.ShowError("El tipo de cambio no es válido");
                    return;
                }

                if (!payment.IdRegimen.isValid())
                {
                    _view.ShowError("El régimen no es válido");
                    return;
                }

                if (payment.FacturasConSaldo.Count().Equals(0) || !payment.FacturasConSaldo.Any(f => f.Abono.isValid() && f.Abono.monto.isValid()))
                {
                    _view.ShowError("No se ha ingresado ningún abono");
                    return;
                }


                //Se llena el pago
                payment.IdUsuario = Session.LoggedUser.idUsuario;
                payment.IdEmpresa = Session.Configuration.Estacion.idEmpresa;

                //Se registra el pago
                var paymentDocument = _payments.Add(payment.ToPago());

                _view.ShowMessage("Abonos registrados existosamente, ahora se realizará el timbrado");

                //Se debe generar el timbrado de los abonos
                _payments.Stamp(paymentDocument);

                _view.ShowMessage("Comprobante de pago timbrado exitosamente");
                //Se debe enviar el cfdi de los abonos timbrados (si aplica)
                try
                {
                    var paymentReporte = Reports.FillReport(new VMRAbonosFacturas(paymentDocument, _invoicePayments.GetPaymentsForReport(paymentDocument.AbonosDeFacturas.ToList()), Session.Configuration));
                    paymentReporte.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, paymentDocument.serie, paymentDocument.folio));
                    //Aqui debo enviar el correo si Guardian esta activado
                    if (Modulos.Envio_De_Correos.IsActive())
                        _mailer.SendMail(paymentDocument);
                }
                catch (Exception ex)
                {
                    _view.ShowError(ex.Message);
                }

                //Se debe generar el reporte de los abonos timbrados
                IPaymentPrintView view;
                PaymentPrintPresenter presenter;

                view = new PaymentPrintView(paymentDocument);
                presenter = new PaymentPrintPresenter(view, _payments, _invoicePayments);

                view.ShowWindow();
                Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Stamp()
        {
            try
            {
                var paymentDocument = _payments.Find(_view.Payment.IdPago);

                //Se debe generar el timbrado
                _payments.Stamp(paymentDocument);

                _view.ShowMessage("Comprobante de pago timbrado exitosamente");
                //Se debe enviar el cfdi de los abonos timbrados (si aplica)
                try
                {
                    var paymentReporte = Reports.FillReport(new VMRAbonosFacturas(paymentDocument, _invoicePayments.GetPaymentsForReport(paymentDocument.AbonosDeFacturas.ToList()), Session.Configuration));
                    paymentReporte.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, paymentDocument.serie, paymentDocument.folio));
                    //Aqui debo enviar el correo si Guardian esta activado
                    if (Modulos.Envio_De_Correos.IsActive())
                        _mailer.SendMail(paymentDocument);
                }
                catch (Exception ex)
                {
                    _view.ShowError(ex.Message);
                }

                //Se debe generar el reporte de los abonos timbrados
                IPaymentPrintView view;
                PaymentPrintPresenter presenter;

                view = new PaymentPrintView(paymentDocument);
                presenter = new PaymentPrintPresenter(view, _payments, _invoicePayments);

                view.ShowWindow();
                Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
