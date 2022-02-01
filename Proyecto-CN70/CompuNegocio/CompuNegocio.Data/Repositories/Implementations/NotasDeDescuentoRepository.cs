using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class NotasDeDescuentoRepository : BaseRepository<NotasDeDescuento>, INotasDeDescuentoRepository
    {
        public NotasDeDescuentoRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var notaDeDescuento = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (notaDeDescuento.isValid())
                    return notaDeDescuento.folio + 1;
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
                var notaDeDescuento = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (notaDeDescuento.isValid())
                    return notaDeDescuento.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotasDeDescuento FindByFolio(int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(f => f.folio == folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeDescuento> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeNotaDeDescuento.Equals(idEstatus.Value)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeDescuento> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeNotaDeDescuento.Equals(idEstatus) && (f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value))).ToList();
                else
                    return _dbSet.Where(f => f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
