using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class SolicitudesDeTraspasoRepository : BaseRepository<SolicitudesDeTraspaso>, ISolicitudesDeTraspasoRepository
    {
        public SolicitudesDeTraspasoRepository(CNEntities context) : base(context) { }

        public int Next()
        {
            try
            {
                var solicitudDeTraspaso = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if(solicitudDeTraspaso.isValid())
                    return solicitudDeTraspaso.folio + 1;
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
                var solicitudDeTraspaso = _dbSet.OrderByDescending(r => r.folio).FirstOrDefault();
                if (solicitudDeTraspaso.isValid())
                    return solicitudDeTraspaso.folio;
                else
                    return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SolicitudesDeTraspaso Find(int folio)
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

        public SolicitudesDeTraspaso FindById(int idEmpresaAsociada, int idTraspaso)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.idTraspaso.Equals(idTraspaso) && r.idEmpresaAsociadaOrigen == idEmpresaAsociada);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SolicitudesDeTraspaso> List()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SolicitudesDeTraspaso> WithFolioOrCompanyLike(string value)
        {
            try
            {
                return _dbSet.Where(r => r.folio.ToString().Contains(value) || r.EmpresasAsociada.nombre.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
