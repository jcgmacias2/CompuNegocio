using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviCuentaBancariaService : CuentaBancariaService
    {
        public AproviCuentaBancariaService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
