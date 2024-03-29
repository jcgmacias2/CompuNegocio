﻿using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class CotizacionesRepository : BaseRepository<Cotizacione>, ICotizacionesRepository
    {
        public CotizacionesRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var cotizacion = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(cotizacion.isValid())
                    return cotizacion.folio + 1;
                else
                    return 1;
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
                var cotizacion = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (cotizacion.isValid())
                    return cotizacion.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione Find(int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.folio.Equals(folio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cotizacione FindById(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idCotizacion.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cotizacione> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeCotizacion.Equals(idEstatus)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Cotizacione> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeCotizacion.Equals(idEstatus)).Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
                else
                    return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDeCotizacion detail)
        {
            try
            {
                var detailDb = _context.DetallesDeCotizacions.Find(detail.idDetalleDeCotizacion);
                detailDb.Impuestos = null;
                _context.DetallesDeCotizacions.Remove(detailDb);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
