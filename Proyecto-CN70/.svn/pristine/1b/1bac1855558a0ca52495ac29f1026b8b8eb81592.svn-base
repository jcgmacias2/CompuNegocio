using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class SaldosPorProveedorService : ISaldosPorProveedorService
    {
        private IUnitOfWork _UOW;
        private IViewSaldosPorProveedorPorMonedaRepository _saldos;
        private IViewResumenPorCompraRepository _purchasesSummary;
        private IAbonosDeCompraRepository _purchasesPayments;

        public SaldosPorProveedorService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _saldos = _UOW.SaldosPorProveedor;
            _purchasesSummary = _UOW.ResumenDeCompras;
            _purchasesPayments = _UOW.AbonosDeCompra;
        }

        public List<VwSaldosPorProveedorPorMoneda> List()
        {
            try
            {
                return _saldos.List();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMAbonoCuentaProveedor> List(Proveedore supplier, DateTime start, DateTime end, bool onlyPendingBalance)
        {
            try
            {
                var purchases = _purchasesSummary.List(supplier.idProveedor, start, end, onlyPendingBalance);

                if (purchases.Count.Equals(0))
                    throw new Exception("No existen compras dentro de los criterios especificados");

                var accountBalances = new List<VMAbonoCuentaProveedor>();

                foreach (var purchase in purchases)
                {
                    var payments = _purchasesPayments.List(purchase.idCompra).Where(a => !a.cancelado).ToList();

                    if (!payments.isValid() || payments.Count.Equals(0))
                    {
                        accountBalances.Add(new VMAbonoCuentaProveedor(purchase));
                        continue;
                    }

                    foreach (var payment in payments)
                    {
                        accountBalances.Add(new VMAbonoCuentaProveedor(purchase, payment));
                    }
                }

                return accountBalances;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
