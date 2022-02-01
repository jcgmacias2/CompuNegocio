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
    public class NotasDeCreditoRepository : BaseRepository<NotasDeCredito>, INotasDeCreditoRepository
    {
        public NotasDeCreditoRepository(CNEntities context) : base(context) { }

        public NotasDeCredito Find(string serie, string folio)
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

        public List<NotasDeCredito> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeNotaDeCredito.Equals(idEstatus.Value)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeCredito> List(DateTime startDate, DateTime endDate)
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

        public List<NotasDeCredito> List(DateTime startDate, DateTime endDate, Cliente customer)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date && x.idCliente == customer.idCliente).OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeCredito> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(f => f.idEstatusDeNotaDeCredito.Equals(idEstatus) && (f.serie.Contains(value) || f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value))).ToList();
                else
                    return _dbSet.Where(f => f.serie.Contains(value) || f.folio.ToString().Contains(value) || f.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotasDeCredito> List(int idBusiness, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date && x.idEmpresa == idBusiness).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<NotasDeCredito> ListByInvoice(int idInvoice)
        {
            try
            {
                return _dbSet.Where(nc => nc.idFactura.HasValue && nc.idFactura.Value.Equals(idInvoice)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
