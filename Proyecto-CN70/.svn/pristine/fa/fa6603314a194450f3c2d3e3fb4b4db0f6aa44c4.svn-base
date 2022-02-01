using System;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IPagosRepository : IBaseRepository<Pago>
    {
        Pago Find(string serie, string folio);

        List<Pago> List(int? idEstatus);

        List<Pago> List(DateTime fechaInicio, DateTime fechaFin);

        List<Pago> WithFolioOrClientLike(string value, int? idEstatus);
    }
}
