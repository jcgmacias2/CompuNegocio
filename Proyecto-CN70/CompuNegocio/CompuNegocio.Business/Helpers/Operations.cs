using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Business.ViewModels;

namespace Aprovi.Business.Helpers
{
    public static class Operations
    {
        #region Operaciones que involucran calculos para los datos de artículos y son reutilizadas en distintas partes de la aplicación

        /// <summary>
        /// Obtiene el total de los impuestos que un artículo determinado debe pagar
        /// </summary>
        /// <param name="price">Precio sin impuestos</param>
        /// <param name="taxes">Lista de impuestos a cargar</param>
        /// <returns>Total de impuestos a pagar</returns>
        public static decimal CalculateTaxes(decimal price, List<Impuesto> taxes)
        {
            //BR: Base gravable IEPS = precio unitario
            //BR: Base gravable IVA = precio unitario +/- IEPS (Traslados y Retenidos)
            try
            {
                decimal total;
                decimal totalIEPS;
                decimal totalIVA;
                decimal baseIEPS;
                decimal baseIVA;

                //1.- Base gravable IEPS = precio
                baseIEPS = Math.Round(price, 2, MidpointRounding.AwayFromZero);

                //2.- Calculo los IEPS
                totalIEPS = 0.0m;
                foreach (var t in taxes.Where(i => i.nombre.Equals(Impuestos.IEPS.ToString())).ToList())
                {
                    total = (t.valor / 100.0m) * baseIEPS;
                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado))
                        totalIEPS = totalIEPS + total;

                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido))
                        totalIEPS = totalIEPS - total;
                }

                //3.- Obtengo la base gravable del IVA
                // Base gravable IVA = precio +/- IEPS
                baseIVA = totalIEPS.isValid() ? price + Math.Abs(totalIEPS) : price - Math.Abs(totalIEPS);

                //4.- Calculo los IVA's
                totalIVA = 0.0m;
                foreach (var t in taxes.Where(i => i.nombre.Equals(Impuestos.IVA.ToString())).ToList())
                {
                    total = (t.valor / 100.0m) * baseIVA;
                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado))
                        totalIVA = totalIVA + total;

                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido))
                        totalIVA = totalIVA - total;
                }

                //5.- Calculo el total de impuestos
                //total = totalIEPS + totalIVA
                total = totalIEPS + totalIVA;

                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite obtener la utilidad basado en el costo unitario y su precio sin impuestos
        /// </summary>
        /// <param name="cost">Costo unitario</param>
        /// <param name="price">Precio sin impuestos</param>
        /// <returns>Porcentaje de utilidad</returns>
        public static decimal CalculateUtility(decimal cost, decimal price)
        {
            try
            {
                return (price - cost) * (100.0m / cost);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite obtener el precio sin impuestos a partir de un precio con impuestos y la lista de impuestos cargados
        /// </summary>
        /// <param name="price">Precio con impuestos</param>
        /// <param name="taxes">Impuestos incluidos en el precio con impuestos</param>
        /// <returns>Precio sin impuestos</returns>
        public static decimal CalculatePriceWithoutTaxes(decimal price, List<Impuesto> taxes)
        {
            try
            {
                decimal priceWithoutTaxes;

                //Si no hay impuestos que calcularle el precio es el mismo
                if (taxes.Count.Equals(0))
                    return price;

                priceWithoutTaxes = 0.0m;

                foreach (var t in taxes)
                {
                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado))
                        priceWithoutTaxes = priceWithoutTaxes - (100.0m * (price / (t.valor + 100.0m)));

                    if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido))
                        priceWithoutTaxes = priceWithoutTaxes + (100.0m * (price / (t.valor + 100.0m)));
                }

                return Math.Abs(priceWithoutTaxes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite obtener el precio sin impuestos a partir del costo y la utilidad
        /// </summary>
        /// <param name="cost">Costo del artículo</param>
        /// <param name="utility">Utilidad del artículo</param>
        /// <returns>Precio sin impuestos</returns>
        public static decimal CalculatePriceWithoutTaxes(decimal cost, decimal utility)
        {
            try
            {
                return Math.Round((cost * (utility / 100.0m)) + cost, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite obtener el precio registrado a partir del precio original y el descuento aplicado
        /// </summary>
        /// <param name="cost">Precio registrado del artículo</param>
        /// <param name="discount">Descuento que se le aplico</param>
        /// <returns>Precio con descuento</returns>
        public static decimal CalculatePriceWithDiscount(decimal cost, decimal discount)
        {
            try
            {
                return cost - Math.Round((discount / 100.0m) * cost, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Permite obtener el porcentaje de descuento correspondiente a la modificación de precios
        /// </summary>
        /// <param name="origialPrice">Precio original</param>
        /// <param name="newPrice">Precio con descuento</param>
        /// <returns>Porcentaje de descuento</returns>
        public static decimal CalculateDiscount(decimal origialPrice, decimal newPrice)
        {
            try
            {
                return Math.Round((origialPrice - newPrice) * (100.0m / origialPrice), 6, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Provee la base gravable de un artículo basado en su precio, cantidad, impuesto a calcular e impuestos que paga
        /// </summary>
        /// <param name="precio">Precio unitario del artículo</param>
        /// <param name="cantidad">Cantidad de unidades</param>
        /// <param name="tax">Impuesto que se va a calcular</param>
        /// <param name="taxes">Impuestos que se cargaran</param>
        /// <returns>Base gravable del artículo</returns>
        public static decimal CalculateTaxBase(decimal precio, decimal cantidad, Impuesto tax, List<Impuesto> taxes)
        {
            //Note:Si las reglas de negocio de esta función cambian, también hay que cambiarlas en sql
            // en la funcion GetTaxBaseByDetail ya que las implementa de igual manera

            //El criterio para la obtención de la base gravable es:
            //BR: Base gravable IEPS = precio unitario
            //BR: Base gravable IVA = precio unitario +/- IEPS (Traslados y Retenidos)

            decimal taxBase;

            try
            {
                //La base más simple
                taxBase = Math.Round(Math.Round(precio, 2, MidpointRounding.AwayFromZero) * cantidad, 2, MidpointRounding.AwayFromZero);

                //Si es un solo impuesto (no importa si es IVA o IEPS, o ISR) la base gravable es precio * cantidad
                if (taxes.Count.Equals(1))
                    return taxBase;

                //Si se desea calcular IEPS la base es precio * cantidad
                if (tax.nombre.Equals(Impuestos.IEPS.ToString()))
                    return taxBase;

                //Si se desea calcular IVA hay que ver si es sobre producto con IEPS o sin él
                if (tax.nombre.Equals(Impuestos.IVA.ToString()))
                {
                    //El iva cuando se mezcla con IEPS, debe calcular primero los IEPS
                    foreach (var t in taxes.Where(i => i.nombre.Equals(Impuestos.IEPS.ToString())))
                    {
                        if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado))
                            taxBase = taxBase + ((precio * cantidad) * t.valor / 100);

                        if (t.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido))
                            taxBase = taxBase - ((precio * cantidad) * t.valor / 100);
                    }
                }

                return taxBase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Compras

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de la compra
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="purchaseDetail">Detalle de la compra que funge como base para el calculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<DetallesDeCompra> purchaseDetail)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in purchaseDetail.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.costoUnitario, d.cantidad, tax, d.Impuestos.ToList());
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Facturas

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de factura
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="invoiceDetail">Detalle de la factura que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDeFactura> invoiceDetail)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in invoiceDetail.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.precioUnitario, d.cantidad, tax, d.Impuestos.ToList());
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Provee la forma de pago perteneciente a una factura en base a su lista de abonos y las reglas del SAT, lo que significa que la forma de pago oficial es la que cubra el mayor monto
        /// </summary>
        /// <param name="payments">Lista de abonos</param>
        /// <returns>Abono con forma de pago oficial</returns>
        public static AbonosDeFactura GetFormaDePago(ICollection<AbonosDeFactura> payments)
        {
            try
            {
                var abonos = payments.Where(p => p.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));
                if (abonos.Count().Equals(0))
                    return null;

                if (abonos.Count().Equals(1))
                    return payments.First();

                return abonos.OrderByDescending(a => a.idMoneda.Equals((int)Monedas.Pesos) ? a.monto : a.monto * a.tipoDeCambio).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Remisiones

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de remisión
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="billOfSaleDetails">Detalle de la factura que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDeRemision> billOfSaleDetails)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in billOfSaleDetails.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.precioUnitario, d.cantidad, tax, d.Impuestos.ToList());
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Cotizaciones

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de cotizacion
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="cotizacionDetails">Detalle de la cotizacion que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDeCotizacion> cotizacionDetails)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in cotizacionDetails.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.precioUnitario, d.cantidad, tax, d.Impuestos.ToList());
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Pedidos

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de pedidos
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="orderDetails">Detalle del pedido que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDePedido> orderDetails)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in orderDetails.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.PrecioUnitario, d.Cantidad, tax, new List<Impuesto>(d.Impuestos));
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Ordenes de compra

        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de orden de compra
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="orderDetails">Detalle de la orden de compra que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDeOrdenDeCompra> orderDetails)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in orderDetails.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.CostoUnitario, d.Cantidad, tax, new List<Impuesto>(d.Impuestos));
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Operaciones con Notas de Credito
        /// <summary>
        /// Calcula el monto gravable del impuesto en cuestion que aplica sobre los detalles de nota de credito
        /// </summary>
        /// <param name="tax">Impuesto del que se desea obtener el monto gravable</param>
        /// <param name="invoiceDetail">Detalle de la nota de credito que funge como base para el cálculo de impuestos</param>
        /// <returns>Importe del impuesto</returns>
        public static decimal CalculateTaxBaseAmount(Impuesto tax, List<VMDetalleDeNotaDeCredito> creditNoteDetail)
        {
            try
            {
                var amount = 0.0m;
                //Solo voy a iterar sobre los artículos que contienen el impuesto
                foreach (var d in creditNoteDetail.Where(d => d.Impuestos.Contains(tax)))
                {
                    //En CalculateTaxBase calcula articulo por articulo
                    amount += CalculateTaxBase(d.precioUnitario, d.cantidad, tax, d.Impuestos.ToList());
                }

                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
