using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class CertificadosRepository : BaseRepository<Certificado>, ICertificadosRepository
    {
        public CertificadosRepository(CNEntities context) : base(context) { }

        public Certificado GetDefault()
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.activo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
