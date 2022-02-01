using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public abstract class ClienteService : IClienteService
    {
        private IUnitOfWork _UOW;
        private IClientesRepository _clients;
        private IDomiciliosRepository _addresses;
        private IViewSaldosPorClientePorMonedaRepository _totals;
        private IViewVentasActivasPorClienteRepository _sales;
        private IViewSaldoDeudorPorClientePorMonedaRepository _debtBalances;

        public ClienteService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _clients = _UOW.Clientes;
            _addresses = _UOW.Domicilios;
            _totals = _UOW.SaldosPorCliente;
            _sales = _UOW.VentasPorCliente;
            _debtBalances = _UOW.SaldoDeudorPorClientePorMoneda;
        }

        public Cliente Add(Cliente client)
        {
            try
            {
                _clients.Add(client);
                _UOW.Save();
                return client;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente Find(int idClient)
        {
            try
            {
                return _clients.Find(idClient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente Find(string code)
        {
            try
            {
                return _clients.Find(code);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cliente> List()
        {
            try
            {
                return _clients.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cliente> WithNameLike(string name)
        {
            try
            {
                return _clients.WithNameLike(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente Update(Cliente client)
        {
            try
            {
                _UOW.Reload();
                var local = _clients.Find(client.idCliente);
                local.activo = client.activo;
                local.correoElectronico = client.correoElectronico;
                local.Domicilio.calle = client.Domicilio.calle;
                local.Domicilio.numeroExterior = client.Domicilio.numeroExterior;
                local.Domicilio.numeroInterior = client.Domicilio.numeroInterior;
                local.Domicilio.ciudad = client.Domicilio.ciudad;
                local.Domicilio.codigoPostal = client.Domicilio.codigoPostal;
                local.Domicilio.colonia = client.Domicilio.colonia;
                local.Domicilio.estado = client.Domicilio.estado;
                local.Domicilio.idPais = client.Domicilio.idPais;
                local.nombreComercial = client.nombreComercial;
                local.razonSocial = client.razonSocial;
                local.rfc = client.rfc;
                local.telefono = client.telefono;
                local.contacto = client.contacto;
                local.condicionDePago = client.condicionDePago;
                local.idListaDePrecio = client.idListaDePrecio;
                local.idVendedor = client.idVendedor;
                local.limiteCredito = client.limiteCredito;
                local.diasCredito = client.diasCredito;
                local.idUsoCFDI = client.idUsoCFDI.isValid() ? client.idUsoCFDI : (int?)null;

                _clients.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Cliente client)
        {
            try
            {
                return _clients.CanDelete(client.idCliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Cliente client)
        {
            try
            {
                if (!client.Domicilio.idDomicilio.isValid())
                    client = _clients.Find(client.idCliente);

                _addresses.Remove(client.Domicilio.idDomicilio);
                _clients.Remove(client.idCliente);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Exist(string rfc)
        {
            try
            {
                return _clients.Exist(rfc).isValid();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Import(string dbcPath)
        {
            try
            {
                //Obtengo la lista de clientes de VFP
                var catalog = _clients.List(dbcPath);

                //Por cada artículos en el catálogo voy a procesarlo para agregarlo a la base de datos
                foreach (var client in catalog)
                {
                    var newClient = new Cliente();
                    //Si ya existe me lo salto
                    newClient = _clients.SearchAll(client.codigo);
                    if (newClient.isValid() || !client.rfc.isValid() || _clients.Exist(client.rfc).isValid())
                        continue;

                    //Si llegó aqui es nuevo
                    newClient = new Cliente();
                    newClient.activo = true;
                    newClient.codigo = client.codigo;
                    newClient.nombreComercial = client.nombreComercial;
                    newClient.razonSocial = client.razonSocial;
                    newClient.rfc = client.rfc.ToTrimmedString(20);
                    newClient.telefono = client.telefono.ToTrimmedString(20);
                    newClient.correoElectronico = client.correoElectronico;
                    newClient.contacto = client.contacto;
                    newClient.Domicilio = client.Domicilio;
                    newClient.Domicilio.numeroInterior = newClient.Domicilio.numeroInterior.ToTrimmedString(10);
                    newClient.Domicilio.numeroExterior = newClient.Domicilio.numeroExterior.ToTrimmedString(10);
                    newClient.idListaDePrecio = client.idListaDePrecio;
                    newClient.CuentasDeCorreos = client.CuentasDeCorreos;
                    newClient.condicionDePago = string.Empty;

                    //Agrego el nuevo artículo
                    _clients.Add(newClient);
                }

                //Una vez que he procesado todos, entonces hago persistentes los cambios
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwSaldosPorClientePorMoneda> GetTotals(Cliente client)
        {
            try
            {
                return _totals.List(client.idCliente);
            }
            catch (TimeoutException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwVentasActivasPorCliente> GetActiveTransactions(Cliente client, string term, bool withDebtOnly)
        {
            try
            {
                if (term.isValid())
                {
                    return _sales.Like(client.idCliente, term, withDebtOnly);
                }
                else
                {
                    return _sales.List(client.idCliente, withDebtOnly);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool HasAvailableCredit(Cliente customer,Moneda currency, decimal amount)
        {
            try
            {
                var customerDb = _clients.Find(customer.idCliente);
                var customerDebt = _debtBalances.FindByCustomerAndCurrency(customer, currency);
                

                decimal currentDebt = customerDebt.Saldo.GetValueOrDefault(0);
                decimal authorizedCredit = customerDb.limiteCredito.GetValueOrDefault(0);

                if ((currentDebt + amount) <= authorizedCredit)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
