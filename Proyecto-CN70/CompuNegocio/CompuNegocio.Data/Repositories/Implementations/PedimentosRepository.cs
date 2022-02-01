using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class PedimentosRepository : BaseRepository<Pedimento> , IPedimentosRepository
    {
        public PedimentosRepository(CNEntities context) : base(context) { }

        public Pedimento FindByDetails(Pedimento customsRequest)
        {
            try
            {
                return _dbSet.FirstOrDefault(x =>
                    x.aduana == customsRequest.aduana && x.añoEnCurso == customsRequest.añoEnCurso &&
                    x.añoOperacion == customsRequest.añoOperacion && x.patente == customsRequest.patente &&
                    x.progresivo == customsRequest.progresivo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
