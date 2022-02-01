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
    public class ViewReporteAntiguedadSaldosFacturas : BaseRepository<VwReporteAntiguedadSaldosFactura>, IViewReporteAntiguedadSaldosFacturasRepository
    {
        public ViewReporteAntiguedadSaldosFacturas(CNEntities context) : base(context) { }

        public List<VwReporteAntiguedadSaldosFactura> List(Cliente customer, Usuario seller, bool onlyExpired, DateTime to)
        {
            try
            {
                //Solo muestra documentos con deuda
                //var data = _dbSet.Where(x => (x.subtotal.Value + x.impuestos.Value - x.abonado) > 0.0m);
                var data = _dbSet.SqlQuery("SELECT Vw.* FROM VwReporteAntiguedadSaldosFacturas AS Vw WHERE ROUND(Vw.subtotal, 2) + ROUND(Vw.impuestos, 2) > Vw.abonado").ToList();

                //Solo muestra documentos vigentes
                data = data.Where(x=> x.idEstatusDeFactura != (int)StatusDeFactura.Anulada && x.idEstatusDeFactura != (int)StatusDeFactura.Cancelada).ToList();

                //Se filtra por cliente
                if (customer.isValid() && customer.idCliente.isValid())
                {
                    data = data.Where(x => x.idCliente == customer.idCliente).ToList();
                }

                //Se filtra por vendedor
                if (seller.isValid() && seller.idUsuario.isValid())
                {
                    data = data.Where(x => x.idUsuario.HasValue && x.idUsuario.Value == seller.idUsuario).ToList();
                }

                //Se filtra por solo expirados
                if (onlyExpired)
                {
                    data = data.Where(x => DbFunctions.TruncateTime(x.fechaHora) < DbFunctions.AddDays(to, -x.diasCredito)).ToList();
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
