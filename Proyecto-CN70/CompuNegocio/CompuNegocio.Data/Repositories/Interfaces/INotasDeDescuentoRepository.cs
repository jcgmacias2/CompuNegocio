using System;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface INotasDeDescuentoRepository : IBaseRepository<NotasDeDescuento>
    {
        int Next();

        int Last();

        NotasDeDescuento FindByFolio(int folio);

        List<NotasDeDescuento> List(int? idEstatus);

        List<NotasDeDescuento> WithFolioOrClientLike(string value, int? idEstatus);
    }
}
