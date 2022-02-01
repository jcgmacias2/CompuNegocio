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
    public abstract class OrdenDeCompraService : IOrdenDeCompraService
    {
        private IUnitOfWork _UOW;
        private IOrdenesDeCompraRepository _orders;
        private IConfiguracionService _config;
        private IComprasRepository _purchases;
        private IArticulosRepository _items;
        private IImpuestosRepository _taxes;

        public OrdenDeCompraService(IUnitOfWork unitOfWork, IConfiguracionService config)
        {
            _UOW = unitOfWork;
            _orders = _UOW.OrdenesDeCompra;
            _config = config;
            _purchases = _UOW.Compras;
            _items = _UOW.Articulos;
            _taxes = _UOW.Impuestos;
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

        public virtual OrdenesDeCompra Add(VMOrdenDeCompra order)
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
                order.idEstatusDeOrdenDeCompra = (int) StatusDeOrdenDeCompra.Registrado;

                //Guardo la orden de compra
                var local = order.ToOrdenDeCompra(_items);

                //Defaults de orden de compra 
                local.Proveedore = null;
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

        public OrdenesDeCompra Find(int idOrder, int idProveedor)
        {
            try
            {
                return _orders.Find(idOrder,idProveedor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra Find(int idOrder)
        {
            try
            {
                return _orders.Find(idOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra FindByFolio(int folio)
        {
            try
            {
                return _orders.FindByFolio(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OrdenesDeCompra> List()
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

        public List<OrdenesDeCompra> WithFolioOrProviderLike(string value)
        {
            try
            {
                return _orders.WithFolioOrProviderLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra Cancel(int idOrder, string reason)
        {
            try
            {
                var order = _orders.FindById(idOrder);
                //No puedo cancelar un pedido que este surtido
                if (order.idEstatusDeOrdenDeCompra != (int) StatusDeOrdenDeCompra.Registrado)
                    throw new Exception("No es posible cancelar un pedido surtido");

                order.idEstatusDeOrdenDeCompra = (int) StatusDeOrdenDeCompra.Cancelado;
                order.EstatusDeOrdenDeCompra = null;
                order.CancelacionesDeOrdenesDeCompra = new CancelacionesDeOrdenesDeCompra();
                order.CancelacionesDeOrdenesDeCompra.fechaHora = DateTime.Now;
                order.CancelacionesDeOrdenesDeCompra.motivo = reason;

                _orders.Update(order);
                _UOW.Save();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra Update(OrdenesDeCompra order)
        {
            try
            {
                _orders.Update(order);
                _UOW.Save();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OrdenesDeCompra Update(OrdenesDeCompra order, List<VMDetalleDeOrdenDeCompra> detail)
        {
            try
            {
                //Se obtiene el pedido original
                var orderDb = _orders.Find((object)order.idOrdenDeCompra);

                if (orderDb.idEstatusDeOrdenDeCompra != (int)StatusDePedido.Registrado)
                {
                    throw new Exception("No se pueden modificar pedidos surtidos o cancelados");
                }

                //Se determina si hubo cambios en el detalle
                var dbDetail = orderDb.DetallesDeOrdenDeCompras.ToList();
                List<DetallesDeOrdenDeCompra> removedItems = new List<DetallesDeOrdenDeCompra>();
                List<DetallesDeOrdenDeCompra> addedItems = new List<DetallesDeOrdenDeCompra>();

                foreach (var d in dbDetail)
                {
                    var detailItem = detail.FirstOrDefault(x => x.idArticulo == d.idArticulo && x.CostoUnitario == d.costoUnitario);

                    if (!detailItem.isValid())
                    {
                        //Se elimino el articulo del detalle
                        removedItems.Add(d);
                        continue;
                    }
                }

                foreach (var d in detail)
                {
                    var item = dbDetail.FirstOrDefault(x => x.idArticulo == d.idArticulo && x.costoUnitario == d.CostoUnitario);
                    //Se buscan detalles nuevos

                    if (!item.isValid())
                    {
                        //Se agrego el articulo al detalle
                        addedItems.Add(d.ToDetalleDeOrdenDeCompra());
                    }
                }

                //Ahora si se pueden efectuar los cambios a la entidad de EF
                removedItems.ForEach(x => _orders.DeleteDetail(x));
                addedItems.ForEach(x => orderDb.DetallesDeOrdenDeCompras.Add(x));

                _UOW.Save();

                return orderDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal PendingUnitsForOrderItem(int idOrdenDeCompra, int idArticulo)
        {
            try
            {
                var purchaseOrder = _orders.Find(idOrdenDeCompra);
                var relatedDetails = _purchases.Find(purchaseOrder);

                var detail = purchaseOrder.DetallesDeOrdenDeCompras.FirstOrDefault(x => x.idArticulo == idArticulo);

                if (!detail.isValid())
                {
                    //Si el articulo no esta en la orden de compra, no puede tener unidades pendientes
                    return 0;
                }

                //Se deben calcular las unidades
                decimal totalSurtido = 0m;
                decimal totalDetalle = 0m;

                foreach (var d in relatedDetails)
                {
                    if (d.idArticulo == idArticulo)
                    {
                        var equivalencia = d.Articulo.Equivalencias.ToList().FirstOrDefault(e => e.idUnidadDeMedida.Equals(d.idUnidadDeMedida));
                        decimal units = (equivalencia.isValid() ? equivalencia.unidades : 1m) * d.cantidad;

                        totalSurtido += units;
                    }
                }

                foreach (var d in purchaseOrder.DetallesDeOrdenDeCompras)
                {
                    if (d.idArticulo == idArticulo)
                    {
                        var equivalencia = d.Articulo.Equivalencias.ToList().FirstOrDefault(e => e.idUnidadDeMedida.Equals(d.idUnidadDeMedida));
                        decimal units = (equivalencia.isValid() ? equivalencia.unidades : 1m) * d.cantidad;

                        totalDetalle += units;
                    }
                }

                return totalDetalle - totalSurtido;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
