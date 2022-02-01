using Aprovi.Application.Helpers;
using Aprovi.Business.Services;
using Aprovi.Data.Models;
using Aprovi.Application.ViewModels;
using Aprovi.Views;
using Aprovi.Views.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;
using Aprovi.Business.Helpers;

namespace Aprovi.Presenters
{
    public class PricesPresenter
    {
        private IPricesView _view;
        private IListaDePrecioService _prices;
        private IArticuloService _items;
        private IClienteService _clients;

        public PricesPresenter(IPricesView view, IListaDePrecioService pricesService, IArticuloService itemsService, IClienteService clientsService)
        {
            _view = view;
            _prices = pricesService;
            _items = itemsService;
            _clients = clientsService;

            //Acciones de la lista de precios
            _view.Find += Find;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.OpenList += OpenList;
            _view.Quit += Quit;

            //Acciones de la pestaña de artículos
            _view.FindPriceItem += FindPriceItem;
            _view.OpenItemsList += OpenItemsList;
            _view.AddOrUpdatePriceItem += AddOrUpdatePriceItem;
            _view.DeletePriceItem += DeletePriceItem;
            _view.SelectPriceItem += SelectPriceItem;
            _view.CalculateByPrice += CalculateByPrice;
            _view.CalculateByUtility += CalculateByUtility;
            _view.CalculateByPriceWithTaxes += CalculateByPriceWithTaxes;

            //Acciones de la pestaña de clientes
            _view.FindClient += FindClient;
            _view.OpenClientsList += OpenClientsList;
            _view.AddClient += AddClient;
            _view.DeleteClient += DeleteClient;
        }

        #region Clientes

