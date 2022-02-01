using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IBancoService 
    {
        Banco Add(Banco banco);

        Banco Find(string nombre);

        Banco Find(int idBanco);

        bool CanDelete(int idBanco);

        void Delete(Banco banco);

        List<Banco> List();

        List<Banco> List(string value);
    }
}
