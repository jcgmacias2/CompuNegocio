using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class AbonosDeCompraRepository : BaseRepository<AbonosDeCompra>, IAbonosDeCompraRepository
    {
        public AbonosDeCompraRepository(CNEntities context) : base(context) { }

        public AbonosDeCompra Find(string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.folio.Equals(folio, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string NextFolio()
        {
            try
            {
                if (_dbSet.Count().Equals(0))
                    return "1";
                else
                    return (_dbSet.Max<AbonosDeCompra>(a => a.folio.ToInt()) + 1).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeCompra> List(int idPurchase)
        {
            try
            {
                return _dbSet.Where(a => a.idCompra.Equals(idPurchase)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
