using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class VentasPorArticuloService : IVentasPorArticuloService
    {
        private IUnitOfWork _UOW;
        private IViewReporteVentasPorArticuloRepository _ventasPorArticulo;

        public VentasPorArticuloService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _ventasPorArticulo = _UOW.VentasPorArticulo;
        }

        public List<VMRDetalleVentasPorArticulo> ListDetailed(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate, bool includeInvoices, bool includeBillsOfSale, bool includeCancelled)
        {
            try
            {
                return _ventasPorArticulo.ListDetailed(item, classification, startDate, endDate, includeInvoices, includeBillsOfSale, includeCancelled).Select(x=> new VMRDetalleVentasPorArticulo(x, classification)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMRDetalleVentasPorArticulo> ListTotals(Articulo item, Clasificacione classification, DateTime startDate, DateTime endDate,
            bool includeInvoices, bool includeBillsOfSale, bool includeCancelled)
        {
            try
            {
                return _ventasPorArticulo.ListTotals(item, classification, startDate, endDate, includeInvoices, includeBillsOfSale, includeCancelled).Select(x => new VMRDetalleVentasPorArticulo(x, classification)).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
