using Aprovi.Data.Core;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public class TiposRelacionRepository : BaseRepository<TiposRelacion>, ITiposRelacionRepository
    {
        public TiposRelacionRepository(CNEntities context) : base(context) { }
    }
}
