using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class AbonoDeCompraService : IAbonoDeCompraService
    {
        private IUnitOfWork _UOW;
        private IAbonosDeCompraRepository _payments;

        public AbonoDeCompraService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _payments = _UOW.AbonosDeCompra;
        }

        public AbonosDeCompra Add(AbonosDeCompra payment)
        {
            try
            {
                payment.cancelado = false;
                _payments.Add(payment);
                _UOW.Save();

                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeCompra Find(int idPayment)
        {
            try
            {
                return _payments.Find(idPayment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeCompra Find(string folio)
        {
            try
            {
                return _payments.Find(folio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetNextFolio()
        {
            try
            {
                return _payments.NextFolio();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AbonosDeCompra Cancel(AbonosDeCompra payment)
        {
            try
            {
                var local = _payments.Find(payment.idAbonoDeCompra);
                payment.cancelado = true;
                _payments.Update(local);
                _UOW.Save();
                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AbonosDeCompra> List(int idPurchase)
        {
            try
            {
                return _payments.List(idPurchase).Where(a => !a.cancelado).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
