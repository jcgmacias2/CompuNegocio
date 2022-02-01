using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewListaFacturasRepository : BaseRepository<VwListaFactura>, IViewListaFacturasRepository
    {
        public ViewListaFacturasRepository(CNEntities context) : base(context) { }

        public List<VwListaFactura> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                {
                    return _dbSet.Where(x => x.razonSocial.Contains(value) || (x.serie + x.folio.ToString()).Contains(value) && x.idEstatusDeFactura == idEstatus.Value).OrderByDescending(x => x.fechaHora).ToList();
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
