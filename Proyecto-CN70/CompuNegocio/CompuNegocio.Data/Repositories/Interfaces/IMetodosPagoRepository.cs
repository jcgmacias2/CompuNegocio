using Aprovi.Data.Core;
using Aprovi.Data.Models;
namespace Aprovi.Data.Repositories
{
    public interface IMetodosPagoRepository : IBaseRepository<MetodosPago>
    {
        bool CanDelete(int idPaymentMethod);
    }
}
