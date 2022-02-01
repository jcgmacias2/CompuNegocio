using System.Linq;
using Aprovi.Data.Core;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public class DirectorioRepository : BaseRepository<Directorio>, IDirectorioRepository
    {
        public DirectorioRepository(CNEntities context) : base(context)
        {
        }

        public override IQueryable<Directorio> List()
        {
            return _dbSet.Where(x => x.activo);
        }
    }
}
