using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class AjustesRepository : BaseRepository<Ajuste>, IAjustesRepository
    {
        public AjustesRepository(CNEntities context) : base(context) { }

        public Ajuste Find(string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.folio.Equals(folio, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> WithFolioLike(string value)
        {
            try
            {
                return _dbSet.Where(a => a.folio.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> List(TiposDeAjuste type)
        {
            try
            {
                return _dbSet.Where(a => a.idTipoDeAjuste.Equals(type.idTipoDeAjuste)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Next()
        {
            try
            {
                var adjustment = _dbSet.OrderByDescending(r => r.idAjuste).FirstOrDefault();
                if(adjustment.isValid())
                    return (adjustment.folio.ToInt() + 1).ToString();
                else
                    return "1";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Ajuste> ListByPeriodAndType(DateTime start, DateTime end, TiposDeAjuste type)
        {
            try
            {
                start = start.ToLastMidnight();
                end = end.ToNextMidnight();

                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end && a.idTipoDeAjuste.Equals(type.idTipoDeAjuste)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
