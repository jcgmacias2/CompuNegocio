using Aprovi.Data.Core;

namespace Aprovi.Business.Services
{
    public class AproviAbonoDeFacturaService : AbonoDeFacturaService
    {
        public AproviAbonoDeFacturaService(IUnitOfWork unitOfWork, IConfiguracionService config, IComprobantFiscaleService fiscalReceipts, ISerieService series) : base(unitOfWork, config, fiscalReceipts, series)
        {
        }
    }
}
