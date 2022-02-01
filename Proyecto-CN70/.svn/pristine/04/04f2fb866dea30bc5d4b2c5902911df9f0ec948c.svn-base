using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteAvaluoRepository : BaseRepository<VwReporteAvaluo>, IViewReporteAvaluoRepository
    {
        public ViewReporteAvaluoRepository(CNEntities context) : base(context) { }

        public List<VwReporteAvaluo> List(FiltroReporteAvaluo filter, DateTime endDate, Clasificacione classification)
        {
            try
            {
                var data = _dbSet.AsNoTracking().AsQueryable();

                switch (filter)
                {
                    case FiltroReporteAvaluo.Clasificacion:
                        return _dbSet.SqlQuery(string.Format("SELECT RA.* FROM dbo.VwReporteAvaluo AS RA JOIN dbo.ClasificacionesPorArticulo AS CPA ON RA.idArticulo = CPA.idArticulo AND CPA.idClasificacion = {0}", classification.idClasificacion)).AsNoTracking().ToList();
                    break;
                    case FiltroReporteAvaluo.Articulos_Nacionales:
                        data = data.Where(x => !x.importado);
                    break;
                    case FiltroReporteAvaluo.Articulos_Extranjeros:
                        data = data.Where(x => x.importado);
                    break;
                }

                data = data.Where(x => x.fechaHora.HasValue && DbFunctions.TruncateTime(x.fechaHora.Value) < endDate.Date);

                return data.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
