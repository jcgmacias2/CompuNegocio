using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewSeriesRepository : BaseRepository<VwFoliosPorSerie>, IViewSeriesRepository
    {
        public ViewSeriesRepository(CNEntities context) : base(context) { }

        public VwFoliosPorSerie Find(string serie)
        {
            try
            {
                return _dbSet.FirstOrDefault(s => s.identificador.Equals(serie));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Next(string serie)
        {
            try
            {
                var folios = _dbSet.FirstOrDefault(s => s.identificador.Equals(serie));
                if(!folios.isValid())
                    throw new Exception("La serie especificada no se encuentra configurada");

                return folios.ultimoFolio + 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
