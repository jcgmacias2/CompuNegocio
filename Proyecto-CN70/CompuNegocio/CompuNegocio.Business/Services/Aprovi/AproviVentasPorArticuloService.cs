using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviVentasPorArticuloService : VentasPorArticuloService
    {
        public AproviVentasPorArticuloService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
