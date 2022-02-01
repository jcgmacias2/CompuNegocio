using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewListaPagosRepository : BaseRepository<VwListaPago>, IViewListaPagosRepository
    {

        public ViewListaPagosRepository(CNEntities context) : base(context) { }

        public List<VwListaPago> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                {
                    return _dbSet.Where(x => x.razonSocial.Contains(value) || (x.serie + x.folio.ToString()).Contains(value) && x.idEstatusDePago == idEstatus.Value).OrderByDescending(x => x.fechaHora).ToList();
                }
                else
                {
                    return _dbSet.Where(x => x.razonSocial.Contains(value) || (x.serie + x.folio.ToString()).Contains(value)).OrderByDescending(x => x.fechaHora).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
