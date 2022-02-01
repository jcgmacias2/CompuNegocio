using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class UsuariosRepository : BaseRepository<Usuario>, IUsuariosRepository
    {
        public UsuariosRepository(CNEntities context) : base(context) {}

        public bool CanDelete(int idUser)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(u => u.idUsuario.Equals(idUser));

                if (local.Compras.Count > 0)
                    return false;

                if (local.Facturas.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario Find(string username)
        {
            try
            {
                return _dbSet.FirstOrDefault(u => u.nombreDeUsuario.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Usuario> WithNameLike(string name)
        {
            try
            {
                return _dbSet.Where(u => u.nombreDeUsuario.Contains(name) || u.nombreCompleto.Contains(name)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario Authenticate(string username, string password)
        {
            try
            {
                return _dbSet.FirstOrDefault(u => u.nombreDeUsuario.Equals(username, StringComparison.InvariantCultureIgnoreCase) && u.contraseña.Equals(password));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
