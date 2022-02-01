using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewReporteListaDePreciosPorListaRepository : BaseRepository<VwReporteListaDePreciosPorLista>, IViewReporteListaDePreciosPorListaRepository
    {
        public ViewReporteListaDePreciosPorListaRepository(CNEntities context) : base(context) { }

        public List<VwReporteListaDePreciosPorLista> WithClassification(ListasDePrecio priceList, Clasificacione classification, bool includeOnlyWithStock, bool includeNonStockedItems)
        {
            try
            {
                if (!classification.isValid() || !classification.idClasificacion.isValid())
                {
                    var data = _dbSet.AsNoTracking().AsQueryable().Where(x=>x.idListaDePrecio == priceList.idListaDePrecio);

                    if (includeOnlyWithStock)
                    {
                        data = data.Where(x => x.existencia > 0);
                    }

                    if (!includeNonStockedItems)
                    {
                        data = data.Where(x => x.inventariado == true);
                    }

                    return data.ToList();
                }

                string query = string.Format("SELECT RLP.* FROM dbo.VwReporteListaDePreciosPorLista AS RLP JOIN dbo.ClasificacionesPorArticulo AS CPA ON RLP.idArticulo = CPA.idArticulo AND CPA.idClasificacion = {0} AND RLP.idListaDePrecio = {1}", classification.idClasificacion, priceList.idListaDePrecio);

                //Se agregan las condiciones
                if (includeOnlyWithStock || !includeNonStockedItems)
                {
                    query += " AND ";
                }

                if (includeOnlyWithStock)
                {
                    query += string.Format("RLP.existencia > 0{0}", (!includeNonStockedItems) ? " AND " : "");
                }

                if (!includeNonStockedItems)
                {
                    query += "RLP.inventariado = 1";
                }

                return _dbSet.SqlQuery(query).AsNoTracking().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
