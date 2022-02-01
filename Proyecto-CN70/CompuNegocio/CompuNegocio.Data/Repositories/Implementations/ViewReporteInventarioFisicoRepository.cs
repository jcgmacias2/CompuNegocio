using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteInventarioFisicoRepository : BaseRepository<VwReporteInventarioFisico>, IViewReporteInventarioFisicoRepository
    {
        public ViewReporteInventarioFisicoRepository(CNEntities context) : base(context) { }

        public List<VwReporteInventarioFisico> WithClassifications(int[] classificationIds)
        {
            try
            {
                List<VwReporteInventarioFisico> items;

                if (classificationIds.IsEmpty())
                {
                    items = _dbSet.AsNoTracking().GroupBy(i => i.idArticulo).Select(x => x.FirstOrDefault()).ToList();
                }
                else
                {
                    items = _dbSet.AsNoTracking().Where(x => classificationIds.Any(y=>x.idClasificacion == y) && x.inventariado).ToList();
                }

                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
