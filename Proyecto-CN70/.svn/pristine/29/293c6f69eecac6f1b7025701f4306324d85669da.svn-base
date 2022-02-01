using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class CostoDeLoVendidoService : ICostoDeLoVendidoService
    {
        private IUnitOfWork _UOW;
        private IViewReporteCostoDeLoVendidoRepository _costoDeLoVendido;

        public CostoDeLoVendidoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _costoDeLoVendido = _UOW.CostoDeLoVendido;
        }

        public List<VMRDetalleCostoDeLoVendido> List(DateTime startDate, DateTime endDate, bool includeBillsOfSale)
        {
            try
            {
                return _costoDeLoVendido.List(startDate, endDate, includeBillsOfSale).Select(x=>new VMRDetalleCostoDeLoVendido(x)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
