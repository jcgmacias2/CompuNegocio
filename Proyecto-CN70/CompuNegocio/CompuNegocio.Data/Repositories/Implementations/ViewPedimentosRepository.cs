using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewPedimentosRepository : BaseRepository<VwExistenciasConPedimento>, IViewPedimentosRepository
    {
        public ViewPedimentosRepository(CNEntities context) : base(context) { }

        public VwExistenciasConPedimento Find(int idArticulo, int idPedimento)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.idPedimento.Equals(idPedimento) && p.idArticulo.Equals(idArticulo));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwExistenciasConPedimento> List(int idArticulo)
        {
            try
            {
                return _dbSet.Where(p => p.idArticulo.Equals(idArticulo) && p.existencia.Value > 0.0m).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
