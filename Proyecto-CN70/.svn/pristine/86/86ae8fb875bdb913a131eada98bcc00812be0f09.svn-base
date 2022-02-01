using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewListaParcialidadesRepository : IBaseRepository<VwListaParcialidade>
    {
        List<VwListaParcialidade> WithFolioOrClientLike(string value);

        List<VwListaParcialidade> List();

        VwListaParcialidade FindWithSerieAndFolio(string serie, int folio);
    }
}
