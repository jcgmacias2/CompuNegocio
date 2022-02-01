using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class ListaDePrecioService : IListaDePrecioService
    {
        private IUnitOfWork _UOW;
        private IListasDePrecioRepository _pricesList;
        private IClientesRepository _clients;
        private IPreciosRepository _prices;

        public ListaDePrecioService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _pricesList = _UOW.ListasDePrecio;
            _clients = _UOW.Clientes;
            _prices = _UOW.Precios;
        }

        public ListasDePrecio Add(ListasDePrecio pricesList)
        {
            try
            {
                //Extraigo los clientes de la lista
                var clients = pricesList.Clientes.ToList();

                //Agrego la lista sin clientes
                pricesList.Clientes.Clear();
                _pricesList.Add(pricesList);
                _UOW.Save();

                //Una vez registrada la lista, le agrego a cada cliente el id de la lista
                foreach (var c in clients)
                {
                    var client = _clients.Find(c.idCliente);
                    client.idListaDePrecio = pricesList.idListaDePrecio;
                    _clients.Update(client);
                }

                //Guardo los cambios efectuados a los cleintes
                _UOW.Save();
                
                //Regreso los clientes a la lista de precios
                pricesList.Clientes = clients;

                return pricesList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> List()
        {
            try
            {
                return _pricesList.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithCodeLike(string code)
        {
            try
            {
                return _pricesList.WithCodeLike(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithItemLike(string item)
        {
            try
            {
                return _pricesList.WithItemLike(item);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithClientLike(string client)
        {
            try
            {
                return _pricesList.WithClientLike(client);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ListasDePrecio Find(string code)
        {
            try
            {
                return _pricesList.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ListasDePrecio Find(int idPricesList)
        {
            try
            {
                return _pricesList.Find(idPricesList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Precio Find(int idItem, int idClient)
        {
            try
            {
                return _prices.First(idItem, idClient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ListasDePrecio Update(ListasDePrecio pricesList)
        {
            try
            {
                return _pricesList.Update(pricesList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(ListasDePrecio pricesList)
        {
            try
            {
                //Paso todos los clientes a la lista A
                foreach (var c in pricesList.Clientes)
                {
                    var client = _clients.Find(c.idCliente);
                    client.ListasDePrecio = null;
                    client.idListaDePrecio = (int)Data.Models.List.A;
                }

                _pricesList.Remove(pricesList.idListaDePrecio);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Precio Add(Precio price)
        {
            try
            {
                _UOW.Reload();
                price.Articulo = null;
                _prices.Add(price);
                _UOW.Save();
                return price;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(Precio price)
        {
            try
            {
                var local = _prices.Find(price.idListaDePrecio, price.idArticulo);
                _prices.Remove(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Precio Update(Precio price)
        {
            try
            {
                var local = _prices.Find(price.idListaDePrecio, price.idArticulo);
                local.utilidad = price.utilidad;
                _prices.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente Add(Cliente client)
        {
            try
            {
                _UOW.Reload();
                var local = _clients.Find(client.idCliente);
                local.idListaDePrecio = client.idListaDePrecio;
                _clients.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Remove(Cliente client)
        {
            try
            {
                var local = _clients.Find(client.idCliente);
                //Si quito al cliente, siempre lo mando a la A
                local.idListaDePrecio = (int)Data.Models.List.A;
                local.ListasDePrecio = null;
                _clients.Update(local);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
