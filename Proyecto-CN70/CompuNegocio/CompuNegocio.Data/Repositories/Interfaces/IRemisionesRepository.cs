using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IRemisionesRepository : IBaseRepository<Remisione>
    {
        int Next();

        int Last();

        Remisione Find(int folio);

        Remisione FindById(int id);

        List<Remisione> List(int? idEstatus);

        List<Remisione> WithFolioOrClientLike(string value, int? idEstatus);

        List<Remisione> List(DateTime fechaInicio, DateTime fechaFin, Tipos_Reporte_Remisiones filtro);

        List<Remisione> List(DateTime fechaInicio, DateTime fechaFin);

        List<Remisione> ListBySeller(DateTime fechaInicio, DateTime fechaFin, Usuario user);

        List<Remisione> ListByInvoice(int factura);

        void restoreRemision(List<Remisione> remisiones);

        void DeleteDetail(DetallesDeRemision detail);
    }
}
