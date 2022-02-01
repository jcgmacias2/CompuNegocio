using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviEnvioDeCorreoService : EnvioDeCorreoService
    {

        public AproviEnvioDeCorreoService(IUnitOfWork unitOfWork, IConfiguracionService config) : base(unitOfWork, config)
        {

        }
    }
}
