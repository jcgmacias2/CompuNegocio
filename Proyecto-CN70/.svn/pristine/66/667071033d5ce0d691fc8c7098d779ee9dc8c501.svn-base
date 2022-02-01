using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IAbonosDeFacturaRepository : IBaseRepository<AbonosDeFactura>
    {
        AbonosDeFactura Find(string folio);

        AbonosDeFactura FindParcialidad(string serie, int folio);

        List<AbonosDeFactura> List(int idInvoice);

        List<AbonosDeFactura> ListParcialidades();

        List<AbonosDeFactura> ParcialidadesWithFolioOrClientLike(string value);

        List<AbonosDeFactura> List(int idRegister, DateTime start, DateTime end);

        List<AbonosDeFactura> List(DateTime start, DateTime end);
    }
}
