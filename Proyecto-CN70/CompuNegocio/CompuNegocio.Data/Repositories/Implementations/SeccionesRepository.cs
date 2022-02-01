using System.Linq;
using Aprovi.Data.Core;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public class SeccionesRepository : BaseRepository<Seccione>, ISeccionesRepository
    {
        public SeccionesRepository(CNEntities context) : base(context)
        {
        }
    }
}
