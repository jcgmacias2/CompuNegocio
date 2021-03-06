using Aprovi.Data.Core;
using Aprovi.Data.Repositories;
using Aprovi.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.Helpers;
using Aprovi.Data.Models;

namespace Aprovi.Business.Services
{
    public abstract class PedidoService : IPedidoService
    {
        private IUnitOfWork _UOW;
        private IPedidosRepository _orders;
        private IConfiguracionService _config;
        private IArticulosRepository _items;
        private IImpuestosRepository _taxes;
        private IEstacionesRepository _stations;
        private IViewReporteEstatusDeLaEmpresaPedidosRepository _companyStatusOrders;
        
        public PedidoService(IUnitOfWork unitOfWork,  IConfiguracionService config)
        {
            _UOW = unitOfWork;
            _orders = _UOW.Pedidos;
            _config = config;
            _items = _UOW.Articulos;
            _taxes = _UOW.Impuestos;
            _stations = _UOW.Estaciones;
            _companyStatusOrders = _UOW.EstatusDeLaEmpresaPedidos;
        }

        public int Next()
        {
            try
            {
                return _orders.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                return _orders.Last();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Pedido Add(VMPedido order)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la empresa configurada
                order.idEmpresa = config.Estacion.idEmpresa;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                order.folio = _orders.Next();

                //Le agrego estado
                order.idEstatusDePedido = (int)StatusDePedido.Registrado;

                //Guardo el pedido
                var local = order.ToPedido(_items);

                //Defaults de pedido
                local.Cliente = null;
                local.Usuario = null;

                //Ahora si guardo
                _orders.Add(local);
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido Add(Pedido order, Estacione station)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la empresa configurada
                order.idEmpresa = _stations.Find(station.idEstacion).idEmpresa;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                order.folio = _orders.Next();

                //Le agrego estado
                order.idEstatusDePedido = (int)StatusDePedido.Registrado;

                //Defaults de pedido
                order.Cliente = null;
                order.Usuario = null;

                //Ahora si guardo
                _orders.Add(order);
                _UOW.Save();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido Find(int idOrder)
        {
            try
            {
                //Este debe pasarse como objecto
                return _orders.Find((object)idOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido FindByFolio(int folio)
        {
            try
            {
                return _orders.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> List()
        {
            try
            {
                return _orders.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Pedido> WithFolioOrClientLike(string value)
        {
            try
            {
                return _orders.WithFolioOrClientLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido Cancel(int idOrder, string reason)
        {
            try
            {
                var order = _orders.FindById(idOrder);
                //No puedo cancelar un pedido que este surtido
                if (order.idEstatusDePedido != (int)StatusDePedido.Registrado)
                    throw new Exception("No es posible cancelar un pedido surtido");

                order.idEstatusDePedido = (int)StatusDePedido.Cancelado;
                order.EstatusDePedido = null;
                order.CancelacionesDePedido = new CancelacionesDePedido();
                order.CancelacionesDePedido.fechaHora = DateTime.Now;
                order.CancelacionesDePedido.motivo = reason;

                _orders.Update(order);
                _UOW.Save();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pedido Update(VMPedido newOrder)
        {
            try
            {
                //Se obtiene el pedido original
                var order = newOrder.ToPedido(_items);
                var orderDb = _orders.Find((object)order.idPedido);

                //El status siempre se actualiza
                orderDb.idEstatusDePedido = order.idEstatusDePedido;

                //El pedido se actualiza siempre que no se este surtiendo
                if (!newOrder.Detalles.Any(x => x.Surtido > 0))
                {
                    if (orderDb.idEstatusDePedido == (int)StatusDePedido.Cancelado)
                    {
                        throw new Exception("No se pueden modificar pedidos cancelados");
                    }

                    //Se actualiza el pedido
                    orderDb.tipoDeCambio = order.tipoDeCambio;
                    orderDb.idMoneda = order.idMoneda;
                    orderDb.DatosExtraPorPedidoes = order.DatosExtraPorPedidoes;

                    var dbDetail = orderDb.DetallesDePedidoes.ToList();

                    //Se elimina el detalle anterior
                    dbDetail.ForEach(x => _orders.DeleteDetail(x));

                    //Se debe crear el detalle nuevo
                    orderDb.DetallesDePedidoes = order.DetallesDePedidoes;
                }

                _UOW.Save();

                return orderDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DetallesDePedido> PendingOrdersReport()
        {
            try
            {
                List<Pedido> orders = new List<Pedido>();

                orders.AddRange(_orders.WithStatus(StatusDePedido.Nuevo));
                orders.AddRange(_orders.WithStatus(StatusDePedido.Surtido_Parcial));

                return orders.SelectMany(x => x.DetallesDePedidoes).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DetallesDePedido> CustomerOrders(Cliente customer)
        {
            try
            {
                return _orders.WithCustomer(customer).OrderByDescending(x=>x.folio).SelectMany(x=>x.DetallesDePedidoes).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListOrdersForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyStatusOrders.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                vm.TotalDolaresPedidos = detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalPesosPedidos = detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalDolaresPedidosImpuestosRetenidos = detailDollars.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalDolaresPedidosImpuestosTrasladados = detailDollars.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));
                vm.TotalPesosPedidosImpuestosRetenidos = detailPesos.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalPesosPedidosImpuestosTrasladados = detailPesos.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
