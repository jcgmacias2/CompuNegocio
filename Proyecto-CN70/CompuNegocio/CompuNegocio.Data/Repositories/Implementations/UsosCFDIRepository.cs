using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class UsosCFDIRepository : BaseRepository<UsosCFDI>, IUsosCFDIRepository
    {
        public UsosCFDIRepository(CNEntities context) : base(context) { }

        public UsosCFDI Find(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(u => u.idUsoCFDI.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UsosCFDI Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UsosCFDI> Like(string value)
        {
            try
            {
                return _dbSet.Where(u => u.codigo.Contains(value) || u.descripcion.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        List<UsosCFDI> IUsosCFDIRepository.List()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
