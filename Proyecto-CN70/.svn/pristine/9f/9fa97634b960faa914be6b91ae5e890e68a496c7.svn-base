using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class PagoService : IPagoService
    {
        private IUnitOfWork _UOW;
        private IPagosRepository _payments;
        private ISerieService _series;
        private IConfiguracionService _config;
        private IComprobantFiscaleService _fiscalReceipts;
        private readonly IFacturaService _invoices;
        private readonly ITiposDeComprobanteRepository _receiptTypes;
        private IAbonoDeFacturaService _invoicePayments;
        private IMetodosPagoRepository _paymentMethods;
        private IViewFoliosPorAbonosRepository _folios;
        private IViewListaPagosRepository _paymentsList;


        public PagoService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, IFacturaService invoices, IAbonoDeFacturaService invoicePayments, ISerieService serie)
        {
            _UOW = unitOfWork;
            _payments = _UOW.Pagos;
            _series = serie;
            _config = config;
            _fiscalReceipts = fiscalReceipts;
            _invoices = invoices;
            _receiptTypes = _UOW.TiposDeComprobante;
            _invoicePayments = invoicePayments;
            _paymentMethods = _UOW.MetodosDePago;
            _folios = _UOW.FoliosPorAbono;
            _paymentsList = _UOW.ListaDePagos;
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
                return _series.Last(serie, TipoDeComprobante.Parcialidad);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pago Add(Pago payment)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la caja configurada
                payment.idEmpresa = config.Estacion.idEmpresa;

                //La hora en que se esta registrando
                payment.fechaHora = DateTime.Now;

                //La serie configurada
                var serie = _receiptTypes.Find((int) TipoDeComprobante.Parcialidad);
                
                //Si hay una serie configurada, utiliza esa, de lo contrario usa la de la primer factura
                if (serie.Series.isValid())
                {
                    payment.serie = serie.Series.identificador;
                }
                else
                {
                    var invoice = _invoices.Find(payment.AbonosDeFacturas.FirstOrDefault().idFactura);
                    payment.serie = invoice.serie;
                }

                payment.idUsoCFDI = (int)UsoCFDI.Por_Definir;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                payment.folio = Next(payment.serie);

                //Obtengo la cadena original
                payment.cadenaOriginal = "Pendiente de timbrado";//_fiscalReceipts.GetCadenaOriginal(payment, config);

                ////Obtengo el sello de esa cadena
                //payment.TimbresDePago.sello = _fiscalReceipts.GetSello(payment.cadenaOriginal, config);

                //Le agrego estado
                payment.idEstatusDePago = (int)StatusDePago.Pendiente_de_timbrado;

                //El método de pago siempre es PPD
                payment.idMetodoDePago = (int)MetodoDePago.Pago_en_parcialidades_o_diferido;

                //Solo requiere la referencia
                payment.MetodosPago = null;
                payment.Usuario = null;
                payment.Cliente = null;
                //Esta propiedad no se utiliza hasta despues del timbrado
                payment.TimbresDePago = null;

                //Guardo el pago
                _UOW.Reload();

                var folio = _invoicePayments.GetNextFolio().ToInt();
                foreach (var a in payment.AbonosDeFacturas)
                {
                    a.folio = folio++.ToString();
                    a.CuentasBancaria = null;
                }

                var local = _payments.Add(payment);

                _UOW.Save();

                //Si tiene abonos, se registra el id del abono
                foreach (var a in payment.AbonosDeFacturas)
                {
                    a.idPago = local.idPago;
                }

                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pago Stamp(Pago payment)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Se inicializa la propiedad de timbrado
                payment.TimbresDePago = new TimbresDePago();

                //Antes de registrarla obtengo el numero de certificado que se utilizara para sellarla por si se actualizó
                payment.TimbresDePago.noCertificado = config.Certificados.FirstOrDefault(c => c.activo).numero;

                //Obtengo la cadena original
                payment.cadenaOriginal = _fiscalReceipts.GetCadenaOriginal(payment, config);

                //Obtengo el sello de esa cadena
                payment.TimbresDePago.sello = _fiscalReceipts.GetSello(payment.cadenaOriginal, config);

                //Genero el xml
                var xml = _fiscalReceipts.CreateCFDI(payment, config);

                //Timbro el xml
                xml = _fiscalReceipts.Timbrar(xml, config);

                //Se guardan el sello y numero de certificado para agregarlos posteriormente
                var cadena = payment.cadenaOriginal;
                var sello = payment.TimbresDePago.sello;
                var noCertificado = payment.TimbresDePago.noCertificado;

                //Si pudo timbrar entonces obtengo el registro original del pago
                _UOW.Reload();
                var local = _payments.Find(payment.idPago);

                //Le agrego el timbre al pago, ya que aún necesito generar el cbb
                payment.TimbresDePago = _fiscalReceipts.GetTimbrePago(xml);

                //Se asignan el sello y numero de certificado
                payment.TimbresDePago.sello = sello;
                payment.TimbresDePago.noCertificado = noCertificado;
                
                //Le actualizo al registro de la base de datos, el timbre agregado, el numero de certificado, la cadena original y el sello, por si acaso difieren
                local.TimbresDePago = payment.TimbresDePago;
                local.cadenaOriginal = cadena;
                local.idEstatusDePago = (int)StatusDePago.Timbrado;

                //Guardo la factura actualizada
                _payments.Update(local);
                _UOW.Save();

                //Genero el xml
                xml.Save(string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, payment.serie, payment.folio));

                //Genero el cbb con el ViewModel
                //Se supone que el pago tiene un total de 0, se debe verificar que esto sea correcto
                _fiscalReceipts.CreateCBB(string.Format("{0}\\{1}{2}.bmp", config.CarpetaCbb, payment.serie, payment.folio), config.rfc, payment.Cliente.rfc, 0m, payment.TimbresDePago.UUID);

                //Regreso la factura timbrada
                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pago Find(int idPayment)
        {
            try
            {
                return _payments.Find(idPayment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pago Find(string serie, string folio)
        {
            try
            {
                return _payments.Find(serie, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaPago> List()
        {
            try
            {
                _UOW.Reload();
                return _paymentsList.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaPago> WithClientOrFolioLike(string value)
        {
            try
            {
                return _paymentsList.WithFolioOrClientLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Pago Cancel(int idPayment, string reason)
        {
            try
            {
                var local = _payments.Find(idPayment);
                //Si ya esta cancelada, tiro excepción

                if (local.idEstatusDePago.Equals((int)StatusDePago.Cancelado) || local.idEstatusDePago.Equals((int)StatusDePago.Anulado))
                    throw new Exception("Este pago ya se encuentra cancelado");

                //Hago el registro de cancelación
                local.idEstatusDePago = (int)StatusDePago.Anulado;
                local.EstatusDePago = null;

                //Si esta timbrado hago la cancelación fiscal
                var config = _config.GetDefault();
                if (local.TimbresDePago.isValid() && local.TimbresDePago.idTimbreDePago.isValid())
                {
                    local.idEstatusDePago = (int)StatusDePago.Cancelado;
                    local.TimbresDePago.CancelacionesDeTimbresDePago = new CancelacionesDeTimbresDePago();
                    local.TimbresDePago.CancelacionesDeTimbresDePago.fechaHora = DateTime.Now;
                    local.TimbresDePago.CancelacionesDeTimbresDePago.acuse = _fiscalReceipts.Cancelar(local.TimbresDePago.UUID, config);
                    //Genero el acuse de cancelación
                    _fiscalReceipts.CreateAcuse(string.Format("{0}\\{1}{2}-Acuse de cancelación.xml", config.CarpetaXml, local.serie, local.folio), local.TimbresDePago.CancelacionesDeTimbresDePago.acuse);
                }

                //Le cambio el estatus a todos los abonos de factura que contempla el pago
                foreach (var a in local.AbonosDeFacturas)
                {
                    a.idEstatusDeAbono = (int)StatusDeAbono.Cancelado;
                    a.EstatusDeAbono = null;
                }

                //Registro el motivo
                local.CancelacionesDePago = new CancelacionesDePago() { fechaHora = DateTime.Now, motivo = reason };

                _payments.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
