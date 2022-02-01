using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aprovi.Data.Core;

namespace Aprovi.Data.Repositories
{
    public class EquivalenciasRepository : BaseRepository<Equivalencia>, IEquivalenciasRepository
    {
        public EquivalenciasRepository(CNEntities context) : base(context) { }
        
        public Equivalencia Find(int idItem, int idUnitOfMeasure)
        {
            try
            {
                return _dbSet.FirstOrDefault(e => e.idArticulo.Equals(idItem) && e.idUnidadDeMedida.Equals(idUnitOfMeasure));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Equivalencia> List(int idItem)
        {
            try
            {
                return _dbSet.Where(e => e.idArticulo.Equals(idItem)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool HasOperations(int idEquivalence)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(e => e.idEquivalencia.Equals(idEquivalence));

                if (local.Articulo.DetallesDeCompras.Count > 0)
                    return true;

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
