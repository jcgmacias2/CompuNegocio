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
using System.IO;
using System.Linq;
using System.Windows;

namespace Aprovi.Presenters
{
    public class QuotesPresenter
    {
        private IQuotesView _view;
        private ICotizacionService _quotes;
        private ICatalogosEstaticosService _catalogs;
        private IClienteService _clients;
        private IArticuloService _items;
        private IListaDePrecioService _pricesList;
        private IFacturaService _invoices;
        private IRemisionService _billsOfSale;
        private IConfiguracionService _config;
        private IEnvioDeCorreoService _mailer;

        public QuotesPresenter(IQuotesView view, IRemisionService billsOfSale, ICatalogosEstaticosService catalogs, IClienteService clients, IArticuloService items, IListaDePrecioService pricesList, IFacturaService invoices, IConfiguracionService config, IEnvioDeCorreoService mailer, ICotizacionService quotes)
        {
            _view = view;
            _billsOfSale = billsOfSale;
            _catalogs = catalogs;
            _clients = clients;
            _items = items;
            _pricesList = pricesList;
            _invoices = invoices;
            _config = config;
            _mailer = mailer;
            _quotes = quotes;

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
            _view.OpenClientsList += OpenClientsList;
            _view.FindClient += FindClient;
            _view.Load += Load;
            _view.OpenNote += OpenNote;
            _view.Update += Update;
            _view.ChangeCurrency += ChangeCurrency;
            _view.Unlink += Unlink;

            _view.FillCombos(_catalogs.ListMonedas());
        }

