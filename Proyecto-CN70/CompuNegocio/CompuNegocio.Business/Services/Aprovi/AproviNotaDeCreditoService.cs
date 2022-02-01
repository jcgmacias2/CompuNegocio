using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviNotaDeCreditoService : NotaDeCreditoService
    {
        public AproviNotaDeCreditoService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, IClienteService clients, IArticuloService items, ISerieService series, ICatalogosEstaticosService staticCatalogs) : base(unitOfWork, config, fiscalReceipts, clients, items, series, staticCatalogs)
        {
        }
    }
}
