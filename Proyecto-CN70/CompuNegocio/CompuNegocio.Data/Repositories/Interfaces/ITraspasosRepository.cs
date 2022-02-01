using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Models;

namespace Aprovi.Data.Repositories
{
    public interface ITraspasosRepository : IBaseRepository<Traspaso>
    {
        int Next();

        int Last();

        Traspaso Find(int folio);

        Traspaso FindById(int id);

        Traspaso FindByIdForTransfer(int id);

        List<Traspaso> List(int? idEstatus);

        List<Traspaso> WithFolioOrCompanyLike(string value, int? idEstatus);

        List<Traspaso> WithStatus(StatusDeTraspaso status);

        void DeleteDetail(DetallesDeTraspaso detail);

        List<Traspaso> List(DateTime? startDate, DateTime? endDate, EmpresasAsociada originCompany, EmpresasAsociada destinationCompany);
    }
}
