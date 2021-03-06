using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;

namespace Aprovi.Business.Services
{
    public abstract class FacturaService : IFacturaService
    {
        private IUnitOfWork _UOW;
        private IFacturasRepository _invoices;
        private ISerieService _series;
        private ICatalogosEstaticosService _staticCatalogs;
        private IConfiguracionService _config;
        private IComprobantFiscaleService _fiscalReceipts;
        private ICotizacionService _quotes;
        private IClienteService _clients;
        private IArticuloService _items;
        private INotaDeCreditoService _creditNotes;
        private IAbonosDeFacturaRepository _payments;
        private IViewFoliosPorAbonosRepository _folios;
        private IMetodosPagoRepository _paymentMethods;
        private IRemisionesRepository _billsOfSale;
        private IViewListaFacturasRepository _listaFacturas;
        private IViewResumenPorFacturaRepository _invoicesSummary;
        private IViewReporteAntiguedadSaldosFacturasRepository _collectableBalances;
        private IViewReporteEstatusDeLaEmpresaFacturasRepository _companyStatusInvoices;
        private IViewReporteEstatusDeLaEmpresaCuentasPorCobrarRepository _collectableBalancesCompanyStatus;

        public FacturaService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, ICotizacionService quotes, IClienteService clients, IArticuloService items, INotaDeCreditoService creditNotes, ISerieService series, ICatalogosEstaticosService staticCatalogs)
        {
            _UOW = unitOfWork;
            _invoicesSummary = unitOfWork.ResumenDeFacturas;
            _invoices = _UOW.Facturas;
            _series = series;
            _staticCatalogs = staticCatalogs;
            _config = config;
            _fiscalReceipts = fiscalReceipts;
            _quotes = quotes;
            _clients = clients;
            _items = items;
            _creditNotes = creditNotes;
            _payments = _UOW.AbonosDeFactura;
            _folios = _UOW.FoliosPorAbono;
            _paymentMethods = _UOW.MetodosDePago;
            _billsOfSale = _UOW.Remisiones;
            _listaFacturas = _UOW.ListaFacturas;
            _collectableBalances = _UOW.AntiguedadSaldosFacturas;
            _collectableBalancesCompanyStatus = _UOW.EstatusDeLaEmpresaCuentasPorCobrar;
            _companyStatusInvoices = _UOW.EstatusDeLaEmpresaFacturas;
        }

