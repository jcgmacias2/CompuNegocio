using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ListasDePrecioRepository : BaseRepository<ListasDePrecio>, IListasDePrecioRepository
    {
        public ListasDePrecioRepository(CNEntities context) : base(context) { }

        public ListasDePrecio Find(string code)
        {
            try
            {
                return _dbSet.FirstOrDefault(l => l.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithCodeLike(string code)
        {
            try
            {
                return _dbSet.Where(l => l.codigo.Contains(code)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithItemLike(string item)
        {
            try
            {

                return _dbSet.SqlQuery(string.Format("SELECT LDP.idListaDePrecio, LDP.codigo FROM ListasDePrecio AS LDP INNER JOIN Precios AS P ON P.idListaDePrecio = LDP.idListaDePrecio INNER JOIN Articulos AS A ON A.idArticulo = P.idArticulo WHERE A.codigo LIKE '%{0}%' OR descripcion LIKE '%{0}%' GROUP BY LDP.idListaDePrecio, LDP.codigo", item)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ListasDePrecio> WithClientLike(string client)
        {
            try
            {
                return _dbSet.SqlQuery(string.Format("SELECT LDP.idListaDePrecio, LDP.codigo FROM ListasDePrecio AS LDP INNER JOIN Clientes AS C ON C.idListaDePrecio = LDP.idListaDePrecio WHERE C.codigo LIKE '%{0}%' OR razonSocial LIKE '%{0}%' GROUP BY LDP.idListaDePrecio, LDP.codigo", client)).ToList(); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
