using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ImpuestoPorFacturaRepository : BaseRepository<ImpuestoPorFactura>, IImpuestoPorFacturaRepository
    {
        public ImpuestoPorFacturaRepository(CNEntities context) : base(context) { }
    }
}
