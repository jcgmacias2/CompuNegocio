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
    public class BillsOfSalePresenter
    {
        private IBillsOfSaleView _view;
        private IRemisionService _billsOfSale;
        private ICatalogosEstaticosService _catalogs;
        private IClienteService _clients;
        private IArticuloService _items;
        private IListaDePrecioService _pricesList;
        private IAbonoDeRemisionService _payments;
        private IFacturaService _invoices;
        private IAbonoDeFacturaService _invoicePayments;
        private ICuentaBancariaService _bankAccounts;
        private IUsosCFDIService _usesCFDI;
        private IConfiguracionService _config;
        private IEnvioDeCorreoService _mailer;
        private ICotizacionService _quotes;
        private IPedimentoService _customsApplications;
        private IUsuarioService _users;
        private ISeguridadService _security;

        public BillsOfSalePresenter(IBillsOfSaleView view, IRemisionService billsOfSale, ICatalogosEstaticosService catalogs, IClienteService clients, IArticuloService items, IListaDePrecioService pricesList, IAbonoDeRemisionService payments, IFacturaService invoices, IAbonoDeFacturaService invoicePayments, ICuentaBancariaService bankAccounts, IUsosCFDIService usesCFDI, IConfiguracionService config, IEnvioDeCorreoService mailer, ICotizacionService quotes, IPedimentoService customsApplications, IUsuarioService users, ISeguridadService security)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _catalogs = catalogs;
            _clients = clients;
            _items = items;
            _pricesList = pricesList;
            _payments = payments;
            _invoices = invoices;
            _invoicePayments = invoicePayments;
            _bankAccounts = bankAccounts;
            _usesCFDI = usesCFDI;
            _config = config;
            _billsOfSale = billsOfSale;
            _mailer = mailer;
            _quotes = quotes;
            _customsApplications = customsApplications;
            _users = users;
            _security = security;

            _view.Save += Save;
            _view.Print += Print;
            _view.OpenBillOfSaleToInvoice += OpenBillOfSaleToInvoice;
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
            _view.OpenClientsList += OpenClientsList;
            _view.FindClient += FindClient;
            _view.Load += Load;
            _view.OpenNote += OpenNote;
            _view.OpenQuotesList += OpenQuotesList;
            _view.ChangeCurrency += ChangeCurrency;
            _view.FindUser += FindUser;
            _view.OpenUsersList += OpenUsersList;
            _view.Update += Update;

            //Abonos
            _view.Add += Add;
            _view.Remove += Remove;

            _view.FillCombos(_catalogs.ListMonedas(), _catalogs.ListFormasDePago().Where(m => m.activa).ToList());
        }

        private void Update()
        {
            try
            {
                string error = "";
                var vmBillOfSale = _view.BillOfSale;

                if (!IsBillOfSaleValid(vmBillOfSale,out error))
                {
                    _view.ShowError(error);
                    return;
                }

                //La remision no debe estar facturada
                if (vmBillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Facturada)
                {
                    _view.ShowError("No se puede modificar una remisión facturada");
                    return;
                }

                //La remision no debe estar cancelada
                if (vmBillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Cancelada)
                {
                    _view.ShowError("No se puede modificar una remisión facturada o cancelada");
                    return;
                }

                //El modulo control de inventario no debe estar activado
                if (Modulos.Control_de_Inventario.IsActive())
                {
                    _view.ShowError("No se pueden modificar una remisión con el modulo control de inventario activo");
                    return;
                }

                 vmBillOfSale.UpdateAccount();

                _billsOfSale.Update(vmBillOfSale);

                _view.ShowMessage("Remisión modificada exitosamente");
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenUsersList()
        {
            try
            {
                IUsersListView usersListView = new UsersListView();
                UsersListPresenter usersListPresenter = new UsersListPresenter(usersListView,_users);

                usersListView.ShowWindow();

                if (usersListView.User.isValid() && usersListView.User.idUsuario.isValid())
                {
                    _view.Show(usersListView.User);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FindUser()
        {
            try
            {
                var user = _users.Find(_view.BillOfSale.Usuario1.nombreDeUsuario);

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

        private void ChangeCurrency()
        {
            try
            {
                Moneda lastCurrency = _view.LastCurrency;

                if (!lastCurrency.isValid())
                {
                    return;
                }

                VMRemision billOfSale = _view.BillOfSale;
                billOfSale.ToCurrency(lastCurrency);

                _view.Show(billOfSale);
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
                    if (view.Quote.idEstatusDeCotizacion != (int) StatusDeCotizacion.Registrada)
                    {
                        throw new Exception("No se puede remisionar una cotizacion remisionada, facturada o cancelada");
                    }

                    var bos = new VMRemision(new VMCotizacion(view.Quote));

                    //Se le asigna un folio a la remision
                    bos.folio = _billsOfSale.Next();

                    _view.Show(bos);

                    //Se verifica el stock de la remision generada para la cotizacion
                    //Solo si tiene el módulo activado
                    if (Modulos.Control_de_Inventario.IsActive())
                    {
                        var itemsWithoutStock = new List<Articulo>();
                        foreach (var d in bos.DetalleDeRemision)
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
                DatosExtraPorRemision nota = null;

                if (_view.BillOfSale.isValid() && _view.BillOfSale.folio.isValid())
                {
                    //Se busca la nota de la remision
                    nota = _view.BillOfSale.DatosExtraPorRemisions.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
                }

                nota = nota ?? new DatosExtraPorRemision() { dato = DatoExtra.Nota.ToString() };

                IBillOfSaleNoteView view;
                BillOfSaleNotePresenter presenter;

                view = new BillOfSaleNoteView();
                presenter = new BillOfSaleNotePresenter(view, nota);

                view.ShowWindow();

                nota = view.Nota;

                //Si la remision ya tiene una nota, se actualiza en vez de agregarla
                DatosExtraPorRemision datoRemision = _view.BillOfSale.DatosExtraPorRemisions.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());

                if (datoRemision.isValid())
                {
                    datoRemision.valor = nota.valor;
                }
                else
                {
                    _view.BillOfSale.DatosExtraPorRemisions.Add(nota);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OpenBillOfSaleToInvoice()
        {
            if(_view.BillOfSale.idFactura.HasValue)
            {
                _view.ShowError("Esta remisión ya fué facturada anteriormente");
                return;
            }

            try
            {
                IBillOfSaleToInvoiceView view;
                BillOfSaleToInvoicePresenter presenter;

                view = new BillOfSaleToInvoiceView(_view.BillOfSale, _bankAccounts, _invoicePayments);
                presenter = new BillOfSaleToInvoicePresenter(view, _invoices, _invoicePayments, _catalogs, _usesCFDI, _config, _billsOfSale, _mailer);

                view.ShowWindow();

                //Cuando cierre la de facturación reinicio esta pantalla
                New();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Load()
        {
            try
            {
                //Establezco los defaults
                var billOfSale = new VMRemision();
                billOfSale.fechaHora = DateTime.Now;
                billOfSale.tipoDeCambio = Session.Configuration.tipoDeCambio;
                billOfSale.idEstatusDeRemision = (int)StatusDeRemision.Nueva;
                billOfSale.folio = _billsOfSale.Next();

                _view.Show(billOfSale);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            if (!_view.BillOfSale.Cliente.codigo.isValid())
                return;

            try
            {
                VMRemision billOfSale;

                var client = _clients.Find(_view.BillOfSale.Cliente.codigo);

                if (!client.isValid())
                {
                    _view.ShowError("No existe ningún cliente con ese código");
                    New();
                    return;
                }

                //Si se esta remisionando una cotizacion, no se debe limpiar la vista
                if (_view.BillOfSale.Cotizaciones.isValid() && !_view.BillOfSale.Cotizaciones.IsEmpty())
                {
                    billOfSale = _view.BillOfSale;
                    billOfSale.Cliente = client;

                    //Se recalculan los precios para el nuevo cliente
                    billOfSale.DetalleDeRemision = _billsOfSale.UpdatePrices(_view.BillOfSale.DetalleDeRemision, client);

                    //Se recalcula el total
                    billOfSale.UpdateAccount();

                    _view.Show(billOfSale);
                    return;
                }

                if (_view.BillOfSale.folio.isValid())
                {
                    billOfSale = new VMRemision(client, _view.BillOfSale.folio.ToString(), Session.Configuration.tipoDeCambio);
                }
                else
                {
                    billOfSale = new VMRemision() { Cliente = client, idCliente = client.idCliente };
                }

                _view.Show(billOfSale);
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
                    VMRemision billOfSale;

                    //Si se esta remisionando una cotizacion, no se debe limpiar la vista
                    if (_view.BillOfSale.Cotizaciones.isValid() && !_view.BillOfSale.Cotizaciones.IsEmpty())
                    {
                        billOfSale = _view.BillOfSale;
                        billOfSale.Cliente = view.Client;

                        //Se recalculan los precios para el nuevo cliente
                        billOfSale.DetalleDeRemision = _billsOfSale.UpdatePrices(_view.BillOfSale.DetalleDeRemision, view.Client);

                        billOfSale.UpdateAccount();

                        _view.Show(billOfSale);
                        return;
                    }

                    if (_view.BillOfSale.folio.isValid())
                        billOfSale = new VMRemision(view.Client, _view.BillOfSale.folio.ToString(), Session.Configuration.tipoDeCambio);
                    else
                        billOfSale = new VMRemision() { Cliente = view.Client, idCliente = view.Client.idCliente, Usuario1 = view.Client.Usuario};

                    _view.Show(billOfSale);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.BillOfSale.folio.isValid())
                return;

            try
            {
                var billOfSale = _billsOfSale.FindByFolio(_view.BillOfSale.folio.ToInt());

                if (billOfSale.isValid()) //Voy a mostrar una remisión existente
                    _view.Show(new VMRemision(billOfSale));
                else
                    _view.Show(new VMRemision(_view.BillOfSale.Cliente, _billsOfSale.Next().ToString(), Session.Configuration.tipoDeCambio));
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
                IBillsOfSaleListView view;
                BillsOfSaleListPresenter presenter;

                view = new BillsOfSaleListView();
                presenter = new BillsOfSaleListPresenter(view, _billsOfSale);

                view.ShowWindow();

                if (view.BillOfSale.idRemision.isValid())
                    _view.Show(new VMRemision(view.BillOfSale));
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindItem()
        {
            if (!_view.CurrentItem.Articulo.codigo.isValid())
                return;

            if (!_view.BillOfSale.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.BillOfSale.idMoneda.isValid())
            {
                _view.ShowError("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if(!_view.BillOfSale.tipoDeCambio.isValid())
            {
                _view.ShowError("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            try
            {   
                //Toda esta parte se trata de obtener el artículo que el cliente busca
                var item = new VMArticulo();
                var items = _items.FindAllForCustomer(_view.CurrentItem.Articulo.codigo, _view.BillOfSale.idCliente).Select(a => new VMArticulo(a)).ToList();

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
                var detail = GetDetail(item, _view.BillOfSale.Cliente.idListaDePrecio);

                //Lo muestro
                _view.Show(detail);

                //Se muestra la existencia del articulo
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
            if (_view.BillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Facturada || _view.BillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Cancelada)
            {
                _view.ShowError("No se pueden editar las remisiones facturadas o canceladas");
                return;
            }

            if (!_view.BillOfSale.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.BillOfSale.idMoneda.isValid())
            {
                _view.ShowError("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.BillOfSale.tipoDeCambio.isValid())
            {
                _view.ShowError("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
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

                    _view.Show(GetDetail(item, _view.BillOfSale.Cliente.idListaDePrecio));
                    //Tambien se muestra la existencia del producto
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
                detail.Articulo = _items.Find(detail.idArticulo);

                //Valido que la existencia actual del artículo sea mayor o igual a la cantidad a vender
                //Solo aplica a inventariados cuando esta activo el control de inventario
                if (_view.CurrentItem.Articulo.inventariado && Modulos.Control_de_Inventario.IsActive())
                {
                    var currentStock = _items.Stock(_view.CurrentItem.idArticulo);
                    if (currentStock < detail.cantidad)
                    {
                        _view.ShowMessage("Existencia insuficiente");
                        return;
                    }
                }

                //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
                var unitCost = detail.Articulo.costoUnitario.ToDocumentCurrency(detail.Articulo.Moneda, _view.BillOfSale.Moneda, _view.BillOfSale.tipoDeCambio);
                //Obtengo el precio de venta para el cliente
                var stockPrice = Math.Round(Operations.CalculatePriceWithoutTaxes(unitCost, detail.Articulo.Precios.First(p => p.idListaDePrecio.Equals(_view.BillOfSale.Cliente.idListaDePrecio)).utilidad), 2);
                //Si el precio unitario fue modificado, calcular el porcentaje de descuento
                if (detail.precioUnitario != stockPrice)
                {
                    detail.descuento = Operations.CalculateDiscount(stockPrice, detail.precioUnitario);
                    //Valido el precio unitario del artículo, si tiene algún descuento válido que no sobrepase el límite permitido
                    if (detail.descuento > Session.LoggedUser.descuento)
                    {
                        _view.ShowError("No tiene privilgios suficientes para otorgar el precio actual");
                        return;
                    }
                }

                //Si ya existe el artículo con el mismo precio unitario, sumo la cantidad a ese registro
                var billOfSale = _view.BillOfSale;
                var exists = billOfSale.DetalleDeRemision.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo) && d.precioUnitario.Equals(detail.precioUnitario));
                if (exists.isValid())
                    exists.cantidad += detail.cantidad;
                else
                {
                    //Si no existe en el detalle actual, agrego el detalle a la lista
                    //Busco los impuestos que el articulo debe pagar
                    detail.Impuestos = _items.Find(detail.idArticulo).Impuestos;
                    billOfSale.DetalleDeRemision.Add(detail);
                }

                //Si se modifican los articulos de la remisión, se eliminan los abonos que puedan estar registrados (solo cuando no se este modificando la remision)
                if (!billOfSale.idRemision.isValid())
                {
                    billOfSale.AbonosDeRemisions = new List<AbonosDeRemision>();
                }

                //Hago el calculo de la cuenta Subtotal, Impuestos, Total
                billOfSale.UpdateAccount();

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra la nueva venta completa con la nueva lista de detalles y la cuenta
                _view.Show(billOfSale);
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

                if (item.ComentariosPorDetalleDeRemision.isValid() && item.ComentariosPorDetalleDeRemision.comentario.isValid())
                    view = new ItemCommentView(item.ComentariosPorDetalleDeRemision.comentario);
                else
                    view = new ItemCommentView();
                ItemCommentPresenter presenter = new ItemCommentPresenter(view);

                view.ShowWindow();
                if (view.Comment.isValid())
                    item.ComentariosPorDetalleDeRemision = new ComentariosPorDetalleDeRemision() { comentario = view.Comment };
                else
                    item.ComentariosPorDetalleDeRemision = null;
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
            if (_view.BillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Facturada || _view.BillOfSale.idEstatusDeRemision == (int)StatusDeRemision.Cancelada)
            {
                _view.ShowError("No se pueden editar las remisiones facturadas o canceladas");
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
                var billOfSale = _view.BillOfSale;
                billOfSale.DetalleDeRemision.Remove(item);

                //Recalculo
                billOfSale.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(billOfSale);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectItem()
        {
            if (!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var billOfSale = _view.BillOfSale;
                billOfSale.DetalleDeRemision.Remove(item);

                billOfSale.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(billOfSale);

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
                var billOfSale = new VMRemision();
                billOfSale.fechaHora = DateTime.Now;
                billOfSale.tipoDeCambio = Session.Configuration.tipoDeCambio;
                billOfSale.idEstatusDeRemision = (int)StatusDeRemision.Nueva;
                billOfSale.folio = _billsOfSale.Next();

                _view.Show(billOfSale);
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
                _view.ShowError("No hay ninguna remisión seleccionada para cancelación");
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

                var billOfSale = new VMRemision(_billsOfSale.Cancel(_view.BillOfSale.idRemision,view.Reason));

                _view.ShowMessage("Remisión cancelada exitosamente");

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
                _view.ShowMessage("No es posible imprimir una remisión que no ha sido registrada");
                return;
            }

            try
            {
                IBillOfSalePrintView view;
                BillOfSalePrintPresenter presenter;

                view = new BillOfSalePrintView(_view.BillOfSale);
                presenter = new BillOfSalePrintPresenter(view, _billsOfSale, _config);

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

        private void Save()
        {
            string error;

            var billOfSale = _view.BillOfSale;
            if (!IsBillOfSaleValid(billOfSale, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le agrego quien la registra
                billOfSale.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                billOfSale.Usuario = Session.LoggedUser;

                //Actualizo la cuenta
                billOfSale.UpdateAccount();

                //Se verifica si el cliente tiene suficiente crédito disponible
                if (!billOfSale.Cliente.limiteCredito.HasValue || billOfSale.Cliente.limiteCredito.Equals(0.0) || !_clients.HasAvailableCredit(billOfSale.Cliente, billOfSale.Moneda, billOfSale.Saldo))
                {
                    //Se envia aviso de que el cliente esta excediendo su limite de credito
                    _view.ShowMessage("El cliente está excediendo su límite de crédito");

                    //Si no tiene suficiente credito y el usuario no tiene permisos totales en Remisiones, se muestra la ventana de autenticacion

                    if (!Session.LoggedUser.HasAccess(AccesoRequerido.Total, "BillsOfSalePresenter", false))
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

                //La hora en que se esta registrando
                billOfSale.fechaHora = DateTime.Now;

                //Actualizo el folio
                billOfSale.folio = _billsOfSale.Next();

                //Pedimentos
                if(billOfSale.DetalleDeRemision.Any(a => a.Articulo.importado))
                {
                    ICustomsApplicationsExitView viewCustomsApplication = new CustomsApplicationsExitView(billOfSale.DetalleDeRemision.GetOnlyImported());
                    CustomsApplicationsExitPresenter presenterCustomsApplication = new CustomsApplicationsExitPresenter(viewCustomsApplication, _customsApplications, _items);

                    viewCustomsApplication.ShowWindow();

                    //Debo asociar los pedimentos al detalle de mi remisión
                    //Itero entre cada uno de los artículos que necesitaba pedimentos
                    foreach (var p in viewCustomsApplication.Items)
                    {
                        //Obtengo el detalle correspondiente al artículo
                        var d = billOfSale.DetalleDeRemision.FirstOrDefault(i => i.idArticulo.Equals(p.Articulo.idArticulo) && p.PrecioUnitario.Equals(i.precioUnitario));
                        //Le asigno a ese detalle los pedimentos y cantidades correspondientes
                        p.Pedimentos.ForEach(pa => d.PedimentoPorDetalleDeRemisions.Add(new PedimentoPorDetalleDeRemision() { idPedimento = pa.IdPedimento, cantidad = pa.Cantidad, Pedimento = _customsApplications.Find(pa.IdPedimento) }));
                    }
                }

                //Agrego la remisión
                billOfSale = new VMRemision(_billsOfSale.Add(billOfSale));

                _view.ShowMessage("Remisión registrada exitosamente");

                IBillOfSalePrintView view;
                BillOfSalePrintPresenter presenter;

                view = new BillOfSalePrintView(billOfSale);
                presenter = new BillOfSalePrintPresenter(view, _billsOfSale, _config);

                view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch(Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #region Abonos

        private void Remove()
        {
            if (!_view.Selected.isValid())
            {
                _view.ShowError("No hay ningún abono seleccionado para cancelar");
                return;
            }

            try
            {
                var billOfSale = _view.BillOfSale;
                if (_view.Selected.idAbonoDeRemision.isValid()) //Lo cancelo si ya estaba registrado
                {
                    _payments.Cancel(_view.Selected.idAbonoDeRemision);
                    _view.ShowMessage("Abono cancelado exitosamente");
                }
                else // Si no solo lo elimino
                {
                    billOfSale.AbonosDeRemisions.Remove(_view.Selected);
                }

                _view.Show(billOfSale.AbonosDeRemisions.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Add()
        {
            try
            {
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

                try
                {
                    var billOfSale = _view.BillOfSale;

                    //Actualizo la cuenta
                    billOfSale.UpdateAccount();

                    //Si el monto (convertido a la moneda de la factura) que voy a abonar es mayor al saldo total reducir el monto al saldo
                    if (payment.monto.ToDocumentCurrency(payment.Moneda, billOfSale.Moneda, billOfSale.tipoDeCambio) > billOfSale.Saldo)
                    {
                        if (payment.idMoneda.Equals(billOfSale.idMoneda))
                            payment.monto = billOfSale.Saldo;
                        else
                            payment.monto = billOfSale.Saldo.ToDocumentCurrency(billOfSale.Moneda, payment.Moneda, billOfSale.tipoDeCambio);
                    }

                    //Si ya esta saldada la cuenta ya no puede agregar mas abonos
                    if (payment.monto <= 0.0m)
                    {
                        _view.ShowMessage("La cuenta ya esta saldada, no es posible realizar más abonos");
                        return;
                    }

                    //Empresa desde la que se registra el abono
                    payment.idEmpresa = Session.Station.idEmpresa;

                    //Si es una remisión ya registrada, registro el abono inmediatamente
                    if (_view.IsDirty)
                    {
                        payment = _payments.Add(payment);
                        _view.ShowMessage("Abono registrado exitosamente");
                    }
                    else
                    {
                        //Agrego el abono ya sea registrado o solo a la colección
                        billOfSale.AbonosDeRemisions.Add(payment);
                    }

                    //Muestro los abonos
                    _view.Show(billOfSale.AbonosDeRemisions.ToList());
                }
                catch (Exception ex)
                {
                    _view.ShowError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Private Utility Functions

        private bool IsBillOfSaleValid(VMRemision billOfSale, out string error)
        {

            if (!billOfSale.Cliente.codigo.isValid())
            {
                error = "El código del cliente no es válido";
                return false;
            }

            if (!billOfSale.idCliente.isValid())
            {
                error = "El cliente no es válida";
                return false;
            }

            if (!billOfSale.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if (!billOfSale.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio es válido";
                return false;
            }

            if (!billOfSale.fechaHora.isValid())
            {
                error = "La fecha no es válida";
                return false;
            }

            if (!billOfSale.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (billOfSale.DetalleDeRemision.Count.Equals(0))
            {
                error = "Los conceptos no son válidos";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private VMDetalleDeRemision GetDetail(Articulo item, int idPriceList)
        {
            //Si lo encontré preparo un Detalle de Venta
            var detail = new VMDetalleDeRemision();
            detail.Articulo = item;
            detail.idArticulo = item.idArticulo;
            detail.cantidad = 1.0m;
            detail.Impuestos = item.Impuestos;

            //Se calcula el precio sin impuestos
            detail.descuento = 0.0m;

            //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
            var unitCost = item.costoUnitario.ToDocumentCurrency(item.Moneda, _view.BillOfSale.Moneda, _view.BillOfSale.tipoDeCambio);
            detail.precioUnitario = Math.Round(Operations.CalculatePriceWithoutTaxes(unitCost, item.Precios.First(p => p.idListaDePrecio.Equals(idPriceList)).utilidad), 2);

            return detail;
        }

        #endregion
    }
}
