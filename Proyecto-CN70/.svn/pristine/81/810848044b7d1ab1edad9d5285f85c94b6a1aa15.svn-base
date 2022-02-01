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
    public class ViewReporteAntiguedadSaldosRemisionesRepository : BaseRepository<VwReporteAntiguedadSaldosRemisione>, IViewReporteAntiguedadSaldosRemisionesRepository
    {
        public ViewReporteAntiguedadSaldosRemisionesRepository(CNEntities context) : base(context) { }

        public List<VwReporteAntiguedadSaldosRemisione> List(Cliente customer, Usuario seller, bool onlyExpired, DateTime to)
        {
            try
            {
                //Solo muestra documentos con deuda
                var data = _dbSet.Where(x=>(x.subtotal + x.impuestos - x.abonado) > 0m);

                //Solo muestra documentos vigentes y activos, no cancelados ni facturados
                data = data.Where(x => x.idEstatusDeRemision == (int)StatusDeRemision.Registrada);

                //Se filtra por cliente
                if (customer.isValid()  && customer.idCliente.isValid())
                {
                    data = data.Where(x => x.idCliente == customer.idCliente);
                }

                //Se filtra por vendedor
                if (seller.isValid() && seller.idUsuario.isValid())
                {
                    data = data.Where(x => x.idUsuario.HasValue && x.idUsuario.Value == seller.idUsuario);
                }

                //Se filtra por solo expirados
                if (onlyExpired)
                {
                    data = data.Where(x=>DbFunctions.TruncateTime(x.fechaHora) < DbFunctions.AddDays(to,-x.diasCredito));
                }

                return data.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
