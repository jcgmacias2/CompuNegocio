using System;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface INotasDeCreditoRepository : IBaseRepository<NotasDeCredito>
    {
        NotasDeCredito Find(string serie, string folio);

        List<NotasDeCredito> List(int? idEstatus);

        List<NotasDeCredito> List(DateTime fechaInicio, DateTime fechaFin);

        List<NotasDeCredito> List(DateTime fechaInicio, DateTime fechaFin, Cliente customer);

        List<NotasDeCredito> WithFolioOrClientLike(string value, int? idEstatus);

        List<NotasDeCredito> List(int idBusiness, DateTime start, DateTime end);

        List<NotasDeCredito> ListByInvoice(int idInvoice);
    }
}
