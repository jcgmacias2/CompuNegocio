using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class SerieService : ISerieService
    {
        private IUnitOfWork _UOW;
        private ISeriesRepository _series;
        private ITiposDeComprobanteRepository _comprobantes;
        private IViewSeriesRepository _folios;

        public SerieService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _series = _UOW.Series;
            _comprobantes = _UOW.TiposDeComprobante;
            _folios = _UOW.Folios;
        }

        public Series Add(Series serie)
        {
            try
            {
                _series.Add(serie);
                _UOW.Save();
                return serie;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Series Find(char identifier)
        {
            try
            {
                return _series.Find(identifier.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Series Find(int idTipoDeComprobante)
        {
            try
            {
                var comprobante = _comprobantes.Find(idTipoDeComprobante);
                if (comprobante.idSerie.HasValue)
                    return _series.Find(comprobante.idSerie.Value);
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Series> List()
        {
            try
            {
                return _series.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Series Update(Series serie)
        {
            try
            {
                var local = _series.Find(serie.idSerie);
                local.folioInicial = serie.folioInicial;
                local.folioFinal = serie.folioFinal;
                _series.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Series serie, TiposDeComprobante comprobante)
        {
            try
            {
                var local = _comprobantes.Find(comprobante.idTipoDeComprobante);
                local.idSerie = serie.idSerie;
                _comprobantes.Update(local);

                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last(string serie, TipoDeComprobante tipo)
        {
            try
            {
                var data = _folios.Find(serie);
                int folio;
                switch (tipo)
                {
                    case TipoDeComprobante.Factura:
                        folio = data.ultimaFactura;
                        break;
                    case TipoDeComprobante.Parcialidad:
                        folio = data.ultimaParcialidad;
                        break;
                    case TipoDeComprobante.Nota_De_Credito:
                        folio = data.ultimaNotaDeCredito;
                        break;
                    default:
                        folio = data.ultimoFolio;
                        break;
                }

                return folio;
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
                return _folios.Find(serie).ultimoFolio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Next(string serie)
        {
            try
            {
                //Me aseguro de que este folio este autorizado
                var folio = _folios.Next(serie);
                var data = _folios.Find(serie);

                //Si el folio es menor al folio inicial, tomo folio inicial como folio
                //Esto sucede con los clientes que inician operaciones en este sistema empezando de un folio distinto al 1
                if (folio < data.folioInicial)
                    folio = data.folioInicial;

                if (folio < data.folioInicial || folio > data.folioFinal)
                    throw new Exception("No hay folios disponibles");
                return folio;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
