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
using System.Linq;
namespace Aprovi.Presenters
{
    public class CreditNotesPresenter
    {
        private ICreditNotesView _view;
        private ICatalogosEstaticosService _catalogs;
        private IClienteService _clients;
        private IArticuloService _items;
        private IListaDePrecioService _pricesList;
        private IConfiguracionService _config;
        private ICuentaBancariaService _bankAccounts;
        private IEnvioDeCorreoService _mailer;
        private IPedimentoService _customsApplications;
        private IUsuarioService _users;
        private ISeguridadService _security;
        private INotaDeCreditoService _creditNotes;

        public CreditNotesPresenter(ICreditNotesView view,INotaDeCreditoService creditNotes, ICatalogosEstaticosService catalogs, IClienteService clients, IArticuloService items, IListaDePrecioService pricesList, IConfiguracionService configuration, ICuentaBancariaService accounts, IEnvioDeCorreoService mailer, IPedimentoService customsApplications, IUsuarioService users, ISeguridadService security)
        {
            _view = view;
            _catalogs = catalogs;
            _clients = clients;
            _items = items;
            _pricesList = pricesList;
            _config = configuration;
            _bankAccounts = accounts;
            _mailer = mailer;
            _customsApplications = customsApplications;
            _users = users;
            _security = security;
            _creditNotes = creditNotes;

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
            _view.OpenItemsList += OpenItemsList;
            _view.FindItem += FindItem;
            _view.OpenList += OpenList;
            _view.Find += Find;
            _view.GetFolio += GetFolio;
            _view.OpenClientsList += OpenClientsList;
            _view.FindClient += FindClient;
            _view.Load += Load;
            _view.OpenNote += OpenNote;
            
            _view.FillCombos(_catalogs.ListMonedas(), _catalogs.ListFormasDePago().Where(m => m.activa).ToList(),_config.GetDefault().Regimenes.ToList(), _bankAccounts.List());
        }

