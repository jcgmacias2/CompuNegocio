using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviNotaDeDescuentoService : NotaDeDescuentoService
    {
        public AproviNotaDeDescuentoService(IUnitOfWork unitOfWork, IConfiguracionService config, IClienteService clients, IArticuloService items) : base(unitOfWork, config, clients, items)
        {
        }
    }
}
