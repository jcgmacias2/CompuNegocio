﻿using Aprovi.Business.ViewModels;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class AbonosService : IAbonosService
    {
        private IUnitOfWork _UOW;
        private IAbonosDeFacturaRepository _invoicesPayments;
        private IAbonosDeRemisionRepository _billsOfSalePayments;
        private INotasDeCreditoRepository _creditNotes;

        public AbonosService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _invoicesPayments = _UOW.AbonosDeFactura;
            _billsOfSalePayments = _UOW.AbonosDeRemision;
            _creditNotes = _UOW.NotasDeCredito;
        }

        public List<VMAbono> ByPeriod(DateTime start, DateTime end)
        {
            try
            {
                List<VMAbono> payments;
                //Ajustar la fecha de inicio para que la hora sea 00:00:01
                start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 1);
                //Ajustar la fecha de terminación para que la hora sea 23:59:59
                end = new DateTime(end.Year, end.Month, end.Day + 1, 0, 0, 0);
                var invoicePayments = _invoicesPayments.List(start, end);
                var billOfLadingPayments = _billsOfSalePayments.List(start, end);

                payments = new List<VMAbono>();
                invoicePayments.ForEach(a => payments.Add(new VMAbono(a)));
                billOfLadingPayments.ForEach(a => payments.Add(new VMAbono(a)));

                return payments.OrderByDescending(p => p.Folio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<VMAbono> ByPeriod(Empresa business, DateTime start, DateTime end)
        {
            try
            {
                List<VMAbono> payments;
                //Ajustar la fecha de inicio para que la hora sea 00:00:01
                start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 1);
                //Ajustar la fecha de terminación para que la hora sea 23:59:59
                end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
                var invoicePayments = _invoicesPayments.List(business.idEmpresa, start, end);
                var billOfSalePayments = _billsOfSalePayments.List(business.idEmpresa, start, end);
                var creditNotes = _creditNotes.List(business.idEmpresa, start, end);

                payments = new List<VMAbono>();
                invoicePayments.ForEach(a => payments.Add(new VMAbono(a)));
                billOfSalePayments.ForEach(a => payments.Add(new VMAbono(a)));
                creditNotes.ForEach(c=> payments.Add(new VMAbono(new VMNotaDeCredito(c))));


                return payments.OrderByDescending(p => p.Folio).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
