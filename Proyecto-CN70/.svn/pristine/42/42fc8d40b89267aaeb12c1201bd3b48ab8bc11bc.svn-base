using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ConfiguracionesRepository : BaseRepository<Configuracione>, IConfiguracionesRepository
    {
        public ConfiguracionesRepository(CNEntities context) : base(context) { }

        public Configuracione GetDefault()
        {
            try
            {
                return _dbSet.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
