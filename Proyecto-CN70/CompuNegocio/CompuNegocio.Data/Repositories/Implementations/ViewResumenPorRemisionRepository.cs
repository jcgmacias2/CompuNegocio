using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewResumenPorRemisionRepository : BaseRepository<VwResumenPorRemision>, IViewResumenPorRemisionRepository
    {
        public ViewResumenPorRemisionRepository(CNEntities context) : base(context) { }

        public List<VwResumenPorRemision> ActiveWithFolioOrClientLike(string value)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT VwRPR.* FROM VwResumenPorRemision AS VwRPR WHERE idEstatusDeRemision = 1 AND (folio like '%{0}%' OR codigo like '{0}%' OR nombreComercial like '{0}%' OR razonSocial like '{0}%')", value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwResumenPorRemision> ActiveWithDateLike(DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(r => r.idEstatusDeRemision.Equals((int)StatusDeRemision.Registrada) && r.fechaHora >= start && r.fechaHora <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwResumenPorRemision> List(int idCliente, DateTime inicio, DateTime fin, bool soloConDeuda)
        {
            try
            {
                inicio = inicio.ToLastMidnight();
                fin = fin.ToNextMidnight();

                //Por regla no deben aparecer documentos facturados o cancelados, solo vigentes
                if (soloConDeuda)
                    return _dbSet.Where(r => r.idCliente.Equals(idCliente) && r.fechaHora >= inicio && r.fechaHora <= fin && r.idEstatusDeRemision == (int)StatusDeRemision.Registrada && r.abonado.Value < r.subtotal.Value + r.impuestos.Value).ToList();
                else
                    return _dbSet.Where(r => r.idCliente.Equals(idCliente) && r.fechaHora >= inicio && r.fechaHora <= fin && r.idEstatusDeRemision == (int)StatusDeRemision.Registrada).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwResumenPorRemision> ListActive()
        {
            try
            {
                return _dbSet.Where(r => r.idEstatusDeRemision.Equals((int)StatusDeRemision.Registrada)).Take(100).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
