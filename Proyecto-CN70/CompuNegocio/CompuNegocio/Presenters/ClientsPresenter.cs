using Aprovi.Business.Services;
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
    public class ClientsPresenter
    {
        private IClientsView _view;
        private IClienteService _clients;
        private ICatalogosEstaticosService _catalog;
        private IListaDePrecioService _pricesList;
        private ICuentaDeCorreoService _accounts;
        private IArticuloService _items;
        private IUsuarioService _users;
        private IUsosCFDIService _cfdiUsages;
        private IRegimenService _registrimen;
        private ICodigoDeArticuloPorClienteService _customerCodes;

        public ClientsPresenter(IClientsView view, IClienteService clientsService, ICatalogosEstaticosService catalogs, IListaDePrecioService pricesList, ICuentaDeCorreoService accounts, IArticuloService items, IUsuarioService users, IUsosCFDIService cfdiUsages, ICodigoDeArticuloPorClienteService customerCodes, IRegimenService regimen)
        {
            _view = view;
            _clients = clientsService;
            _catalog = catalogs;
            _pricesList = pricesList;
            _accounts = accounts;
            _items = items;
            _users = users;
            _cfdiUsages = cfdiUsages;
            _registrimen = regimen;
            _customerCodes = customerCodes;

            _view.Quit += Quit;
            _view.New += New;
            _view.Delete += Delete;
            _view.Save += Save;
            _view.Find += Find;
            _view.Update += Update;
            _view.OpenList += OpenList;
            _view.OpenSoldItemsList += OpenSoldItemsList;
            _view.OpenUsersList += OpenUsersList;
            _view.FindUser += FindUser;
            _view.OpenSalesList += OpenSalesList;
            _view.OpenItemsList += OpenItemsList;
            _view.FindItem += FindItem;
            _view.AddItemCode += AddItemCode;
            _view.DeleteItemCode += DeleteItemCode;

            _view.FillCombo(_catalog.ListPaises(), _pricesList.List(), _cfdiUsages.List(), _registrimen.List());

            //Cuentas de correo
            _view.AddAccount += AddAccount;
            _view.RemoveAccount += RemoveAccount;
            
        }

        private void OpenSalesList()
        {
            try
            {
                if (!_view.Client.isValid() || !_view.Client.idCliente.isValid())
                {
                    _view.ShowError("Se debe seleccionar un cliente");
                    return;
                }

                IClientSalesListView view;
                ClientSalesListPresenter presenter;

                view = new ClientSalesListView(_view.Client);
                presenter = new ClientSalesListPresenter(view, _clients);

                view.ShowWindow();
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
                Usuario user = _users.Find(_view.Client.Usuario.nombreDeUsuario);

                if (!user.isValid())
                {
                    _view.ShowError("No se encontró un usuario con el nombre de usuario proporcionado");
                    return;
                }

                _view.Show(user);
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
                UsersListPresenter usersPresenter = new UsersListPresenter(usersListView,_users);

                usersListView.ShowWindow();

                if (usersListView.User.isValid() && usersListView.User.idUsuario.isValid())
                {
                    _view.Show(usersListView.User);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenSoldItemsList()
        {
            if (!_view.Client.isValid() || !_view.Client.idCliente.isValid())
            {
                _view.ShowError("Se debe seleccionar un cliente");
                return;
            }

            try
            {
                ISoldItemsListView view;
                SoldItemsListPresenter presenter;

                view = new SoldItemsListView(_view.Client);
                presenter = new SoldItemsListPresenter(view, _items);

                view.ShowWindow();
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
                IClientsListView view;
                ClientsListPresenter presenter;

                view = new ClientsListView();
                presenter = new ClientsListPresenter(view, _clients);

                view.ShowWindow();

                if (view.Client.isValid() && view.Client.idCliente.isValid())
                    ShowClient(view.Client);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Update()
        {
            string error;

            if (!IsClientValid(_view.Client, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var client = _view.Client;
                client.activo = true;

                //Se usa la llave primaria como referencia
                client.Usuario = null;
                client.UsosCFDI = null;

                _clients.Update(client);

                _view.ShowMessage(string.Format("Cliente {0} modificado exitosamente", _view.Client.nombreComercial));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Find()
        {
            if (!_view.Client.codigo.isValid())
                return;

            try
            {
                var client = _clients.Find(_view.Client.codigo);

                if (client.isValid() && !client.activo)
                    _view.ShowMessage("El cliente ya existe pero esta marcado como inactivo, para reactivarlo solo de click en Guardar");

                if (client == null)
                    client = new Cliente() { codigo = _view.Client.codigo, Domicilio = new Domicilio() };

                ShowClient(client);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void ShowClient(Cliente client)
        {
            try
            {
                _view.Show(client);

                //Totales del cliente
                List<VwSaldosPorClientePorMoneda> totals = _clients.GetTotals(client);
                var totalDolares = totals.FirstOrDefault(x => x.idMoneda == (int)Monedas.Dólares);
                var totalPesos = totals.FirstOrDefault(x => x.idMoneda == (int)Monedas.Pesos);

                _view.ShowTotals(totalDolares, totalPesos);
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void Save()
        {
            string error;
            if (!IsClientValid(_view.Client, out error))
            {
                _view.ShowError(error);
                return;
            }

            try
            {
                var client = _view.Client;
                client.activo = true;

                //Se usa la llave primaria
                client.Usuario = null;
                client.comentario = ""; //En la tabla de clientes se indica que el campo comentario es obligatorio. JCRV 27/03/23
                
                _clients.Add(client);
                _view.ShowMessage(string.Format("El cliente {0} ha sido agregado exitosamente", client.nombreComercial));
                _view.Clear();
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void Delete()
        {
            if (!_view.IsDirty)
            {
                _view.ShowMessage("No existe un cliente seleccionado para eliminar");
                return;
            }

            try
            {
                if (_clients.CanDelete(_view.Client))
                {
                    _clients.Delete(_view.Client);
                }
                else
                {
                    var client = _view.Client;
                    client.activo = false;
                    _clients.Update(client);
                }

                _view.ShowMessage(string.Format("Cliente {0} removido exitosamente", _view.Client.nombreComercial));
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

        private void AddAccount()
        {
            try
            {
                var account = _view.Account;

                if (!account.cuenta.isValid())
                    throw new Exception("Debe especificar la cuenta de correo que desea agregar");

                var clientAccounts = _view.Client.CuentasDeCorreos;
                if (!clientAccounts.isValid())
                    clientAccounts = new List<CuentasDeCorreo>();

                if (clientAccounts.Any(c => c.cuenta.Equals(account.cuenta, StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception("Esta cuenta ya se encuentra registrada");

                //La agrego a la colección
                clientAccounts.Add(account);

                //Si es un cliente ya registrado, entonces la registro en la base de datos
                if (_view.IsDirty)
                    _accounts.Add(account);

                _view.Fill(clientAccounts.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void RemoveAccount()
        {
            try
            {
                if (!_view.Selected.cuenta.isValid())
                    throw new Exception("Debe seleccionar la cuenta a eliminar");

                var account = _view.Selected;
                var clientAccounts = _view.Client.CuentasDeCorreos;

                clientAccounts.Remove(account);

                if (_view.IsDirty)
                    _accounts.Remove(account);

                _view.Fill(clientAccounts.ToList());
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private bool IsClientValid(Cliente client, out string error)
        {

            if(!client.codigo.isValid())
            {
                error = "Código del cliente inválido";
                return false;
            }

            if(!client.nombreComercial.isValid())
            {
                error = "Nombre comercial no es válido";
                return false;
            }

            if(!client.razonSocial.isValid())
            {
                error = "Razón social no es válida";
                return false;
            }

            if(!client.Domicilio.calle.isValid())
            {
                error = "La calle no es válida";
                return false;
            }

            if(!client.Domicilio.numeroExterior.isValid())
            {
                error = "Número exterior no es válido";
                return false;
            }

            if(!client.Domicilio.colonia.isValid())
            {
                error = "Colonia no es válida";
                return false;
            }

            if(!client.Domicilio.ciudad.isValid())
            {
                error = "Ciudad no es válida";
                return false;
            }
            
            if(!client.Domicilio.estado.isValid())
            {
                error = "Estado no es válida";
                return false;
            }

            if(!client.Domicilio.idPais.isValid())
            {
                error = "País no es válido";
                return false;
            }

            if(!client.Domicilio.codigoPostal.isValid())
            {
                error = "Código postal no es válido";
                return false;
            }

            if(!client.idListaDePrecio.isValid())
            {
                error = "Debe especificar la lista de precios para el cliente";
                return false;
            }

            if (client.limiteCredito.GetValueOrDefault() < 0m)
            {
                error = "El límite de crédito solo puede contener números";
                return false;
            }

            if (client.diasCredito.GetValueOrDefault() < 0)
            {
                error = "Los días de crédito solo pueden contener números";
                return false;
            }

            error = string.Empty;
            return true;
        }

        #region Homologados

        private void DeleteItemCode()
        {
            try
            {
                var selected = _view.SelectedItemCode;

                if (!selected.isValid() || !selected.idCodigoDeArticuloPorCliente.isValid())
                    return;

                _customerCodes.Remove(selected);

                _view.ShowMessage("Código removido exitosamente");
                _view.Show(_view.Client.CodigosDeArticuloPorClientes.ToList());
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void AddItemCode()
        {
            try
            {
                var current = _view.CurrentItemCode;

                if (!current.idCliente.isValid())
                    throw new Exception("Debe existir un cliente selecionado para agregar códigos alternos");

                if (!current.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno para el producto");

                if (!current.Articulo.idArticulo.isValid())
                {
                    throw new Exception("Debe existir un artículo seleccionado para agregar códigos alternos");
                }

                var codes = _view.ItemCodes;
                //Si existe, cambio el código nuevo por el anterior
                var code = new CodigosDeArticuloPorCliente();
                var customer = _view.Client;
                //Reviso si ya existe un código para este artículo
                code = codes.FirstOrDefault(c => c.idArticulo == current.Articulo.idArticulo);

                //Si ya existe solo lo actualizo, si no entonces lo agrego
                if (code.isValid())
                {
                    code.codigo = current.codigo;
                    _customerCodes.Update(code);
                }
                else
                {
                    code = _customerCodes.Add(new CodigosDeArticuloPorCliente() { Articulo = current.Articulo, idArticulo = current.Articulo.idArticulo, codigo = current.codigo, idCliente = current.idCliente });
                    customer.CodigosDeArticuloPorClientes.Add(code);
                }

                _view.ShowMessage("Código alterno registrado exitosamente");
                _view.Show(customer.CodigosDeArticuloPorClientes.ToList());
                _view.ClearItemCode();
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void FindItem()
        {
            try
            {
                if (!_view.CurrentItemCode.isValid() || !_view.CurrentItemCode.codigo.isValid())
                    throw new Exception("Debe capturar el código alterno antes de seleccionar un artículo");

                var current = _view.CurrentItemCode;

                if (!current.Articulo.isValid() || !current.Articulo.codigo.isValid())
                {
                    current.Articulo = new Articulo();
                    _view.Show(current);
                    return;
                }

                var item = _items.Find(current.Articulo.codigo);

                if (!item.isValid())
                {
                    current.Articulo = new Articulo();
                    _view.Show(current);
                    throw new Exception("No existe ningún artículo con este código");
                }
                else
                {
                    current.Articulo = item;
                    _view.Show(current);
                }
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        private void OpenItemsList()
        {
            try
            {
                IItemsListView view;
                ItemsListPresenter presenter;

                view = new ItemsListView(true);
                presenter = new ItemsListPresenter(view, _items);

                view.ShowWindow();

                //Si seleccionó alguno lo muestro
                if (view.Item.idArticulo.isValid())
                    _view.Show(new CodigosDeArticuloPorCliente() { codigo = _view.CurrentItemCode.codigo, Articulo = view.Item});
            }
            catch (Exception e)
            {
                _view.ShowError(e.Message);
            }
        }

        #endregion
    }
}
