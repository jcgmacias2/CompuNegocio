using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewFoliosPorAbonos : BaseRepository<VwFoliosPorAbono>, IViewFoliosPorAbonosRepository
    {
        public ViewFoliosPorAbonos(CNEntities context) : base(context) { }

        public string Next()
        {
            try
            {
                return (_dbSet.Max<VwFoliosPorAbono>(f => f.Folio + 1).ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
