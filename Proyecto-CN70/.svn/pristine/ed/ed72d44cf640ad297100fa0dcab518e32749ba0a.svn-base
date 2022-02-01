using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class FormasPagoRepository : BaseRepository<FormasPago>, IFormasPagoRepository
    {
        public FormasPagoRepository(CNEntities context) : base(context) { }

        public FormasPago Find(string description)
        {
            try
            {
                return _dbSet.FirstOrDefault(f => f.descripcion.Equals(description, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FormasPago> WithDescriptionLike(string description)
        {
            try
            {
                return _dbSet.Where(f => f.descripcion.Contains(description)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