        private void DeleteClient()
        {
            if (!_view.CurrentClient.isValid())
            {
                _view.ShowError("No hay ningún cliente seleccionado para eliminar");
                return;
            }

            try
            {
                var pricesList = _view.PricesList;

                pricesList.Clientes.Remove(_view.CurrentClient);

                //Si es una lista en edición elimino la relación hasta la base de datos
                if (_view.IsDirty)
                    _prices.Remove(_view.CurrentClient);

                //Envío mensaje al usuario
                _view.ShowMessage("Cliente eliminado exitosamente");

                //Actualizo la lista que estoy mostrando
                _view.Show(pricesList);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddClient()
        {
            if(!_view.Client.isValid() || !_view.Client.idCliente.isValid())
            {
                _view.ShowMessage("No hay ningún cliente para agregar a la lista");
                return;
            }
            
            try
            {
                //Debo relacionar la lista de precios con el cliente y agregar el cliente a la colección de la lista

                //Reviso que no sea un cliente repetido
                if (!_view.PricesList.Clientes.isValid() || _view.PricesList.Clientes.ToList().Exists(c => c.idCliente.Equals(_view.Client.idCliente)))
                    return;

                //Si es una lista en edición guardo el cambio hasta la base de datos
                if (_view.IsDirty)
                    _prices.Add(_view.Client);

                //Agrego el cliente a la colección de clientes de la lista
                var pricesList = _view.PricesList;
                pricesList.Clientes.Add(_view.Client);

                //Envío mensaje al usuario
                _view.ShowMessage("Cliente agregado exitosamente");

                //Actualizo la lista que se esta mostrando
                _view.Show(pricesList);  

            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenClientsList()
        {
            if (!_view.PricesList.codigo.isValid())
            {
                _view.ShowMessage("Debe seleccionar o crear una lista primero");
                _view.ClearClient();
                return;
            }

            try
            {
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _clients);

                view.ShowWindow();

                //Si no hay ningún cliente seleccionado simplemente regreso
                if (!view.Client.isValid() || !view.Client.idCliente.isValid())
                    return;

                //Si selecciono alguno y tiene lista de precio le digo que ya esta en una lista
                if(view.Client.idListaDePrecio.isValid())
                {
                    _view.ShowMessage(string.Format("El cliente {0} ya esta ligado a la lista de precio {1}", view.Client.codigo, view.Client.ListasDePrecio.codigo));
                    _view.ClearClient();
                    return;
                }

                //Si no tuvo ninguna lista entonces lo muestro
                _view.Show(view.Client);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindClient()
        {
            if (!_view.Client.isValid() || !_view.Client.codigo.isValid())
                return;

            if (!_view.PricesList.codigo.isValid())
            {
                _view.ShowMessage("Debe seleccionar o crear una lista primero");
                _view.ClearClient();
                return;
            }

            try
            {
                var client = _clients.Find(_view.Client.codigo);

                //Si no encontré el cliente me regreso
                if(client == null)
                {
                    _view.ShowMessage("No existe ningún cliente con ese código");
                    _view.ClearClient();
                    return;
                }

                //Si lo encontré pero no esta activo me regreso
                if(!client.activo)
                {
                    _view.ShowMessage("El cliente que intenta agregar esta desactivado");
                    _view.ClearClient();
                    return;
                }

                //Si lo encontre pero tiene lista de precio le digo que ya esta en una lista
                if (client.idListaDePrecio.isValid())
                {
                    _view.ShowMessage(string.Format("El cliente {0} ya esta ligado a la lista de precio {1}", client.codigo, client.ListasDePrecio.codigo));
                    _view.ClearClient();
                    return;
                }

                //De lo contrario muestro el cliente seleccionado
                _view.Show(client);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        #endregion

        #region Artículos

        private void CalculateByPriceWithTaxes()
        {
            if (!_view.Price.isValid() || !_view.Price.idArticulo.isValid())
                return;

            if (!_view.Price.Articulo.costoUnitario.isValid())
            {
                _view.ShowMessage("El artículo debe tener un costo registrado");
                var price = _view.Price;
                price.Articulo.costoUnitario = 0.0m;
                _view.Show(price);
                return;
            }

            if (!_view.Price.PrecioConImpuestos.isValid())
            {
                _view.ShowMessage("Debe capturar un precio con impuestos válido");
                var price = _view.Price;
                price.PrecioConImpuestos = 0.0m;
                _view.Show(price);
                return;
            }

            try
            {
                var price = _view.Price;

                //Debo calcular precio sin impuesto y utilidad 
                price.Precio = Operations.CalculatePriceWithoutTaxes(price.PrecioConImpuestos, price.Articulo.Impuestos.ToList());
                price.utilidad = Operations.CalculateUtility(price.Articulo.costoUnitario, price.Precio);
                _view.Show(price);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void CalculateByUtility()
        {
            if (!_view.Price.isValid() || !_view.Price.idArticulo.isValid())
                return;

            if (!_view.Price.Articulo.costoUnitario.isValid())
            {
                _view.ShowMessage("El artículo debe tener un costo registrado");
                var price = _view.Price;
                price.Articulo.costoUnitario = 0.0m;
                _view.Show(price);
                return;
            }

            if (!_view.Price.utilidad.isValid())
            {
                _view.ShowMessage("Debe capturar una utilidad válida");
                var price = _view.Price;
                price.utilidad = 0.0m;
                _view.Show(price);
                return;
            }

            try
            {
                var price = _view.Price;

                //Debo calcular precio sin impuesto y precio con impuestos
                price.Precio = Operations.CalculatePriceWithoutTaxes(price.Articulo.costoUnitario, price.utilidad);
                price.PrecioConImpuestos = price.Precio + Operations.CalculateTaxes(price.Precio, price.Articulo.Impuestos.ToList());
                _view.Show(price);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void CalculateByPrice()
        {
            if (!_view.Price.isValid() || !_view.Price.idArticulo.isValid())
                return;

            if (!_view.Price.Articulo.costoUnitario.isValid())
            {
                _view.ShowMessage("El artículo debe tener un costo registrado");
                var price = _view.Price;
                price.Articulo.costoUnitario = 0.0m;
                _view.Show(price);
                return;
            }

            if (!_view.Price.Precio.isValid())
            {
                _view.ShowMessage("Debe capturar un precio válido");
                var price = _view.Price;
                price.Precio = 0.0m;
                _view.Show(price);
                return;
            }

            try
            {
                var price = _view.Price;

                //Debo calcular utilidad y precio con impuestos
                price.utilidad = Operations.CalculateUtility(price.Articulo.costoUnitario, price.Precio);
                price.PrecioConImpuestos = price.Precio + Operations.CalculateTaxes(price.Precio, price.Articulo.Impuestos.ToList());
                _view.Show(price);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void SelectPriceItem()
        {
            if (!_view.CurrentPrice.isValid())
            {
                _view.ShowError("No hay ningún precio de artículo seleccionado para eliminar");
                return;
            }

            try
            {
                //Muestro la información del precio seleccionado
                _view.Show(_view.CurrentPrice);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void DeletePriceItem()
        {
            if (!_view.CurrentPrice.isValid())
            {
                _view.ShowError("No hay ningún precio de artículo seleccionado para eliminar");
                return;
            }

            try
            {

                var pricesList = _view.PricesList;

                pricesList.Precios.Remove(_view.PricesList.Precios.FirstOrDefault(p => p.idArticulo.Equals(_view.CurrentPrice.idArticulo)));

                //Si es una lista en edición lo elimino hasta la base de datos
                if(_view.IsDirty)
                    _prices.Remove(_view.CurrentPrice.ToPrecio());

                //Envío mensaje al usuario
                _view.ShowMessage("Precio eliminado exitosamente");

                //Actualizo la lista que estoy mostrando
                _view.Show(pricesList);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void AddOrUpdatePriceItem()
        {
            string error;
            bool dirtyPrice;

            if(!IsPriceItemValid(_view.Price,out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //Debo agregar el precio a la lista de precios o actualizar la información si ya es parte de ella

                //Reviso si es un artículo existente que solo se actualizará
                dirtyPrice = !_view.PricesList.Precios.isValid() || _view.PricesList.Precios.ToList().Exists(p => p.idArticulo.Equals(_view.Price.idArticulo));

                //Obtengo la lista de precios que se esta trabajando
                var pricesList = _view.PricesList;

                //Si es un artículo ya existente lo elimino de la lista
                if (dirtyPrice)
                    pricesList.Precios.Remove(_view.PricesList.Precios.FirstOrDefault(p => p.idArticulo.Equals(_view.Price.idArticulo)));

                //Agrego el artículo/precio nuevo o modificado a la lista de precios
                pricesList.Precios.Add(_view.Price.ToPrecio());

                //Si es una lista en edición debo guardar el cambio hasta la base de datos
                if (_view.IsDirty)
                {
                    //Si el artículo ya existia lo actualizo
                    if (dirtyPrice)
                        _prices.Update(_view.Price.ToPrecio());
                    else //De lo contrario lo agrego
                        _prices.Add(_view.Price.ToPrecio());
                }

                //Envío mensaje al usuario
                if (dirtyPrice)
                    _view.ShowMessage("Precio de artículo actualizado exitosamente");
                else
                    _view.ShowMessage("Precio de artículo agregado exitosamente");

                //Actualizo la lista que se esta mostrando
                _view.Show(pricesList);              
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void OpenItemsList()
        {
            if (!_view.PricesList.codigo.isValid())
            {
                _view.ShowMessage("Debe seleccionar o crear una lista primero");
                _view.ClearPrice();
                return;
            }

            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                //Si seleccionó uno lo muestro
                if (view.Item.idArticulo.isValid())
                {
                    //La lista de articulos ahora regresa una viewModel, se debe obtener el item correspondiente
                    var item = _items.Find(view.Item.idArticulo);

                    _view.Show(new VMPrecio(item));
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void FindPriceItem()
        {
            if (!_view.Price.Articulo.isValid() || !_view.Price.Articulo.codigo.isValid())
                return;

            if(!_view.PricesList.codigo.isValid())
            {
                _view.ShowMessage("Debe seleccionar o crear una lista primero");
                _view.ClearPrice();
                return;
            }

            try
            {
                var item = _items.Find(_view.Price.Articulo.codigo);

                //Si no encontré el artículo me regreso
                if(item == null)
                {
                    _view.ShowMessage("No existe ningún artículo con ese código");
                    _view.ClearPrice();
                    return;
                }

                //Si lo encontré pero no esta activo me regreso
                if(!item.activo)
                {
                    _view.ShowMessage("El artículo que intenta agregar esta desactivado");
                    _view.ClearPrice();
                    return;
                }

                //Convierto el Articulo en un VMPrecio y lo muestro para su edición
                var priceItem = new VMPrecio(item);

                _view.Show(priceItem);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsPriceItemValid(VMPrecio price, out string error)
        {
            if(!price.idArticulo.isValid())
            {
                error = "No hay un artículo seleccionado";
                return false;
            }

            if(!price.Articulo.codigo.isValid())
            {
                error = "Código del artículo no es válido";
                return false;
            }

            if(!price.Articulo.costoUnitario.isValid())
            {
                error = "El costo no es válido";
                return false;
            }

            if(!price.utilidad.isValid())
            {
                error = "La utilidad no puede ser menor a 0.0";
                return false;
            }

            if(!price.Precio.isValid())
            {
                error = "El precio no es válido";
                return false;
            }

            if(!price.PrecioConImpuestos.isValid())
            {
                error = "El precio con impuestos no es válido";
                return false;
            }

            error = string.Empty;
            return true;
        }

        #endregion

        #region Lista de precios

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
                IPricesListView view;
                PricesListPresenter presenter;

                view = new PricesListView();
                presenter = new PricesListPresenter(view, _prices);

                view.ShowWindow();

                //Si seleccionó alguna lista la muestro
                if (view.PricesList.idListaDePrecio.isValid())
                    _view.Show(view.PricesList);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Save()
        {
            string error;

            if(!IsPricesListValid(_view.PricesList, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                //La lista de precios tiene ViewModels, hay que convertirlos a Modelos reales
                var pricesList = _view.PricesList;
                pricesList.Precios = pricesList.Precios.ToBaseModelList();

                //Una vez validada la agrego a la lista existente
                _prices.Add(pricesList);

                //Mando el mensaje de éxito al usuario
                _view.ShowMessage(string.Format("Lista de precios {0} agregada exitosamente", _view.PricesList.codigo));

                //Limpio los datos en la vista
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if(!_view.PricesList.isValid() || !_view.IsDirty)
            {
                _view.ShowMessage("No hay ninguna lista seleccionada para eliminar");
                return;
            }

            try
            {
                //elimino la lista 
                _prices.Delete(_view.PricesList);

                //Envio mensaje al usuario
                _view.ShowMessage("Lista eliminada exitosamente");

                //Limpio la pantalla
                _view.Clear();
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
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.PricesList.isValid() || !_view.PricesList.codigo.isValid())
                return;

            try
            {
                var pricesList = _prices.Find(_view.PricesList.codigo);

                if (pricesList == null)
                    pricesList = new ListasDePrecio() { codigo = _view.PricesList.codigo, Clientes = new List<Cliente>(), Precios = new List<Precio>() };

                _view.Show(pricesList);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsPricesListValid(ListasDePrecio pricesList, out string error)
        {
            if(!pricesList.isValid())
            {
                error = "Lista de precios no es válida";
                return false;
            }

            if(!pricesList.codigo.isValid())
            {
                error = "El código de la lista de precios no es válido";
                return false;
            }

            if(!pricesList.Precios.isValid() || pricesList.Precios.Count.Equals(0))
            {
                error = "Debe haber al menos 1 precio de artículo en la lista";
                return false;
            }

            if(!pricesList.Clientes.isValid() || pricesList.Clientes.Count.Equals(0))
            {
                error = "Debe haber al menos 1 cliente en la lista";
                return false;
            }

            error = string.Empty;
            return true;
        }

        #endregion
    }
}