        private void Unlink()
        {
            try
            {
                if (!_view.Quote.isValid() || !_view.Quote.idCotizacion.isValid())
                {
                    _view.ShowError("No se ha seleccionado una cotización para desligar");
                    return;
                }

                var quote = _quotes.Unlink(_view.Quote);

                _view.Show(new VMCotizacion(quote));

                _view.ShowMessage("Cotización desligada exitosamente");
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
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

                VMCotizacion quote = _view.Quote;
                quote.ToCurrency(lastCurrency);

                _view.Show(quote);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Update()
        {
            string error;

            var quote = _view.Quote;
            if (!IsQuoteValid(quote, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //No se puede modificar una cotizacion cancelada
                if (quote.idEstatusDeCotizacion == (int)StatusDeCotizacion.Cancelada)
                {
                    throw new Exception("No es posible modificar una cotización cancelada");
                }

                //Le agrego quien la edita
                quote.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                quote.Usuario = Session.LoggedUser;
                //La hora en que se esta editando
                quote.fechaHora = DateTime.Now;

                //Actualizo la cuenta
                quote.UpdateAccount();

                //Actualiza la cotizacion
                quote = new VMCotizacion(_quotes.Update(quote));

                _view.ShowMessage("Cotización guardada exitosamente");

                //Inicializo nuevamente
                Load();
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
                DatosExtraPorCotizacion nota = null;

                if (_view.Quote.isValid() && _view.Quote.idCotizacion.isValid())
                {
                    //Se busca la nota de la remision
                    nota = _view.Quote.DatosExtraPorCotizacions.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());
                }

                nota = nota ?? new DatosExtraPorCotizacion() { dato = DatoExtra.Nota.ToString() };

                IQuoteNoteView view;
                QuoteNotePresenter presenter;

                view = new QuoteNoteView();
                presenter = new QuoteNotePresenter(view, nota);

                view.ShowWindow();

                nota = view.Nota;

                //Si la remision ya tiene una nota, se actualiza en vez de agregarla
                DatosExtraPorCotizacion datoCotizacion = _view.Quote.DatosExtraPorCotizacions.FirstOrDefault(x => x.dato == DatoExtra.Nota.ToString());

                if (datoCotizacion.isValid())
                {
                    datoCotizacion.valor = nota.valor;
                }
                else
                {
                    _view.Quote.DatosExtraPorCotizacions.Add(nota);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Load()
        {
            try
            {
                //Establezco los defaults
                var quote = new VMCotizacion();
                quote.fechaHora = DateTime.Now;
                quote.tipoDeCambio = Session.Configuration.tipoDeCambio;
                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Nueva;
                quote.folio = _quotes.Next();

                _view.Show(quote);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            if (!_view.Quote.Cliente.codigo.isValid())
                return;

            try
            {
                VMCotizacion quote;

                var client = _clients.Find(_view.Quote.Cliente.codigo);

                if (!client.isValid())
                {
                    _view.ShowError("No existe ningún cliente con ese código");
                    New();
                    return;
                }

                if (_view.Quote.folio.isValid())
                    quote = new VMCotizacion(client, _view.Quote.folio.ToString(), Session.Configuration.tipoDeCambio);
                else
                    quote = new VMCotizacion() { Cliente = client, idCliente = client.idCliente };

                _view.Show(quote);
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
                    VMCotizacion quote;

                    if (_view.Quote.folio.isValid())
                        quote = new VMCotizacion(view.Client, _view.Quote.folio.ToString(), Session.Configuration.tipoDeCambio);
                    else
                        quote = new VMCotizacion() { Cliente = view.Client, idCliente = view.Client.idCliente };

                    _view.Show(quote);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Quote.folio.isValid())
                return;

            try
            {
                var quote = _quotes.FindByFolio(_view.Quote.folio.ToInt());

                if (quote.isValid()) //Voy a mostrar una cotizacion existente
                    _view.Show(new VMCotizacion(quote));
                else
                    _view.Show(new VMCotizacion(_view.Quote.Cliente, _quotes.Next().ToString(), Session.Configuration.tipoDeCambio));
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
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

                if (view.Quote.idCotizacion.isValid())
                    _view.Show(new VMCotizacion(view.Quote));
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

            if (!_view.Quote.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Quote.idMoneda.isValid())
            {
                _view.ShowError("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if(!_view.Quote.tipoDeCambio.isValid())
            {
                _view.ShowError("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            try
            {
                //Toda esta parte se trata de obtener el artículo que el cliente busca
                var item = new VMArticulo();
                var items = _items.FindAllForCustomer(_view.CurrentItem.Articulo.codigo, _view.Quote.idCliente).Select(a => new VMArticulo(a)).ToList();

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
                var detail = GetDetail(item, _view.Quote.Cliente.idListaDePrecio);

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
            if (!_view.Quote.idMoneda.isValid())
            {
                _view.ShowError("Se requiere la selección de una moneda para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Quote.Cliente.idCliente.isValid())
            {
                _view.ShowMessage("Debe seleccionar un cliente para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Quote.tipoDeCambio.isValid())
            {
                _view.ShowError("Se requiere el tipo de cambio para la transacción antes de agregar artículos");
                _view.ClearItem();
                return;
            }

            if (!_view.Quote.idCliente.isValid() || !_view.Quote.Cliente.isValid())
            { 
                _view.ShowError("Debe seleccionar un cliente para cotizar");
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

                    _view.Show(GetDetail(item, _view.Quote.Cliente.idListaDePrecio));
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

                detail.Articulo = _items.Find(_view.CurrentItem.Articulo.idArticulo);

                //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
                var unitCost = detail.Articulo.costoUnitario.ToDocumentCurrency(detail.Articulo.Moneda, _view.Quote.Moneda, _view.Quote.tipoDeCambio);
                //Obtengo el precio de venta para el cliente
                var stockPrice = Math.Round(Operations.CalculatePriceWithoutTaxes(unitCost, detail.Articulo.Precios.First(p => p.idListaDePrecio.Equals(_view.Quote.Cliente.idListaDePrecio)).utilidad), 2);
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
                var quote = _view.Quote;
                var exists = quote.DetalleDeCotizacion.FirstOrDefault(d => d.idArticulo.Equals(detail.idArticulo) && d.precioUnitario.Equals(detail.precioUnitario));
                if (exists.isValid())
                    exists.cantidad += detail.cantidad;
                else
                {
                    //Si no existe en el detalle actual, agrego el detalle a la lista
                    //Busco los impuestos que el articulo debe pagar
                    detail.Impuestos = _items.Find(detail.idArticulo).Impuestos;
                    quote.DetalleDeCotizacion.Add(detail);
                }

                //Hago el calculo de la cuenta Subtotal, Impuestos, Total
                quote.UpdateAccount();

                //Limpio el artículo en edición
                _view.ClearItem();

                //Muestra la nueva venta completa con la nueva lista de detalles y la cuenta
                _view.Show(quote);
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

                if (item.ComentariosPorDetalleDeCotizacion.isValid() && item.ComentariosPorDetalleDeCotizacion.comentario.isValid())
                    view = new ItemCommentView(item.ComentariosPorDetalleDeCotizacion.comentario);
                else
                    view = new ItemCommentView();
                ItemCommentPresenter presenter = new ItemCommentPresenter(view);

                view.ShowWindow();
                if (view.Comment.isValid())
                    item.ComentariosPorDetalleDeCotizacion = new ComentariosPorDetalleDeCotizacion() { comentario = view.Comment };
                else
                    item.ComentariosPorDetalleDeCotizacion = null;
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
            if (!_view.SelectedItem.idArticulo.isValid())
            {
                _view.ShowError("No existe ningún detalle seleccionado");
                return;
            }

            try
            {
                var item = _view.SelectedItem;

                //Lo elimino del detalle
                var quote = _view.Quote;
                quote.DetalleDeCotizacion.Remove(item);

                //Recalculo
                quote.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(quote);
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
                var quote = _view.Quote;
                quote.DetalleDeCotizacion.Remove(item);

                quote.UpdateAccount();

                //Muestro la venta sin el articulo
                _view.Show(quote);

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
                var quote = new VMCotizacion();
                quote.fechaHora = DateTime.Now;
                quote.tipoDeCambio = Session.Configuration.tipoDeCambio;
                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Nueva;
                quote.folio = _quotes.Next();

                _view.Show(quote);
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
                _view.ShowError("No hay ninguna cotización seleccionada para cancelación");
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

                var quote = new VMCotizacion(_quotes.Cancel(_view.Quote.idCotizacion,view.Reason));

                _view.ShowMessage("Cotización cancelada exitosamente");

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
                _view.ShowMessage("No es posible imprimir una cotización que no ha sido registrada");
                return;
            }

            try
            {
                IQuotePrintView view;
                QuotePrintPresenter presenter;

                view = new QuotePrintView(_view.Quote, Modulos.Envio_De_Correos.IsActive());
                presenter = new QuotePrintPresenter(view, _quotes, _mailer);

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

            var quote = _view.Quote;
            if (!IsQuoteValid(quote, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Le agrego quien la registra
                quote.idUsuarioRegistro = Session.LoggedUser.idUsuario;
                quote.Usuario = Session.LoggedUser;
                //La hora en que se esta registrando
                quote.fechaHora = DateTime.Now;

                //Actualizo el folio
                quote.folio = _quotes.Next();

                //Actualizo la cuenta
                quote.UpdateAccount();

                //Agrego la cotizacion
                quote = new VMCotizacion(_quotes.Add(quote));

                _view.ShowMessage("Cotización registrada exitosamente");

                IQuotePrintView view;
                QuotePrintPresenter presenter;

                view = new QuotePrintView(_view.Quote, Modulos.Envio_De_Correos.IsActive());
                presenter = new QuotePrintPresenter(view, _quotes, _mailer);

                view.ShowWindow();

                //Inicializo nuevamente
                Load();
            }
            catch(Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #region Private Utility Functions

        private bool IsQuoteValid(VMCotizacion quote, out string error)
        {

            if (!quote.Cliente.codigo.isValid())
            {
                error = "El código del cliente no es válido";
                return false;
            }

            if (!quote.idCliente.isValid())
            {
                error = "El cliente no es válido";
                return false;
            }

            if (!quote.idMoneda.isValid())
            {
                error = "La moneda no es válida";
                return false;
            }

            if (!quote.tipoDeCambio.isValid())
            {
                error = "El tipo de cambio es válido";
                return false;
            }

            if (!quote.fechaHora.isValid())
            {
                error = "La fecha no es válida";
                return false;
            }

            if (!quote.folio.isValid())
            {
                error = "El folio no es válido";
                return false;
            }

            if (quote.DetalleDeCotizacion.Count.Equals(0))
            {
                error = "Los conceptos no son válidos";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private VMDetalleDeCotizacion GetDetail(Articulo item, int idPriceList)
        {
            //Si lo encontré preparo un Detalle de Venta
            var detail = new VMDetalleDeCotizacion();
            detail.Articulo = item;
            detail.idArticulo = item.idArticulo;
            detail.cantidad = 1.0m;
            detail.Impuestos = item.Impuestos;

            //Se calcula el precio sin impuestos
            detail.descuento = 0.0m;

            //Si la moneda del artículo es distinta a la del documento, calcular el precio unitario en base a la moneda correcta
            var unitCost = item.costoUnitario.ToDocumentCurrency(item.Moneda, _view.Quote.Moneda, _view.Quote.tipoDeCambio);
            detail.precioUnitario = Operations.CalculatePriceWithoutTaxes(unitCost, item.Precios.First(p => p.idListaDePrecio.Equals(idPriceList)).utilidad);

            return detail;
        }

        #endregion
    }
}
