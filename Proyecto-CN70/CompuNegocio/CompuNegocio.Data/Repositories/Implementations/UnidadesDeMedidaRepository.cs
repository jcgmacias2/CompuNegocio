using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class UnidadesDeMedidaRepository : BaseRepository<UnidadesDeMedida>, IUnidadesDeMedidaRepository
    {
        public UnidadesDeMedidaRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idUnitOfMeasure)
        {
            try
            {

                var local = _dbSet.First(u => u.idUnidadDeMedida.Equals(idUnitOfMeasure));

                if (local.Articulos.Count > 0)
                    return false;

                if (local.DetallesDeCompras.Count > 0)
                    return false;

                if (local.Equivalencias.Count > 0)
                    return false;

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public UnidadesDeMedida Find(string code)
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

        public UnidadesDeMedida SearchAll(string code)
        {
            try
            {
                //Lo busco en la base de datos
                var unit = _dbSet.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Si existe lo regreso
                if (unit.isValid())
                    return unit;

                //Si no existe en la base de datos, reviso el store local
                unit = _dbSet.Local.FirstOrDefault(u => u.codigo.Equals(code, StringComparison.InvariantCultureIgnoreCase));

                //Regreso lo que haya obtenido, ya sea un null o lo que encontré localmente
                return unit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<UnidadesDeMedida> Like(string value)
        {
            try
            {
                return _dbSet.Where(u => u.codigo.Contains(value) || u.descripcion.ToUpper().Contains(value.ToUpper())).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
