using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Aprovi.Data.Core;

namespace Aprovi.Data.Repositories
{
    public class EmpresasRepository : BaseRepository<Empresa>, IEmpresasRepository
    {
        public EmpresasRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idBusiness)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(c => c.idEmpresa.Equals(idBusiness));

                if (local.AbonosDeFacturas.Count > 0)
                    return false;

                if (local.AbonosDeRemisions.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Empresa Find(string description)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.descripcion.Equals(description, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Empresa> WithDescriptionLike(string description)
        {
            try
            {
                return _dbSet.Where(c => c.descripcion.Contains(description)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
