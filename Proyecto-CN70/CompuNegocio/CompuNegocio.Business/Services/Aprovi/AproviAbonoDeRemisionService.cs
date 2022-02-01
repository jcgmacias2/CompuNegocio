using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviAbonoDeRemisionService : AbonoDeRemisionService
    {
        public AproviAbonoDeRemisionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
