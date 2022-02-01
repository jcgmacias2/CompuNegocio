using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviEmpresaService : EmpresaService
    {
        public AproviEmpresaService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
