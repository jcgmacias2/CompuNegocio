using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviSolicitudDeTraspasoService : SolicitudDeTraspasoService
    {
        public AproviSolicitudDeTraspasoService(IUnitOfWork unitOfWork, IEmpresaAsociadaService associatedCompanies) : base(unitOfWork, associatedCompanies)
        {
        }
    }
}
