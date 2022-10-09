using Aprovi.Application.Helpers;
using Aprovi.Business.Helpers;
using Aprovi.Business.Services;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Aprovi.Helpers;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
namespace Aprovi.Presenters
{
    public class InvoicesPresenter
    {
        private IInvoicesView _view;
        private IFacturaService _invoices;
        private ICatalogosEstaticosService _catalogs;
        private IClienteService _clients;
        private IArticuloService _items;
        private IAbonoDeFacturaService _invoicePayments;
        private IListaDePrecioService _pricesList;
        private IUsosCFDIService _uses;
        private IConfiguracionService _config;
        private ICuentaBancariaService _bankAccounts;
        private ICuentaPredialService _propertyAccounts;
        private IEnvioDeCorreoService _mailer;
        private ICotizacionService _quotes;
        private IPedimentoService _customsApplications;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private IPagoService _payments;
        private INotaDeCreditoService _creditNotes;
        private INotaDeDescuentoService _discountNotes;

        public InvoicesPresenter(IInvoicesView view, IFacturaService invoices, ICatalogosEstaticosService catalogs, IClienteService clients, IArticuloService items, IAbonoDeFacturaService invoicePayments, IListaDePrecioService pricesList, IUsosCFDIService CFDIUses, IConfiguracionService configuration, ICuentaBancariaService accounts, ICuentaPredialService propertyAccounts, IEnvioDeCorreoService mailer, ICotizacionService quotes, IPedimentoService customsApplications, IUsuarioService users, ISeguridadService security, IPagoService payments, INotaDeCreditoService creditNotes, INotaDeDescuentoService discountNotes)
        {
            _view = view;
            _invoices = invoices;
            _catalogs = catalogs;
            _clients = clients;
            _items = items;
            _invoicePayments = invoicePayments;
            _pricesList = pricesList;
            _uses = CFDIUses;
            _config = configuration;
            _bankAccounts = accounts;
            _propertyAccounts = propertyAccounts;
            _mailer = mailer;
            _quotes = quotes;
            _customsApplications = customsApplications;
            _users = users;
            _security = security;
            _payments = payments;
            _creditNotes = creditNotes;
            _discountNotes = discountNotes;

            _view.Stamp += Stamp;
            _view.Save += Save;
            _view.Print += Print;
            _view.Cancel += Cancel;
            _view.New += New;
            _view.Quit += Quit;
            _view.SelectItem += SelectItem;
            _view.RemoveItem += RemoveItem;
            _view.ViewTaxDetails += ViewTaxDetails;
            _view.AddItem += AddItem;
            _view.AddItemComment += AddItemComment;
            _view.OpenItemsList += OpenItemsList;
            _view.FindItem += FindItem;
            _view.OpenList += OpenList;
            _view.Find += Find;
            _view.GetFolio += GetFolio;
            _view.OpenClientsList += OpenClientsList;
            _view.FindClient += FindClient;
            _view.Load += Load;
            _view.OpenNote += OpenNote;
            _view.OpenQuotesList += OpenQuotesList;
            _view.OpenUsersList += OpenUsersList;
            _view.FindUser += FindUser;
            _view.ChangeCurrency += ChangeCurrency;
            _view.ToCreditNote += ToCreditNote;
            _view.AddDisccount += AddDiscount;
            _view.RemoveDisccount += RemoveDisccount;

            //Abonos
            _view.AddPayment += AddPayment;
            _view.RemovePayment += RemovePayment;
            _view.StampPayment += StampPayment;
            _view.OpenFiscalPaymentReportView += OpenFiscalPaymentReportView;
            _view.ValidatePayment += ValidatePayment;


            //Sustitucion
            _view.FindInvoice += FindInvoice;
            _view.OpenInvoicesList += OpenInvoicesList;

            _view.FillCombos(_catalogs.ListMonedas(), _catalogs.ListMetodosDePago(), _catalogs.ListFormasDePago().Where(m => m.activa).ToList(), _uses.List().Where(u => u.activo).ToList(), _config.GetDefault().Regimenes.ToList(), _catalogs.ListTiposRelacion(), _bankAccounts.List());
        }

