using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Aprovi.Data.Core;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteEstatusDeLaEmpresaPedidosRepository : BaseRepository<VwReporteEstatusDeLaEmpresaPedido>, IViewReporteEstatusDeLaEmpresaPedidosRepository
    {
        public ViewReporteEstatusDeLaEmpresaPedidosRepository(CNEntities context) : base(context)
        {
        }

        public List<VwReporteEstatusDeLaEmpresaPedido> List(DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= start.Date && DbFunctions.TruncateTime(x.fechaHora) <= end.Date).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}