using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public interface ISolicitudesDeTraspasoRepository : IBaseRepository<SolicitudesDeTraspaso>
    {
        int Next();

        int Last();

        SolicitudesDeTraspaso Find(int folio);

        SolicitudesDeTraspaso FindById(int idEmpresaAsociada, int idTraspaso);

        List<SolicitudesDeTraspaso> List();

        List<SolicitudesDeTraspaso> WithFolioOrCompanyLike(string value);
    }
}
