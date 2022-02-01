using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviUsuarioService : UsuarioService
    {
        public AproviUsuarioService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
