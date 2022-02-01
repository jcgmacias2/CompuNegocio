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
    public abstract class CotizacionService : ICotizacionService
    {
        private IUnitOfWork _UOW;
        private ICotizacionesRepository _quotes;
        private IConfiguracionService _config;
        private IClientesRepository _clients;
        private IArticulosRepository _items;
        private IViewCotizacionesPorPeriodoRepository _quotesPerPeriod;
        
        public CotizacionService(IUnitOfWork unitOfWork,  IConfiguracionService config)
        {
            _UOW = unitOfWork;
            _quotes = _UOW.Cotizaciones;
            _config = config;
            _clients = _UOW.Clientes;
            _items = _UOW.Articulos;
            _quotesPerPeriod = _UOW.CotizacionesPorPeriodo;
        }

        public int Next()
        {
            try
            {
                return _quotes.Next();
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
                return _quotes.Last();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual Cotizacione Add(VMCotizacion quote)
        {
            try
            {
                //Obtengo una instancia de la configuración
                var config = _config.GetDefault();

                //Le asigno la empresa configurada
                quote.idEmpresa = config.Estacion.idEmpresa;

                //Antes de registrarla obtengo nuevamente el folio, por si acaso ya se utilizo mientras agregaba los artículos
                quote.folio = _quotes.Next();

                //Le agrego estado
                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Registrada;

                //Guardo la remisión
                var local = quote.ToCotizacion(_items);

                //Defaults de remisión
                local.Cliente = null;
                local.Usuario = null;
                local.Factura = null;
                local.Remisione = null;

                //Ahora si guardo
                _quotes.Add(local);
                _UOW.Save();

                quote.idCotizacion = local.idCotizacion;

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Update(VMCotizacion quote)
        {
            try
            {
                var newQuote = quote.ToCotizacion(_items);

                var dbQuote = _quotes.FindById(quote.idCotizacion);

                dbQuote.fechaHora = quote.fechaHora;
                dbQuote.tipoDeCambio = quote.tipoDeCambio;
                dbQuote.numeroDePedido = quote.numeroDePedido;
                dbQuote.idCliente = quote.idCliente;
                dbQuote.idUsuarioRegistro = quote.idUsuarioRegistro;
                dbQuote.idEstatusDeCotizacion = quote.idEstatusDeCotizacion;
                dbQuote.idEmpresa = quote.idEmpresa;
                dbQuote.idFactura = quote.idFactura;
                dbQuote.idRemision = quote.idRemision;
                dbQuote.CancelacionesDeCotizacione = quote.CancelacionesDeCotizacione;
                dbQuote.DatosExtraPorCotizacions = quote.DatosExtraPorCotizacions;

                //Elimino el detalle actual
                dbQuote.DetallesDeCotizacions.ToList().ForEach(d => _quotes.DeleteDetail(d));

                //Agrego el nuevo
                dbQuote.DetallesDeCotizacions = newQuote.DetallesDeCotizacions;

                //Ahora si se puede actualizar el idMoneda de la cotizacion
                dbQuote.idMoneda = quote.idMoneda;

                _quotes.Update(dbQuote);
                _UOW.Save();

                return dbQuote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Find(int idQuote)
        {
            try
            {
                //Este debe pasarse como objecto
                return _quotes.Find((object)idQuote);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione FindByFolio(int folio)
        {
            try
            {
                return _quotes.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cotizacione> List()
        {
            try
            {
                return _quotes.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cotizacione> WithFolioOrClientLike(string value)
        {
            try
            {
                return _quotes.WithFolioOrClientLike(value, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Cancel(int idQuote, string reason)
        {
            try
            {
                var quote = _quotes.FindById(idQuote);
                //No puedo cancelar una cotizacion facturada
                if (quote.idFactura.isValid())
                    throw new Exception("No es posible cancelar una cotización facturada");

                //No se puede cancelar una cotizacion remisionada
                if (quote.idRemision.isValid())
                {
                    throw new Exception("No es posible cancelar una cotización remisionada");
                }

                //No se puede cancelar una cotizacion cancelada
                if (quote.idEstatusDeCotizacion == (int) StatusDeCotizacion.Cancelada)
                {
                    throw new Exception("No es posible cancelar una cotización cancelada");
                }

                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Cancelada;
                quote.EstatusDeCotizacion = null;
                quote.CancelacionesDeCotizacione = new CancelacionesDeCotizacione();
                quote.CancelacionesDeCotizacione.fechaHora = DateTime.Now;
                quote.CancelacionesDeCotizacione.motivo = reason;

                _quotes.Update(quote);
                _UOW.Save();

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Invoiced(int idQuote, int idInvoice)
        {
            try
            {
                var quote = _quotes.FindById(idQuote);
                if (quote.idFactura.isValid())
                    throw new Exception("Esta cotización ya fue facturada");

                if (quote.idRemision.isValid())
                {
                    throw new Exception("Esta cotización ya fue remisionada");
                }

                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Facturada;
                quote.idFactura = idInvoice;
                _UOW.Save();

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Remitted(int idQuote, int idRemision)
        {
            try
            {
                var quote = _quotes.FindById(idQuote);
                if (quote.idFactura.isValid())
                    throw new Exception("Esta cotización ya fue facturada");

                if (quote.idRemision.isValid())
                {
                    throw new Exception("Esta cotización ya fue remisionada");
                }

                quote.idEstatusDeCotizacion = (int)StatusDeCotizacion.Remisionada;
                quote.idRemision = idRemision;
                _UOW.Save();

                return quote;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleDeCotizacion> GetDetailsForReport(DateTime from, DateTime to, bool onlySalePending)
        {
            try
            {
                return _quotesPerPeriod.ListByPeriod(from, to, onlySalePending).Select(x=> new VMRDetalleDeCotizacion(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleDeCotizacion> GetDetailsForReport(DateTime from, DateTime to, Cliente customer, bool onlySalePending)
        {
            try
            {
                return _quotesPerPeriod.ListByPeriodAndCustomer(from, to, customer, onlySalePending).Select(x => new VMRDetalleDeCotizacion(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Unlink(Cotizacione quote)
        {
            try
            {
                var dbQuote = _quotes.FindById(quote.idCotizacion);

                dbQuote.idFactura = null;
                dbQuote.idRemision = null;
                dbQuote.idEstatusDeCotizacion = (int) StatusDeCotizacion.Registrada;
                dbQuote.Factura = null;
                dbQuote.Remisione = null;

                _quotes.Update(dbQuote);
                _UOW.Save();

                return dbQuote;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