        private void ChangeCurrency()
        {
            try
            {
                Moneda lastCurrency = _view.LastCurrency;

                if (!lastCurrency.isValid())
                {
                    return;
                }

                VMFactura invoice = _view.Invoice;
                invoice.ToCurrency(lastCurrency);

                _view.Show(invoice);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindUser()
        {
            try
            {
                var user = _users.Find(_view.Invoice.Usuario1.nombreDeUsuario);

                if (user.isValid())
                {
                    _view.Show(user);
                }
                else
                {
                    _view.ShowError("No se encontró un usuario con el nombre de usuario proporcionado");
                    _view.Show(new Usuario());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenUsersList()
        {
            try
            {
                IUsersListView usersView = new UsersListView();
                UsersListPresenter usersPresenter = new UsersListPresenter(usersView, _users);

                usersView.ShowWindow();

                if (usersView.User.isValid() && usersView.User.idUsuario.isValid())
                {
                    _view.Show(usersView.User);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenQuotesList()
        {
            try
            {
                IQuotesListView view;
                QuotesListPresenter presenter;

                view = new QuotesListView();
                presenter = new QuotesListPresenter(view, _quotes);

                view.ShowWindow();

                if (view.Quote.isValid() && view.Quote.idCotizacion.isValid())
                {
                    if (view.Quote.idEstatusDeCotizacion != (int)StatusDeCotizacion.Registrada)
                    {
                        throw new Exception("No se puede facturar una cotizacion remisionada, facturada o cancelada");
                    }

                    var factura = new VMFactura(new VMCotizacion(view.Quote));

                    //Se le asigna serie y folio a la factura
                    factura.serie = Session.SerieFacturas.identificador;
                    factura.folio = _invoices.Next(factura.serie);

                    _view.Show(factura);

                    //Se verifica el stock de la factura generada para la cotizacion
                    //Solo cuando se controla inventario
                    if (Modulos.Control_de_Inventario.IsActive())
                    {
                        var itemsWithoutStock = new List<Articulo>();
                        foreach (var d in factura.DetalleDeFactura)
                        {
                            if (d.Articulo.inventariado && _items.Stock(d.idArticulo) < d.cantidad)
                            {
                                itemsWithoutStock.Add(d.Articulo);
                            }
                        }

                        //Si algun articulo no tiene existencia, se muestra aviso y se desactiva el boton de guardar
                        if (!itemsWithoutStock.IsEmpty())
                        {
                            string[] itemCodesWithoutStock = itemsWithoutStock.Select(x => x.codigo).ToArray();
                            _view.ShowError(string.Format("Existencias insuficientes: {0}", string.Join(",", itemCodesWithoutStock)));
                            _view.DisableSaveButton();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenNote()
        {
            try
            {
                DatosExtraPorFactura nota = null;

                if (_view.Invoice.isValid() && _view.Invoice.folio.isValid())
                {
                    //Se busca la nota de la factura
                    nota = _view.Invoice.DatosExtraPorFacturas.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
                }

                nota = nota ?? new DatosExtraPorFactura() { dato = DatoExtra.Nota.ToString() };

                IInvoiceNoteView view;
                InvoiceNotePresenter presenter;

                //Si la factura ya existe, no se puede editar la nota
                view = new InvoiceNoteView(_view.Invoice.idFactura.isValid());
                presenter = new InvoiceNotePresenter(view, nota);

                view.ShowWindow();

                nota = view.Nota;

                //Si la factura ya esta registrada, no se edita la nota
                if (_view.Invoice.isValid() && _view.Invoice.idFactura.isValid())
                {
                    return;
                }

                //Si la factura ya tiene una nota, se actualiza en vez de agregarla
                DatosExtraPorFactura datoFactura = _view.Invoice.DatosExtraPorFacturas.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());

                if (datoFactura.isValid())
                {
                    datoFactura.valor = nota.valor;
                }
                else
                {
                    _view.Invoice.DatosExtraPorFacturas.Add(nota);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Load()
        {
            if (Session.Configuration.Series.Count <= 0)
            {
                _view.ShowError("Antes de facturar debe configurar series de facturación");
                return;
            }

            if (!Session.SerieFacturas.isValid() || !Session.SerieFacturas.idSerie.isValid())
            {
                _view.ShowError("Debe configurar una serie default para facturas antes de realizar registros de las mismas");
            }

            if (!Session.Configuration.CarpetaXml.isValid())
            {
                _view.ShowError("Antes de facturar debe configurar la carpeta para depositar los comprobantes xml");
                return;
            }
            if (!Session.Configuration.CarpetaCbb.isValid())
            {
                _view.ShowError("Antes de facturar debe configurar la carpeta para depositar los codigos bidimensionales");
                return;
            }
            if (!Session.Configuration.CarpetaPdf.isValid())
            {
                _view.ShowError("Antes de facturar debe configurar la carpeta para depositar los comprobantes pdf");
                return;
            }

            var cert = Session.Configuration.Certificados.FirstOrDefault(c => c.activo);
            if (!cert.isValid())
            {
                _view.ShowError("Antes de facturar debe configurar un certificado de sello digital");
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
                var invoice = new VMFactura();
                invoice.fechaHora = DateTime.Now;
                invoice.tipoDeCambio = Session.Configuration.tipoDeCambio;
                invoice.idEstatusDeFactura = (int)StatusDeFactura.Nueva;
                invoice.NotasDeCreditoes = new List<NotasDeCredito>();

                //Si existe una serie default la cargo
                {
                    invoice.serie = Session.SerieFacturas.identificador;
                    invoice.folio = _invoices.Next(invoice.serie);
                }

                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            if (!_view.Invoice.Cliente.codigo.isValid())
                return;

            try
            {
                VMFactura invoice;

                var client = _clients.Find(_view.Invoice.Cliente.codigo);

                if (!client.isValid())
                {
                    _view.ShowError("No existe ningún cliente con ese código");
                    New();
                    return;
                }

                if (client.idCliente.Equals(_view.Invoice.Cliente.idCliente))
                    return;

                //Si se esta facturando una cotizacion, no se debe limpiar la vista
                if (_view.Invoice.Cotizaciones.isValid() && !_view.Invoice.Cotizaciones.IsEmpty())
                {
                    invoice = _view.Invoice;
                    invoice.Cliente = client;

                    //Se recalculan los precios para el nuevo cliente
                    invoice.DetalleDeFactura = _invoices.UpdatePrices(_view.Invoice.DetalleDeFactura, client);

                    //Se recalcula el total
                    invoice.UpdateAccount();

                    _view.Show(invoice);
                    return;
                }

                if (_view.Invoice.folio.isValid())
                    invoice = new VMFactura(client, _view.Invoice.serie, _view.Invoice.folio.ToString(), Session.Configuration.tipoDeCambio);
                else
                    invoice = new VMFactura() { Cliente = client, idCliente = client.idCliente, serie = _view.Invoice.serie, Usuario1 = client.Usuario };

                _view.Show(invoice);
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

                if (view.Client.idCliente.isValid())
                {
                    VMFactura invoice;

                    //Si se esta facturando una cotizacion, no se debe limpiar la vista
                    if (_view.Invoice.Cotizaciones.isValid() && !_view.Invoice.Cotizaciones.IsEmpty())
                    {
                        invoice = _view.Invoice;
                        invoice.Cliente = view.Client;

                        //Se recalculan los precios para el nuevo cliente
                        invoice.DetalleDeFactura = _invoices.UpdatePrices(_view.Invoice.DetalleDeFactura, view.Client);

                        //Se recalcula el total
                        invoice.UpdateAccount();

                        _view.Show(invoice);
                        return;
                    }

                    if (_view.Invoice.folio.isValid())
                        invoice = new VMFactura(view.Client, _view.Invoice.serie, _view.Invoice.folio.ToString(), Session.Configuration.tipoDeCambio);
                    else
                        invoice = new VMFactura() { Cliente = view.Client, idCliente = view.Client.idCliente, Usuario1 = view.Client.Usuario };

                    _view.Show(invoice);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void GetFolio()
        {
            if (!_view.Invoice.serie.isValid())
                return;

            try
            {
                VMFactura invoice;

                var folio = _invoices.Next(_view.Invoice.serie);

                if (_view.Invoice.Cliente.idCliente.isValid())
                    invoice = new VMFactura(_view.Invoice.Cliente, _view.Invoice.serie, folio.ToString(), Session.Configuration.tipoDeCambio);
                else
                    invoice = new VMFactura() { serie = _view.Invoice.serie, folio = folio, tipoDeCambio = Session.Configuration.tipoDeCambio, fechaHora = DateTime.Now };

                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                Load();
            }
        }

        private void Find()
        {
            if (!_view.Invoice.serie.isValid() || !_view.Invoice.folio.isValid())
                return;

            try
            {
                var invoice = _invoices.Find(_view.Invoice.serie, _view.Invoice.folio.ToString());

                if (invoice.isValid()) //Voy a mostrar una factura existente
                    _view.Show(new VMFactura(invoice));
                else
                    _view.Show(new VMFactura(_view.Invoice.Cliente, _view.Invoice.serie, _invoices.Next(_view.Invoice.serie).ToString(), Session.Configuration.tipoDeCambio));

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
                IInvoicesListView view;
                InvoicesListPresenter presenter;

                view = new InvoicesListView();
                presenter = new InvoicesListPresenter(view, _invoices);

                view.ShowWindow();

                if (view.Invoice.isValid() && view.Invoice.idFactura.isValid())
                {
                    //Obtiene la factura de la base de datos
                    var dbInvoice = _invoices.Find(view.Invoice.idFactura);

                    if (dbInvoice.isValid() && dbInvoice.idFactura.isValid())
                    {
                        _view.Show(new VMFactura(dbInvoice));
                    }
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.Invoice.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.CurrentItem.Articulo.codigo.isValid())
                return;

            if (!_view.Invoice.Moneda.isValid())
            {
                _view.ShowMessage("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Invoice.tipoDeCambio.isValid())
            {
                _view.ShowMessage("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            try
            {
                //Toda esta parte se trata de obtener el artículo que el cliente busca
                var item = new VMArticulo();
                var items = _items.FindAllForCustomer(_view.CurrentItem.Articulo.codigo, _view.Invoice.idCliente).Select(a => new VMArticulo(a)).ToList();

                if (items.IsEmpty())
                {
                    _view.ShowError("No existe ningún artículo con el código especificado");
                    _view.ClearItem();
                    return;
                }

                if (!items.HasSingleItem())
                {
                    //Se muestra los artículos que coinciden
                    IItemSelectionView view;
                    ItemSelectionPresenter presenter;

                    view = new ItemSelectionView();
                    presenter = new ItemSelectionPresenter(view, items);

                    view.ShowWindow();

                    //Se valida que se haya escogido algún artículo
                    if (!view.Item.isValid() || !view.Item.idArticulo.isValid())
                    {
                        _view.ClearItem();
                        throw new Exception("No se seleccionó ninguna de las coincidencias");
                    }

                    item = view.Item;
                }
                else
                {
                    item = items.First();
                }

                //Si lo encontré preparo un Detalle de Venta
                var detail = GetDetail(item, _view.Invoice.Cliente.idListaDePrecio);

                //Lo muestro
                _view.Show(detail);

                //Se muestra la existencia
                var existencia = item.inventariado ? _items.Stock(detail.idArticulo) : 0.0m;
                _view.ShowStock(existencia);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenItemsList()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las facturas registradas");
                return;
            }

            if (!_view.Invoice.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Invoice.Moneda.isValid())
            {
                _view.ShowMessage("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Invoice.tipoDeCambio.isValid())
            {
                _view.ShowMessage("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    _view.Show(GetDetail(item, _view.Invoice.Cliente.idListaDePrecio));
                    //Tambien se muestra la existencia
                    var existencia = item.inventariado ? _items.Stock(view.Item.idArticulo) : 0.0m;
                    _view.ShowStock(existencia);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddItem()
        {
            if (!_view.CurrentItem.Articulo.idArticulo.isValid())
            {
                _view.ClearItem();
                return;
            }

            try
            {
                //Me lo traigo para manipulación local
                var detail = _view.CurrentItem;
                detail.Articulo = _items.Find(detail.Articulo.idArticulo);

                //Valido que la existencia actual del artículo sea mayor o igual a la cantidad a vender
                //Solo aplica a inventariados cuando esta activo el control de inventario
                if (detail.Articulo.inventariado && Modulos.Control_de_Inventario.IsActive())
                {
                    var currentStock = _items.Stock(_view.CurrentItem.idArticulo);
                    if (currentStock < detail.cantidad)
                    {
                        _view.ShowMessage("Existencia insuficiente");
                        return;
                    }
                }

                var unitCost = detail.Articulo.costoUnitario.ToDocumentCurrency(detail.Articulo.Moneda, _view.Invoice.Moneda, _view.Invoice.tipoDeCambio);
                //Si el precio unitario fue modificado, calcular el porcentaje de descuento
                //Obtengo el precio de venta para el cliente
                var stockPrice = Math.Round(Operations.CalculatePriceWithoutTaxes(unitCost, detail.Articulo.Precios.First(p => p.idListaDePrecio.Equals(_view.Invoice.Cliente.idListaDePrecio)).utilidad), 2);
                if (detail.precioUnitario != stockPrice)
                {
                    detail.descuento = Operations.CalculateDiscount(stockPrice, detail.precioUnitario);
                    //Valido el precio unitario del artículo, si tiene algún descuento válido que no sobrepase el límite permitido
                    if (detail.descuento > Session.LoggedUser.descuento)
                    {
                        _view.ShowMessage("No tiene privilgios suficientes para otorgar el precio actual");
                        return;
                    }
                }

                //Si el módulo predial esta activo y el artículo lo tiene dentro de sus clasificaciones le pido seleccionar la cuenta
                if (Modulos.Predial.IsActive() && detail.Articulo.Clasificaciones.HasPredial())
                {
                    IPropertyAccountsListView listView;
                    PropertyAccountsListPresenter listPresenter;

                    listView = new PropertyAccountsListView();
                    listPresenter = new PropertyAccountsListPresenter(listView, _propertyAccounts);

                    listView.ShowWindow();

                    if (listView.Account.idCuentaPredial.isValid())
                        detail.CuentaPredialPorDetalle = new CuentaPredialPorDetalle() { cuenta = listView.Account.cuenta };
                }


                //Si ya existe el artículo con el mismo precio unitario, sumo la cantidad a ese registro
                var invoice = _view.Invoice;
                var exists = invoice.DetalleDeFactura.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo) && d.precioUnitario.Equals(detail.precioUnitario));
                if (exists.isValid())
                    exists.cantidad += detail.cantidad;
                else
                {
                    //Si no existe en el detalle actual, agrego el detalle a la lista
                    //Busco los impuestos que el articulo debe pagar
                    detail.Impuestos = _items.Find(detail.idArticulo).Impuestos;
                    invoice.DetalleDeFactura.Add(detail);
                }

                //Si se modifican los articulos de la factura, se eliminan los abonos que puedan estar registrados
                invoice.AbonosDeFacturas = new List<AbonosDeFactura>();

                //Hago el calculo de la cuenta Subtotal, Impuestos, Total
                invoice.UpdateAccount();

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra la nueva venta completa con la nueva lista de detalles y la cuenta
                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddItemComment()
        {
            try
            {
                var item = _view.SelectedItem;

                if (!item.isValid())
                    return;

                IItemCommentView view;

                if (item.ComentariosPorDetalleDeFactura.isValid() && item.ComentariosPorDetalleDeFactura.comentario.isValid())
                    view = new ItemCommentView(item.ComentariosPorDetalleDeFactura.comentario);
                else
                    view = new ItemCommentView();
                ItemCommentPresenter presenter = new ItemCommentPresenter(view);

                view.ShowWindow();
                if (view.Comment.isValid())
                    item.ComentariosPorDetalleDeFactura = new ComentariosPorDetalleDeFactura() { comentario = view.Comment };
                else
                    item.ComentariosPorDetalleDeFactura = null;
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ViewTaxDetails()
        {
            try
            {
                if (_view.SelectedTax.idImpuesto < 0)
                {
                    _view.ShowError("No existe ningún impuesto seleccionado");
                    return;
                }

                _view.Show(_view.SelectedTax);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RemoveItem()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las facturas registradas");
                return;
            }

            if (!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var invoice = _view.Invoice;
                invoice.DetalleDeFactura.Remove(item);

                //Recalculo
                invoice.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectItem()
        {
            if (_view.IsDirty)
            {
                _view.ShowError("No se pueden editar las facturas registradas");
                return;
            }

            if (!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var invoice = _view.Invoice;
                invoice.DetalleDeFactura.Remove(item);

                invoice.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(invoice);

                //Muestro en edición el artículo seleccionado
                _view.Show(item);
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
                //Establezco los defaults
                var invoice = new VMFactura();
                invoice.fechaHora = DateTime.Now;
                invoice.tipoDeCambio = Session.Configuration.tipoDeCambio;
                invoice.idEstatusDeFactura = (int)StatusDeFactura.Nueva;
                invoice.Usuario1 = new Usuario();

                //Si existe una serie default la cargo
                if (Session.SerieFacturas.isValid())
                {
                    invoice.serie = Session.SerieFacturas.identificador;
                    invoice.folio = _invoices.Next(invoice.serie);
                }

                _view.Show(invoice);
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

            if (_view.Invoice.NotasDeCreditoes.Where(nc => nc.idEstatusDeNotaDeCredito != (int)StatusDeNotaDeCredito.Cancelada && nc.idEstatusDeNotaDeCredito != (int)StatusDeNotaDeCredito.Anulada).Count() > 0)
            {
                _view.ShowMessage("No puede cancelar una factura que contiene notas de crédito asociadas");
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

                var invoice = new VMFactura(_invoices.Cancel(_view.Invoice.idFactura, view.Reason));

                _view.ShowMessage("Factura cancelada exitosamente");

                //Aquí genero el pdf si fue una cancelación fiscal
                if (invoice.TimbresDeFactura.isValid())
                {
                    var receipt = new VMAcuse(invoice, Session.Configuration);
                    var report = Reports.FillReport(receipt);
                    //_view.ShowMessage("Se requiere el siguiente archivo para generar Acuse de Cancelacion:\n\n"+report.Archivo); //JCRV Para ver que documento y en donde se esta buscando
                    report.Export(string.Format("{0}\\{1}{2}-Acuse Cancelación.pdf", Session.Configuration.CarpetaPdf, invoice.serie, invoice.folio));
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
            if (!_view.IsDirty)
            {
                _view.ShowMessage("No es posible imprimir una factura que no ha sido registrada");
                return;
            }

            if (!_view.Invoice.TimbresDeFactura.isValid() || !_view.Invoice.TimbresDeFactura.idTimbreDeFactura.isValid())
            {
                _view.ShowMessage("No es posible imprimir una factura que no ha sido timbrada");
                return;
            }

            try
            {
                IInvoicePrintView view;
                InvoicePrintPresenter presenter;

                view = new InvoicePrintView(_view.Invoice);
                presenter = new InvoicePrintPresenter(view, _invoices);

                view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            var invoice = _view.Invoice;
            var discountNotes = invoice.NotasDeDescuentoes.ToList();

            if (!Session.Station.isValid())
            {
                _view.ShowError("Este equipo no cuenta con ninguna estación asociada");
                return;
            }

            if (!IsInvoiceValid(invoice, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le agrego quien la registra
                invoice.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                invoice.Usuario = Session.LoggedUser;

                /* JCRV Se agrega que se muestra la ruta de donde se esta leyendo el archivo de configuracion 
                var config_path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
                var usuario = Session.Configuration.usuarioPAC;
                var pass = Session.Configuration.contraseñaPAC;
                var csd = ConfigurationManager.AppSettings["CSD"].ToString();
                var msg = "UsuarioPAC: " + usuario + "\n\ncontraseñaPAC " + pass + "\n\nRuta CSD: " + csd + "\n\n"; 
                _view.ShowMessage(msg+"path de configuracion: " + config_path);
                 Borrar */

                //Actualizo la cuenta
                invoice.UpdateAccount();

                //Se verifica si el cliente tiene suficiente credito disponible
                if (!invoice.Cliente.limiteCredito.HasValue || invoice.Cliente.limiteCredito.Equals(0.0) || !_clients.HasAvailableCredit(invoice.Cliente, invoice.Moneda, invoice.Saldo))
                {
                    //Se envia aviso de que el cliente esta excediendo su limite de credito
                    _view.ShowMessage("El cliente está excediendo su límite de crédito");

                    //Si no tiene suficiente credito y el usuario no tiene permisos totales en facturas,  se muestra la ventana de autenticacion
                    if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "InvoicesPresenter", false))
                    {
                        //Se muestra la ventana de autorizacion
                        IAuthorizationView authorizationView = new AuthorizationView();
                        AuthorizationPresenter authorizationPresenter = new AuthorizationPresenter(authorizationView, _users, _security, AccesoRequerido.Total, "InvoicesPresenter");

                        authorizationView.ShowWindow();

                        if (!authorizationView.Authorized)
                        {
                            //Se no efectuó la autorizacion
                            _view.ShowError("No se autorizó la transacción");
                            return;
                        }
                    }
                }

                //Actualizo el folio
                invoice.folio = _invoices.Next(invoice.serie);

                //Obtiene los datos extras necesarios para la addenda
                bool requiresAddenda = Modulos.Addendas.IsActive() && !invoice.Cliente.AddendaDeClientes.IsEmpty();
                if (requiresAddenda)
                {
                    var addenda = invoice.Cliente.AddendaDeClientes.First().Addenda;

                    //Se obtienen los datos extra para la addenda del cliente
                    var datosExtra = GetDatosExtra(addenda);

                    //Se agregan los datos extra de la addenda a la factura
                    datosExtra.ForEach(x => invoice.DatosExtraPorFacturas.Add(x));
                }

                //Pedimentos
                if (invoice.DetalleDeFactura.Any(a => a.Articulo.importado))
                {
                    ICustomsApplicationsExitView viewPedimentos = new CustomsApplicationsExitView(invoice.DetalleDeFactura.GetOnlyImported());
                    CustomsApplicationsExitPresenter presenterPedimentos = new CustomsApplicationsExitPresenter(viewPedimentos, _customsApplications, _items);

                    viewPedimentos.ShowWindow();

                    //Debo asociar los pedimentos al detalle de mi factura
                    //Itero entre cada uno de los artículos que necesitaba pedimentos
                    foreach (var p in viewPedimentos.Items.Where(p => p.Asociados > 0.0m))
                    {
                        //Obtengo el detalle correspondiente al artículo
                        var d = invoice.DetalleDeFactura.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo) && i.precioUnitario.Equals(p.PrecioUnitario));
                        //Le asigno a ese detalle los pedimentos y cantidades correspondientes
                        p.Pedimentos.ForEach(pa => d.PedimentoPorDetalleDeFacturas.Add(new PedimentoPorDetalleDeFactura() { idPedimento = pa.IdPedimento, cantidad = pa.Cantidad, Pedimento = _customsApplications.Find(pa.IdPedimento) }));
                    }
                }

                var periodicidad = invoice.Periodicidad;

                //Agrego la factura
                invoice = new VMFactura(_invoices.Add(invoice));
                invoice.Periodicidad = periodicidad;

                //Timbro la factura
                invoice = new VMFactura(_invoices.Stamp(invoice, requiresAddenda));

                // Aquí debería timbrar los abonos
                if (invoice.MetodosPago.idMetodoPago.Equals((int)MetodoDePago.Pago_en_parcialidades_o_diferido))
                {
                    foreach (var a in invoice.AbonosDeFacturas.Where(a => !a.TimbresDeAbonosDeFactura.isValid()))
                    {
                        _invoicePayments.Stamp(invoice, a);
                    }
                }

                //Aqui se registran y timbra la nota de credito
                if (!discountNotes.IsEmpty())
                {
                    var creditNote = _creditNotes.Add(invoice, discountNotes);

                    _creditNotes.Stamp(new VMNotaDeCredito(creditNote));
                }

                _view.ShowMessage("Factura registrada exitosamente");

                //Aquí genero el pdf de la factura
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

                //Ahora el pdf de la nota de crédito
                if (!invoice.NotasDeCreditoes.IsEmpty())
                {
                    var nc = invoice.NotasDeCreditoes.First();
                    var creditNoteReport = Reports.FillReport(new VMRNotaDeCredito(new VMNotaDeCredito(nc), Session.Configuration));
                    creditNoteReport.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, nc.serie, nc.folio));
                    //Aqui debo enviar el correo si Guardian esta activado
                    if (Modulos.Envio_De_Correos.IsActive())
                        _mailer.SendMail(nc);
                }

                IInvoicePrintView view;
                InvoicePrintPresenter presenter;

                view = new InvoicePrintView(invoice);
                presenter = new InvoicePrintPresenter(view, _invoices);

                view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Stamp()
        {
            var abonos = new List<AbonosDeFactura>();

            //Si ya tiene timbre ignoro
            if (_view.Invoice.TimbresDeFactura.isValid() && _view.Invoice.TimbresDeFactura.idTimbreDeFactura.isValid())
                return;

            try
            {
                var invoice = _view.Invoice;

                //Actualizo la cuenta
                invoice.UpdateAccount();

                //Verifica si la factura requiere addenda o no
                bool requiresAddenda = Modulos.Addendas.IsActive() && !invoice.Cliente.AddendaDeClientes.IsEmpty();

                //Timbro la factura
                invoice = new VMFactura(_invoices.Stamp(invoice, requiresAddenda));

                _view.ShowMessage("Factura timbrada exitosamente");

                //Aquí genero el pdf
                Usuario user = GetSellerForInvoice(invoice);
                var report = Reports.FillReport(new VMRFactura(invoice, Session.Configuration, user));
                report.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, invoice.serie, invoice.folio));

                //Aqui debo enviar el correo si Guardian esta activado
                if (Modulos.Envio_De_Correos.IsActive())
                    _mailer.SendMail(invoice);

                IInvoicePrintView view;
                InvoicePrintPresenter presenter;

                view = new InvoicePrintView(invoice);
                presenter = new InvoicePrintPresenter(view, _invoices);

                view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                _view.ShowError(ex.Message);
            }
        }

        #region Notas de crédito

        private void RemoveDisccount()
        {
            try
            {
                var vm = _view.Invoice;
                var itemToRemove = _view.SelectedCreditNote;

                if (itemToRemove.isValid())
                {
                    vm.NotasDeCreditoes = vm.NotasDeCreditoes.Where(x => x.idNotaDeCredito != itemToRemove.idNotaDeCredito).ToList();
                    vm.UpdateAccount();
                    _view.Show(vm);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void AddDiscount()
        {

            try
            {
                if (_view.IsDirty)
                    throw new Exception("No es posible agregar notas de crédito por descuento a una factura ya registrada");

                if (!_view.Invoice.idCliente.isValid())
                    throw new Exception("Debe seleccionar un cliente para realizar esta búsqueda");

                var invoice = _view.Invoice;

                if (invoice.Saldo.Equals(0))
                    throw new Exception("Debe tener un saldo pendiente para realizar esta búsqueda");

                IDiscountNotesListView view;
                DiscountNotesListPresenter presenter;

                view = new DiscountNotesListView(true, true);
                presenter = new DiscountNotesListPresenter(view, _discountNotes, invoice.idCliente);

                view.ShowWindow();

                //Si no seleccionó nada solo lo regreso
                if (!view.DiscountNote.isValid() || !view.DiscountNote.idNotaDeDescuento.isValid())
                    return;

                //Si no pasa la validación le envío un mensaje de error y el mismo catch hace el trabajo de sacarlo de la linea de ejecución
                if (view.DiscountNote.idFactura.isValid())
                    throw new Exception("No se puede agregar una nota de crédito asociada a otra factura");

                //No puede agregar una nota por un monto mayor al saldo
                var monto = view.DiscountNote.monto.ToDocumentCurrency(view.DiscountNote.Moneda, invoice.Moneda, invoice.tipoDeCambio);

                if (monto > invoice.Saldo)
                    throw new Exception("No es posible agregar una nota de crédito mayor al saldo de la factura");

                if (invoice.NotasDeDescuentoes.Any(x => x.idNotaDeDescuento == view.DiscountNote.idNotaDeDescuento))
                {
                    throw new Exception("La nota de crédito ya esta asignada a la factura");
                }

                //Si llega aquí entonces todo es válido y puedo continuar
                invoice.NotasDeDescuentoes.Add(view.DiscountNote);
                invoice.UpdateAccount();
                _view.Show(invoice);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void ToCreditNote()
        {
            try
            {
                var invoice = _view.Invoice;

                //La factura debe estar timbrada y sin cancelar
                if (!invoice.TimbresDeFactura.isValid() || !invoice.TimbresDeFactura.UUID.isValid() || !invoice.idFactura.isValid() || invoice.idEstatusDeFactura == (int)StatusDeFactura.Anulada ||
                    invoice.idEstatusDeFactura == (int)StatusDeFactura.Cancelada)
                {
                    _view.ShowError("No se puede crear una nota de crédito de una factura sin timbrar o cancelada");
                    return;
                }

                var creditNote = new VMNotaDeCredito(invoice);

                ICreditNotesView view;
                CreditNotesPresenter presenter;

                view = new CreditNotesView();
                presenter = new CreditNotesPresenter(view, _creditNotes, _catalogs, _clients, _items, _pricesList, _config, _bankAccounts, _mailer, _customsApplications, _users, _security);

                view.ShowWindowIndependent();

                creditNote.fechaHora = DateTime.Now;
                creditNote.serie = Session.SerieNotasDeCredito.identificador;
                creditNote.folio = _creditNotes.Next(creditNote.serie);
                creditNote.UpdateAccount();
                creditNote.UpdateDescription();

                view.Show(creditNote);
                New();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        #endregion

        #region Abonos

        private void AddPayment()
        {
            if (_view.IsDirty)
            { 
                _view.ShowError("Para agregar abonos a una factura registrada debe hacerlo desde la pantalla de abonos");
                return;
            }

            var payment = _view.Payment;
            if (!payment.idFormaPago.isValid())
            {
                _view.ShowError("Debe seleccionar la forma de pago del abono");
                return;
            }

            if (!payment.idMoneda.isValid())
            {
                _view.ShowError("Debe seleccionar la moneda del abono");
                return;
            }

            if (!payment.monto.isValid())
            {
                _view.ShowError("La cantidad a abonar no es válida");
                return;
            }

            //BR: 3.3 Si la forma de pago es bancarizada, debe llevar, sino puede quedarse en blanco
            if (payment.FormasPago.bancarizado && !payment.idCuentaBancaria.Value.isValid())
            {
                _view.ShowError(string.Format("La forma de pago {0} requiere una cuenta beneficiaria", payment.FormasPago.descripcion));
                return;
            }

            //La fecha de registro del abono no puede ser una fecha futura
            if (payment.fechaHora.Date > DateTime.Now.ToNextMidnight())
            {
                _view.ShowError("El abono no puede ser de una fecha futura");
                return;
            }

            //ni anterior al abono previo
            var prev = _view.Invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado).OrderByDescending(a => a.fechaHora).FirstOrDefault();
            if (prev.isValid() && payment.fechaHora.Date < prev.fechaHora.Date)
            {
                _view.ShowError("El abono no puede ser anterior al abono previo");
                return;
            }
            else//ni anterior al registro de la factura
            {
                if (payment.fechaHora.Date < _view.Invoice.fechaHora.Date)
                {
                    _view.ShowError("El abono no puede ser anterior al registro de la factura");
                    return;
                }
            }

            //Si el abono anterior (no cancelado) no ha sido timbrado y debería timbrarse.
            //No es valido si no tiene un timbre y tampoco forma parte de un pago
            if (prev.isValid() && ((!prev.TimbresDeAbonosDeFactura.isValid() || !prev.TimbresDeAbonosDeFactura.idTimbreDeAbonoDeFactura.isValid()) && !prev.idPago.HasValue))
            {
                _view.ShowError("No es posible agregar un nuevo abono sin antes timbrar el anterior");
                return;
            }

            try
            {
                var invoice = _view.Invoice;

                //Actualizo la cuenta
                invoice.UpdateAccount();

                //Si el monto (convertido a la moneda de la factura) que voy a abonar es mayor al saldo total reducir el monto al saldo
                if (payment.monto.ToDocumentCurrency(payment.Moneda, invoice.Moneda, invoice.tipoDeCambio) > invoice.Saldo)
                {
                    if (payment.idMoneda.Equals(invoice.idMoneda))
                        payment.monto = invoice.Saldo;
                    else
                        payment.monto = invoice.Saldo.ToDocumentCurrency(invoice.Moneda, payment.Moneda, invoice.tipoDeCambio);
                }

                //Si ya esta saldada la cuenta ya no puede agregar mas abonos
                if (payment.monto <= 0.0m)
                {
                    _view.ShowMessage("La cuenta ya esta saldada, no es posible realizar más abonos");
                    return;
                }

                //Empresa desde la que se registra el abono
                payment.idEmpresa = Session.Station.idEmpresa;

                //Si es una factura ya registrada, registro y timbro el abono inmediatamente
                if (_view.IsDirty)
                {
                    //Agrego el abono
                    payment = _invoicePayments.Add(payment); //automaticamente se agrega a la lista de abonos de la factura por el UOW

                    //Si no esta en una sola exhibición debe timbrar el abono
                    if (!_view.Invoice.idMetodoPago.Equals((int)MetodoDePago.Pago_en_una_sola_exhibicion))
                    {
                        payment = _invoicePayments.Stamp(_view.Invoice, payment);
                        //Genero el pdf
                        var paymentReporte = Reports.FillReport(new VMPago(payment, Session.Configuration));
                        paymentReporte.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio));
                    }

                    _view.ShowMessage("{0} exitosamente", !_view.Invoice.idMetodoPago.Equals((int)MetodoDePago.Pago_en_una_sola_exhibicion) ? "Pago registrado" : "Abono registrado");

                    //Aqui debo enviar el correo si Guardian esta activado
                    if (Modulos.Envio_De_Correos.IsActive())
                        _mailer.SendMail(payment);
                }
                else
                    invoice.AbonosDeFacturas.Add(payment);

                //Muestro los abonos
                _view.Show(invoice.AbonosDeFacturas.ToList());

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RemovePayment()
        {
            if (!_view.Selected.isValid())
            {
                _view.ShowError("No hay ningún abono seleccionado para cancelar");
                return;
            }

            try
            {
                var invoice = _view.Invoice;

                //Si el abono viene de un abono multiple, se muestra aviso
                if (_view.Selected.Pago.isValid() && _view.Selected.Pago.idPago.isValid())
                {
                    _view.ShowMessage(string.Format("Este abono pertenece al comprobante de pago múltiple {0}{1}", _view.Selected.Pago.serie, _view.Selected.Pago.folio));
                    return;
                }

                if (_view.Selected.idAbonoDeFactura.isValid()) //Lo cancelo si ya estaba registrado
                {
                    var payment = _invoicePayments.Cancel(_view.Selected.idAbonoDeFactura);
                    //Aquí genero el pdf si fue una cancelación fiscal
                    var receipt = new VMAcuse(payment, Session.Configuration);
                    var report = Reports.FillReport(receipt);
                    report.Export(string.Format("{0}\\{1}{2}-Acuse Cancelación.pdf", Session.Configuration.CarpetaPdf, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio));
                    _view.ShowMessage("Abono cancelado exitosamente");
                }
                else // Si no solo lo elimino
                {
                    invoice.AbonosDeFacturas.Remove(_view.Selected);
                }

                _view.Show(invoice.AbonosDeFacturas.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void StampPayment()
        {
            if (!_view.Selected.isValid())
            {
                _view.ShowError("No hay ningún abono seleccionado para timbrar");
                return;
            }

            //Si el abono viene de un abono multiple, se muestra aviso
            if (_view.Selected.Pago.isValid() && _view.Selected.Pago.idPago.isValid())
            {
                _view.ShowMessage(string.Format("Este abono pertenece al comprobante de pago múltiple {0}{1}", _view.Selected.Pago.serie, _view.Selected.Pago.folio));
                return;
            }

            if (_view.Selected.TimbresDeAbonosDeFactura.isValid() && _view.Selected.TimbresDeAbonosDeFactura.UUID.isValid())
            {
                _view.ShowError("Este abono ya se encuentra timbrado");
                return;
            }

            try
            {
                var payment = _view.Selected;

                payment = _invoicePayments.Stamp(_view.Invoice, payment);
                _view.ShowMessage("Pago timbrado exitosamente");

                //Genero el pdf
                var paymentReporte = Reports.FillReport(new VMPago(payment, Session.Configuration));
                paymentReporte.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio));

                //Aqui debo enviar el correo si Guardian esta activado
                if (Modulos.Envio_De_Correos.IsActive())
                    _mailer.SendMail(payment);

                //Muestro los abonos
                _view.Show(_view.Invoice.AbonosDeFacturas.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex);
            }
        }

        private void OpenFiscalPaymentReportView()
        {
            if (!_view.Selected.isValid())
            {
                _view.ShowError("No hay ningún abono seleccionado para reportear");
                return;
            }

            //Si el abono viene de un abono multiple, se muestra aviso
            if (_view.Selected.Pago.isValid() && _view.Selected.Pago.idPago.isValid())
            {
                _view.ShowMessage(string.Format("Este abono pertenece al comprobante de pago múltiple {0}{1}", _view.Selected.Pago.serie, _view.Selected.Pago.folio));
                return;
            }

            try
            {
                var payment = _view.Selected;

                IFiscalPaymentPrintView view;
                FiscalPaymentPrintPresenter presenter;

                view = new FiscalPaymentPrintView(payment);
                presenter = new FiscalPaymentPrintPresenter(view, _invoicePayments, _payments);

                view.ShowWindow();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex);
            }
        }

        private void ValidatePayment()
        {

            try
            {
                //Si tiene una moneda distinta realizar el cambio de divisas y mostrarlo
                var payment = _view.Payment;
                if (payment.idMoneda.Equals(_view.Invoice.idMoneda))
                    return;

                ICurrencyExchangeView view;
                CurrencyExchangePresenter presenter;

                view = new CurrencyExchangeView(payment);
                presenter = new CurrencyExchangePresenter(view, _catalogs);

                view.ShowWindow();

                _view.Show(view.PaymentExchange);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Sustitucion

        private void OpenInvoicesList()
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
                    //Obtiene la factura de la base de datos
                    var dbInvoice = _invoices.Find(view.Invoice.idFactura);

                    if (dbInvoice.isValid() && dbInvoice.idFactura.isValid())
                    {
                        if ((!dbInvoice.TimbresDeFactura.isValid() || !dbInvoice.TimbresDeFactura.UUID.isValid()))
                        {
                            throw new Exception("Esta factura no cuenta con un timbre fiscal, por lo que no puede relacionarse");
                        }

                        _view.Show(dbInvoice);
                    }
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindInvoice()
        {
            if (!_view.Invoice.Factura1.serie.isValid() || !_view.Invoice.Factura1.folio.isValid())
                return;

            try
            {
                var invoice = _invoices.Find(_view.Invoice.Factura1.serie, _view.Invoice.Factura1.folio.ToString());

                if (!invoice.isValid())
                    throw new Exception("No existe ninguna factura con ese folio");

                if (!invoice.TimbresDeFactura.isValid() || !invoice.TimbresDeFactura.UUID.isValid())
                    throw new Exception("Esta factura no cuenta con un timbre fiscal, por lo que no puede relacionarse");

                _view.Show(invoice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Private Utility Functions

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

        private bool IsInvoiceValid(VMFactura invoice, out string error)
        {

            if (!invoice.Cliente.codigo.isValid())
            {
                error = "El código del cliente no es válido";
                return false;
            }

            if (!invoice.idCliente.isValid())
            {
                error = "El cliente no es válida";
                return false;
            }

            if (!invoice.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if (!invoice.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio es válido";
                return false;
            }

            if (!invoice.fechaHora.isValid())
            {
                error = "La fecha no es válida";
                return false;
            }

            if (!invoice.serie.isValid())
            {
                error = "La serie no es válida";
                return false;
            }

            if (!invoice.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (invoice.DetalleDeFactura.Count.Equals(0))
            {
                error = "Los conceptos no son válidos";
                return false;
            }

            if (!invoice.idMetodoPago.isValid())
            {
                error = "La forma de pago no es válida";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private VMDetalleDeFactura GetDetail(Articulo item, int idPriceList)
        {
            try
            {

                //Si lo encontré preparo un Detalle de Venta
                var detail = new VMDetalleDeFactura();
                detail.Articulo = item;
                detail.idArticulo = item.idArticulo;
                detail.cantidad = 1.0m;
                detail.Impuestos = item.Impuestos;

                //Se calcula el precio sin descuento
                detail.descuento = 0.0m;

                //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
                var unitCost = item.costoUnitario.ToDocumentCurrency(item.Moneda, _view.Invoice.Moneda, _view.Invoice.tipoDeCambio);
                detail.precioUnitario = Operations.CalculatePriceWithoutTaxes(unitCost, item.Precios.First(p => p.idListaDePrecio.Equals(idPriceList)).utilidad);

                return detail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Datos Extra

        private List<DatosExtraPorFactura> GetDatosExtra(Addenda addenda)
        {
            try
            {
                //List<DatosExtraPorFactura> datosExtra = _view.Invoice.DatosExtraPorFacturas.ToList();
                IAddendaView view = null;

                switch (addenda.idAddenda)
                {
                    case (int)Addendas.Gayosso:
                        AddendaGayossoPresenter presenterGayosso;

                        view = new AddendaGayossoView();
                        presenterGayosso = new AddendaGayossoPresenter((AddendaGayossoView)view);
                        break;
                    case (int)Addendas.Jardines:
                        AddendaJardinesPresenter presenterJardines;

                        view = new AddendaJardinesView();
                        presenterJardines = new AddendaJardinesPresenter((AddendaJardinesView)view);
                        break;
                    case (int)Addendas.Calimax:
                        AddendaCalimaxPresenter presenterCalimax;

                        view = new AddendaCalimaxView();
                        presenterCalimax = new AddendaCalimaxPresenter((AddendaCalimaxView)view, _catalogs);
                        break;
                    case (int)Addendas.ComercialMexicana:
                        AddendaComercialMexicanaPresenter presenterComercialMexicana;

                        view = new AddendaComercialMexicanaView();
                        presenterComercialMexicana =
                            new AddendaComercialMexicanaPresenter((AddendaComercialMexicanaView)view, _catalogs);
                        break;
                }

                view.ShowWindow();

                //Se agregan los datos extras obtenidos a los del invoice
                //datosExtra.AddRange(view.DatosExtra);

                return view.DatosExtra;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
