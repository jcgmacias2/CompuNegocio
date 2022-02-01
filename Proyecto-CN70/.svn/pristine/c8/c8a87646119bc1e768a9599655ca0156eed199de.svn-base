using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public class ViewResumenPorFacturaRepository : BaseRepository<VwResumenPorFactura>, IViewResumenPorFacturaRepository
    {
        public ViewResumenPorFacturaRepository(CNEntities context) : base(context) { }

        public List<VwResumenPorFactura> List(int idCliente, DateTime inicio, DateTime fin, bool soloConDeuda)
        {
            try
            {
                inicio = inicio.ToLastMidnight();
                fin = fin.ToNextMidnight();

                if (soloConDeuda)
                    return _dbSet.Where(f => f.idCliente.Equals(idCliente) && f.fechaHora >= inicio && f.fechaHora <= fin && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada && f.abonado.Value < f.subtotal.Value + f.impuestos.Value).ToList();
                else
                    return _dbSet.Where(f => f.idCliente.Equals(idCliente) && f.fechaHora >= inicio && f.fechaHora <= fin && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwResumenPorFactura> ListByCustomer(int idCliente, bool soloConDeuda)
        {
            try
            {
                if (soloConDeuda)
                    return _dbSet.AsNoTracking().Where(f => f.idCliente.Equals(idCliente) && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada && Math.Round(f.abonado.Value, 2) < Math.Round(f.subtotal.Value, 2) + Math.Round(f.impuestos.Value, 2)).OrderBy(x => x.folio).ToList();
                else
                    return _dbSet.AsNoTracking().Where(f => f.idCliente.Equals(idCliente) && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada).OrderBy(x => x.folio).ToList();

                //if (soloConDeuda)
                //    return _dbSet.AsNoTracking().Where(f => f.idCliente.Equals(idCliente) && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada && f.abonado.Value < f.subtotal.Value + f.impuestos.Value).OrderBy(x => x.folio).ToList();
                //else
                //    return _dbSet.AsNoTracking().Where(f => f.idCliente.Equals(idCliente) && f.idEstatusDeFactura != (int)StatusDeFactura.Anulada && f.idEstatusDeFactura != (int)StatusDeFactura.Cancelada).OrderBy(x => x.folio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VwResumenPorFactura> ListByIds(List<int> invoicesList)
        {
            try
            {
                var invoices = new List<VwResumenPorFactura>();
                foreach (var id in invoicesList)
                {
                    invoices.Add(_dbSet.FirstOrDefault(f => f.idFactura.Equals(id)));
                }

                return invoices;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
