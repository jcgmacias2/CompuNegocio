using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Aprovi
{
    public static partial class Extensions
    {
        /// <summary>
        /// Realiza el cambio de divisas de una cantidad a la moneda del documento
        /// </summary>
        /// <param name="amount">Cantidad que se desea convertir</param>
        /// <param name="amountCurrency">Moneda en que esta la cantidad a convertir</param>
        /// <param name="documentCurrency">Moneda en que esta registrado el documento</param>
        /// <param name="documentExchangeRate">Tipo de cambio del documento</param>
        /// <returns></returns>
        public static decimal ToDocumentCurrency(this decimal amount, Moneda amountCurrency, Moneda documentCurrency, decimal documentExchangeRate)
        {
            try
            {
                //Si son iguales la cantidad es la misma
                if (documentCurrency.idMoneda.Equals(amountCurrency.idMoneda))
                    return amount;

                //Si el documento esta en Pesos, cambio los dolares a pesos
                if (documentCurrency.idMoneda.Equals((int)Monedas.Pesos))
                    return amount * documentExchangeRate;

                //Si el documento esta en Dolares, cambios los pesos a dolares
                if (documentCurrency.idMoneda.Equals((int)Monedas.Dólares))
                    return amount / documentExchangeRate;

                //Si se anexa otra moneda podría darse el caso de que ningún if se cumpla, para ese caso y por el momento regreso la misma cantidad
                return amount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToTdCFDI_Importe(this decimal amount)
        {
            try
            {
                return Math.Round(amount, 6, MidpointRounding.AwayFromZero).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToStringRoundedCurrency(this decimal amount, Moneda currency)
        {
            try
            {
                return Math.Round(amount, currency.decimales, MidpointRounding.AwayFromZero).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static decimal ToRoundedCurrency(this decimal amount, Moneda currency)
        {
            try
            {
                return Math.Round(amount, currency.decimales, MidpointRounding.AwayFromZero);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToPorcentageString(this decimal number)
        {
            try
            {
                return (number / 100.0m).ToString("0.000000");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una fecha a su formato UTC
        /// </summary>
        /// <param name="dt">fecha hora</param>
        /// <returns>Fecha hora en formato UTC</returns>
        public static string ToUTCFormat(this DateTime dt)
        {
            try
            {
                return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Parses a datetime on UTC format on a string into a valid DateTime value
        /// </summary>
        /// <param name="str">Datetime string on UTC format</param>
        /// <returns>Datetime equivalent</returns>
        public static DateTime ToDateFromUTC(this string str)
        {
            try
            {
                return DateTime.SpecifyKind(DateTime.Parse(str), DateTimeKind.Utc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Masks a double value into a 17 characters long string, using 10 chars for int and 6 for decimal
        /// </summary>
        /// <param name="number">Decimal value</param>
        /// <returns>17 characters string</returns>
        public static string ToFormat17(this decimal number)
        {
            try
            {
                return number.ToString("0000000000.000000");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Precios

        /// <summary>
        /// Convierte una colección de Precio a su ViewModel VMPrecio
        /// </summary>
        /// <param name="prices">Colección de Precio's</param>
        /// <returns>Colección de VMPrecio's</returns>
        public static List<VMPrecio> ToViewModelList(this ICollection<Precio> prices)
        {
            try
            {
                List<VMPrecio> vmPrecios;
                vmPrecios = new List<VMPrecio>();

                prices.ToList().ForEach(p => vmPrecios.Add(new VMPrecio(p)));

                return vmPrecios;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una colección de Precio's con metadata de VMPrecio a su base Model Precio
        /// </summary>
        /// <param name="prices">Colección de VMPrecio's</param>
        /// <returns>Colección de Precio's</returns>
        public static List<Precio> ToBaseModelList(this ICollection<Precio> prices)
        {
            try
            {
                List<Precio> precios;
                precios = new List<Precio>();

                prices.ToList().ForEach(p => precios.Add(new Precio() { idArticulo = p.idArticulo, idListaDePrecio = p.idListaDePrecio, utilidad = p.utilidad }));

                return precios;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Articulos

        /// <summary>
        /// Convierte una colección de Articulos a su ViewModel VMArticulo
        /// </summary>
        /// <param name="items">Colección de Articulos</param>
        /// <returns>Colección de VMArticulo's</returns>
        public static List<VMArticulo> ToViewModelList(this ICollection<Articulo> items)
        {
            try
            {
                List<VMArticulo> vmArticulos;
                vmArticulos = new List<VMArticulo>();

                items.ToList().ForEach(i => vmArticulos.Add(new VMArticulo(i)));

                return vmArticulos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una colección de Articulos a su ViewModel VMArticulo asignandole el precio del cliente en caso de que exista un precio especial
        /// </summary>
        /// <param name="items">Colección de Articulos</param>
        /// <param name="client">Cliente sobre el cual revisar si existe precio especial</param>
        /// <returns>Colección de VMArticulo's</returns>
        public static List<VMArticulo> ToViewModelList(this ICollection<Articulo> items, Cliente client)
        {
            try
            {
                List<VMArticulo> vmArticulos;
                vmArticulos = new List<VMArticulo>();

                items.ToList().ForEach(i => vmArticulos.Add(new VMArticulo(i, client.ListasDePrecio.Precios.ToList())));

                return vmArticulos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Compras

        /// <summary>
        /// Convierte una colección de Compra a su ViewModel VMCompra
        /// </summary>
        /// <param name="purchases">Colección de Compra's</param>
        /// <returns>Colección de VMCompra's</returns>
        public static List<VMCompra> ToViewModelList(this ICollection<Compra> purchases)
        {
            try
            {
                List<VMCompra> vmCompras;
                vmCompras = new List<VMCompra>();

                purchases.ToList().ForEach(p => vmCompras.Add(new VMCompra(p)));

                return vmCompras;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de una compra
        /// </summary>
        /// <param name="purchase">Compra a la que se va a recalcular</param>
        /// <returns>Compra recalculada</returns>
        public static VMCompra UpdateAccount(this VMCompra purchase)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                purchase.Subtotal = purchase.DetallesDeCompras.Sum(d => Math.Round(d.cantidad * d.costoUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                purchase.DetallesDeCompras.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                purchase.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>()) //Solo impuestos unicos
                {
                    purchase.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, purchase.DetallesDeCompras.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = purchase.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe) - purchase.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe);
                purchase.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Calculo el total de la compra
                purchase.Total = Math.Round(purchase.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero) + Math.Round(purchase.cargos, 2, MidpointRounding.AwayFromZero) - Math.Round(purchase.descuentos, 2, MidpointRounding.AwayFromZero);

                //Calculo lo abonado
                purchase.Abonado = purchase.AbonosDeCompras.Where(a => !a.cancelado).Sum(a => a.monto.ToDocumentCurrency(a.Moneda, purchase.Moneda, purchase.tipoDeCambio));

                return purchase;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Facturas

        /// <summary>
        /// Convierte una colección de Factura su ViewModel VMFactura
        /// </summary>
        /// <param name="invoices">Colección de Factura's</param>
        /// <returns>Colección de VMFactura's</returns>
        public static List<VMFactura> ToViewModelList(this ICollection<Factura> invoices)
        {
            try
            {
                List<VMFactura> vmFacturas;
                vmFacturas = new List<VMFactura>();

                invoices.ToList().ForEach(i => vmFacturas.Add(new VMFactura(i)));

                return vmFacturas;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<DetallesDeFactura> ToDetalleDeFactura(this ICollection<VMDetalleDeFactura> detail)
        {
            return detail.Select(x => x.ToDetalleFactura()).ToList();
        }

        public static List<VMDetalleDeFactura> ToViewModelList(this ICollection<DetallesDeFactura> detail)
        {
            return detail.Select(x => new VMDetalleDeFactura(x)).ToList();
        }

        /// <summary>
        /// Convierte una colección de AbonosDeFactura a su ViewModel VMParcialidad
        /// </summary>
        /// <param name="payments">Colección de AbonosDeFactura's</param>
        /// <returns>Colección de VMParcialidad's</returns>
        public static List<VMParcialidad> ToViewModelList(this ICollection<AbonosDeFactura> payments)
        {
            try
            {
                List<VMParcialidad> vmParcialidades;
                vmParcialidades = new List<VMParcialidad>();

                payments.ToList().ForEach(a => vmParcialidades.Add(new VMParcialidad(a)));

                return vmParcialidades;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de una factura
        /// </summary>
        /// <param name="invoice">Factura a la que se va a recalcular</param>
        /// <returns>Factura recalculada</returns>
        public static VMFactura UpdateAccount(this VMFactura invoice)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                invoice.Subtotal = invoice.DetalleDeFactura.Sum(d => Math.Round(d.cantidad * d.precioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                invoice.DetalleDeFactura.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                invoice.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    invoice.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, invoice.DetalleDeFactura.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe.ToRoundedCurrency(invoice.Moneda)) - invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe.ToRoundedCurrency(invoice.Moneda));
                invoice.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Se calcula el total de descuentos
                //Notas de descuento que no hayan sido convertidas a nota de crédito
                var descuentos = invoice.NotasDeDescuentoes.Where(nd => nd.idEstatusDeNotaDeDescuento != (int)StatusDeNotaDeDescuento.Cancelada && !nd.idNotaDeCredito.HasValue).Sum(nd => nd.monto.ToDocumentCurrency(nd.Moneda, invoice.Moneda, invoice.tipoDeCambio));
                var creditos = invoice.NotasDeCreditoes.Where(nc => nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Pendiente_De_Timbrado) || nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Timbrada)).Sum(nc => nc.importe.ToDocumentCurrency(nc.Moneda, invoice.Moneda, invoice.tipoDeCambio));
                invoice.Acreditado = Math.Round(descuentos, 2, MidpointRounding.AwayFromZero) + Math.Round(creditos, 2, MidpointRounding.AwayFromZero);

                //Calculo el total de la factura
                invoice.Total = Math.Round(invoice.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero); 

                //Calculo lo abonado (ignoro los cancelados)
                var x = invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));
                invoice.Abonado = Math.Round(x.Sum(a => a.monto.ToDocumentCurrency(a.Moneda, invoice.Moneda, invoice.tipoDeCambio)),2,MidpointRounding.AwayFromZero);
                //invoice.Abonado = invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado)).Sum(a => a.monto.ToDocumentCurrency(a.Moneda, invoice.Moneda, invoice.tipoDeCambio));

                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de una factura
        /// </summary>
        /// <param name="invoice">Factura a la que se va a recalcular</param>
        /// <returns>Factura recalculada</returns>
        public static VMRFactura UpdateAccount(this VMRFactura invoice)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                invoice.Subtotal = invoice.DetalleDeFactura.Sum(d => Math.Round(d.cantidad * d.precioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                invoice.DetalleDeFactura.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                invoice.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    invoice.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, invoice.DetalleDeFactura.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe.ToRoundedCurrency(invoice.Moneda)) - invoice.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe.ToRoundedCurrency(invoice.Moneda));
                invoice.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //4.- Calculo el total de la factura
                invoice.Total = Math.Round(invoice.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                //5.- Calculo lo abonado (ignoro los cancelados)
                var x = invoice.AbonosDeFacturas.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado));
                invoice.Abonado = x.Sum(a => a.monto.ToDocumentCurrency(a.Moneda, invoice.Moneda, invoice.tipoDeCambio));

                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMArticulosConPedimento> GetOnlyImported(this ICollection<VMDetalleDeFactura> details)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                details.ToList().Where(d => d.Articulo.importado).ToList().ForEach(d => imported.Add(new VMArticulosConPedimento() { Articulo = d.Articulo, Vendidos = d.cantidad, Pedimentos = new List<VMPedimentoAsociado>(), Descuento = d.descuento, PrecioUnitario = d.precioUnitario}));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMFactura ToCurrency(this VMFactura invoice, Moneda from)
        {
            if (invoice.idMoneda == from.idMoneda)
            {
                //La moneda no cambió, se deja igual
                return invoice;
            }

            foreach (VMDetalleDeFactura d in invoice.DetalleDeFactura)
            {
                d.precioUnitario = d.precioUnitario.ToDocumentCurrency(from, invoice.Moneda, invoice.tipoDeCambio);
            }

            //Se actualizan los totales
            invoice.UpdateAccount();

            return invoice;
        }

        public static List<VMDetalleDeNotaDeCredito> ToDetallesDeNotaDeCredito(this ICollection<VMDetalleDeFactura> detail)
        {
            return detail.Select(x => x.ToDetalleDeNotaDeCredito()).ToList();
        }
        #endregion

        #region Remisiones

        /// <summary>
        /// Convierte una colección de Remisión su ViewModel VMRemision
        /// </summary>
        /// <param name="billOfSales">Colección de Remisione's</param>
        /// <returns>Colección de VMRemision's</returns>
        public static List<VMRemision> ToViewModelList(this ICollection<Remisione> billOfSales)
        {
            try
            {
                List<VMRemision> vmRemisiones;
                vmRemisiones = new List<VMRemision>();

                billOfSales.ToList().ForEach(bol => vmRemisiones.Add(new VMRemision(bol)));

                return vmRemisiones;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una colección de Impuestos a su ViewModel VMImpuesto
        /// </summary>
        /// <param name="taxes">Colección de Impuestos</param>
        /// <returns>Colección de VMImpuestos</returns>
        public static List<VMImpuesto> ToViewModelList(this ICollection<Impuesto> taxes)
        {
            try
            {
                List<VMImpuesto> vmTaxes;
                vmTaxes = new List<VMImpuesto>();

                taxes.ToList().ForEach(t => vmTaxes.Add(new VMImpuesto(t)));

                return vmTaxes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeRemision> ToViewModelList(this ICollection<DetallesDeRemision> detail)
        {
            return detail.Select(x => new VMDetalleDeRemision(x)).ToList();
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de una factura
        /// </summary>
        /// <param name="billOfSale">Factura a la que se va a recalcular</param>
        /// <returns>Factura recalculada</returns>
        public static VMRemision UpdateAccount(this VMRemision billOfSale)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                billOfSale.Subtotal = billOfSale.DetalleDeRemision.Sum(d => Math.Round(d.cantidad * d.precioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                billOfSale.DetalleDeRemision.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                billOfSale.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    billOfSale.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, billOfSale.DetalleDeRemision.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = billOfSale.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe) - billOfSale.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe);
                billOfSale.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Calculo el total de la compra
                billOfSale.Total = Math.Round(billOfSale.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                //Calculo lo abonado
                billOfSale.Abonado = billOfSale.AbonosDeRemisions.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado)).Sum(a => a.monto.ToDocumentCurrency(a.Moneda, billOfSale.Moneda, billOfSale.tipoDeCambio));

                return billOfSale;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeFactura> ToDetalleDeFactura(this ICollection<VMDetalleDeRemision> detail)
        {
            return detail.Select(x => x.ToDetalleDeFactura()).ToList();
        }

        public static List<DetallesDeRemision> ToDetalleDeRemision(this ICollection<VMDetalleDeRemision> detail)
        {
            return detail.Select(x => x.ToDetalleDeRemision()).ToList();
        }

        public static List<AbonosDeFactura> ToInvoicePayments(this ICollection<AbonosDeRemision> payments)
        {
            try
            {
                List<AbonosDeFactura> iPayments;
                iPayments = new List<AbonosDeFactura>();

                payments.Where(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado)).ToList().ForEach(p => iPayments.Add(new AbonosDeFactura() { fechaHora = DateTime.Now, monto = p.monto, idMoneda = p.idMoneda, Moneda = p.Moneda, idFormaPago = p.idFormaPago, FormasPago = p.FormasPago, tipoDeCambio = p.tipoDeCambio, idEstatusDeAbono = (int)StatusDeAbono.Registrado }));

                return iPayments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMArticulosConPedimento> GetOnlyImported(this ICollection<VMDetalleDeRemision> details)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                details.ToList().Where(d => d.Articulo.importado).ToList().ForEach(d => imported.Add(new VMArticulosConPedimento() { Articulo = d.Articulo, Vendidos = d.cantidad, Pedimentos = new List<VMPedimentoAsociado>(), PrecioUnitario = d.precioUnitario, Descuento = d.descuento}));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMRemision> ToCurrency(this ICollection<VMRemision> billsOfSale, Moneda moneda)
        {
            foreach (var billOfSale in billsOfSale)
            {
                //Si la moneda no es la moneda destino, se convierte el precio de cada articulo
                if (billOfSale.idMoneda != moneda.idMoneda)
                {
                    foreach (var dr in billOfSale.DetalleDeRemision)
                    {
                        dr.precioUnitario =
                            dr.precioUnitario.ToDocumentCurrency(new Moneda(){idMoneda = billOfSale.idMoneda}, moneda, billOfSale.tipoDeCambio);
                    }

                    billOfSale.idMoneda = moneda.idMoneda;
                    billOfSale.Moneda = null;
                }

                //Convierte los abonos a la moneda destino
                foreach (var abono in billOfSale.AbonosDeRemisions)
                {
                    if (abono.idMoneda != moneda.idMoneda)
                    {
                        abono.monto = abono.monto.ToDocumentCurrency(new Moneda(){idMoneda = abono.idMoneda}, moneda, abono.tipoDeCambio);
                        abono.idMoneda = moneda.idMoneda;

                        //Se requiere en la lista de abonos
                        abono.Moneda = moneda;
                    }
                }
            }

            return billsOfSale.ToList();
        }

        public static List<VMRemision> ToCurrency(this ICollection<VMRemision> billsOfSale, Moneda moneda, decimal exchangeRate)
        {
            foreach (var billOfSale in billsOfSale)
            {
                //Si la moneda no es la moneda destino, se convierte el precio de cada articulo
                if (billOfSale.idMoneda != moneda.idMoneda)
                {
                    foreach (var dr in billOfSale.DetalleDeRemision)
                    {
                        dr.precioUnitario =
                            dr.precioUnitario.ToDocumentCurrency(new Moneda() { idMoneda = billOfSale.idMoneda }, moneda, exchangeRate);
                    }

                    billOfSale.idMoneda = moneda.idMoneda;
                    billOfSale.Moneda = null;
                }

                //Convierte los abonos a la moneda destino
                foreach (var abono in billOfSale.AbonosDeRemisions)
                {
                    if (abono.idMoneda != moneda.idMoneda)
                    {
                        abono.monto = abono.monto.ToDocumentCurrency(new Moneda() { idMoneda = abono.idMoneda }, moneda, exchangeRate);
                        abono.idMoneda = moneda.idMoneda;

                        //Se requiere en la vista de abonos
                        abono.Moneda = moneda;
                    }
                }
            }

            return billsOfSale.ToList();
        }

        public static VMRemision ToCurrency(this VMRemision billOfSale, Moneda from)
        {
            if (billOfSale.idMoneda == from.idMoneda)
            {
                //La moneda no cambió, se deja igual
                return billOfSale;
            }

            foreach (VMDetalleDeRemision d in billOfSale.DetalleDeRemision)
            {
                d.precioUnitario = d.precioUnitario.ToDocumentCurrency(from, billOfSale.Moneda, billOfSale.tipoDeCambio);
            }

            //Se actualizan los totales
            billOfSale.UpdateAccount();

            return billOfSale;
        }
        #endregion

        #region Cotizaciones

        /// <summary>
        /// Convierte una colección de Cotizacion su ViewModel VMCotizacion
        /// </summary>
        /// <param name="quotes">Colección de Cotizaciones</param>
        /// <returns>Colección de VMCotizacion</returns>
        public static List<VMCotizacion> ToViewModelList(this ICollection<Cotizacione> quotes)
        {
            try
            {
                List<VMCotizacion> vmCotizaciones;
                vmCotizaciones = new List<VMCotizacion>();

                quotes.ToList().ForEach(c => vmCotizaciones.Add(new VMCotizacion(c)));

                return vmCotizaciones;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total de una cotizacion
        /// </summary>
        /// <param name="quote">Cotizacion que se va a recalcular</param>
        /// <returns>Cotizacion recalculada</returns>
        public static VMCotizacion UpdateAccount(this VMCotizacion quote)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                quote.Subtotal = quote.DetalleDeCotizacion.Sum(d => Math.Round(d.cantidad * d.precioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                quote.DetalleDeCotizacion.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                quote.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    quote.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, quote.DetalleDeCotizacion.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = quote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe) - quote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe);
                quote.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Calculo el total de la compra
                quote.Total = Math.Round(quote.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMCotizacion ToCurrency(this VMCotizacion quote, Moneda from)
        {
            try
            {
                if (quote.idMoneda == from.idMoneda)
                {
                    //La moneda no cambió, se deja igual
                    return quote;
                }

                foreach (VMDetalleDeCotizacion d in quote.DetalleDeCotizacion)
                {
                    d.precioUnitario = d.precioUnitario.ToDocumentCurrency(from, quote.Moneda, quote.tipoDeCambio);
                }

                //Se actualizan los totales
                quote.UpdateAccount();

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<DetallesDeCotizacion> ToDetallesDeCotizacion(this ICollection<VMDetalleDeCotizacion> detail)
        {
            try
            {
                return detail.Select(x => x.ToDetalleDeCotizacion()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeCotizacion> ToViewModel(this ICollection<DetallesDeCotizacion> detail)
        {
            try
            {
                return detail.Select(x => new VMDetalleDeCotizacion(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeFactura> ToDetallesDeFactura(this ICollection<VMDetalleDeCotizacion> detail)
        {
            try
            {
                return detail.Select(x => x.ToDetalleDeFactura()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeRemision> ToDetallesDeRemision(this ICollection<VMDetalleDeCotizacion> detail)
        {
            try
            {
                return detail.Select(x => x.ToDetalleDeRemision()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Pedidos
        /// <summary>
        /// Totaliza una lista de detalles de pedidos
        /// </summary>
        /// <param name="details">detalles</param>
        /// <returns></returns>
        public static List<VMDetalleDePedido> Totalize(this List<VMDetalleDePedido> details)
        {
            try
            {
                List<VMDetalleDePedido> orders = new List<VMDetalleDePedido>();

                foreach (var detail in details)
                {
                    VMDetalleDePedido d = orders.FirstOrDefault(x => x.idArticulo == detail.idArticulo);

                    if (d.isValid())
                    {
                        //Se actualiza la cantidad del detalle
                        d.Cantidad += detail.Cantidad;
                        d.surtidoEnOtros += detail.surtidoEnOtros;
                    }
                    else
                    {
                        //Se agrega el detalle
                        orders.Add(detail);
                    }
                }

                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de un pedido
        /// </summary>
        /// <param name="order">Pedido que se va a recalcular</param>
        /// <returns>Pedido recalculado</returns>
        public static VMPedido UpdateAccount(this VMPedido order)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                order.Subtotal = order.Detalles.Sum(d => Math.Round(d.Cantidad * d.PrecioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                order.Detalles.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                order.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    order.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, order.Detalles.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = order.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe) - order.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe);
                order.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Calculo el total de la compra
                order.Total = Math.Round(order.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMPedido ToCurrency(this VMPedido order, Moneda from)
        {
            try
            {
                if (order.idMoneda == from.idMoneda)
                {
                    //La moneda no cambió, se deja igual
                    return order;
                }

                foreach (VMDetalleDePedido d in order.Detalles)
                {
                    d.PrecioUnitario = d.PrecioUnitario.ToDocumentCurrency(from, order.Moneda, order.tipoDeCambio);
                }

                //Se actualizan los totales
                order.UpdateAccount();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Pedimentos

        public static List<PedimentoPorDetalleDeFactura> ToPedimentosFactura(this ICollection<PedimentoPorDetalleDeRemision> customsApplications)
        {
            try
            {
                var pedimentos = new List<PedimentoPorDetalleDeFactura>();
                customsApplications.ToList().ForEach(ca => pedimentos.Add(new PedimentoPorDetalleDeFactura() { idPedimento = ca.idPedimento, cantidad = ca.cantidad, Pedimento = ca.Pedimento }));
                return pedimentos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ToPedimentText(this Pedimento pedimento)
        {
            return string.Format("{0}  {1}  {2}  {3}{4}",pedimento.añoOperacion,pedimento.aduana,pedimento.patente,pedimento.añoEnCurso,pedimento.progresivo);
        }

        #endregion

        #region Ajustes

        public static List<VMArticulosConPedimento> GetOnlyImported(this ICollection<DetallesDeAjuste> details)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                details.ToList().Where(d => d.Articulo.importado).ToList().ForEach(d => imported.Add(new VMArticulosConPedimento() { Articulo = d.Articulo, Vendidos = d.cantidad, Pedimentos = new List<VMPedimentoAsociado>() }));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Inventario Fisico

        public static List<VMInventario> ToVMInventario(this List<VwReporteInventarioFisico> vw)
        {
            List<VMInventario> stock = new List<VMInventario>();

            foreach (var item in vw)
            {
                stock.Add(new VMInventario(item));
            }

            return stock;
        }

        #endregion

        #region Ordenes De Compra
        /// <summary>
        /// Actualiza el subtotal, impuestos, total y abonado de un pedido
        /// </summary>
        /// <param name="order">Pedido que se va a recalcular</param>
        /// <returns>Pedido recalculado</returns>
        public static VMOrdenDeCompra UpdateAccount(this VMOrdenDeCompra order)
        {
            try
            {
                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                order.Subtotal = order.Detalles.Sum(d => Math.Round(d.Cantidad * d.CostoUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                order.Detalles.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                order.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    order.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, order.Detalles.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = order.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe) - order.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe);
                order.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //Calculo el total de la compra
                order.Total = Math.Round(order.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool HasChanged(this List<VMDetalleDeOrdenDeCompra> detail, List<DetallesDeOrdenDeCompra> orderDetail)
        {
            foreach (var d in orderDetail)
            {
                var detailItem = detail.FirstOrDefault(x => x.idArticulo == d.idArticulo && x.CostoUnitario == d.costoUnitario);

                if (!detailItem.isValid())
                {
                    //Se elimino el articulo del detalle
                    return true;
                }
            }

            foreach (var d in detail)
            {
                var item = orderDetail.FirstOrDefault(x => x.idArticulo == d.idArticulo && x.costoUnitario == d.CostoUnitario);
                //Se buscan detalles nuevos

                if (!item.isValid())
                {
                    //Se agrego el articulo al detalle
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Traspasos
        public static bool HasChanged(this ICollection<DetallesDeTraspaso> detail, ICollection<DetallesDeTraspaso> transferDetail)
        {
            foreach (var d in transferDetail)
            {
                var detailItem = detail.FirstOrDefault(x => x.idArticulo == d.idArticulo);

                if (!detailItem.isValid())
                {
                    //Se elimino el articulo del detalle
                    return true;
                }
            }

            foreach (var d in detail)
            {
                var item = transferDetail.FirstOrDefault(x => x.idArticulo == d.idArticulo);
                //Se buscan detalles nuevos

                if (!item.isValid())
                {
                    //Se agrego el articulo al detalle
                    return true;
                }
            }

            return false;
        }

        public static VMTraspaso UpdateAccount(this VMTraspaso transfer)
        {
            try
            {
                //Debo recalcular el total

                //Calculo el total de la compra
                transfer.Total = transfer.Detalle.Sum(x => Math.Round(x.cantidadEnviada * x.costoUnitario, 2, MidpointRounding.AwayFromZero));

                return transfer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMArticulosConPedimento> GetOnlyImported(this ICollection<DetallesDeTraspaso> details)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                details.ToList().Where(d => d.Articulo.importado).ToList().ForEach(d => imported.Add(new VMArticulosConPedimento() { Articulo = d.Articulo, Vendidos = d.cantidadEnviada, Pedimentos = new List<VMPedimentoAsociado>() }));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMArticulosConPedimento> ReadjustImported(this ICollection<DetallesDeTraspaso> details)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                details.ToList().Where(d => d.Articulo.importado).ToList().ForEach(d => imported.Add(new VMArticulosConPedimento() { Articulo = d.Articulo, Vendidos = d.cantidadAceptada.GetValueOrDefault(0m), Pedimentos = d.PedimentoPorDetalleDeTraspasoes.Select(p => new VMPedimentoAsociado(){Articulo = d.Articulo, Cantidad = p.cantidad, IdPedimento = p.idPedimento, NumeroDePedimento = p.Pedimento.ToPedimentText().Replace(" ","")}).ToList() }));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Pagos

        public static VMPagoMultiple UpdateAccount(this VMPagoMultiple payment)
        {
            try
            {
                payment.TotalAbonadoPesos = payment.FacturasConSaldo.Where(a => a.Abono.isValid() && a.Abono.idMoneda == (int)Monedas.Pesos).Sum(s => Math.Round(s.Abono.monto, 2, MidpointRounding.AwayFromZero));
                payment.TotalAbonadoDolares = payment.FacturasConSaldo.Where(a => a.Abono.isValid() && a.Abono.idMoneda == (int)Monedas.Dólares).Sum(s => Math.Round(s.Abono.monto, 2, MidpointRounding.AwayFromZero));

                return payment;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region CFDI
        /// <summary>
        /// Valida que el documento xml sea un CFDI
        /// </summary>
        /// <param name="doc">Documento a validar</param>
        /// <returns>True si el archivo pertenece a un cfdi</returns>
        public static bool IsCFDI(this XmlDocument doc)
        {
            //Verifica que contenga el nodo Comprobante
            if (!doc.DocumentElement.isValid() || doc.DocumentElement.Name != "cfdi:Comprobante")
            {
                return false;
            }

            //Verifica que el TipoDeComprobante sea I
            var tipoComprobante = doc.DocumentElement.Attributes["TipoDeComprobante"];
            if (!tipoComprobante.isValid() || tipoComprobante.Value != "I")
            {
                return false;
            }

            //Verifica que contenga emisor
            var emisor = doc.DocumentElement["cfdi:Emisor"];
            if (!emisor.isValid() || !emisor.HasAttributes)
            {
                return false;
            }

            //Verifica que contenga receptor
            var receptor = doc.DocumentElement["cfdi:Receptor"];
            if (!receptor.isValid() || !receptor.HasAttributes)
            {
                return false;
            }

            //Verifica que contenga conceptos
            var conceptos = doc.DocumentElement["cfdi:Conceptos"];
            if (!conceptos.isValid() || !conceptos["cfdi:Concepto"].isValid())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene el rfc del emisor del CFDI
        /// </summary>
        /// <param name="doc">CFDI a buscar</param>
        /// <returns>Rfc del emisor</returns>
        public static string GetIssuerRfcFromCFDI(this XmlDocument doc)
        {
            string rfc = doc.DocumentElement["cfdi:Emisor"].Attributes["Rfc"].Value;

            return rfc;
        }
        #endregion

        #region Notas de credito
        /// <summary>
        /// Convierte una colección de NotaDeCredito su ViewModel VMNotaDeCredito
        /// </summary>
        /// <param name="creditNotes">Colección de Notas de credito</param>
        /// <returns>Colección de VMNotasDeCredito</returns>
        public static List<VMNotaDeCredito> ToViewModelList(this ICollection<NotasDeCredito> creditNotes)
        {
            try
            {
                List<VMNotaDeCredito> vmNotasDeCredito;
                vmNotasDeCredito = new List<VMNotaDeCredito>();

                creditNotes.ToList().ForEach(i => vmNotasDeCredito.Add(new VMNotaDeCredito(i)));

                return vmNotasDeCredito;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMDetalleDeNotaDeCredito> ToViewModelList(this ICollection<DetallesDeNotaDeCredito> detail)
        {
            return detail.Select(x => new VMDetalleDeNotaDeCredito(x)).ToList();
        }

        /// <summary>
        /// Actualiza el subtotal, impuestos, total de una nota de credito
        /// </summary>
        /// <param name="creditNote">Nota de credito a la que se va a recalcular</param>
        /// <returns>Nota de credito recalculada</returns>
        public static VMNotaDeCredito UpdateAccount(this VMNotaDeCredito creditNote)
        {
            try
            {
                //Si NO es una nota de crédito por devolución, entonces no hay nada que calcular
                if (!creditNote.PorDevolucion)
                {
                    creditNote.Subtotal = creditNote.importe;
                    creditNote.Impuestos = new List<VMImpuesto>();
                    creditNote.Total = creditNote.importe;
                    return creditNote;
                }

                //Debo recalcular el subtotal, los impuestos y el total
                decimal taxesTotal;
                List<Impuesto> taxes;

                //Subtotal = SUMA ((cantidad * costoUnitario) por cada detalle)
                creditNote.Subtotal = creditNote.DetalleDeNotaDeCredito.Sum(d => Math.Round(d.cantidad * d.precioUnitario, 2, MidpointRounding.AwayFromZero));

                //Impuestos
                //1.- Obtengo los impuestos que se manejan dentro de los artículos
                taxes = new List<Impuesto>();
                //1.1.- Agrego todos los impuestos
                creditNote.DetalleDeNotaDeCredito.ToList().ForEach(d => taxes.AddRange(d.Impuestos));

                //2.- Agrego a la lista de impuestos de la cuenta, impuesto por impuesto calculando su base gravable
                creditNote.Impuestos = new List<VMImpuesto>();
                foreach (var tax in taxes.Distinct<Impuesto>())
                {
                    creditNote.Impuestos.Add(new VMImpuesto(tax, Operations.CalculateTaxBaseAmount(tax, creditNote.DetalleDeNotaDeCredito.ToList())));
                }

                //3.- Inserto un dummy con el total de impuestos
                taxesTotal = creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado)).Sum(t => t.Importe.ToRoundedCurrency(creditNote.Moneda)) - creditNote.Impuestos.Where(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido)).Sum(t => t.Importe.ToRoundedCurrency(creditNote.Moneda));
                creditNote.Impuestos.Insert(0, new VMImpuesto(taxesTotal));

                //4.- Calculo el total de la nota de credito
                creditNote.Total = Math.Round(creditNote.Subtotal, 2, MidpointRounding.AwayFromZero) + Math.Round(taxesTotal, 2, MidpointRounding.AwayFromZero);

                return creditNote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convierte una lista de viewmodels de notas de credito a su tipo base
        /// </summary>
        /// <param name="creditNotes">Notas de credito a convertir</param>
        /// <returns></returns>
        public static List<NotasDeCredito> ToNotasDeCredito(this List<VMNotaDeCredito> creditNotes)
        {
            try
            {
                return creditNotes.Select(x => x.ToCreditNote()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<DetallesDeNotaDeCredito> ToDetalleDeNotaDeCredito(this ICollection<VMDetalleDeNotaDeCredito> detail)
        {
            try
            {
                return detail.Select(x => x.ToDetalleNotaDeCredito()).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<VMArticulosConPedimento> GetOnlyImported(this ICollection<VMDetalleDeNotaDeCredito> details, Factura invoice)
        {
            try
            {
                var imported = new List<VMArticulosConPedimento>();

                
                details.ToList()
                    .Where(d => d.Articulo.importado)
                    .ToList()
                    .ForEach(d => imported.Add(new VMArticulosConPedimento() {
                        Articulo = d.Articulo,
                        Vendidos = d.cantidad,
                        Pedimentos = invoice.DetallesDeFacturas.FirstOrDefault(x=>x.idArticulo == d.idArticulo).PedimentoPorDetalleDeFacturas.Select(p => new VMPedimentoAsociado() { Articulo = d.Articulo, Cantidad = p.cantidad, IdPedimento = p.idPedimento, NumeroDePedimento = p.Pedimento.ToPedimentText().Replace(" ", "") }).ToList() }));

                return imported;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica si una nota de credito es a venta futura
        /// </summary>
        /// <param name="creditNote">Nota de credito a verificar</param>
        /// <param name="invoice">Factura con la que se compra la nota de credito</param>
        /// <returns></returns>
        public static bool IsPreSaleCreditNote(this NotasDeCredito creditNote, VMFactura invoice)
        {
            try
            {
                return creditNote.fechaHora < invoice.fechaHora;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Actualiza la descripcion de una nota de crédito
        /// </summary>
        /// <param name="creditNote">nota de crédito a la que se le generara la descripcion</param>
        public static void UpdateDescription(this VMNotaDeCredito creditNote)
        {
            //Se limpia la descripcion anterior
            creditNote.descripcion = "";

            if (!creditNote.PorDevolucion)
            {
                //Si es por descuento, solo se genera la descripcion del descuento
                creditNote.descripcion = string.Format("Descuento de {0}", creditNote.importe.ToDecimalString());

                return;
            }

            //Genero la descripción
            if (!creditNote.DetalleDeNotaDeCredito.IsEmpty())
            {
                creditNote.descripcion = "Devolución de ";
                creditNote.descripcion += string.Join(",", creditNote.DetalleDeNotaDeCredito.Select(x => string.Format("{0} {1}", x.cantidad.ToDecimalString(), x.Articulo.descripcion)));
            }
        }
        #endregion

        #region Api

        /// <summary>
        /// Agrega el prefijo apropiado para realizar una llamada a la API
        /// </summary>
        /// <param name="queryUrl">Query string</param>
        /// <returns>Full url api request</returns>
        public static string ToApiUrl(this string queryUrl)
        {
            try
            {
                return string.Format("https://www.api.aprovi.com.mx/v3/{0}", queryUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
