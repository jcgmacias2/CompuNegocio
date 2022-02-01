using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class FormaPagoService : IFormaPagoService
    {
        private IUnitOfWork _UOW;
        private IFormasPagoRepository _paymentForms;

        public FormaPagoService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _paymentForms = _UOW.FormasDePago;
        }

        public List<FormasPago> List()
        {
            try
            {
                return _paymentForms.List().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FormasPago> WithDescriptionLike(string description)
        {
            try
            {
                return _paymentForms.WithDescriptionLike(description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FormasPago Find(int idPaymentForm)
        {
            try
            {
                return _paymentForms.Find(idPaymentForm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FormasPago Find(string description)
        {
            try
            {
                return _paymentForms.Find(description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(FormasPago paymentForm)
        {
            try
            {
                var local = _paymentForms.Find(paymentForm.idFormaPago);
                local.activa = false;
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public FormasPago Update(FormasPago paymentForm)
        {
            try
            {
                var local = _paymentForms.Find(paymentForm.idFormaPago);
                local.activa = true;
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
