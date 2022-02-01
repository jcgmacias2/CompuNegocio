using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class RegimenesRepository : BaseRepository<Regimene>, IRegimenesRepository
    {
        public RegimenesRepository(CNEntities context) : base(context) { }

        public Regimene Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(r => r.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
