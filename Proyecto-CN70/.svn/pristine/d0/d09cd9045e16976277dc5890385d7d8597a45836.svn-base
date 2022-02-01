using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class MetodosDePagoRepository : BaseRepository<MetodosPago>, IMetodosPagoRepository
    {
        public MetodosDePagoRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idPaymentMethod)
        {
            try
            {
                var payment = _dbSet.FirstOrDefault(m => m.idMetodoPago.Equals(idPaymentMethod));

                if (payment.Facturas.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
