using Aprovi.Data.Models;
using System.Collections.Generic;

namespace Aprovi.Business.Services
{
    public interface IComprobanteEnviadoService
    {
        ComprobantesEnviado Find(int id);

        List<ComprobantesEnviado> List(bool onlyPending);
    }
}
