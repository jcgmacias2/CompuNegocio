﻿using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Aprovi.Business.Helpers;

namespace Aprovi.Business.Services
{
    public abstract class NotaDeCreditoService : INotaDeCreditoService
    {
        private IUnitOfWork _UOW;
        private IFacturasRepository _invoices;
        private INotasDeCreditoRepository _creditNotes;
        private ISerieService _series;
        private ICatalogosEstaticosService _staticCatalogs;
        private IConfiguracionService _config;
        private IComprobantFiscaleService _fiscalReceipts;
        private IClienteService _clients;
        private IArticuloService _items;
        private IViewReporteEstatusDeLaEmpresaNotasDeCreditoRepository _companyStatusCreditNotes;

        public NotaDeCreditoService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, IClienteService clients, IArticuloService items, ISerieService series, ICatalogosEstaticosService staticCatalogs)
        {
            _UOW = unitOfWork;
            _invoices = _UOW.Facturas;
            _series = series;
            _staticCatalogs = staticCatalogs;
            _config = config;
            _fiscalReceipts = fiscalReceipts;
            _clients = clients;
            _items = items;
            _creditNotes = _UOW.NotasDeCredito;
            _companyStatusCreditNotes = _UOW.EstatusDeLaEmpresaNotasDeCredito;
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
                return _series.Last(serie, TipoDeComprobante.Nota_De_Credito);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Add(VMNotaDeCredito creditNote)
        {
            try
            {
                //Se obtiene la factura de la que viene la nota de credito
                Factura factura = null;

                if (creditNote.Factura.isValid() && creditNote.Factura.idFactura.isValid())
                {
                    factura = creditNote.Factura;
                }

                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la caja configurada
                creditNote.idEmpresa = config.Estacion.idEmpresa;

                //La hora en que se esta registrando
                creditNote.fechaHora = DateTime.Now;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                creditNote.folio = Next(creditNote.serie);
                creditNote.cadenaOriginal = "Por timbrar";

                //Le agrego estado
                creditNote.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Pendiente_De_Timbrado;

                //Solo requiere la referencia
                creditNote.Factura = null;
                creditNote.Usuario = null;

                //Guardo la nota de credito
                _UOW.Reload();
                var local = _creditNotes.Add(creditNote.ToCreditNote());

                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Stamp(VMNotaDeCredito creditNote)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                creditNote.TimbresDeNotasDeCredito = new TimbresDeNotasDeCredito();

                //Antes de registrarla obtengo el numero de certificado que se utilizara para sellarla por si se actualizó
                creditNote.TimbresDeNotasDeCredito.noCertificado = config.Certificados.FirstOrDefault(c => c.activo).numero;

                //Obtengo la cadena original
                //creditNote.cadenaOriginal = _fiscalReceipts.GetCadenaOriginal(creditNote, config);
                creditNote.cadenaOriginal = "";

                //Obtengo el sello de esa cadena
                //creditNote.TimbresDeNotasDeCredito.sello = _fiscalReceipts.GetSello(creditNote.cadenaOriginal, config);
                creditNote.TimbresDeNotasDeCredito.sello = "";

                //Genero el xml
                var xml = _fiscalReceipts.CreateCFDI(creditNote, config);

                //Timbro el xml
                //xml = _fiscalReceipts.Timbrar(xml, config);
                xml = _fiscalReceipts.Timbrar_v2(xml, config);

                //Si pudo timbrar entonces obtengo el registro original de la factura
                _UOW.Reload();
                var local = _creditNotes.Find(creditNote.idNotaDeCredito);

                //Se guardan el sello y numero de certificado para asignarlos posteriormente
                var sello = creditNote.TimbresDeNotasDeCredito.sello;
                var noCertificado = creditNote.TimbresDeNotasDeCredito.noCertificado;

                //Le agrego el timbre a la factura, ya que aún necesito generar el cbb
                creditNote.TimbresDeNotasDeCredito = _fiscalReceipts.GetTimbreNotaDeCredito(xml);

                //Se restauran el sello y numero de certificado
                creditNote.TimbresDeNotasDeCredito.sello = sello;
                creditNote.TimbresDeNotasDeCredito.noCertificado = noCertificado;

                //    //Le actualizo al registro de la base de datos, el timbre agregado, el numero de certificado, la cadena original y el sello, por si acaso difieren
                local.TimbresDeNotasDeCredito = creditNote.TimbresDeNotasDeCredito;
                local.cadenaOriginal = creditNote.cadenaOriginal;

                //Le cambio el estado
                local.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Timbrada;
                local.EstatusDeNotaDeCredito = null;


                //Guardo la nota de credito actualizada
                _creditNotes.Update(local);
                _UOW.Save();

                //Genero el xml
                xml.Save(string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, creditNote.serie, creditNote.folio));

                //Genero el cbb con el ViewModel
                _fiscalReceipts.CreateCBB(string.Format("{0}\\{1}{2}.bmp", config.CarpetaCbb, creditNote.serie, creditNote.folio), config.rfc, creditNote.Cliente.rfc, creditNote.Total, creditNote.TimbresDeNotasDeCredito.UUID);

                //Regreso la nota de credito timbrada
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Find(int idCreditNote)
        {
            try
            {
                return _creditNotes.Find(idCreditNote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Find(string serie, string folio)
        {
            try
            {
                return _creditNotes.Find(serie, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Cancel(int idCreditNotes, string reason)
        {
            try
            {
                var creditNote = _creditNotes.Find(idCreditNotes);

                //Si ya esta cancelada, tiro excepción
                if (creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Anulada) || creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Cancelada))
                    throw new Exception("Esta nota de credito ya se encuentra cancelada");

                //Se agrega registro en la tabla de cancelaciones
                creditNote.CancelacionesDeNotaDeCredito = new CancelacionesDeNotaDeCredito() { fechaHora = DateTime.Now, motivo = reason };

                //Al cancelar una nota de credito que no fue timbrada se pasa a anulada
                if (creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Pendiente_De_Timbrado))
                {
                    creditNote.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Anulada;
                    creditNote.EstatusDeNotaDeCredito = null;

                    _UOW.Save();
                    //Por tanto aquí se acaba el flujo
                    return creditNote;
                }

                //Si llega aquí debe estar Timbrada y requiere cancelación fiscal
                if (!creditNote.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Timbrada))
                    throw new Exception("Estado de la nota de credito desconocido");

                var config = _config.GetDefault();
                creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito = new CancelacionesDeTimbresDeNotasDeCredito();
                creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito.fechaHora = DateTime.Now;
                creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito.acuse = _fiscalReceipts.Cancelar(creditNote.TimbresDeNotasDeCredito.UUID, creditNote.TimbresDeNotasDeCredito.noCertificado, config);
                creditNote.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Cancelada;
                creditNote.EstatusDeNotaDeCredito = null;

                _UOW.Save();

                //Genero el acuse de cancelación
                _fiscalReceipts.CreateAcuse(string.Format("{0}\\{1}{2}-Acuse de cancelación.xml", config.CarpetaXml, creditNote.serie, creditNote.folio), creditNote.TimbresDeNotasDeCredito.CancelacionesDeTimbresDeNotasDeCredito.acuse);

                return creditNote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMNotaDeCredito> WithClientOrFolioLike(string value)
        {
            try
            {
                return _creditNotes.WithFolioOrClientLike(value, null).ToList().Select(x=> new VMNotaDeCredito(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMNotaDeCredito> List()
        {
            try
            {
                return _creditNotes.List().ToList().Select(x=>new VMNotaDeCredito(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMNotaDeCredito> ByPeriod(DateTime start, DateTime end)
        {
            try
            {
                return _creditNotes.List(start, end).Select(x => new VMNotaDeCredito(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMNotaDeCredito> ByPeriodAndCustomer(DateTime start, DateTime end, Cliente customer)
        {
            try
            {
                return _creditNotes.List(start, end, customer).Select(x => new VMNotaDeCredito(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeCredito Add(Factura invoice, List<NotasDeDescuento> discountNotes)
        {
            try
            {
                var serie = _staticCatalogs.ListTiposDeComprobante().FirstOrDefault(t => t.idTipoDeComprobante.Equals((int)TipoDeComprobante.Nota_De_Credito)).Series;

                NotasDeCredito nc = new NotasDeCredito();

                nc.idFactura = invoice.idFactura;
                nc.descripcion = string.Join(",", discountNotes.Select(x => x.descripcion).ToArray());
                nc.importe = discountNotes.Sum(x => x.monto.ToDocumentCurrency(x.Moneda, invoice.Moneda, x.tipoDeCambio));
                nc.idMoneda = invoice.idMoneda;
                nc.idCliente = invoice.idCliente;
                nc.folio = Next(serie.identificador);
                nc.serie = serie.identificador;
                nc.tipoDeCambio = invoice.tipoDeCambio;
                nc.idEmpresa = invoice.idEmpresa;
                nc.fechaHora = DateTime.Now;
                nc.idEstatusDeNotaDeCredito = (int)StatusDeNotaDeCredito.Pendiente_De_Timbrado;
                nc.idUsuarioRegistro = invoice.idUsuarioRegistro;
                nc.idFormaDePago = (int)Formas_De_Pago.Por_Definir;
                nc.idRegimen = invoice.idRegimen;
                nc.cadenaOriginal = "Pendiente de timbrado";

                nc.Cliente = null;

                var creditNote = _creditNotes.Add(nc);
                _UOW.Save();

                foreach (var d in discountNotes)
                {
                    d.idNotaDeCredito = creditNote.idNotaDeCredito;
                    d.idFactura = invoice.idFactura;
                }

                _UOW.Save();

                return nc;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VMEstadoDeLaEmpresa ListCreditNotesForCompanyStatus(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate)
        {
            try
            {
                var detail = _companyStatusCreditNotes.List(startDate, endDate);

                var detailPesos = detail.Where(x => x.idMoneda == (int)Monedas.Pesos).ToList();
                var detailDollars = detail.Where(x => x.idMoneda == (int)Monedas.Dólares).ToList();

                vm.TotalDolaresNotasDeCredito = detailDollars.Sum(x => x.importe);
                vm.TotalPesosNotasDeCredito = detailPesos.Sum(x => x.importe);
                vm.TotalDolaresNotasDeCreditoImpuestosRetenidos = detailDollars.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalDolaresNotasDeCreditoImpuestosTrasladados = detailDollars.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));
                vm.TotalPesosNotasDeCreditoImpuestosRetenidos = detailPesos.Sum(x => x.impuestosRetenidos.GetValueOrDefault(0m));
                vm.TotalPesosNotasDeCreditoImpuestosTrasladados = detailPesos.Sum(x => x.impuestosTrasladados.GetValueOrDefault(0m));

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanCreateForInvoice(VMNotaDeCredito creditNote, Factura invoice)
        {
            try
            {
                VMFactura vmInvoice = new VMFactura(creditNote.Factura);
                Dictionary<int, decimal> items = new Dictionary<int, decimal>();
                Dictionary<int, decimal> creditedItems = new Dictionary<int, decimal>();

                //Se totalizan los articulos devueltos en notas de credito anteriores Timbradas o Pendientes de timbrar
                foreach (var lastCreditNote in creditNote.Factura.NotasDeCreditoes.Where(nc => nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Pendiente_De_Timbrado) || nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Timbrada)))
                {
                    foreach (var i in lastCreditNote.DetallesDeNotaDeCreditoes.ToList())
                    {
                        if (creditedItems.ContainsKey(i.idArticulo))
                        {
                            creditedItems[i.idArticulo] += i.cantidad;
                        }
                        else
                        {
                            creditedItems.Add(i.idArticulo, i.cantidad);
                        }
                    }
                }

                foreach (var d in creditNote.Factura.DetallesDeFacturas)
                {
                    if (items.ContainsKey(d.idArticulo))
                    {
                        items[d.idArticulo] += d.cantidad;
                    }
                    else
                    {
                        items.Add(d.idArticulo, d.cantidad);
                    }
                }

                //Se valida el total de articulos
                foreach (var d in creditNote.DetalleDeNotaDeCredito.ToList())
                {
                    if ((creditedItems.ContainsKey(d.idArticulo) ? creditedItems[d.idArticulo] : 0m) + d.cantidad > items[d.idArticulo])
                    {
                        throw new Exception("No se puede crear una nota de crédito con devoluciones mayores a la factura");
                    }
                }

                //Se verifica el total
                if (vmInvoice.Total <
                    (creditNote.Factura.NotasDeCreditoes.Where(nc => nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Pendiente_De_Timbrado) || nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Timbrada)).Select(x => new VMNotaDeCredito(x)).Sum(x => x.Total)) +
                    (new VMNotaDeCredito(creditNote)).Total)
                {
                    throw new Exception("No se puede crear una nota de crédito que exceda el total de la factura");
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
