using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace Aprovi.Business.Services
{
    public abstract class CompraService : ICompraService
    {
        private IUnitOfWork _UOW;
        private IConfiguracionService _config;
        private IOrdenDeCompraService _purchaseOrders;
        private ICatalogosEstaticosService _catalogs;
        private IUnidadDeMedidaService _measureUnits;
        private IComprasRepository _purchases;
        private IEquivalenciasRepository _equivalencies;
        private IArticulosRepository _items;
        private IViewReporteEstatusDeLaEmpresaComprasRepository _companyStatusPurchases;
        private IViewReporteEstatusDeLaEmpresaCuentasPorPagarRepository _companyStatusPayableAmounts;
        
        public CompraService(IUnitOfWork unitOfWork, IConfiguracionService config, IOrdenDeCompraService purchaseOrders, ICatalogosEstaticosService catalogs, IUnidadDeMedidaService measureUnits)
        {
            _UOW = unitOfWork;
            _config = config;
            _purchaseOrders = purchaseOrders;
            _catalogs = catalogs;
            _measureUnits = measureUnits;
            _purchases = _UOW.Compras;
            _equivalencies = _UOW.Equivalencias;
            _items = _UOW.Articulos;
            _companyStatusPurchases = _UOW.EstatusDeLaEmpresaCompras;
            _companyStatusPayableAmounts = _UOW.EstatusDeLaEmpresaCuentasPorPagar;
        }

        public Compra Add(Compra purchase)
        {
            try
            {
                Articulo item;

                //Elimino los datos que no requiere
                purchase.Proveedore = null;
                purchase.EstatusDeCompra = null;
                purchase.Usuario = null;
                purchase.Moneda = null;
                purchase.OrdenesDeCompra = null;

                //Agrego la compra, (la registro)
                _purchases.Add(purchase);

                //Se guardan los cambios de la compra
                _UOW.Save();

                //Si el articulo tiene una orden de compra, se debe actualizar el estado
                if (purchase.idOrdenDeCompra.HasValue && purchase.idOrdenDeCompra.Value.isValid())
                {
                    OrdenesDeCompra orderDb = _purchaseOrders.Find(purchase.idOrdenDeCompra.Value);
                    VMOrdenDeCompra orderVM = new VMOrdenDeCompra(orderDb, Find(orderDb));

                    if (orderVM.Detalles.Any(x => x.Pendiente > 0))
                    {
                        orderDb.idEstatusDeOrdenDeCompra = (int) StatusDeOrdenDeCompra.Surtido_Parcial;
                    }
                    else
                    {
                        orderDb.idEstatusDeOrdenDeCompra = (int)StatusDeOrdenDeCompra.Surtido_Total;
                    }

                    _purchaseOrders.Update(orderDb);
                }

                //Al grabar la compra deben actualizarse los costos de los articulos y la moneda

                //Debido a que el artículo puede aparecer multiples veces en el detalle, debo verificar por cada artículo que tengo el precio más alto
                foreach (var d in purchase.DetallesDeCompras.Distinct<DetallesDeCompra>(new CompareOnlyItem()))
                {
                    //Busco el artículo que voy a actualizar
                    item = _items.Find(d.idArticulo);
                    
                    //Le calculo el nuevo costo
                    //Se modifica el articulo de acuerdo a la configuracion de costos
                    Configuracion config = _config.GetDefault();

                    //al ya no actualizar la moneda, se debe calcular el costo en base a la moneda del articulo
                    decimal costo = GetHighestCost(item.idArticulo, purchase.DetallesDeCompras).ToDocumentCurrency(new Moneda(){idMoneda = purchase.idMoneda}, item.Moneda, purchase.tipoDeCambio).ToRoundedCurrency(item.Moneda);

                    if (item.costoUnitario > costo)
                    {
                        //Aumento el costo
                        if (config.idOpcionCostoAumenta == (int) Opciones_Costos.Utilidad_bruta)
                        {
                            SetUtilities(item, costo);
                        }
                    }

                    if (item.costoUnitario < costo)
                    {
                        //Disminuyo el costo
                        if (config.idOpcionCostoDisminuye == (int) Opciones_Costos.Utilidad_bruta)
                        {
                            SetUtilities(item,costo);
                        }
                    }

                    //Se actualiza el costo del articulo
                    item.costoUnitario = costo;

                    //Actualizo la información del artículo
                    _items.Update(item);
                }

                //Guardo los cambios, tanto el registro de la compra como la actualización de costos
                _UOW.Save();
                return purchase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Compra Find(int idSupplier, string folio)
        {
            try
            {
                //El filtro de que ignore las canceladas se encuentra en el repository
                return _purchases.Find(idSupplier, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compra> WithFolioOrSupplierLike(string search)
        {
            try
            {
                return _purchases.WithFolioOrSupplierLike(search).Where(c => !c.idEstatusDeCompra.Equals((int)StatusDeCompra.Cancelada)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Compra> List()
        {
            try
            {
                return _purchases.List().Where(c => !c.idEstatusDeCompra.Equals((int)StatusDeCompra.Cancelada)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMCompra> ByPeriod(DateTime start, DateTime end)
        {
            try
            {
                var vmPurchases = new List<VMCompra>();

                var purchases = _purchases.List(start, end);

                purchases.ForEach(c => vmPurchases.Add(new VMCompra(c)));

                return vmPurchases;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMCompra> ByPeriodAndSupplier(DateTime start, DateTime end, Proveedore supplier)
        {
            try
            {
                var vmPurchases = new List<VMCompra>();

                var purchases = _purchases.List(start, end, supplier);

                purchases.ForEach(c => vmPurchases.Add(new VMCompra(c)));

                return vmPurchases;
            }
            catch (Exception)
            {   
                throw;
            }
        }

        public Compra Cancel(Compra purchase, string reason)
        {
            try
            {
                var local = _purchases.Find(purchase.idCompra);

                //Se agrega registro en la tabla de cancelaciones
                local.CancelacionesDeCompra = new CancelacionesDeCompra()
                {
                    fechaHora = DateTime.Now,
                    motivo = reason
                };

                local.idEstatusDeCompra = (int)StatusDeCompra.Cancelada;
                //Marco los abonos como cancelados
                local.AbonosDeCompras.ToList().ForEach(a => a.cancelado = true);
                _purchases.Update(local);
                _UOW.Save();

                return local;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Compra Last(Proveedore supplier)
        {
            try
            {
                return _purchases.Last(supplier.idProveedor);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Compra Find(int idPurchase)
        {
            try
            {
                return _purchases.Find(idPurchase);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DetallesDeCompra> Find(OrdenesDeCompra purchaseOrder)
        {
            try
            {
                return _purchases.Find(purchaseOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Compra Import(Compra purchase, XmlDocument doc)
        {
            //Se procede a efectuar la importacion
            string serie;
            string folio;
            decimal exchangeRate;
            string currency;
            DateTime date;
            List<DetallesDeCompra> items = new List<DetallesDeCompra>();

            //Folio
            serie = doc.DocumentElement.Attributes["Serie"].Value;
            folio = doc.DocumentElement.Attributes["Folio"].Value;

            //Tipo de cambio
            if (doc.DocumentElement.Attributes["TipoCambio"].isValid())
            {
                exchangeRate = decimal.Parse(doc.DocumentElement.Attributes["TipoCambio"].Value);
            }
            else
            {
                exchangeRate = 1.0m;
            }

            //Moneda
            currency = doc.DocumentElement.Attributes["Moneda"].Value;

            //Fecha
            date = DateTimeOffset.ParseExact(doc.DocumentElement.Attributes["Fecha"].Value,"yyyy-MM-ddTHH:mm:ss",CultureInfo.InvariantCulture).Date;

            var itemsNodes = doc.DocumentElement["cfdi:Conceptos"].ChildNodes;

            foreach (var node in itemsNodes)
            {
                var element = node as XmlElement;

                string codigo;
                string unidad;
                decimal cantidad;
                decimal costo;
                Articulo item;
                UnidadesDeMedida measureUnit;

                //Codigo
                codigo = element.Attributes["NoIdentificacion"].Value;

                //Unidad
                unidad = element.Attributes["ClaveUnidad"].Value;

                //Cantidad
                cantidad = decimal.Parse(element.Attributes["Cantidad"].Value);

                //Costo Unitario
                costo = decimal.Parse(element.Attributes["ValorUnitario"].Value);

                //Unidad de medida
                measureUnit = _measureUnits.Find(unidad);

                if (!measureUnit.isValid())
                {
                    throw new Exception(string.Format("No se encontró la unidad de medida con código:{0}", unidad));
                }

                //Articulo
                item = _items.Find(codigo);

                if (!item.isValid())
                {
                    //Se busca por codigo alterno
                    item = _items.FindAllForProvider(codigo, purchase.Proveedore.idProveedor).FirstOrDefault();
                }

                if (!item.isValid())
                {
                    throw new Exception(string.Format("No se encontró el artículo con código:{0}", codigo));
                }

                items.Add(new DetallesDeCompra()
                {
                    Articulo = item,
                    cantidad = cantidad,
                    costoUnitario = costo,
                    UnidadesDeMedida = measureUnit,
                    Impuestos = item.Impuestos,
                    idArticulo = item.idArticulo,
                    idUnidadDeMedida = measureUnit.idUnidadDeMedida
                });
            }

            var localCurrency = _catalogs.ListMonedas().FirstOrDefault(x => x.codigo.Equals(currency, StringComparison.InvariantCultureIgnoreCase));
            purchase.folio = string.Format("{0}{1}", serie, folio);
            purchase.tipoDeCambio = exchangeRate;
            purchase.Moneda = localCurrency;
            purchase.fechaHora = date;
            purchase.DetallesDeCompras = items;
            purchase.idMoneda = localCurrency.idMoneda;

            return purchase;
        }

        public VMEstadoDeLaEmpresa ListPurchasesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyStatusPurchases.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int) Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int) Monedas.Dólares).ToList();

                vm.TotalDolaresCompras = detailDollars.Sum(x=>x.importe.GetValueOrDefault(0m));
                vm.TotalPesosCompras = detailPesos.Sum(x=>x.importe.GetValueOrDefault(0m));
                vm.TotalDolaresComprasImpuestosRetenidos = detailDollars.Sum(x=>x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalDolaresComprasImpuestosTrasladados = detailDollars.Sum(x=>x.impuestosTrasladados.GetValueOrDefault(0m));
                vm.TotalPesosComprasImpuestosRetenidos = detailPesos.Sum(x=>x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalPesosComprasImpuestosTrasladados = detailPesos.Sum(x=>x.impuestosTrasladados.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListPayableBalancesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyStatusPayableAmounts.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                vm.TotalDolaresCuentasPorPagar = detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalPesosCuentasPorPagar = detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));
                vm.TotalDolaresCuentasPorPagarImpuestosRetenidos = detailDollars.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalDolaresCuentasPorPagarImpuestosTrasladados = detailDollars.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));
                vm.TotalPesosCuentasPorPagarImpuestosRetenidos = detailPesos.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalPesosCuentasPorPagarImpuestosTrasladados = detailPesos.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private Helpers

        /// <summary>
        /// Obtiene el costo más alto de un artículo determinado dentro de un detalle de compras
        /// </summary>
        /// <param name="idItem">Id del artículo del cual se desea conocer el costo unitario más alto</param>
        /// <param name="details">Detalle de la compra donde puede haber multiples veces el artículo, inclusive en distintas presentaciones</param>
        /// <returns>Costo unitario más alto</returns>
        private decimal GetHighestCost(int idItem, ICollection<DetallesDeCompra> details)
        {
            try
            {
                decimal highest;
                decimal cost;

                //Inicializo el costo mas alto para tener una base de comparación
                highest = 0.0m;

                //Itero hasta encontrar el más alto 
                foreach (var d in details.Where(d => d.idArticulo.Equals(idItem)))
                {
                    //Calculo el costo unitario por la unidad de medida base, ya que en la compra se puede adquirir por distintas unidades de medida
                    cost = GetUnitCostByBaseUnit(d.Articulo, d.UnidadesDeMedida, d.costoUnitario);
                    //Si el costo (calculado) es mayor que el más alto existente, entonces highest = cost, de lo contrario sigue siendo highest
                    highest = cost > highest ? cost : highest;
                }

                return highest;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Calcula el costo unitario por la unidad base
        /// </summary>
        /// <param name="item">Artículo con unidad de medida base</param>
        /// <param name="purchaseUnit">Unidad en la que se compro</param>
        /// <param name="purchaseUnitCost">Costo unitario por la unidad en que se compro</param>
        /// <returns>Costo unitario por unidad base</returns>
        private decimal GetUnitCostByBaseUnit(Articulo item, UnidadesDeMedida purchaseUnit, decimal purchaseUnitCost)
        {
            try
            {
                //Si la unidad de medida base es igual a la unidad de medida de compra entonces el precio unitario es el que se paso
                if (item.idUnidadDeMedida.Equals(purchaseUnit.idUnidadDeMedida))
                    return purchaseUnitCost;

                //Si no son iguales entonces busco la equivalencia para saber entre cuantas unidades debo dividir el precio unitario de compra
                return purchaseUnitCost / item.Equivalencias.ToList().FirstOrDefault(e => e.idUnidadDeMedida.Equals(purchaseUnit.idUnidadDeMedida)).unidades;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Establece las utilidades de un articulo en base al costo
        /// </summary>
        /// <param name="item">Articulo al que se le estableceran las utilidades</param>
        /// <param name="costo">Costo del articulo</param>
        private void SetUtilities(Articulo item, decimal costo)
        {
            foreach (Precio p in item.Precios)
            {
                decimal precio = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, p.utilidad);
                p.utilidad = Operations.CalculateUtility(costo, precio);
            }
        }
        #endregion

    }
}
