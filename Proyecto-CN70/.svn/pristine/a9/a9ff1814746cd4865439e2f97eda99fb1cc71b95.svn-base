using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class EstacionesRepository : BaseRepository<Estacione>, IEstacionesRepository
    {
        public EstacionesRepository(CNEntities context) : base(context) { }

        public List<Estacione> List(int idRegister)
        {
            try
            {
                return _dbSet.Where(e => e.idEmpresa.Equals(idRegister)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione Find(string description)
        {
            try
            {
                return _dbSet.FirstOrDefault(e => e.descripcion.Equals(description, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Estacione HasStation(string computerCode)
        {
            try
            {
                return _dbSet.FirstOrDefault(e => e.equipo.Equals(computerCode));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Estacione> WithDescriptionLike(string description)
        {
            try
            {
                return _dbSet.Where(e => e.descripcion.Contains(description) || e.Empresa.descripcion.Contains(description)).ToList();
            }
            catch (Exception)
            {    
                throw;
            }
        }
    }
}
