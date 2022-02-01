using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviCuentaGuardianService : CuentaGuardianService
    {
        public AproviCuentaGuardianService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
