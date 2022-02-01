using System;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IFacturasRepository : IBaseRepository<Factura>
    {
        Factura Find(string serie, string folio);

        List<Factura> List(int? idEstatus);

        List<Factura> List(DateTime fechaInicio, DateTime fechaFin);

        List<Factura> ListBySeller(DateTime fechaInicio, DateTime fechaFin, Usuario user);

        List<Factura> WithFolioOrClientLike(string value, int? idEstatus);
    }
}
