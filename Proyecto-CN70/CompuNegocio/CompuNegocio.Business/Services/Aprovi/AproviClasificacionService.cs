using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviClasificacionService : ClasificacionService
    {
        public AproviClasificacionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
