using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class AbonosDeFacturaRepository : BaseRepository<AbonosDeFactura>, IAbonosDeFacturaRepository
    {
        public AbonosDeFacturaRepository(CNEntities context) : base(context) { }

        public AbonosDeFactura Find(string folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.folio.Equals(folio, StringComparison.InvariantCultureIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura FindParcialidad(string serie, int folio)
        {
            try
            {
                return _dbSet.FirstOrDefault(a => a.TimbresDeAbonosDeFactura != null && a.TimbresDeAbonosDeFactura.serie.Equals(serie) && a.TimbresDeAbonosDeFactura.folio.Equals(folio));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AbonosDeFactura> List(int idInvoice)
        {
            try
            {
                return _dbSet.Where(a => a.idFactura.Equals(idInvoice)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeFactura> ListParcialidades()
        {
            try
            {
                return _dbSet.Where(a => a.TimbresDeAbonosDeFactura != null || (a.Pago !=  null && a.Pago.TimbresDePago != null)).ToList();
            }
            catch (Exception)
            {    
                throw;
            }
        }

        public List<AbonosDeFactura> ParcialidadesWithFolioOrClientLike(string value)
        {
            try
            {
                return _dbSet.Where(p => p.TimbresDeAbonosDeFactura != null || (p.Pago != null && p.Pago.TimbresDePago != null))
                    .Where(a => (a.TimbresDeAbonosDeFactura != null && (a.TimbresDeAbonosDeFactura.serie + a.TimbresDeAbonosDeFactura.folio.ToString()).Contains(value)) || a.Factura.Cliente.razonSocial.Contains(value)
                            || (a.Pago != null && (a.Pago.serie + a.Pago.folio.ToString()).Contains(value) || a.Pago.Cliente.razonSocial.Contains(value))).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeFactura Find(string serie, int folio)
        {
            try
            {
                return _dbSet.Where(p => p.TimbresDeAbonosDeFactura != null).FirstOrDefault(a => a.TimbresDeAbonosDeFactura.serie.Equals(serie) && a.TimbresDeAbonosDeFactura.folio.Equals(folio));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeFactura> List(int idRegister, DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end && a.idEmpresa.Equals(idRegister)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeFactura> List(DateTime start, DateTime end)
        {
            try
            {
                return _dbSet.Where(a => a.fechaHora >= start && a.fechaHora <= end).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
