using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ClasificacionesRepository : BaseRepository<Clasificacione>, IClasificacionesRepository
    {
        public ClasificacionesRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idClasification)
        {
            try
            {
                var clasification = _dbSet.FirstOrDefault(c => c.idClasificacion.Equals(idClasification));

                return clasification.Articulos.Count == 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Clasificacione Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.descripcion.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Clasificacione> WithNameLike(string name)
        {
            try
            {
                return _dbSet.Where(c => c.descripcion.Contains(name)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
