using Aprovi.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aprovi.Business.Services
{
    public class AproviComprobanteEnviadoService : ComprobanteEnviadoService
    {
        public AproviComprobanteEnviadoService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
