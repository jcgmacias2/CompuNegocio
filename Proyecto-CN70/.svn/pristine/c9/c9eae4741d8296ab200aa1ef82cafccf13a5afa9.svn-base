using Aprovi.Data.Core;
using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Data.Repositories
{
    public interface IViewResumenPorFacturaRepository : IBaseRepository<VwResumenPorFactura>
    {
        List<VwResumenPorFactura> List(int idCliente, DateTime inicio, DateTime fin, bool soloConDeuda);

        List<VwResumenPorFactura> ListByCustomer(int idCliente, bool soloConDeuda);

        List<VwResumenPorFactura> ListByIds(List<int> invoicesList);
    }
}
