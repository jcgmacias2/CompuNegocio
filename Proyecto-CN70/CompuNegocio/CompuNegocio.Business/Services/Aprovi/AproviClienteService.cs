using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviClienteService : ClienteService
    {
        public AproviClienteService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
