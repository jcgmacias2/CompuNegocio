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
    public class TraspasosRepository : BaseRepository<Traspaso>, ITraspasosRepository
    {
        public TraspasosRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var traspaso = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(traspaso.isValid())
                    return traspaso.folio + 1;
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
                var traspaso = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (traspaso.isValid())
                    return traspaso.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso Find(int folio)
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

        public Traspaso FindById(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idTraspaso.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Traspaso FindByIdForTransfer(int id)
        {
            try
            {
                return _dbSet.Include("EstatusDeTraspaso")
                             .Include("DetallesDeTraspasoes")
                             .Include("DetallesDeTraspasoes.Articulo")
                             .Include("DetallesDeTraspasoes.PedimentoPorDetalleDeTraspasoes")
                             .Include("DetallesDeTraspasoes.PedimentoPorDetalleDeTraspasoes.Pedimento")
                             .Include("EmpresasAsociada")
                             .Include("EmpresasAsociada1").FirstOrDefault(x => x.idTraspaso.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Traspaso> List(int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeTraspaso.Equals(idEstatus)).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Traspaso> WithFolioOrCompanyLike(string value, int? idEstatus)
        {
            try
            {
                if (idEstatus.HasValue)
                    return _dbSet.Where(r => r.idEstatusDeTraspaso.Equals(idEstatus)).Where(r => r.folio.ToString().Contains(value) || r.EmpresasAsociada.nombre.Contains(value) || r.EmpresasAsociada1.nombre.Contains(value)).ToList();
                else
                    return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.EmpresasAsociada.nombre.Contains(value) || r.EmpresasAsociada1.nombre.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Traspaso> WithStatus(StatusDeTraspaso status)
        {
            try
            {
                return _dbSet.Where(x => x.idEstatusDeTraspaso == (int) status).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDetail(DetallesDeTraspaso detail)
        {
            var detailDb = _context.DetallesDeTraspasoes.Find(detail.idDetalleDeTraspaso);
            _context.DetallesDeTraspasoes.Remove(detailDb);
        }

        public List<Traspaso> List(DateTime? startDate, DateTime? endDate, EmpresasAsociada originCompany, EmpresasAsociada destinationCompany)
        {
            try
            {
                var transfers = _dbSet.AsQueryable();

                if (startDate.HasValue)
                {
                    transfers = transfers.Where(x => DbFunctions.TruncateTime(x.fechaHora) >= DbFunctions.TruncateTime(startDate));
                }

                if (endDate.HasValue)
                {
                    transfers = transfers.Where(x => DbFunctions.TruncateTime(x.fechaHora) <= DbFunctions.TruncateTime(endDate));    
                }

                if (originCompany.isValid() && originCompany.idEmpresaAsociada.isValid())
                {
                    transfers = transfers.Where(x => x.idEmpresaAsociadaOrigen == originCompany.idEmpresaAsociada);
                }

                if (destinationCompany.isValid() && destinationCompany.idEmpresaAsociada.isValid())
                {
                    transfers = transfers.Where(x=>x.idEmpresaAsociadaDestino == destinationCompany.idEmpresaAsociada);
                }

                return transfers.ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
