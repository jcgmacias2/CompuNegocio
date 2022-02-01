using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class SeriesRepository : BaseRepository<Series>, ISeriesRepository
    {
        public SeriesRepository(CNEntities context) : base(context) { }

        public Series Find(string identificador)
        {
            try
            {
                return _dbSet.FirstOrDefault(s => s.identificador.Equals(identificador));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
