using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class SaldosPorClienteService : ISaldosPorClienteService
    {
        private IUnitOfWork _UOW;
        private IViewSaldosPorClientePorMonedaRepository _saldos;
        private IViewResumenPorFacturaRepository _invoices;
        private IViewResumenPorRemisionRepository _billsOfSale;
        private IAbonosDeFacturaRepository _invoicesPayments;
        private IAbonosDeRemisionRepository _billsOfSalePayments;
        private INotasDeCreditoRepository _creditNotes;

        public SaldosPorClienteService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _saldos = _UOW.SaldosPorCliente;
            _invoices = _UOW.ResumenDeFacturas;
            _billsOfSale = _UOW.ResumenDeRemisiones;
            _invoicesPayments = _UOW.AbonosDeFactura;
            _billsOfSalePayments = _UOW.AbonosDeRemision;
            _creditNotes = _UOW.NotasDeCredito;
        }

        public List<VwSaldosPorClientePorMoneda> List()
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

        public List<VMAbonoCuentaCliente> List(Cliente client, DateTime start, DateTime end, bool onlyPendingBalance)
        {
            try
            {
                var invoices = _invoices.List(client.idCliente, start, end, onlyPendingBalance);
                var billsOfSales = _billsOfSale.List(client.idCliente, start, end, onlyPendingBalance);

                if (invoices.Count.Equals(0) && billsOfSales.Count.Equals(0))
                    throw new Exception("No existen movimientos dentro de los criterios especificados");

                var accountBalances = new List<VMAbonoCuentaCliente>();

                //Proceso las facturas
                foreach (var invoice in invoices)
                {
                    var payments = _invoicesPayments.List(invoice.idFactura).Where(a => a.idEstatusDeAbono == (int)StatusDeAbono.Registrado).ToList();

                    var creditNotes = _creditNotes.ListByInvoice(invoice.idFactura).Where(nc => nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Pendiente_De_Timbrado) || nc.idEstatusDeNotaDeCredito.Equals((int)StatusDeNotaDeCredito.Timbrada));

                    //Si una factura no tiene abonos simplemente la agrego
                    if(!payments.isValid() || payments.Count.Equals(0))
                    {
                        accountBalances.Add(new VMAbonoCuentaCliente(invoice));
                        continue;
                    }

                    //Si una factura tiene abonos agrego un registro de la factura y su abono por cada abono
                    foreach (var p in payments)
                    {
                        accountBalances.Add(new VMAbonoCuentaCliente(invoice, p));
                    }

                    //Si tiene notas de crédito las registro como abonos
                    foreach (var nc in creditNotes)
                    {
                        accountBalances.Add(new VMAbonoCuentaCliente(invoice, nc));
                    }
                }

                //Proceso las remisiones
                foreach (var billOfSale in billsOfSales)
                {
                    var payments = _billsOfSalePayments.List(billOfSale.idRemision).Where(a => a.idEstatusDeAbono == (int)StatusDeAbono.Registrado).ToList();

                    if (!payments.isValid() || payments.Count.Equals(0))
                    {
                        accountBalances.Add(new VMAbonoCuentaCliente(billOfSale));
                        continue;
                    }

                    foreach (var payment in payments)
                    {
                        accountBalances.Add(new VMAbonoCuentaCliente(billOfSale, payment));
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
