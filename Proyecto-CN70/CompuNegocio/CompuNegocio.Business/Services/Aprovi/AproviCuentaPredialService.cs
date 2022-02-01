using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviCuentaPredialService : CuentaPredialService
    {
        public AproviCuentaPredialService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
