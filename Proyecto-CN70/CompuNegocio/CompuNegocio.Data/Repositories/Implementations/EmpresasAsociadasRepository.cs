
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Data.Repositories
{
    public class EmpresasAsociadasRepository : BaseRepository<EmpresasAsociada>, IEmpresasAsociadasRepository
    {
        public EmpresasAsociadasRepository(CNEntities context) : base(context) { }

        public bool CanDelete(EmpresasAsociada empresaAsociada)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(c => c.idEmpresaAsociada.Equals(empresaAsociada.idEmpresaAsociada));

                if (local.SolicitudesDeTraspasoes.Count > 0)
                    return false;

                if (local.Traspasos.Count > 0)
                    return false;

                if (local.Traspasos1.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada Find(string nombre)
        {
            try
            {
                return _dbSet.FirstOrDefault(c => c.nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EmpresasAsociada FindByDatabaseName(string databaseName)
        {
            try
            {
                return _dbSet.FirstOrDefault(x=>x.baseDeDatos.Equals(databaseName, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExistsByDatabaseName(string databaseName)
        {
            try
            {
                return _dbSet.Any(x => x.baseDeDatos == databaseName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EmpresasAsociada> Like(string value)
        {
            try
            {
                return _dbSet.Where(c => c.nombre.Contains(value) || c.Empresa.descripcion.Contains(value)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
