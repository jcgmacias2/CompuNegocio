using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ComprobantesEnviadosRepository : BaseRepository<ComprobantesEnviado>, IComprobantesEnviadosRepository
    {

        public ComprobantesEnviadosRepository(CNEntities context) : base(context) { }

        public List<ComprobantesEnviado> List(bool onlyPending)
        {
            try
            {
                if (onlyPending)
                    return _dbSet.Where(c => !c.pdf && !c.xml).ToList();
                else
                    return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
