using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviAbonoDeCompraService : AbonoDeCompraService
    {
        public AproviAbonoDeCompraService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
