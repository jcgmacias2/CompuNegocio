using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class PeriodicidadRepository : BaseRepository<Periodicidad>, IPeriodicidadRepository
    {
        public PeriodicidadRepository(CNEntities context) : base(context) { }
    }
}