        public int Next(string serie)
        {
            try
            {
                return _series.Next(serie);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last(string serie)
        {
            try
            {
                return _series.Last(serie, TipoDeComprobante.Factura);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Factura Add(VMFactura invoice)
        {
            try
            {
                //Se obtiene la cotizacion de la que viene la factura
                Cotizacione cotizacion = null;
                List<Remisione> remisiones = null;
                if (invoice.Cotizaciones.isValid() && !invoice.Cotizaciones.IsEmpty())
                {
                    cotizacion = invoice.Cotizaciones.FirstOrDefault();
                }

                //Se obtienen las remisiones de las que viene la factura
                if (invoice.Remisiones.isValid() && !invoice.Remisiones.IsEmpty())
                {
                    remisiones = invoice.Remisiones.ToList();
                }

                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la caja configurada
                invoice.idEmpresa = config.Estacion.idEmpresa;

                //La hora en que se esta registrando
                invoice.fechaHora = DateTime.Now;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                invoice.folio = Next(invoice.serie);

                //Antes de registrarla obtengo el numero de certificado que se utilizara para sellarla
                invoice.noCertificado = config.Certificados.First(c => c.activo).numero;

                //Obtengo la cadena original
                invoice.cadenaOriginal = _fiscalReceipts.GetCadenaOriginal(invoice, config);

                //Obtengo el sello de esa cadena
                invoice.sello = _fiscalReceipts.GetSello(invoice.cadenaOriginal, config);

                //Le agrego estado
                invoice.idEstatusDeFactura = (int)StatusDeFactura.Pendiente_de_timbrado;

                //El método de pago depende de si hubo o no abono y por cuanto

                //Si fue pagada en su totalidad es una sola exhibición (No lo que seleccione el cliente)
                if (invoice.Abonado.Equals(invoice.Total))
                    invoice.idMetodoPago = (int)MetodoDePago.Pago_en_una_sola_exhibicion;
                else
                    invoice.idMetodoPago = (int)MetodoDePago.Pago_en_parcialidades_o_diferido;

                //Solo requiere la referencia

                invoice.NotasDeDescuentoes = null;
                invoice.MetodosPago = null;
                invoice.TiposRelacion = null;
                invoice.Factura1 = null;
                invoice.Usuario1 = null;
                invoice.Remisiones = null;

                //Si es una sustitución
                if (!invoice.idComprobanteOriginal.HasValue || invoice.idComprobanteOriginal <= 0)
                {
                    invoice.idTipoRelacion = null;
                    invoice.idComprobanteOriginal = null;
                }

                //Si tiene abonos, le pongo los defaults a cada abono
                //Obtengo el siguiente folio menos 1 para que se autoincrementen aqui.
                var folios = _folios.Next().ToInt();
                foreach (var a in invoice.AbonosDeFacturas)
                {
                    a.fechaHora = DateTime.Now;
                    a.folio = (folios++).ToString();
                    a.idEstatusDeAbono = (int)StatusDeAbono.Registrado;
                    a.FormasPago = null;
                    a.Moneda = null;
                    if (!a.idCuentaBancaria.HasValue || a.idCuentaBancaria.Value.Equals(0))
                        a.idCuentaBancaria = null;
                }

                //Estatus crediticio
                invoice.idEstatusCrediticio = invoice.Saldo > 0.0m ? (int)StatusCrediticio.Saldo_pendiente : (int)StatusCrediticio.Saldado;

                //Guardo la factura
                _UOW.Reload();
                var local = _invoices.Add(invoice.ToFactura());

                _UOW.Save();

                //Si venia de una cotizacion, se guarda el id de la factura en la cotizacion
                if (cotizacion.isValid())
                {
                    cotizacion.idFactura = local.idFactura;
                    cotizacion.idEstatusDeCotizacion = (int) StatusDeCotizacion.Facturada;

                    _quotes.Update(new VMCotizacion(cotizacion));
                    _UOW.Save();
                }

                //Si venia de una facturacion de remisiones, se asigna el id de la factura a las remisiones
                if (remisiones.isValid() && !remisiones.IsEmpty())
                {
                    foreach (Remisione r in remisiones)
                    {
                        Remisione remision = _billsOfSale.Find((object) r.idRemision);
                        remision.idFactura = local.idFactura;
                        remision.idEstatusDeRemision = (int) StatusDeRemision.Facturada; //Se agrega que actualice el campo de EstatusDeRemision a Facturada(2) - JCRV
                    }

                    _UOW.Save();
                }

                invoice.idFactura = local.idFactura;

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Factura Stamp(VMFactura invoice, bool requiresAddenda)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Antes de registrarla obtengo el numero de certificado que se utilizara para sellarla por si se actualizó
                invoice.noCertificado = config.Certificados.FirstOrDefault(c => c.activo).numero;

                //Obtengo la cadena original
                invoice.cadenaOriginal = _fiscalReceipts.GetCadenaOriginal(invoice, config);

                //Obtengo el sello de esa cadena
                invoice.sello = _fiscalReceipts.GetSello(invoice.cadenaOriginal, config);

                //Genero el xml
                var xml = _fiscalReceipts.CreateCFDI(invoice, config, requiresAddenda);

                //Timbro el xml
                xml = _fiscalReceipts.Timbrar(xml, config);

                //Si pudo timbrar entonces obtengo el registro original de la factura
                _UOW.Reload();
                var local = _invoices.Find(invoice.idFactura);
                
                //Le agrego el timbre a la factura, ya que aún necesito generar el cbb
                invoice.TimbresDeFactura = _fiscalReceipts.GetTimbreFactura(xml);
                
                //Le actualizo al registro de la base de datos, el timbre agregado, el numero de certificado, la cadena original y el sello, por si acaso difieren
                local.TimbresDeFactura = invoice.TimbresDeFactura;
                local.noCertificado = invoice.noCertificado;
                local.cadenaOriginal = invoice.cadenaOriginal;
                local.sello = invoice.sello;
                //Le cambio el estado
                local.idEstatusDeFactura = (int)StatusDeFactura.Timbrada;
                local.EstatusDeFactura = null;
                

                //Guardo la factura actualizada
                _invoices.Update(local);
                _UOW.Save();

                //Genero el xml
                xml.Save(string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, invoice.serie, invoice.folio));

                //Genero el cbb con el ViewModel
                _fiscalReceipts.CreateCBB(string.Format("{0}\\{1}{2}.bmp", config.CarpetaCbb, invoice.serie, invoice.folio), config.rfc, invoice.Cliente.rfc, invoice.Total, invoice.TimbresDeFactura.UUID);

                //Regreso la factura timbrada
                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Factura Find(int idInvoice)
        {
            try
            {
                return _invoices.Find(idInvoice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Factura Find(string serie, string folio)
        {
            try
            {
                return _invoices.Find(serie, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Factura> ListBySeller(DateTime startDate, DateTime endDate, Usuario user = null)
        {
            try
            {
                _UOW.Reload();

                if (user.isValid() && user.idUsuario.isValid())
                {
                    return _invoices.ListBySeller(startDate, endDate, user);
                }
                else
                {
                    return _invoices.List(startDate, endDate);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Factura Cancel(int idInvoice, string reason)
        {
            try
            {
                var invoice = _invoices.Find(idInvoice);
                var remisiones = _billsOfSale.ListByInvoice(idInvoice);

                //Si ya esta cancelada, tiro excepción
                if (invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Anulada) || invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Cancelada))
                    throw new Exception("Esta factura ya se encuentra cancelada");

                //No puedo cancelar una factura que tenga parcialidades activas
                if (invoice.AbonosDeFacturas.Count(a => a.idEstatusDeAbono.Equals((int)StatusDeAbono.Registrado)) > 0)
                    throw new Exception("Esta factura contiene parcialidades activas, por lo tanto no puede ser eliminada");

                //No se puede cancelar una factura con notas de crédito
                if (invoice.NotasDeCreditoes.Any(nc => nc.idEstatusDeNotaDeCredito != (int)StatusDeNotaDeCredito.Cancelada && nc.idEstatusDeNotaDeCredito != (int)StatusDeNotaDeCredito.Anulada))
                {
                    throw new Exception("Esta factura contiene notas de crédito, por lo tanto no puede ser eliminada");
                }

                //Se agrega registro en la tabla de cancelaciones
                invoice.CancelacionesDeFactura = new CancelacionesDeFactura() { fechaHora = DateTime.Now, motivo = reason };

                //Al cancelar una factura que no fue timbrada se pasa a anulada
                if (invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Pendiente_de_timbrado))
                {
                    invoice.idEstatusDeFactura = (int)StatusDeFactura.Anulada;
                    invoice.EstatusDeFactura = null;

                    //JCRV - Si la factura esta ligada a una o mas remisiones, se elimina relacion y se regresa su estatus a Registrada, para poder ser facturada de nuevo.
                    if (remisiones.isValid() && !remisiones.IsEmpty())
                        _billsOfSale.restoreRemision(remisiones);

                    _UOW.Save();
                    //Por tanto aquí se acaba el flujo
                    return invoice;
                }

                //Si llega aquí debe estar Timbrada y requiere cancelación fiscal
                if (!invoice.idEstatusDeFactura.Equals((int)StatusDeFactura.Timbrada))
                    throw new Exception("Estado de la factura desconocido");

                //JCRV - Si la factura esta ligada a una o mas remisiones, se elimina relacion y se regresa su estatus a Registrada, para poder ser facturada de nuevo.
                if (remisiones.isValid() && !remisiones.IsEmpty())
                    _billsOfSale.restoreRemision(remisiones);

                var config = _config.GetDefault();
                invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura = new CancelacionesDeTimbresDeFactura();
                invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura.fechaHora = DateTime.Now;
                invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura.acuse = _fiscalReceipts.Cancelar(invoice.TimbresDeFactura.UUID, config);
                invoice.idEstatusDeFactura = (int)StatusDeFactura.Cancelada;
                invoice.EstatusDeFactura = null;
                _UOW.Save();

                //Genero el acuse de cancelación
                _fiscalReceipts.CreateAcuse(string.Format("{0}\\{1}{2}-Acuse de cancelación.xml", config.CarpetaXml, invoice.serie, invoice.folio), invoice.TimbresDeFactura.CancelacionesDeTimbresDeFactura.acuse);

                return invoice;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleVentaPorPeriodo> GetSalesDetailsForPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var invoices = _invoices.List(startDate, endDate);

                return invoices.Select(x => new VMRDetalleVentaPorPeriodo(new VMFactura(x))).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleComision> GetComissionDetailsForPeriodAndUser(DateTime startDate, DateTime endDate, Usuario user)
        {
            try
            {
                var invoices = ListBySeller(startDate, endDate, user).ToViewModelList();

                return invoices.Select(x => new VMRDetalleComision(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaFactura> WithClientOrFolioLike(string value)
        {
            try
            {
                return _listaFacturas.WithFolioOrClientLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaFactura> List()
        {
            try
            {
                return _listaFacturas.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMFacturaConSaldo> List(Cliente customer)
        {
            try
            {
                //Solo regresa facturas timbradas (se utiliza para abonos por cliente)
                return _invoicesSummary.ListByCustomer(customer.idCliente, true).Where(s => s.idEstatusDeFactura.Equals((int)StatusDeFactura.Timbrada)).Select(x=> new VMFacturaConSaldo(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMFacturaConSaldo> List(Pago payment)
        {
            try
            {
                //Obtengo la lista de facturas a las que pertenecen los abonos del pago
                var invoices = _invoicesSummary.ListByIds(payment.AbonosDeFacturas.Select(a => a.idFactura).ToList());

                //Ahora debo agregar cada factura asociandola con su pago
                var relatedInvoices = new List<VMFacturaConSaldo>();
                foreach (var a in payment.AbonosDeFacturas)
                {
                    relatedInvoices.Add(new VMFacturaConSaldo(invoices.FirstOrDefault(i => i.idFactura.Equals(a.idFactura)), a));
                }

                return relatedInvoices;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleAntiguedadSaldos> ListForDetailedCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to)
        {
            try
            {
                return _collectableBalances.List(customer, seller, onlyExpired, to).Select(x=> new VMRDetalleAntiguedadSaldos(x, to)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRTotalAntiguedadSaldos> ListForTotalCollectableBalancesReport(Cliente customer, Usuario seller, bool onlyExpired, DateTime to)
        {
            try
            {
                return _collectableBalances.List(customer, seller, onlyExpired, to).Select(x => new VMRTotalAntiguedadSaldos(x, to)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMDetalleDeFactura> UpdatePrices(List<VMDetalleDeFactura> details, Cliente customer)
        {
            try
            {
                var customerDb = _clients.Find(customer.idCliente);

                foreach (var d in details)
                {
                    var item = _items.Find(d.idArticulo);
                    d.precioUnitario = Operations.CalculatePriceWithoutTaxes(item.costoUnitario, item.Precios.FirstOrDefault(x => x.idListaDePrecio == customerDb.idListaDePrecio).utilidad);
                    d.descuento = 0m; //Se recalculan precios para un cliente en particular
                }

                return details;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListInvoicesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                try
                {
                    var detail = _companyStatusInvoices.List(startDate, endDate);

                    var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                    var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                    vm.TotalDolaresVentas += detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                    vm.TotalPesosVentas += detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));
                    vm.TotalDolaresVentasImpuestosRetenidos += detailDollars.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                    vm.TotalDolaresVentasImpuestosTrasladados += detailDollars.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));
                    vm.TotalPesosVentasImpuestosRetenidos += detailPesos.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                    vm.TotalPesosVentasImpuestosTrasladados += detailPesos.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));

                    return vm;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListCollectableBalancesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                try
                {
                    var detail = _collectableBalancesCompanyStatus.List(startDate, endDate);

                    var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                    var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                    vm.TotalDolaresCuentasPorCobrar += detailDollars.Sum(x => x.importe.GetValueOrDefault(0m));
                    vm.TotalPesosCuentasPorCobrar += detailPesos.Sum(x => x.importe.GetValueOrDefault(0m));
                    vm.TotalDolaresCuentasPorCobrarImpuestosRetenidos += detailDollars.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                    vm.TotalDolaresCuentasPorCobrarImpuestosTrasladados += detailDollars.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));
                    vm.TotalPesosCuentasPorCobrarImpuestosRetenidos += detailPesos.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                    vm.TotalPesosCuentasPorCobrarImpuestosTrasladados += detailPesos.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));

                    return vm;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
