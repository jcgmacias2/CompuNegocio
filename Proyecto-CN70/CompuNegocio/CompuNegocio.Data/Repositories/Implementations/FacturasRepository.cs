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
    public class FacturasRepository : BaseRepository<Factura>, IFacturasRepository
    {
        public FacturasRepository(CNEntities context) : base(context) { }

        public Factura Find(string serie, string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(f => f.serie.Equals(serie, StringComparison.InvariantCultureIgnoreCase) && f.folio.ToString().Equals(folio, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Factura> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeFactura.Equals(idEstatus.Value)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Factura> List(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date).OrderBy(x=>x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Factura> ListBySeller(DateTime startDate, DateTime endDate, Usuario user)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date && x.Usuario1 != null && x.Usuario1.idUsuario == user.idUsuario).OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Factura> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeFactura.Equals(idEstatus) && (f.serie.Contains(value) || f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value))).ToList();
                else
                    return _dbSet.Where(f => f.serie.Contains(value) || f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
