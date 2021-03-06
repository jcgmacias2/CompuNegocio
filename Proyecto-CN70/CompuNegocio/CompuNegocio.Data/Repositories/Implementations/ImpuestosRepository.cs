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
    public class ImpuestosRepository : BaseRepository<Impuesto>, IImpuestosRepository
    {
        public ImpuestosRepository(CNEntities context) : base(context) { }

        public bool CanDelete(int idTax)
        {
            try
            {
                var local = _dbSet.FirstOrDefault(i => i.idImpuesto.Equals(idTax));

                if (local.Articulos.Count > 0)
                    return false;

                if (local.DetallesDeCompras.Count > 0)
                    return false;

                if (local.DetallesDeFacturas.Count > 0)
                    return false;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto Find(string code, decimal rate, TiposDeImpuesto type)
        {
            try
            {
                return _dbSet.FirstOrDefault(i => i.codigo.Equals(code) && i.valor.Equals(rate) && i.idTipoDeImpuesto.Equals(type.idTipoDeImpuesto));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto Find(string description, int type, decimal rate)
        {
            try
            {
                return _dbSet.FirstOrDefault(i => i.nombre.Equals(description, StringComparison.InvariantCultureIgnoreCase) && i.idTipoDeImpuesto.Equals(type) && i.valor.Equals(rate));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Impuesto SearchAll(string description, int type, decimal rate)
        {
            try
            {
                //Lo busco en la base de datos
                var tax = _dbSet.FirstOrDefault(i => i.nombre.Equals(description, StringComparison.InvariantCultureIgnoreCase) && i.idTipoDeImpuesto.Equals(type) && i.valor.Equals(rate));

                //Si lo encuentro lo regreso
                if (tax.isValid())
                    return tax;

                //Si no lo encuentro, lo busco en el cache local
                tax = _dbSet.Local.FirstOrDefault(i => i.nombre.Equals(description, StringComparison.InvariantCultureIgnoreCase) && i.idTipoDeImpuesto.Equals(type) && i.valor.Equals(rate));

                //Regreso lo que encuentre ya sea un null o el objeto
                return tax;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwReporteImpuestosPorPeriodo> List(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _context.VwReporteImpuestosPorPeriodoes.AsNoTracking().Where(x => DbFunctions.TruncateTime(x.fechaHora) >= startDate.Date && DbFunctions.TruncateTime(x.fechaHora) <= endDate.Date).OrderBy(x => x.fechaHora).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Impuesto> WithNameLike(string name)
        {
            try
            {
                return _dbSet.Where(i => i.nombre.ToUpper().Contains(name.ToUpper())).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
