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
    public class RemisionesRepository : BaseRepository<Remisione>, IRemisionesRepository
    {
        public RemisionesRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var remision = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(remision.isValid())
                    return remision.folio + 1;
                else
                    return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Last()
        {
            try
            {
                var remision = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (remision.isValid())
                    return remision.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Remisione Find(int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.folio.Equals(folio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Remisione FindById(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idRemision.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeRemision.Equals(idEstatus)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> WithFolioOrClientLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeRemision.Equals(idEstatus)).Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
                else
                    return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.Cliente.razonSocial.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> List(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= fechaInicio.Date && DbFunctions.TruncateTime(x.fechaHora) <= fechaFin.Date).OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> ListBySeller(DateTime fechaInicio, DateTime fechaFin, Usuario user)
        {
            try
            {
                return _dbSet.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= fechaInicio.Date && DbFunctions.TruncateTime(x.fechaHora) <= fechaFin.Date && x.Usuario1 != null && x.Usuario1.idUsuario == user.idUsuario).OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDeRemision detail)
        {
            try
            {
                var detailDb = _context.DetallesDeRemisions.Find(detail.idDetalleDeRemision);
                detailDb.Impuestos = null;
                _context.DetallesDeRemisions.Remove(detailDb);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> List(DateTime fechaInicio, DateTime fechaFin, Tipos_Reporte_Remisiones filtro)
        {
            try
            {
                List<Remisione> remisiones;

                if (filtro == Tipos_Reporte_Remisiones.Todas)
                {
                    remisiones = _dbSet.Where(x => x.fechaHora >= fechaInicio && x.fechaHora <= fechaFin).ToList();
                }
                else
                {
                    remisiones = _dbSet.Where(x => x.fechaHora >= fechaInicio && x.fechaHora <= fechaFin && x.idEstatusDeRemision == (int)filtro).ToList();
                }

                return remisiones;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Remisione> ListByInvoice(int factura)
        {
            try
            {
                return _dbSet.Where(r => r.idFactura == factura).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void restoreRemision(List<Remisione> remisiones)
        {
            try
            {
                foreach (Remisione rem in remisiones)
                {
                    var reg = _context.Remisiones.SingleOrDefault(r => r.idRemision.Equals(rem.idRemision));
                    reg.idFactura = null;
                    reg.idEstatusDeRemision = (int)StatusDeRemision.Registrada;
                }

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