        private void OpenNote()
        {
            try
            {
                DatosExtraPorNotaDeCredito nota = null;

                if (_view.CreditNote.isValid() && _view.CreditNote.folio.isValid())
                {
                    //Se busca la nota de la nota de credito
                    nota = _view.CreditNote.DatosExtraPorNotaDeCreditoes.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
                }

                nota = nota ?? new DatosExtraPorNotaDeCredito() { dato = DatoExtra.Nota.ToString() };

                INoteView view;
                NotePresenter presenter;

                //Si la nota de credito ya existe, no se puede editar la nota
                view = new NoteView();
                presenter = new NotePresenter(view, nota.valor);

                view.ShowWindow();

                nota.valor = view.Nota;

                //Si la nota de credito ya esta registrada, no se edita la nota
                if (_view.CreditNote.isValid() && _view.CreditNote.idNotaDeCredito.isValid())
                {
                    return;
                }

                //Si la nota de credito ya tiene una nota, se actualiza en vez de agregarla
                DatosExtraPorNotaDeCredito datoNotaDeCredito = _view.CreditNote.DatosExtraPorNotaDeCreditoes.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());

                if (datoNotaDeCredito.isValid())
                {
                    datoNotaDeCredito.valor = nota.valor;
                }
                else
                {
                    _view.CreditNote.DatosExtraPorNotaDeCreditoes.Add(nota);
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
                _view.ShowError("Antes de crear una nota de crédito debe configurar series de notas de crédito");
                return;
            }

            if (!Session.SerieNotasDeCredito.isValid() || !Session.SerieNotasDeCredito.idSerie.isValid())
            {
                _view.ShowError("Debe configurar una serie default para notas de crédito antes de realizar registros de las mismas");
            }

            if (!Session.Configuration.CarpetaXml.isValid())
            {
                _view.ShowError("Antes de crear notas de crédito debe configurar la carpeta para depositar los comprobantes xml");
                return;
            }
            if (!Session.Configuration.CarpetaCbb.isValid())
            {
                _view.ShowError("Antes de crear notas de crédito debe configurar la carpeta para depositar los codigos bidimensionales");
                return;
            }
            if (!Session.Configuration.CarpetaPdf.isValid())
            {
                _view.ShowError("Antes de crear notas de crédito debe configurar la carpeta para depositar los comprobantes pdf");
                return;
            }

            var cert = Session.Configuration.Certificados.FirstOrDefault(c => c.activo);
            if (!cert.isValid())
            {
                _view.ShowError("Antes de crear notas de crédito debe configurar un certificado de sello digital");
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
                var creditNote = new VMNotaDeCredito();
                creditNote.fechaHora = DateTime.Now;
                creditNote.tipoDeCambio = Session.Configuration.tipoDeCambio;
                creditNote.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Nueva;

                //Si existe una serie default la cargo
                if(Session.SerieNotasDeCredito.isValid() && Session.SerieNotasDeCredito.idSerie.isValid())
                {
                    creditNote.serie = Session.SerieNotasDeCredito.identificador;
                    creditNote.folio = _creditNotes.Next(creditNote.serie);
                }

                _view.Show(creditNote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            if (!_view.CreditNote.Cliente.codigo.isValid())
                return;

            try
            {
                VMNotaDeCredito creditNote;

                var client = _clients.Find(_view.CreditNote.Cliente.codigo);

                if (!client.isValid())
                {
                    _view.ShowError("No existe ningún cliente con ese código");
                    New();
                    return;
                }

                if (client.idCliente.Equals(_view.CreditNote.Cliente.idCliente))
                    return;

                if (_view.CreditNote.folio.isValid())
                    creditNote = new VMNotaDeCredito(client, _view.CreditNote.serie, _view.CreditNote.folio.ToString(), Session.Configuration.tipoDeCambio);
                else
                    creditNote = new VMNotaDeCredito() { Cliente = client, idCliente = client.idCliente, serie = _view.CreditNote.serie };

                _view.Show(creditNote);
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
                    VMNotaDeCredito creditNote;

                    if (_view.CreditNote.folio.isValid())
                        creditNote = new VMNotaDeCredito(view.Client, _view.CreditNote.serie, _view.CreditNote.folio.ToString(), Session.Configuration.tipoDeCambio);
                    else
                        creditNote = new VMNotaDeCredito() { Cliente = view.Client, idCliente = view.Client.idCliente };

                    _view.Show(creditNote);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void GetFolio()
        {
            if (!_view.CreditNote.serie.isValid())
                return;

            try
            {
                VMNotaDeCredito creditNote;

                var folio = _creditNotes.Next(_view.CreditNote.serie);

                if (_view.CreditNote.Cliente.idCliente.isValid())
                    creditNote = new VMNotaDeCredito(_view.CreditNote.Cliente, _view.CreditNote.serie, folio.ToString(), Session.Configuration.tipoDeCambio);
                else
                    creditNote = new VMNotaDeCredito() { serie = _view.CreditNote.serie, folio = folio, tipoDeCambio = Session.Configuration.tipoDeCambio, fechaHora = DateTime.Now };

                _view.Show(creditNote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
                Load();
            }
        }

        private void Find()
        {
            if (!_view.CreditNote.serie.isValid() || !_view.CreditNote.folio.isValid())
                return;

            try
            {
                var creditNote = _creditNotes.Find(_view.CreditNote.serie, _view.CreditNote.folio.ToString());

                if (creditNote.isValid()) //Voy a mostrar una nota de credito existente
                    _view.Show(new VMNotaDeCredito(creditNote));
                else
                    _view.Show(new VMNotaDeCredito(_view.CreditNote.Cliente, _view.CreditNote.serie, _creditNotes.Next(_view.CreditNote.serie).ToString(), Session.Configuration.tipoDeCambio));

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
                ICreditNotesListView view;
                CreditNotesListPresenter presenter;

                view = new CreditNotesListView();
                presenter = new CreditNotesListPresenter(view, _creditNotes, -1);

                view.ShowWindow();

                if (view.CreditNote.isValid() && view.CreditNote.idNotaDeCredito.isValid())
                {
                    var dbCreditNote = _creditNotes.Find(view.CreditNote.idNotaDeCredito);

                    _view.Show(new VMNotaDeCredito(dbCreditNote));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.CreditNote.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.CurrentItem.Articulo.codigo.isValid())
                return;

            if (!_view.CreditNote.Moneda.isValid())
            {
                _view.ShowMessage("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.CreditNote.tipoDeCambio.isValid())
            {
                _view.ShowMessage("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            try
            {
                //Aqui se debe buscar en los articulos de la factura, si aplica
                //VMArticulo itemVM = null;
                //List<VMArticulo> items = _items.FindAll(_view.CurrentItem.Articulo.codigo).Select(x=> new VMArticulo(x)).ToList();

                //if (items.IsEmpty())
                //{
                //    _view.ShowError("No existe ningún artículo con ese código");
                //    _view.ClearItem();
                //    return;
                //}

                //if (!items.HasSingleItem() && !items.IsEmpty())
                //{
                //    //Se muestra la vista de seleccion
                //    IItemSelectionView view;
                //    ItemSelectionPresenter presenter;

                //    view = new ItemSelectionView(true);
                //    presenter = new ItemSelectionPresenter(view, items);

                //    view.ShowWindow();

                //    itemVM = view.Item;
                //}
                //else
                //{
                //    itemVM = items.FirstOrDefault();
                //}

                ////Se valida que se haya escogido algun articulo
                //if (!itemVM.isValid() || !itemVM.idArticulo.isValid() && !items.IsEmpty())
                //{
                //    _view.ClearItem();
                //    return;
                //}

                ////Se obtiene el articulo seleccionado
                //var item = _items.Find(itemVM.idArticulo);

                //if (!item.activo)
                //{
                //    _view.ClearItem();
                //    return;
                //}

                ////Si lo encontré preparo un Detalle de Venta
                //var detail = GetDetail(item, _view.Invoice.Cliente.idListaDePrecio);

                ////Lo muestro
                //_view.Show(detail);

                ////Se muestra la existencia
                //var existencia = item.inventariado ? _items.Stock(detail.idArticulo) : 0.0m;
                //_view.ShowStock(existencia);
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

            if (!_view.CreditNote.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.CreditNote.Moneda.isValid())
            {
                _view.ShowMessage("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.CreditNote.tipoDeCambio.isValid())
            {
                _view.ShowMessage("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            //Se debe mostrar la lista de los articulos de la factura
            try
            {
                IItemSelectionView view;
                ItemSelectionPresenter presenter;

                view = new ItemSelectionView();

                //Se debe crear una lista de articulos con los precios de la factura
                var items = _view.CreditNote.Factura.DetallesDeFacturas.Select(x =>
                {
                    var i = new VMArticulo(x.Articulo);

                    i.PrecioA = Operations.CalculatePriceWithDiscount(x.precioUnitario, x.descuento);

                    return i;
                }).ToList();

                presenter = new ItemSelectionPresenter(view, items);

                view.ShowWindow();

                if (view.Item.idArticulo.isValid())
                {
                    _view.Show(GetDetail(view.Item));
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

                //Si el módulo predial esta activo y el artículo lo tiene dentro de sus clasificaciones le pido seleccionar la cuenta
                //if (Modulos.Predial.IsActive() && detail.Articulo.Clasificaciones.HasPredial())
                //{
                //    IPropertyAccountsListView listView;
                //    PropertyAccountsListPresenter listPresenter;

                //    listView = new PropertyAccountsListView();
                //    listPresenter = new PropertyAccountsListPresenter(listView, _predialAccounts);

                //    listView.ShowWindow();

                //    if (listView.Account.idCuentaPredial.isValid())
                //        detail.CuentaPredialPorDetalle = new CuentaPredialPorDetalle() { cuenta = listView.Account.cuenta };
                //}

                //Si ya existe el artículo con el mismo precio unitario, sumo la cantidad a ese registro
                var creditNote = _view.CreditNote;
                var exists = creditNote.DetalleDeNotaDeCredito.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo) && d.precioUnitario.Equals(detail.precioUnitario));

                //Se verifica que la cantidad no exceda a la cantidad en la factura
                var invoice = _view.CreditNote.Factura;
                var invoiceDetail = invoice.DetallesDeFacturas.FirstOrDefault(x => x.idArticulo == detail.idArticulo && Math.Round(Operations.CalculatePriceWithDiscount(x.precioUnitario, x.descuento), 2, MidpointRounding.AwayFromZero) == Math.Round(detail.precioUnitario, 2, MidpointRounding.AwayFromZero));

                if (!invoiceDetail.isValid())
                {
                    throw new Exception("El artículo no existe en la factura");
                }

                if (detail.cantidad > invoiceDetail.cantidad)
                {
                    throw new Exception("No se puede efectuar una devolución con una cantidad mayor a la de la factura");
                }

                if (exists.isValid())
                    exists.cantidad += detail.cantidad;
                else
                {
                    //Si no existe en el detalle actual, agrego el detalle a la lista
                    //Busco los impuestos que el articulo debe pagar
                    detail.Impuestos = _items.Find(detail.idArticulo).Impuestos;
                    creditNote.DetalleDeNotaDeCredito.Add(detail);
                }

                //Se actualiza la descripcion
                creditNote.UpdateDescription();

                //Hago el calculo de la cuenta Subtotal, Impuestos, Total
                creditNote.UpdateAccount();

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra la nueva venta completa con la nueva lista de detalles y la cuenta
                _view.Show(creditNote);
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
                _view.ShowError("No se pueden editar las notas de crédito registradas");
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
                var creditNote = _view.CreditNote;
                creditNote.DetalleDeNotaDeCredito.Remove(item);

                //Recalculo
                creditNote.UpdateAccount();

                //Se genera la descripcion nuevamente
                creditNote.UpdateDescription();

                //Muestro la venta sin el articulo
                _view.Show(creditNote);
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
                _view.ShowError("No se pueden editar las notas de crédito registradas");
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
                var creditNote = _view.CreditNote;
                creditNote.DetalleDeNotaDeCredito.Remove(item);

                creditNote.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(creditNote);

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
                var creditNote = new VMNotaDeCredito();
                creditNote.fechaHora = DateTime.Now;
                creditNote.tipoDeCambio = Session.Configuration.tipoDeCambio;
                creditNote.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Nueva;
                creditNote.Cliente = new Cliente();

                //Si existe una serie default la cargo
                if (Session.SerieNotasDeCredito.isValid())
                {
                    creditNote.serie = Session.SerieNotasDeCredito.identificador;
                    creditNote.folio = _creditNotes.Next(creditNote.serie);
                }

                _view.Show(creditNote);
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
                _view.ShowError("No hay ninguna nota de crédito seleccionada para cancelación");
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

                var creditNote = new VMNotaDeCredito(_creditNotes.Cancel(_view.CreditNote.idNotaDeCredito, view.Reason));

                _view.ShowMessage("Nota de crédito cancelada exitosamente");

                //Aquí genero el pdf si fue una cancelación fiscal
                if (creditNote.TimbresDeNotasDeCredito.isValid())
                {
                    var receipt = new VMAcuse(creditNote, Session.Configuration);
                    var report = Reports.FillReport(receipt);
                    report.Export(string.Format("{0}\\{1}{2}-Acuse Cancelación.pdf", Session.Configuration.CarpetaPdf, creditNote.serie, creditNote.folio));
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
                _view.ShowMessage("No es posible imprimir una nota de crédito que no ha sido registrada");
                return;
            }

            if (!_view.CreditNote.TimbresDeNotasDeCredito.isValid() || !_view.CreditNote.TimbresDeNotasDeCredito.idTimbreDeNotaDeCredito.isValid())
            {
                _view.ShowMessage("No es posible imprimir una nota de crédito que no ha sido timbrada");
                return;
            }

            try
            {
                ICreditNotePrintView view;
                CreditNotePrintPresenter presenter;

                view = new CreditNotePrintView(_view.CreditNote);
                presenter = new CreditNotePrintPresenter(view, _creditNotes);

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

            var creditNote = _view.CreditNote;

            if (!Session.Station.isValid())
            {
                _view.ShowError("Este equipo no cuenta con ninguna estación asociada");
                return;
            }

            //MT: Esta linea tira una excepcion por comparar un decimal con un double (le agrege la m a 0.0)
            //No deberia ser creditNote.Total.Equals(0.0m)? sin la negacion y el updateAccount mas arriba?
            if (!creditNote.descripcion.isValid() || !creditNote.Total.Equals(0.0m))
                throw new Exception("Debe capturar descripción y monto para registrar la nota de crédito");

            //Esto de validar que la nota de crédito no exceda el inventario es una validación de datos o una validación por regla de negocio?
            //Se valida que las notas de credito no excedan el inventario o total de la factura
            if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid() && !_creditNotes.CanCreateForInvoice(creditNote, creditNote.Factura))
            {
                return;
            }

            creditNote.UpdateAccount();

            if (!IsCreditNoteValid(creditNote, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le agrego quien la registra
                creditNote.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                creditNote.Usuario = Session.LoggedUser;

                //Actualizo el folio
                creditNote.folio = _creditNotes.Next(creditNote.serie);

                //Pedimentos
                if (creditNote.DetalleDeNotaDeCredito.Any(a => a.Articulo.importado))
                {
                    ICustomsApplicationsExitView viewPedimentos = new CustomsApplicationsExitView(creditNote.DetalleDeNotaDeCredito.GetOnlyImported(creditNote.Factura));
                    CustomsApplicationsEntrancePresenter presenterPedimentos = new CustomsApplicationsEntrancePresenter(viewPedimentos, _customsApplications, _items);

                    viewPedimentos.ShowWindow();

                    //Debo asociar los pedimentos al detalle de mi remisión
                    //Itero entre cada uno de los artículos que necesitaba pedimentos
                    foreach (var p in viewPedimentos.Items.Where(p => p.Asociados > 0.0m))
                    {
                        //Obtengo el detalle correspondiente al artículo
                        var d = creditNote.DetalleDeNotaDeCredito.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo));
                        //Le asigno a ese detalle los pedimentos y cantidades correspondientes
                        p.Pedimentos.ForEach(pa => d.PedimentoPorDetalleDeNotaDeCreditoes.Add(new PedimentoPorDetalleDeNotaDeCredito() { idPedimento = pa.IdPedimento, cantidad = pa.Cantidad, Pedimento = _customsApplications.Find(pa.IdPedimento) }));
                    }
                }

                //Agrego la nota de credito
                creditNote = new VMNotaDeCredito(_creditNotes.Add(creditNote));

                //Si no viene de una factura, no se timbra, es un documento interno.
                if(creditNote.idFactura.HasValue)
                    creditNote = new VMNotaDeCredito(_creditNotes.Stamp(creditNote));

                _view.ShowMessage("Nota de crédito registrada exitosamente");

                //Aquí genero el pdf de la nota de crédito
                var report = Reports.FillReport(new VMRNotaDeCredito(new VMNotaDeCredito(creditNote), Session.Configuration));
                report.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, creditNote.serie, creditNote.folio));


                //Aqui debo enviar el correo si Guardian esta activado
                if (Modulos.Envio_De_Correos.IsActive())
                    _mailer.SendMail(creditNote);

                ICreditNotePrintView view;
                CreditNotePrintPresenter presenter;

                view = new CreditNotePrintView(creditNote);
                presenter = new CreditNotePrintPresenter(view, _creditNotes);

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
            //Si ya tiene timbre ignoro
            if (_view.CreditNote.TimbresDeNotasDeCredito.isValid() && _view.CreditNote.TimbresDeNotasDeCredito.idTimbreDeNotaDeCredito.isValid())
                return;

            try
            {
                var creditNote = _view.CreditNote;

                //Actualizo la cuenta
                creditNote.UpdateAccount();

                //Verifica si la factura requiere addenda o no
                //bool requiresAddenda = Modulos.Addendas.IsActive() && !invoice.Cliente.AddendaDeClientes.IsEmpty();

                ////Timbro la factura
                creditNote = new VMNotaDeCredito(_creditNotes.Stamp(creditNote));

                _view.ShowMessage("Nota de crédito timbrada exitosamente");

                ////Aquí genero el pdf
                //Usuario user = GetSellerForInvoice(invoice);
                //var report = Reports.FillReport(new VMRFactura(invoice, Session.Configuration, user));
                //report.Export(string.Format("{0}\\{1}{2}.pdf", Session.Configuration.CarpetaPdf, invoice.serie, invoice.folio));

                ////Aqui debo enviar el correo si Guardian esta activado
                //if (Modulos.Envio_De_Correos.IsActive())
                //    _mailer.SendMail(invoice);

                //IInvoicePrintView view;
                //InvoicePrintPresenter presenter;

                //view = new InvoicePrintView(invoice);
                //presenter = new InvoicePrintPresenter(view, _invoices);

                //view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                _view.ShowError(ex.Message);
            }
        }

        #region Private Utility Functions

        private VMDetalleDeNotaDeCredito GetDetail(VMArticulo item)
        {
            try
            {

                //Si lo encontré preparo un Detalle de Nota de credito
                var detail = new VMDetalleDeNotaDeCredito();
                detail.Articulo = item;
                detail.idArticulo = item.idArticulo;
                detail.cantidad = 1.0m;
                detail.Impuestos = item.Impuestos;

                //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
                var unitCost = item.PrecioA.ToDocumentCurrency(item.Moneda, _view.CreditNote.Moneda, _view.CreditNote.tipoDeCambio);
                detail.precioUnitario = unitCost;

                return detail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsCreditNoteValid(VMNotaDeCredito creditNote, out string error)
        {

            if (!creditNote.Cliente.codigo.isValid())
            {
                error = "El código del cliente no es válido";
                return false;
            }

            if (!creditNote.idCliente.isValid())
            {
                error = "El cliente no es válido";
                return false;
            }

            if (!creditNote.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if (!creditNote.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio es válido";
                return false;
            }

            if (!creditNote.fechaHora.isValid())
            {
                error = "La fecha no es válida";
                return false;
            }

            if (!creditNote.serie.isValid())
            {
                error = "La serie no es válida";
                return false;
            }

            if (!creditNote.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid() && creditNote.DetalleDeNotaDeCredito.Count.Equals(0))
            {
                error = "Los conceptos no son válidos";
                return false;
            }

            if (!creditNote.idFormaDePago.isValid())
            {
                error = "La forma de pago no es válida";
                return false;
            }

            if (creditNote.FormasPago.bancarizado && !creditNote.idCuentaBancaria.isValid())
            {
                error = "La cuenta bancaria no es válida";
                return false;
            }

            if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid() && creditNote.Total > new VMFactura(creditNote.Factura).Total)
            {
                error = "No se puede crear una nota de crédito con valor superior al de la factura";
                return false;
            }

            error = string.Empty;
            return true;
        }

        #endregion
    }
}
