using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class EstadoDeLaEmpresaService : IEstadoDeLaEmpresaService
    {
        private IUnitOfWork _UOW;
        private IAjusteService _adjustments;
        private IArticuloService _items;
        private ICompraService _purchases;
        private IFacturaService _invoices;
        private INotaDeCreditoService _creditNotes;
        private INotaDeDescuentoService _discountNotes;
        private IPedidoService _orders;
        private IRemisionService _billsOfSale;

        public EstadoDeLaEmpresaService(IUnitOfWork unitOfWork, IAjusteService adjustments, IArticuloService items, ICompraService purchases, IFacturaService invoices, INotaDeCreditoService creditNotes, INotaDeDescuentoService discountNotes, IPedidoService orders, IRemisionService billsOfSale)
        {
            _UOW = unitOfWork;
            _adjustments = adjustments;
            _items = items;
            _purchases = purchases;
            _invoices = invoices;
            _creditNotes = creditNotes;
            _discountNotes = discountNotes;
            _orders = orders;
            _billsOfSale = billsOfSale;
        }

        public VMEstadoDeLaEmpresa List(VMEstadoDeLaEmpresa vm, DateTime startDate, DateTime endDate, bool includeBillsOfSale)
        {
            try
            {
                _adjustments.ListEntranceAdjustmentsForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _adjustments.ListExitAdjustmentsForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _items.ListItemsForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _purchases.ListPurchasesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _purchases.ListPayableBalancesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _invoices.ListCollectableBalancesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _creditNotes.ListCreditNotesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _discountNotes.ListDiscountNotesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _orders.ListOrdersForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                _invoices.ListInvoicesForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);

                //si se deben incluir remisiones
                if (includeBillsOfSale)
                {
                    _billsOfSale.ListForCompanyStatus(vm, vm.FechaInicio, vm.FechaFin);
                }

                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
