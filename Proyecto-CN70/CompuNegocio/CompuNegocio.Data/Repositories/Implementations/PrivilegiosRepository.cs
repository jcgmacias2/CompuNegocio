using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class PrivilegiosRepository : BaseRepository<Privilegio>, IPrivilegiosRepository
    {
        public PrivilegiosRepository(CNEntities context) : base(context) { }

        public List<Privilegio> List(int idUser)
        {
            try
            {
                return _dbSet.Where(p => p.idUsuario.Equals(idUser)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Privilegio Find(int idUser, int idView)
        {
            try
            {
                return _dbSet.FirstOrDefault(p => p.idUsuario.Equals(idUser) && p.idPantalla.Equals(idView));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
