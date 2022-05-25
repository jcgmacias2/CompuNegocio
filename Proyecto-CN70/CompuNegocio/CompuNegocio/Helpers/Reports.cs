﻿using Aprovi.Application.ViewModels;
using Aprovi.Business.Helpers;
using Aprovi.Business.ViewModels;
using Aprovi.Data.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace Aprovi.Application.Helpers
{
    public static class Reports
    {
        private static Conversor _convert;

        static Reports()
        {
            //General Params
            Generals = new List<ReportParameter>();
            Generals.Add(new ReportParameter("RazonSocial", Session.Configuration.razonSocial));
            Generals.Add(new ReportParameter("Rfc", Session.Configuration.rfc));
            Generals.Add(new ReportParameter("DomicilioR1",
                string.Format("{0}, #{1} {2}", Session.Configuration.Domicilio.calle,
                    Session.Configuration.Domicilio.numeroExterior, Session.Configuration.Domicilio.numeroInterior)));
            Generals.Add(new ReportParameter("DomicilioR2",
                string.Format("{0}, C.P. {1}", Session.Configuration.Domicilio.colonia,
                    Session.Configuration.Domicilio.codigoPostal)));
            Generals.Add(new ReportParameter("DomicilioR3",
                string.Format("{0}, {1}", Session.Configuration.Domicilio.ciudad,
                    Session.Configuration.Domicilio.estado)));
            var regimen = string.Empty;
            Session.Configuration.Regimenes.ToList().ForEach(r => regimen += " " + r.descripcion);
            Generals.Add(new ReportParameter("Regimen", regimen.Trim()));

            //Folder
            Folder = ConfigurationManager.AppSettings["Reports"].ToString();
            //Logo
            Logo = ConfigurationManager.AppSettings["Logo"].ToString();

            //Instancia de Conver
            _convert = new Conversor();
            _convert.ApocoparUnoParteEntera = true;
        }

        public static string Folder { get; set; }
        public static string Logo { get; set; }

        public static string TicketsPrinter
        {
            get
            {
                return Session.Station.ImpresorasPorEstacions
                           .FirstOrDefault(i => i.idTipoDeImpresora.Equals((int) TipoDeImpresora.Recibos)).impresora ??
                       string.Empty;
            }
        }

        public static string ReportsPrinter
        {
            get
            {
                return !Session.Configuration.Mode.Equals(Ambiente.Configuration)
                    ? Session.Station.ImpresorasPorEstacions
                          .FirstOrDefault(i => i.idTipoDeImpresora.Equals((int) TipoDeImpresora.Reportes)).impresora ??
                      string.Empty
                    : null;
            }
        }

        public static List<ReportParameter> Generals { get; set; }

        /// <summary>
        /// Llena el reporte de estado de cuenta de cliente
        /// </summary>
        /// <param name="balances">Lista de las cuentas y movimientos</param>
        /// <param name="start">Inicio del período que se reporte</param>
        /// <param name="end">Fin del período que se reporta</param>
        /// <param name="client">Cliente del cual pertenece el estado de cuenta</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMAbonoCuentaCliente> balances, DateTime start, DateTime end,
            Cliente client)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Debe tomar en cuenta que por cada abono de un documento se repite el documento mismo
                var totales = balances.DistinctBy(i => i.Folio);
                var totalPesos = totales.Where(x => x.IdMonedaDocumento == (int) Monedas.Pesos).Sum(x => x.Total);
                var totalDolares = totales.Where(x => x.IdMonedaDocumento == (int) Monedas.Dólares).Sum(x => x.Total);
                var abonosPesos = totales.Where(x => x.IdMonedaDocumento == (int) Monedas.Pesos).Sum(x => x.Abonado);
                var abonosDolares = totales.Where(x => x.IdMonedaDocumento == (int) Monedas.Dólares)
                    .Sum(x => x.Abonado);
                var saldoPesos = totalPesos - abonosPesos;
                var saldoDolares = totalDolares - abonosDolares;

                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Estado de cuenta del cliente {0} del {1} al {2}", client.nombreComercial,
                        start.ToShortDateString(), end.ToShortDateString())));
                source.Add(new ReportParameter("TotalPesos", totalPesos.ToCurrencyString()));
                source.Add(new ReportParameter("TotalDolares", totalDolares.ToCurrencyString()));
                source.Add(new ReportParameter("AbonosPesos", abonosPesos.ToCurrencyString()));
                source.Add(new ReportParameter("AbonosDolares", abonosDolares.ToCurrencyString()));
                source.Add(new ReportParameter("SaldoPesos", saldoPesos.ToCurrencyString()));
                source.Add(new ReportParameter("SaldoDolares", saldoDolares.ToCurrencyString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Cuentas", balances));

                report = new VMReporte("EstadoDeCuentaCliente", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de antiguedad de saldos
        /// </summary>
        /// <param name="collectableBalances">Lista de saldos de cliente</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRTotalAntiguedadSaldos> collectableBalances, DateTime to)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Antigüedad de saldos por cliente al {0}", to.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Saldos", collectableBalances));

                report = new VMReporte("SaldosPorCobrar", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de avaluos
        /// </summary>
        /// <param name="report">Datos para mostrar en el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRAvaluo detail, Empresa company)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Avalúo al {0}", detail.Fecha.ToShortDateString())));
                source.Add(new ReportParameter("Empresa", company.descripcion));
                source.Add(new ReportParameter("FechaImpresion", DateTime.Today.ToShortDateString()));
                source.Add(new ReportParameter("Total", detail.Total.ToDecimalString()));
                source.Add(new ReportParameter("TotalPorcentaje", detail.PorcentajeTotal.ToDecimalString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detail.Detalle));

                report = new VMReporte("Avaluo", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de antiguedad de saldos detallado
        /// </summary>
        /// <param name="collectableBalances">Lista de saldos de clientes</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRDetalleAntiguedadSaldos> collectableBalances, int periodo,
            DateTime to)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Antigüedad de saldos por cliente al {0}", to.ToShortDateString())));
                source.Add(new ReportParameter("Periodo", periodo.ToString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();


                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", collectableBalances));

                report = new VMReporte("AntiguedadSaldosDetallado", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMReporte FillReport(List<VMDetalleKardex> kardex, DateTime start, DateTime end, Articulo item)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Kardex de {0} del {1} al {2}", item.descripcion, start.ToShortDateString(),
                        end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Kardex", kardex));

                report = new VMReporte("Kardex", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMReporte FillReport(List<VMFlujoPorArticulo> stockFlow, DateTime start, DateTime end)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Flujo de inventario del {0} al {1}", start.ToShortDateString(),
                        end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Flujo", stockFlow));

                report = new VMReporte("FlujoDeInventario", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMReporte FillReport(List<VMRDetalleCostoDeLoVendido> detail, DateTime start, DateTime end)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo", "Costo de lo vendido"));
                source.Add(new ReportParameter("FechaInicio", start.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("FechaFin", end.ToString("dd/MM/yyyy")));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detail));

                report = new VMReporte("CostoDeLoVendido", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Proporciona el reporte de lista de precios
        /// </summary>
        /// <param name="data">datos del reporte</param>
        /// <returns></returns>
        public static VMReporte FillReport(VMRListaDePrecios vm)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                
                //Header

                source.Add(new ReportParameter("Titulo", string.Format("Lista de precios({0}) de artículos en {1}", vm.ReportType.ToString().Replace("_"," "),vm.Moneda.descripcion)));

                source.Add(new ReportParameter("IncluyeImpuestos", vm.IncluirImpuestos.ToString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                //data.Add(new ReportDataSource("Detalle", vm.Detalle));

                if (vm.ReportType.Equals(ReportesListaDePrecios.Todos_Los_Precios))
                {
                    report = new VMReporte("ListaDePreciosMultiple", Reports.ReportsPrinter, data, source);
                }
                else
                {
                    report = new VMReporte("ListaDePreciosSimple", Reports.ReportsPrinter, data, source);
                }

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de estado de cuenta de proveedor
        /// </summary>
        /// <param name="balances">Lista de las cuentas y movimientos</param>
        /// <param name="start">Inicio del período que se reporte</param>
        /// <param name="end">Fin del período que se reporta</param>
        /// <param name="supplier">Proveedor del cual pertenece el estado de cuenta</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMAbonoCuentaProveedor> balances, DateTime start, DateTime end,
            Proveedore supplier)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Estado de cuenta con proveedor {0} del {1} al {2}", supplier.nombreComercial,
                        start.ToShortDateString(), end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Cuentas", balances));

                report = new VMReporte("EstadoDeCuentaProveedor", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de antiguedad de saldos
        /// </summary>
        /// <param name="payableBalances">Lista de saldos a proveedores</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VwSaldosPorProveedorPorMoneda> payableBalances)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Antigüedad de saldos por proveedor al {0}", DateTime.Now.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Saldos", payableBalances));

                report = new VMReporte("SaldosPorPagar", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de traspasos por periodo
        /// </summary>
        /// <param name="transfers">Traspasos con los que se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRTraspaso> transfers, DateTime startDate, DateTime endDate)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicio", startDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("FechaFin", endDate.ToString("dd/MM/yyyy")));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales del traspaso
                var generales = new List<VMRTraspaso>();
                generales.AddRange(transfers);
                //Los detalles
                var detalles = transfers.SelectMany(x => x.DetallesDeTraspasoes)
                    .Select(x => new VMRDetalleDeTraspaso(x)).ToList();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalles", detalles));

                //Subreporte de Header y Footer
                report = new VMReporte("TraspasosPorPeriodo", Reports.ReportsPrinter, data, source, (obj, args) =>
                {
                    int folio = args.Parameters["folio"].Values.FirstOrDefault().ToIntOrDefault();

                    args.DataSources.Clear();
                    args.DataSources.Add(new ReportDataSource("Detalle", detalles.Where(x => x.folio == folio)));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de traspasos por periodo
        /// </summary>
        /// <param name="transfers">Traspasos con los que se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRDetalleVentasPorArticulo> transactions, DateTime startDate,
            DateTime endDate, TiposDeReporteVentasPorArticulo reportType, bool withPercentages)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicio", startDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("FechaFin", endDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("ReportFilters", " "));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los detalles
                //var detalles = transfers.SelectMany(x => x.DetallesDeTraspasoes).Select(x => new VMRDetalleDeTraspaso(x, stock.FirstOrDefault(y => y.idArticulo == x.idArticulo).existencia.GetValueOrDefault(0m))).ToList();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", transactions));

                //Subreporte de Header y Footer
                switch (reportType)
                {
                    case TiposDeReporteVentasPorArticulo.Totalizado:
                        if (withPercentages)
                        {
                            report = new VMReporte("VentasPorArticuloTotalizadoConPorcentajes", Reports.ReportsPrinter, data, source);
                        }
                        else
                        {
                            report = new VMReporte("VentasPorArticuloTotalizado", Reports.ReportsPrinter, data, source);
                        }
                        break;
                    case TiposDeReporteVentasPorArticulo.Detallado:
                        report = new VMReporte("VentasPorArticuloDetallado", Reports.ReportsPrinter, data, source);
                        break;
                    case TiposDeReporteVentasPorArticulo.Detallado_Con_Datos_Del_Cliente:
                        report = new VMReporte("VentasPorArticuloDetallado", Reports.ReportsPrinter, data, source);
                        break;
                }

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de factura para una factura electrónica
        /// </summary>
        /// <param name="invoice">Factura con la cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRFactura invoice)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal = "00/100";
                _convert.SeparadorDecimalSalida = invoice.DescripcionMoneda;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(invoice.Total))));

                //Todos los impuestos por separado
                //IVA Trasladado
                source.Add(new ReportParameter("IVATrasladado",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Trasladado) &&
                                    i.codigo.Equals(((int) Impuestos.IVA).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));
                //IVA Retenido
                source.Add(new ReportParameter("IVARetenido",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Retenido) &&
                                    i.codigo.Equals(((int) Impuestos.IVA).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));
                //IEPS Trasladado
                source.Add(new ReportParameter("IEPSTrasladado",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Trasladado) &&
                                    i.codigo.Equals(((int) Impuestos.IEPS).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));
                //IEPS Retenido
                source.Add(new ReportParameter("IEPSRetenido",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Retenido) &&
                                    i.codigo.Equals(((int) Impuestos.IEPS).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));
                //ISR Trasladado
                source.Add(new ReportParameter("ISRTrasladado",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Trasladado) &&
                                    i.codigo.Equals(((int) Impuestos.ISR).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));
                //ISR Retenido
                source.Add(new ReportParameter("ISRRetenido",
                    invoice.Impuestos
                        .Where(i => i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Retenido) &&
                                    i.codigo.Equals(((int) Impuestos.ISR).ToString("000"))).Sum(im => im.Importe)
                        .ToCurrencyString()));

                //Codigo de barras bidimensional
                source.Add(new ReportParameter("cbb",
                    new Uri(string.Format("{0}\\{1}.bmp", Session.Configuration.CarpetaCbb, invoice.FolioFactura))
                        .AbsoluteUri));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var generales = new List<VMRFactura>();
                generales.Add(invoice);
                //Los detalles
                var detalles = new List<VMDetalle>();

                invoice.DetalleDeFactura.ToList().ForEach(d => detalles.Add(new VMDetalle(d)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalles", detalles));

                //Subreporte de Header y Footer
                report = new VMReporte("FacturaExtended", Reports.ReportsPrinter, data, source, (obj, args) =>
                {
                    args.DataSources.Clear();
                    args.DataSources.Add(new ReportDataSource("Generales", generales));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de nota de credito
        /// </summary>
        /// <param name="creditNote">Nota de credito con la cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRNotaDeCredito creditNote)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal = "00/100";
                _convert.SeparadorDecimalSalida = creditNote.DescripcionMoneda;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(creditNote.Total))));

                //Codigo de barras bidimensional
                source.Add(new ReportParameter("cbb",
                    new Uri(string.Format("{0}\\{1}.bmp", Session.Configuration.CarpetaCbb,
                        creditNote.FolioNotaDeCredito)).AbsoluteUri));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var generales = new List<VMRNotaDeCredito>();
                generales.Add(creditNote);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));

                //Subreporte de Header y Footer
                report = new VMReporte("NotaDeCreditoExtended", Reports.ReportsPrinter, data, source, (obj, args) =>
                {
                    args.DataSources.Clear();
                    args.DataSources.Add(new ReportDataSource("Generales", generales));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de nota de descuento
        /// </summary>
        /// <param name="discountNote">Nota de descuento con la cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRNotaDeDescuento discountNote)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal = "00/100";
                _convert.SeparadorDecimalSalida = discountNote.DescripcionMoneda;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(discountNote.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var generales = new List<VMRNotaDeDescuento>();
                generales.Add(discountNote);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));

                //Subreporte de Header y Footer
                report = new VMReporte("NotaDeDescuentoExtended", Reports.ReportsPrinter, data, source, (obj, args) =>
                {
                    args.DataSources.Clear();
                    args.DataSources.Add(new ReportDataSource("Generales", generales));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de notas de descuento
        /// </summary>
        /// <param name="vm">Notas de descuento con las cuales se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMReporteNotasDeDescuento vm)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo", "Reporte de notas de descuento"));
                source.Add(new ReportParameter("FechaInicio", vm.StartDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("FechaFin", vm.EndDate.ToString("dd/MM/yyyy")));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", vm.Detail));

                //Subreporte de Header y Footer
                report = new VMReporte("NotasDeDescuento", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte abonos de facturas
        /// </summary>
        /// <param name="payments">Abonos con los cuales se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRAbonosFacturas payment)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;
                var payments = payment.Detalle;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                decimal totalPesos = payments.Where(x => x.IdMoneda == (int) Monedas.Pesos).Sum(x => x.SaldoPagado);
                decimal totalDolares = payments.Where(x => x.IdMoneda == (int) Monedas.Dólares).Sum(x => x.SaldoPagado);

                _convert.MascaraSalidaDecimal = "00/100";
                _convert.SeparadorDecimalSalida = "Pesos"; //"Pesos"; 
                source.Add(new ReportParameter("ImporteConLetraPesos",
                    string.Format("{0}", _convert.ToCustomString(totalPesos))));
                source.Add(new ReportParameter("TotalPesos", totalPesos.ToDecimalString()));
                _convert.SeparadorDecimalSalida = "Dolares"; //"Pesos"; 
                source.Add(new ReportParameter("ImporteConLetraDolares",
                    string.Format("{0}", _convert.ToCustomString(totalDolares))));
                source.Add(new ReportParameter("TotalDolares", totalDolares.ToDecimalString()));

                //Codigo de barras bidimensional
                source.Add(new ReportParameter("cbb",
                    new Uri(string.Format("{0}\\{1}.bmp", Session.Configuration.CarpetaCbb, payment.FolioAbono))
                        .AbsoluteUri));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var generales = new List<VMRAbonosFacturas>();
                generales.Add(payment);

                var pago_impuestos = new List<VMImpuestoPorFactura>();
                payments.ForEach(d => pago_impuestos = d.pago_impuestos);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalle", payments));
                data.Add(new ReportDataSource("pago_impuestos", pago_impuestos));

                report = new VMReporte("PagoExtended", Reports.ReportsPrinter, data, source, (obj, args) =>
                {
                    args.DataSources.Clear();
                    args.DataSources.Add(new ReportDataSource("Generales", generales));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static VMReporte FillReport(VMPago payment)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var pago = new List<VMPago>();
                pago.Add(payment);
                //Los detalles
                var relacionados = new List<VMCFDIRelacionado>();
                payment.DocumentosRelacionados.ForEach(d => relacionados.Add(d));

                var pago_impuestos = new List<VMImpuestoPorFactura>();
                payment.pago_impuestos.ForEach(d => pago_impuestos.Add(d));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Pago", pago));
                data.Add(new ReportDataSource("Relacionados", relacionados));
                data.Add(new ReportDataSource("pago_impuestos", pago_impuestos));

                report = new VMReporte("ComprobanteDePago", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de compras por periodos
        /// </summary>
        /// <param name="purchases">Lista de compras con las cuales llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMCompra> purchases, DateTime start, DateTime end, Proveedore supplier)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                if (supplier.idProveedor.isValid())
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Compras del proveedor {0} del {1} al {2}", supplier.razonSocial,
                            start.ToShortDateString(), end.ToShortDateString())));
                else
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Compras del {0} al {1}", start.ToShortDateString(), end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la compra
                var compras = new List<VMReporteCompra>();
                purchases.ForEach(c => compras.Add(new VMReporteCompra(c)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Compras", compras));

                report = new VMReporte("ComprasPorProveedor", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de compras por periodos detalladas
        /// </summary>
        /// <param name="purchases">Lista de compras con las cuales llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMCompra> purchases, List<VMRDetalleCompra> detalles, DateTime start,
            DateTime end, Proveedore supplier)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                if (supplier.idProveedor.isValid())
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Compras del proveedor {0} del {1} al {2}", supplier.razonSocial,
                            start.ToShortDateString(), end.ToShortDateString())));
                else
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Compras del {0} al {1}", start.ToShortDateString(), end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la compra
                var compras = new List<VMReporteCompra>();
                purchases.ForEach(c => compras.Add(new VMReporteCompra(c)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Compras", compras));

                report = new VMReporte("ComprasPorProveedorDetalladas", Reports.ReportsPrinter, data, source,
                    (obj, args) =>
                    {
                        int idProveedor = args.Parameters["idProveedor"].Values[0].ToIntOrDefault();
                        string folio = args.Parameters["folio"].Values[0];

                        args.DataSources.Clear();

                        //Must find the user detail

                        List<VMRDetalleCompra> vmDetalle =
                            detalles.Where(x => x.IdProveedor == idProveedor && x.Folio == folio).ToList();

                        args.DataSources.Add(new ReportDataSource("Detalle", vmDetalle));
                    });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Compra
        /// </summary>
        /// <param name="purchase">Compra con la cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMCompra purchase)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Impuestos
                var ivaTrasladado = purchase.Impuestos.FirstOrDefault(i =>
                    i.idTipoDeImpuesto.Equals((int) TipoDeImpuesto.Trasladado) &&
                    i.nombre.Equals(Impuestos.IVA.ToString()));
                source.Add(new ReportParameter("IVATrasladado",
                    ivaTrasladado.isValid() ? ivaTrasladado.Importe.ToDecimalString() : "0.0"));
                //var ivaRetenido = purchase.Impuestos.FirstOrDefault(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido) && i.nombre.Equals(Impuestos.IVA.ToString()));
                //source.Add(new ReportParameter("IVARetenido", ivaRetenido.isValid() ? ivaRetenido.Importe.ToDecimalString() : "0.0"));
                //var iepsTrasladado = purchase.Impuestos.FirstOrDefault(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado) && i.nombre.Equals(Impuestos.IEPS.ToString()));
                //source.Add(new ReportParameter("IEPSTrasladado", iepsTrasladado.isValid() ? iepsTrasladado.Importe.ToDecimalString() : "0.0"));
                //var iepsRetenido = purchase.Impuestos.FirstOrDefault(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido) && i.nombre.Equals(Impuestos.IEPS.ToString()));
                //source.Add(new ReportParameter("IEPSRetenido", iepsRetenido.isValid() ? iepsRetenido.Importe.ToDecimalString() : "0.0"));
                //var isrTrasladado = purchase.Impuestos.FirstOrDefault(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Trasladado) && i.nombre.Equals(Impuestos.ISR.ToString()));
                //source.Add(new ReportParameter("ISRTrasladado", isrTrasladado.isValid() ? isrTrasladado.Importe.ToDecimalString() : "0.0"));
                //var isrRetenido = purchase.Impuestos.FirstOrDefault(i => i.idTipoDeImpuesto.Equals((int)TipoDeImpuesto.Retenido) && i.nombre.Equals(Impuestos.ISR.ToString()));
                //source.Add(new ReportParameter("ISRRetenido", isrRetenido.isValid() ? isrRetenido.Importe.ToDecimalString() : "0.0"));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la compra
                var compras = new List<VMCompra>();
                compras.Add(purchase);
                //La moneda
                var monedas = new List<Moneda>();
                monedas.Add(purchase.Moneda);
                //El proveedor
                var proveedores = new List<Proveedore>();
                proveedores.Add(purchase.Proveedore);
                //Los detalles
                var detalles = new List<VMDetalleDeCompra>();
                purchase.DetallesDeCompras.ToList().ForEach(d => detalles.Add(new VMDetalleDeCompra(d)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Compra", compras));
                data.Add(new ReportDataSource("Moneda", monedas));
                data.Add(new ReportDataSource("Proveedor", proveedores));
                data.Add(new ReportDataSource("Detalles", detalles));

                report = new VMReporte("Compra", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Acuse de cancelación para un comprobante cancelado
        /// </summary>
        /// <param name="receipt">Acuse con el cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMAcuse receipt)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la factura
                var acuse = new List<VMAcuse>();
                acuse.Add(receipt);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Acuse", acuse));

                report = new VMReporte("Acuse", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Remisión
        /// </summary>
        /// <param name="billOfSale">Remisión con la cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRRemision billOfSale)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal =
                    "00/100"; //billOfSale.idMoneda.Equals((int)Monedas.Pesos) ? "00/100 MXN" : "00/100 USD";
                _convert.SeparadorDecimalSalida = billOfSale.Moneda.descripcion;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(billOfSale.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la remisión

                var generales = new List<VMRRemision>();
                generales.Add(billOfSale);

                //Los detalles
                var detalles = new List<VMDetalle>();
                billOfSale.DetalleDeRemision.ToList().ForEach(d => detalles.Add(new VMDetalle(d)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalles", detalles));

                report = new VMReporte(billOfSale.Archivo, Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Pedido
        /// </summary>
        /// <param name="order">Pedido con el cual se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRPedido order)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal =
                    order.idMoneda.Equals((int) Monedas.Pesos) ? "00/100 MXN" : "00/100 USD";
                _convert.SeparadorDecimalSalida = order.Moneda.descripcion;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(order.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la remisión

                var generales = new List<VMRPedido>();
                generales.Add(order);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalles", order.Detalles.ToList()));

                report = new VMReporte("Pedido", Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Traspaso
        /// </summary>
        /// <param name="order">Pedido con el cual se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRTraspaso transfer)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal = "00/100 MXN";
                _convert.SeparadorDecimalSalida = "Pesos";
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(transfer.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();
                var items = new List<VMRDetalleDeTraspaso>();

                foreach (var d in transfer.DetallesDeTraspasoes.ToList())
                {
                    if (d.PedimentoPorDetalleDeTraspasoes.Count > 0)
                    {
                        //Se agrega un detalle por cada pedimento
                        foreach (var p in d.PedimentoPorDetalleDeTraspasoes.ToList())
                        {
                            items.Add(new VMRDetalleDeTraspaso(p));
                        }
                    }
                    else
                    {
                        items.Add(new VMRDetalleDeTraspaso(d));
                    }
                }

                //Los generales de la remisión

                var generales = new List<VMRTraspaso>();
                generales.Add(transfer);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("General", generales));
                data.Add(new ReportDataSource("Detalle", items));

                report = new VMReporte("Traspaso", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Ventas por periodo
        /// </summary>
        /// <param name="sales">Ventas con las cuales se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRVentasPorPeriodo sales)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicio", sales.FechaInicio.ToString()));
                source.Add(new ReportParameter("FechaFin", sales.FechaFin.ToString()));
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToString()));

                //totales
                var detalle = sales.Detalle;
                detalle.OrderByDescending(d => d.Folio);

                decimal totalVentasPesos = detalle.Where(x => !x.FechaCancelacion.isValid() && x.Moneda == "P")
                    .Sum(x => x.Total);
                decimal totalVentasDolares = detalle.Where(x => !x.FechaCancelacion.isValid() && x.Moneda == "D")
                    .Sum(x => x.Total);
                decimal totalAbonosPesos = detalle.Where(x => !x.FechaCancelacion.isValid() && x.Moneda == "P")
                    .Sum(x => x.Abonos);
                decimal totalAbonosDolares = detalle.Where(x => !x.FechaCancelacion.isValid() && x.Moneda == "D")
                    .Sum(x => x.Abonos);

                source.Add(new ReportParameter("TotalVentasPesos", totalVentasPesos.ToCurrencyString()));
                source.Add(new ReportParameter("TotalVentasDolares", totalVentasDolares.ToCurrencyString()));
                source.Add(new ReportParameter("TotalAbonosDolares", totalAbonosDolares.ToCurrencyString()));
                source.Add(new ReportParameter("TotalAbonosPesos", totalAbonosPesos.ToCurrencyString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", sales.Detalle));

                report = new VMReporte("ReporteVentasPorPeriodo", Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Impuestos por periodo
        /// </summary>
        /// <param name="taxes">Impuestos con los cuales se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMRImpuestosPorPeriodo taxes)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicio", taxes.StartDate.ToString()));
                source.Add(new ReportParameter("FechaFin", taxes.EndDate.ToString()));
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToString()));
                source.Add(new ReportParameter("Titulo", "Reporte de Impuestos por período"));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("DetalleIngresos", taxes.DetalleIngresos));
                data.Add(new ReportDataSource("DetalleEgresos", taxes.DetalleEgresos));

                report = new VMReporte("ImpuestosPorPeriodo", Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Comisiones por periodo
        /// </summary>
        /// <param name="commissions">Comisiones con las cuales se llenara el reporte</param>
        /// <param name="startDate">Fecha de inicio del periodo</param>
        /// <param name="endDate">Fecha de fin del periodo</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRComisiones> comissions, List<VMRDetalleComision> transactions,
            DateTime startDate, DateTime endDate)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicio", startDate.ToShortDateString()));
                source.Add(new ReportParameter("FechaFin", endDate.ToShortDateString()));
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToShortDateString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detail", comissions));

                report = new VMReporte("ReporteComisiones", Reports.TicketsPrinter, data, source, (obj, args) =>
                {
                    string nombreUsuario = args.Parameters["NombreUsuario"].Values[0];

                    args.DataSources.Clear();

                    //Must find the user detail

                    List<VMRDetalleComision> vmDetalle =
                        transactions.Where(x => x.NombreUsuario == nombreUsuario).ToList();

                    args.DataSources.Add(new ReportDataSource("Detalle", vmDetalle));
                });

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de abonos
        /// </summary>
        /// <param name="payments">Lista de abonos con la cual llenar el reporte</param>
        /// <param name="start">Fecha de inicio del período a reportar</param>
        /// <param name="end">Fecha de fin del período a reportar</param>
        /// <param name="business">Empresa de la cual se reportan los abonos</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMAbono> payments, DateTime start, DateTime end, Empresa business)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("FechaInicial", start.ToShortDateString()));
                source.Add(new ReportParameter("FechaFinal", end.ToShortDateString()));
                source.Add(new ReportParameter("Empresa", business.descripcion));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Generales del emisor
                var generales = new List<Configuracion>();
                generales.Add(Session.Configuration);


                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Abonos", payments));

                report = new VMReporte("Abonos", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de inventario físico
        /// </summary>
        /// <param name="stock">Lista de inventario físico actual</param>
        /// <param name="withStockOnly">Indica si el reporte solo debe contener productos con stock</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMInventario> stock, List<Clasificacione> classifications,
            bool withStockOnly)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Se filtran los articulos si el filtro de solo en stock es true
                if (withStockOnly)
                {
                    stock = stock.Where(x => x.Existencia > 0).ToList();
                }

                //Total de artículos
                source.Add(new ReportParameter("ArticulosTotales", stock.Count.ToString()));
                source.Add(new ReportParameter("Clasificaciones",
                    string.Join(",", classifications.Select(x => x.descripcion).ToArray())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Inventario", stock));

                report = new VMReporte("Inventario", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Ajuste
        /// </summary>
        /// <param name="adjustment">Ajuste con el cual llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(Ajuste adjustment)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                source.Add(new ReportParameter("Folio", adjustment.folio));
                source.Add(new ReportParameter("Fecha", adjustment.fechaHora.ToShortDateString()));
                source.Add(new ReportParameter("Usuario", adjustment.Usuario.nombreDeUsuario));
                source.Add(new ReportParameter("Tipo", adjustment.TiposDeAjuste.descripcion));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                var details = new List<VMDetalleDeAjuste>();

                adjustment.DetallesDeAjustes.ToList().ForEach(d => details.Add(new VMDetalleDeAjuste(adjustment.folio,
                    d.Articulo.codigo, d.Articulo.descripcion, d.cantidad, adjustment.fechaHora,
                    adjustment.Usuario.nombreDeUsuario)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", details));

                report = new VMReporte("Ajuste", string.Empty, data, source);

                return report;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de ajustes
        /// </summary>
        /// <param name="adjustments">Lista de ajustea a reportear</param>
        /// <param name="subtitle">Subtitulo del reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<Ajuste> adjustments, string subtitle)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                source.Add(new ReportParameter("Subtitulo", subtitle));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                var details = new List<VMDetalleDeAjuste>();
                foreach (Ajuste a in adjustments)
                {
                    a.DetallesDeAjustes.ToList().ForEach(d => details.Add(new VMDetalleDeAjuste(a.folio,
                        d.Articulo.codigo, d.Articulo.descripcion, d.cantidad, a.fechaHora,
                        a.Usuario.nombreDeUsuario)));
                }

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Ajustes", details));

                report = new VMReporte("Ajustes", string.Empty, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de estado de cuenta de proveedor
        /// </summary>
        /// <param name="balances">Lista de las cuentas y movimientos</param>
        /// <param name="start">Inicio del período que se reporte</param>
        /// <param name="end">Fin del período que se reporta</param>
        /// <param name="supplier">Proveedor del cual pertenece el estado de cuenta</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRemision> detail, DateTime startDate, DateTime endDate,
            Tipos_Reporte_Remisiones filter)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                decimal facturadasDlls = detail.Where(x =>
                    x.idMoneda == (int) Monedas.Dólares &&
                    x.idEstatusDeRemision == (int) Tipos_Reporte_Remisiones.Solo_Facturadas).Sum(x => x.Total);
                decimal pendienteDlls = detail.Where(x =>
                    x.idMoneda == (int) Monedas.Dólares && x.idEstatusDeRemision ==
                    (int) Tipos_Reporte_Remisiones.Pendientes_de_Facturar).Sum(x => x.Total);
                decimal totalDlls = detail.Where(x => x.idMoneda == (int) Monedas.Dólares).Sum(x => x.Total);
                decimal facturadasPesos = detail.Where(x =>
                    x.idMoneda == (int) Monedas.Pesos &&
                    x.idEstatusDeRemision == (int) Tipos_Reporte_Remisiones.Solo_Facturadas).Sum(x => x.Total);
                decimal pendientesPesos = detail.Where(x =>
                    x.idMoneda == (int) Monedas.Pesos &&
                    x.idEstatusDeRemision == (int) Tipos_Reporte_Remisiones.Pendientes_de_Facturar).Sum(x => x.Total);
                decimal totalPesos = detail.Where(x => x.idMoneda == (int) Monedas.Pesos).Sum(x => x.Total);

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                source.Add(new ReportParameter("FechaInicio", startDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("FechaFin", endDate.ToString("dd/MM/yyyy")));
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToString("dd/MM/yyyy - hh:mm")));
                source.Add(new ReportParameter("Total", detail.Count.ToString()));
                source.Add(new ReportParameter("FacturadasDlls", facturadasDlls.ToDecimalString()));
                source.Add(new ReportParameter("FacturadasPesos", facturadasPesos.ToDecimalString()));
                source.Add(new ReportParameter("PendienteDlls", pendienteDlls.ToDecimalString()));
                source.Add(new ReportParameter("PendientesPesos", pendientesPesos.ToDecimalString()));
                source.Add(new ReportParameter("TotalDlls", totalDlls.ToDecimalString()));
                source.Add(new ReportParameter("TotalPesos", totalPesos.ToDecimalString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                var details = detail.Select(x => new VMReporteRemisiones(x)).ToList();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Remisiones", details));

                report = new VMReporte("RemisionesPorPeriodo", string.Empty, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de cotizacion
        /// </summary>
        /// <param name="quote">cotizacion a reportear</param>
        /// <returns></returns>
        public static VMReporte FillReport(VMRCotizacion quote)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();

                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                _convert.MascaraSalidaDecimal = quote.idMoneda.Equals((int) Monedas.Pesos) ? "00/100 MXN" : "00/100 USD";
                _convert.SeparadorDecimalSalida = quote.Moneda.descripcion;

                source.Add(new ReportParameter("ImporteConLetra", string.Format("{0}", _convert.ToCustomString(quote.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                var generales = new List<VMRCotizacion>();
                generales.Add(quote);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalle", quote.DetalleCotizacion));

                report = new VMReporte("Cotizacion", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de articulos pendientes de surtir
        /// </summary>
        /// <param name="detail">listado de articulos pendientes de surtir</param>
        /// <returns></returns>
        public static VMReporte FillReport(List<VMDetalleDePedido> detail)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToShortDateString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detail));

                report = new VMReporte("ReporteArticulosPendientesDeSurtir", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de pedidos por cliente
        /// </summary>
        /// <param name="order">Datos del cliente</param>
        /// <param name="detail">Articulos de los pedidos del cliente</param>
        /// <returns></returns>
        public static VMReporte FillReport(Cliente customer, List<VMRDetalleDePedido> detail)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Fecha", DateTime.Now.ToShortDateString()));
                source.Add(new ReportParameter("TotalRestantePesos",
                    detail.Where(x => x.Moneda == "P").Sum(x => (x.Pendiente * x.PrecioUnitario)).ToDecimalString()));
                source.Add(new ReportParameter("TotalRestanteDolares",
                    detail.Where(x => x.Moneda == "D").Sum(x => (x.Pendiente * x.PrecioUnitario)).ToDecimalString()));
                source.Add(new ReportParameter("TotalSurtidoPesos",
                    detail.Where(x => x.Moneda == "P").Sum(x => (x.surtidoEnOtros * x.PrecioUnitario))
                        .ToDecimalString()));
                source.Add(new ReportParameter("TotalSurtidoDolares",
                    detail.Where(x => x.Moneda == "D").Sum(x => (x.surtidoEnOtros * x.PrecioUnitario))
                        .ToDecimalString()));
                source.Add(new ReportParameter("TotalPedidoPesos",
                    detail.Where(x => x.Moneda == "P").Sum(x => (x.Cantidad * x.PrecioUnitario)).ToDecimalString()));
                source.Add(new ReportParameter("TotalPedidoDolares",
                    detail.Where(x => x.Moneda == "D").Sum(x => (x.Cantidad * x.PrecioUnitario)).ToDecimalString()));
                source.Add(new ReportParameter("Cliente", customer.razonSocial));

                Domicilio address = customer.Domicilio;
                source.Add(new ReportParameter("Domicilio",
                    string.Format("{0} {1}-{2} {3} C.P. {4}", address.calle, address.numeroExterior,
                        address.numeroInterior, address.colonia, address.codigoPostal)));
                source.Add(new ReportParameter("Ciudad", address.ciudad));
                source.Add(new ReportParameter("Contacto", customer.contacto));
                source.Add(new ReportParameter("Telefono", customer.telefono));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detail));

                report = new VMReporte("PedidosDeCliente", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Orden de compra
        /// </summary>
        /// <param name="order">Orden de compra con la cual se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMROrdenDeCompra order, bool withoutPrices)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Logo", new Uri(Reports.Logo).AbsoluteUri));

                //Importe con letra
                _convert.MascaraSalidaDecimal =
                    order.idMoneda.Equals((int) Monedas.Pesos) ? "00/100 MXN" : "00/100 USD";
                _convert.SeparadorDecimalSalida = order.Moneda.descripcion;
                source.Add(new ReportParameter("ImporteConLetra",
                    string.Format("{0}", _convert.ToCustomString(order.Total))));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la remisión

                var generales = new List<VMROrdenDeCompra>();
                generales.Add(order);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Generales", generales));
                data.Add(new ReportDataSource("Detalles",
                    order.Detalles.Select(x => new VMRDetalleDeOrdenDeCompra(x)).ToList()));

                report = withoutPrices
                    ? new VMReporte("OrdenDeCompraSinPrecios", Reports.TicketsPrinter, data, source)
                    : new VMReporte("OrdenDeCompra", Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de Cotizaciones por periodo
        /// </summary>
        /// <param name="order">Orden de compra con la cual se llenara el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMRDetalleDeCotizacion> detail, DateTime from, DateTime to)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                //Header
                source.Add(new ReportParameter("Titulo",
                    string.Format("Cotizaciones del {0} al {1}", from.ToString("dd/MM/yyyy"),
                        to.ToString("dd/MM/yyyy"))));
                source.Add(new ReportParameter("TotalPesos",
                    detail.Where(x => x.idMoneda == (int) Monedas.Pesos).Sum(x => x.Total).ToString()));
                source.Add(new ReportParameter("TotalDolares",
                    detail.Where(x => x.idMoneda == (int) Monedas.Dólares).Sum(x => x.Total).ToString()));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detail));

                report = new VMReporte("CotizacionesPorPeriodo", Reports.TicketsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de notas de credito por periodos
        /// </summary>
        /// <param name="creditNotes">Lista de notas de credito con las cuales llenar el reporte</param>
        /// <param name="start">Lista de notas de credito con las cuales llenar el reporte</param>
        /// <param name="end">Lista de notas de credito con las cuales llenar el reporte</param>
        /// <param name="customer">Lista de notas de credito con las cuales llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(List<VMNotaDeCredito> creditNotes, DateTime start, DateTime end,
            Cliente customer)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                if (customer.idCliente.isValid())
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Notas de crédito del cliente {0} del {1} al {2}", customer.razonSocial,
                            start.ToShortDateString(), end.ToShortDateString())));
                else
                    source.Add(new ReportParameter("Titulo",
                        string.Format("Notas de crédito del {0} al {1}", start.ToShortDateString(),
                            end.ToShortDateString())));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales de la nota de credito
                var notasDeCredito = new List<VMReporteNotaDeCredito>();
                creditNotes.ForEach(nc => notasDeCredito.Add(new VMReporteNotaDeCredito(nc)));

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", notasDeCredito));

                report = new VMReporte("NotasDeCreditoPorCliente", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Llena el reporte de estado de la empresa por periodo
        /// </summary>
        /// <param name="vm">Datos con las cuales se debe llenar el reporte</param>
        /// <returns>Reporte listo para ver o imprimir</returns>
        public static VMReporte FillReport(VMEstadoDeLaEmpresa vm)
        {
            try
            {
                //Aqui lleno las propiedades y data sources del reporte, así como el archivo físico, etc.
                VMReporte report = null;

                //Lleno mis propiedades
                var source = new List<ReportParameter>();
                source.Add(new ReportParameter("Empresa", Session.Configuration.Estacion.Empresa.descripcion));
                source.Add(new ReportParameter("Fecha", DateTime.Today.ToString("dd/MM/yyyy")));

                //Ahora lleno los dataSources
                var data = new List<ReportDataSource>();

                //Los generales del reporte
                var detalle = new List<VMEstadoDeLaEmpresa>();
                detalle.Add(vm);

                //Agrego todos los data sources a la lista "data"
                data.Add(new ReportDataSource("Detalle", detalle));

                report = new VMReporte("EstadoDeLaEmpresa", Reports.ReportsPrinter, data, source);

                return report;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
