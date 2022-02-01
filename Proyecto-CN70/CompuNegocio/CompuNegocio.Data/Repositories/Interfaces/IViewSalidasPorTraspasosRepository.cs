using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IViewSalidasPorTraspasosRepository : IBaseRepository<VwSalidasPorTraspaso>
    {
        List<VwSalidasPorTraspaso> List(int idItem, DateTime start, DateTime end);

        decimal GetTotal(int idItem, DateTime start, DateTime end);
    }
}