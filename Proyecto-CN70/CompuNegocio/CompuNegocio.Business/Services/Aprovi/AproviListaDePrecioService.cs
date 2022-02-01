using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviListaDePrecioService : ListaDePrecioService
    {
        public AproviListaDePrecioService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
