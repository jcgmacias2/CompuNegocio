using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Aprovi.Business.Services
{
    public abstract class AbonoDeFacturaService : IAbonoDeFacturaService
    {
        private IUnitOfWork _UOW;
        private IAbonosDeFacturaRepository _payments;
        private IConfiguracionService _config;
        private IComprobantFiscaleService _fiscalReceipts;
        private ISerieService _folios;
        private ITiposDeComprobanteRepository _receiptTypes;
        private IViewFoliosPorAbonosRepository _foliosPorAbonos;
        private ITimbresDeAbonosDeFacturaRepository _stamps;
        private IViewListaParcialidadesRepository _paymentsList;

        public AbonoDeFacturaService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, ISerieService series)
        {
            _UOW = unitOfWork;
            _payments = _UOW.AbonosDeFactura;
            _config = config;
            _fiscalReceipts = fiscalReceipts;
            _folios = series;
            _receiptTypes = _UOW.TiposDeComprobante;
            _foliosPorAbonos = _UOW.FoliosPorAbono;
            _stamps = _UOW.TimbresParcialidades;
            _paymentsList = _UOW.ListaParcialidades;
        }

        public AbonosDeFactura Add(AbonosDeFactura payment)
        {
            try
            {
                //Agrego los defaults
                payment.fechaHora = DateTime.Now;
                payment.folio = GetNextFolio();
                payment.idEstatusDeAbono = (int)StatusDeAbono.Registrado;
                payment.FormasPago = null;
                payment.Moneda = null;
                if (payment.idCuentaBancaria.HasValue && !payment.idCuentaBancaria.Value.isValid())
                    payment.idCuentaBancaria = null;

                _payments.Add(payment);
                _UOW.Save();

                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura Find(int idPayment)
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

        public AbonosDeFactura Find(string folio)
        {
            try
            {
                return _payments.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeFactura> List(int idInvoice)
        {
            try
            {
                return _payments.List(idInvoice);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNextFolio()
        {
            try
            {
                return _foliosPorAbonos.Next();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura Cancel(int idPayment)
        {
            try
            {
                var local = _payments.Find(idPayment);
                //Si ya esta cancelada, tiro excepción
                if (local.idEstatusDeAbono.Equals((int)StatusDeAbono.Cancelado))
                    throw new Exception("Este abono ya se encuentra cancelado");

                local.idEstatusDeAbono = (int)StatusDeAbono.Cancelado;
                local.EstatusDeAbono = null;

                //Si es parcialidad hago la cancelación fiscal
                var config = _config.GetDefault();
                if(local.TimbresDeAbonosDeFactura.isValid() && local.TimbresDeAbonosDeFactura.idTimbreDeAbonoDeFactura.isValid())
                {
                    local.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura = new CancelacionesDeTimbreDeAbonosDeFactura();
                    local.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura.fechaHora = DateTime.Now;
                    local.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura.acuse = _fiscalReceipts.Cancelar(local.TimbresDeAbonosDeFactura.UUID, local.TimbresDeAbonosDeFactura.noCertificado, config);
                    //Genero el acuse de cancelación
                    _fiscalReceipts.CreateAcuse(string.Format("{0}\\{1}{2}-Acuse de cancelación.xml", config.CarpetaXml, local.TimbresDeAbonosDeFactura.serie, local.TimbresDeAbonosDeFactura.folio), local.TimbresDeAbonosDeFactura.CancelacionesDeTimbreDeAbonosDeFactura.acuse);
                }

                _payments.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleAbonosFacturas> GetPaymentsForReport(List<AbonosDeFactura> payments)
        {
            try
            {
                List<VMRDetalleAbonosFacturas> detail = new List<VMRDetalleAbonosFacturas>();

                foreach (var payment in payments)
                {
                    var numParcialidad = payment.Factura.AbonosDeFacturas.Count(a => a.idEstatusDeAbono != (int)StatusDeAbono.Cancelado && a.fechaHora <= payment.fechaHora.ToNextMidnight());

                    detail.Add(new VMRDetalleAbonosFacturas(payment, numParcialidad));
                }

                return detail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VwListaParcialidade FindVMBySerieAndFolio(string serie, int folio)
        {
            try
            {
                return _paymentsList.FindWithSerieAndFolio(serie, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura Stamp(VMFactura invoice, AbonosDeFactura payment)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Veo si hay alguna serie default configurada
                var serie = _receiptTypes.Find((int)TipoDeComprobante.Parcialidad);

                if (payment.TimbresDeAbonosDeFactura == null)
                    payment.TimbresDeAbonosDeFactura = new TimbresDeAbonosDeFactura();

                if (serie.Series.isValid())
                    payment.TimbresDeAbonosDeFactura.serie = serie.Series.identificador;
                else
                    payment.TimbresDeAbonosDeFactura.serie = invoice.serie;

                //Obtengo el siguiente folio
                payment.TimbresDeAbonosDeFactura.folio = _folios.Next(payment.TimbresDeAbonosDeFactura.serie);

                //Antes de registrarla obtengo el numero de certificado que se utilizara para sellarla
                payment.TimbresDeAbonosDeFactura.noCertificado = config.Certificados.FirstOrDefault(c => c.activo).numero;

                //Obtengo la cadena original
                payment.TimbresDeAbonosDeFactura.cadenaOriginal = _fiscalReceipts.GetCadenaOriginal(invoice, payment, config);

                //Obtengo el sello de esa cadena
                payment.TimbresDeAbonosDeFactura.sello = _fiscalReceipts.GetSello(payment.TimbresDeAbonosDeFactura.cadenaOriginal, config);

                //Genero el xml
                var xml = _fiscalReceipts.CreateCFDI(invoice, payment, config);

                //Timbro el xml
                xml = _fiscalReceipts.Timbrar(xml, config);

                //Si pudo timbrar entonces agrego el timbre al abono, ya que aún necesito generar el cbb
                var stamp = _fiscalReceipts.GetTimbreAbono(xml);
                //El abono del timbre complementa la informacion fiscal de TimbreDeAbonoDeFactura
                payment.TimbresDeAbonosDeFactura.version = stamp.version;
                payment.TimbresDeAbonosDeFactura.selloSAT = stamp.selloSAT;
                payment.TimbresDeAbonosDeFactura.selloCFD = stamp.selloCFD;
                payment.TimbresDeAbonosDeFactura.noCertificadoSAT = stamp.noCertificadoSAT;
                payment.TimbresDeAbonosDeFactura.UUID = stamp.UUID;
                payment.TimbresDeAbonosDeFactura.fechaTimbrado = stamp.fechaTimbrado;
                payment.TimbresDeAbonosDeFactura.Leyenda = stamp.Leyenda;
                payment.TimbresDeAbonosDeFactura.RfcProvCertif = stamp.RfcProvCertif;
                payment.TimbresDeAbonosDeFactura.cadenaOriginalAbono = stamp.cadenaOriginalAbono;

                //Guardo la factura actualizada
                _stamps.Add(payment.TimbresDeAbonosDeFactura);
                _UOW.Save();

                //Genero el xml
                xml.Save(string.Format("{0}\\{1}{2}.xml", config.CarpetaXml, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio));

                //Genero el cbb con el ViewModel
                _fiscalReceipts.CreateCBB(string.Format("{0}\\{1}{2}.bmp", config.CarpetaCbb, payment.TimbresDeAbonosDeFactura.serie, payment.TimbresDeAbonosDeFactura.folio), config.rfc, invoice.Cliente.rfc, payment.monto, payment.TimbresDeAbonosDeFactura.UUID);

                //Regreso la factura timbrada
                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaParcialidade> ListParcialidades()
        {
            try
            {
                return _paymentsList.List();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwListaParcialidade> ListParcialidadesLike(string value)
        {
            try
            {
                return _paymentsList.WithFolioOrClientLike(value);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura Find(string serie, int folio)
        {
            try
            {
                return _payments.FindParcialidad(serie, folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura LastParcialidad(string serie)
        {
            try
            {
                var folio = _folios.Last(serie, TipoDeComprobante.Parcialidad);
                if (!folio.isValid())
                    throw new Exception("La serie no es válida");

                return _payments.FindParcialidad(serie.ToUpper(), folio);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
