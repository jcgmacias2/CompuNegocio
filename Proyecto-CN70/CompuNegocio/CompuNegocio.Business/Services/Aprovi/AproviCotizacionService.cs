using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviCotizacionService : CotizacionService
    {
        public AproviCotizacionService(IUnitOfWork unitOfWork, IConfiguracionService config) : base(unitOfWork, config)
        {
        }
    }
}
