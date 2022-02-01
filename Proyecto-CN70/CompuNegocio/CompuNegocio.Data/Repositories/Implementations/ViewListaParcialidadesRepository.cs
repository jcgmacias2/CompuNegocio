using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewListaParcialidadesRepository : BaseRepository<VwListaParcialidade>, IViewListaParcialidadesRepository
    {
        public ViewListaParcialidadesRepository(CNEntities context) : base(context) { }

        public List<VwListaParcialidade> WithFolioOrClientLike(string value)
        {
            try
            {
                return _dbSet.Where(x => x.Cliente.Contains(value) || x.FolioFacturaTexto.Contains(value) || x.FolioAbonoTexto.Contains(value)).OrderByDescending(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public VwListaParcialidade FindWithSerieAndFolio(string serie, int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(x => x.SerieAbono == serie && x.FolioAbono == folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public new List<VwListaParcialidade> List()
        {
            try
            {
                return _dbSet.OrderByDescending(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
