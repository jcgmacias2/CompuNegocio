using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Data.Repositories
{
    public interface IUsosCFDIRepository : IBaseRepository<UsosCFDI>
    {
        new List<UsosCFDI> List();

        List<UsosCFDI> Like(string value);

        UsosCFDI Find(int id);

        UsosCFDI Find(string code);
    }
}
