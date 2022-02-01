using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IEstacionesRepository : IBaseRepository<Estacione>
    {
        List<Estacione> List(int idRegister);

        Estacione Find(string description);

        Estacione HasStation(string computerCode);

        List<Estacione> WithDescriptionLike(string description);
    }
}
