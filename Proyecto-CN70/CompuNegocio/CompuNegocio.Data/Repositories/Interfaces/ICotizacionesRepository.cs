using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface ICotizacionesRepository : IBaseRepository<Cotizacione>
    {
        int Next();

        int Last();

        Cotizacione Find(int folio);

        Cotizacione FindById(int id);

        List<Cotizacione> List(int? idEstatus);

        List<Cotizacione> WithFolioOrClientLike(string value, int? idEstatus);

        void DeleteDetail(DetallesDeCotizacion detail);
    }
}
